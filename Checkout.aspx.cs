using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Checkout : System.Web.UI.Page
    {
        private const decimal DeliveryFee = 150m;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            List<CartItem> cart = Session["CartItems"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                Response.Redirect("~/Cart.aspx");
                return;
            }

            if (!IsPostBack)
            {
                CalculateTotals(cart);

                // Pre-fill name from session
                if (Session["FullName"] != null)
                {
                    txtFullName.Text = Session["FullName"].ToString();
                }
            }
        }

        private void CalculateTotals(List<CartItem> cart)
        {
            decimal subtotal = 0;
            foreach (CartItem item in cart)
            {
                subtotal += item.Total;
            }

            lblSubtotal.Text = "Rs. " + subtotal.ToString("N0");
            lblDeliveryFee.Text = "Rs. " + DeliveryFee.ToString("N0");
            lblTotal.Text = "Rs. " + (subtotal + DeliveryFee).ToString("N0");
        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            List<CartItem> cart = Session["CartItems"] as List<CartItem>;
            if (cart == null || cart.Count == 0)
            {
                Response.Redirect("~/Cart.aspx");
                return;
            }

            // Determine selected payment method
            string paymentMethod = "Cash on Delivery";
            if (rbEsewa.Checked)
                paymentMethod = "eSewa";
            else if (rbKhalti.Checked)
                paymentMethod = "Khalti";
            else if (rbBankTransfer.Checked)
                paymentMethod = "Bank Transfer";

            // Calculate totals
            decimal subtotal = 0;
            foreach (CartItem item in cart)
            {
                subtotal += item.Total;
            }
            decimal totalAmount = subtotal + DeliveryFee;

            int userId = Convert.ToInt32(Session["UserID"]);

            try
            {
                // 1. Insert into Orders table and get OrderID
                string orderQuery = @"INSERT INTO Orders (OrderDate, TotalAmount, OrderStatus, UserID, ShippingFullName, ShippingPhone, Province, District, Municipality, WardNo, Landmark, DeliveryFee)
                                      VALUES (@OrderDate, @TotalAmount, @OrderStatus, @UserID, @ShippingFullName, @ShippingPhone, @Province, @District, @Municipality, @WardNo, @Landmark, @DeliveryFee);
                                      SELECT SCOPE_IDENTITY();";

                object result = DBHelper.ExecuteScalar(orderQuery,
                    new SqlParameter("@OrderDate", DateTime.Now),
                    new SqlParameter("@TotalAmount", totalAmount),
                    new SqlParameter("@OrderStatus", "Placed"),
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@ShippingFullName", txtFullName.Text.Trim()),
                    new SqlParameter("@ShippingPhone", txtPhone.Text.Trim()),
                    new SqlParameter("@Province", ddlProvince.SelectedValue),
                    new SqlParameter("@District", txtDistrict.Text.Trim()),
                    new SqlParameter("@Municipality", txtMunicipality.Text.Trim()),
                    new SqlParameter("@WardNo", txtWardNo.Text.Trim()),
                    new SqlParameter("@Landmark", txtLandmark.Text.Trim()),
                    new SqlParameter("@DeliveryFee", DeliveryFee)
                );

                int orderId = Convert.ToInt32(result);

                // 2. Insert each cart item into OrderItems table
                foreach (CartItem item in cart)
                {
                    string itemQuery = @"INSERT INTO OrderItems (Quantity, UnitPrice, OrderID, ProductID)
                                         VALUES (@Quantity, @UnitPrice, @OrderID, @ProductID)";

                    DBHelper.ExecuteNonQuery(itemQuery,
                        new SqlParameter("@Quantity", item.Quantity),
                        new SqlParameter("@UnitPrice", item.Price),
                        new SqlParameter("@OrderID", orderId),
                        new SqlParameter("@ProductID", item.ProductID)
                    );

                    // 5. Update product stock quantities
                    string stockQuery = @"UPDATE Products SET StockQuantity = StockQuantity - @Qty WHERE ProductID = @ProductID";
                    DBHelper.ExecuteNonQuery(stockQuery,
                        new SqlParameter("@Qty", item.Quantity),
                        new SqlParameter("@ProductID", item.ProductID)
                    );
                }

                // 3. Insert into Payments table
                string paymentQuery = @"INSERT INTO Payments (PaymentMethod, PaymentStatus, PaymentDate, OrderID)
                                        VALUES (@PaymentMethod, @PaymentStatus, @PaymentDate, @OrderID)";

                string paymentStatus = paymentMethod == "Cash on Delivery" ? "Pending" : "Pending";
                DBHelper.ExecuteNonQuery(paymentQuery,
                    new SqlParameter("@PaymentMethod", paymentMethod),
                    new SqlParameter("@PaymentStatus", paymentStatus),
                    new SqlParameter("@PaymentDate", DateTime.Now),
                    new SqlParameter("@OrderID", orderId)
                );

                // 4. Send order confirmation email
                string orderNumber = "CN-" + orderId.ToString("D5");
                string customerEmail = "";
                string customerName = txtFullName.Text.Trim();

                // Get user email from database
                DataRow userRow = DBHelper.ExecuteSingleRow(
                    "SELECT Email FROM Users WHERE UserID = @UserID",
                    new SqlParameter("@UserID", userId));
                if (userRow != null)
                {
                    customerEmail = userRow["Email"].ToString();
                }

                if (!string.IsNullOrEmpty(customerEmail))
                {
                    EmailHelper.SendOrderConfirmation(customerEmail, customerName, orderNumber, totalAmount);
                }

                // 6. Clear cart session
                Session["CartItems"] = null;
                Session["CartCount"] = 0;

                // 7. Redirect to Orders page with success
                Response.Redirect("~/Orders.aspx?msg=success");
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred while placing your order. Please try again.";
                lblError.Visible = true;
            }
        }
    }
}
