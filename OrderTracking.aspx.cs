using System;
using System.Data;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify
{
    public partial class OrderTracking : System.Web.UI.Page
    {
        private int orderId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string idParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out orderId))
            {
                Response.Redirect("~/Orders.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadOrderDetails();
            }
        }

        private void LoadOrderDetails()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Load order - verify it belongs to current user
            string orderQuery = @"SELECT OrderID, OrderDate, TotalAmount, OrderStatus, ShippingFullName, ShippingPhone,
                                  Province, District, Municipality, WardNo, Landmark, DeliveryFee
                                  FROM Orders WHERE OrderID = @OrderID AND UserID = @UserID";

            DataRow order = DBHelper.ExecuteSingleRow(orderQuery,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@UserID", userId));

            if (order == null)
            {
                pnlOrderDetails.Visible = false;
                lblMessage.Text = "Order not found.";
                lblMessage.CssClass = "error-message";
                lblMessage.Visible = true;
                return;
            }

            // Order number and status
            lblOrderNumber.Text = Convert.ToInt32(order["OrderID"]).ToString("D5");
            string status = order["OrderStatus"].ToString();
            lblStatus.Text = status;
            lblStatus.CssClass = "status-badge status-" + status.ToLower();

            // Order details
            lblOrderDate.Text = Convert.ToDateTime(order["OrderDate"]).ToString("dd MMM yyyy, hh:mm tt");
            lblTotal.Text = "Rs. " + Convert.ToDecimal(order["TotalAmount"]).ToString("N0");

            // Shipping address
            lblShipName.Text = order["ShippingFullName"].ToString();
            lblShipPhone.Text = order["ShippingPhone"].ToString();
            string address = string.Format("Ward {0}, {1}, {2}, {3}",
                order["WardNo"], order["Municipality"], order["District"], order["Province"]);
            string landmark = order["Landmark"].ToString();
            if (!string.IsNullOrEmpty(landmark))
            {
                address += " (Near " + landmark + ")";
            }
            lblShipAddress.Text = address;

            // Payment info
            string paymentQuery = "SELECT PaymentMethod, PaymentStatus FROM Payments WHERE OrderID = @OrderID";
            DataRow payment = DBHelper.ExecuteSingleRow(paymentQuery, new SqlParameter("@OrderID", orderId));
            if (payment != null)
            {
                lblPaymentMethod.Text = payment["PaymentMethod"].ToString();
                lblPaymentStatus.Text = payment["PaymentStatus"].ToString();
            }
            else
            {
                lblPaymentMethod.Text = "N/A";
                lblPaymentStatus.Text = "N/A";
            }

            // Status timeline
            UpdateTimeline(status);

            // Order items
            string itemsQuery = @"SELECT oi.Quantity, oi.UnitPrice, p.ProductName
                                  FROM OrderItems oi
                                  INNER JOIN Products p ON oi.ProductID = p.ProductID
                                  WHERE oi.OrderID = @OrderID";
            DataTable items = DBHelper.ExecuteQuery(itemsQuery, new SqlParameter("@OrderID", orderId));
            rptOrderItems.DataSource = items;
            rptOrderItems.DataBind();

            // Show cancel button only if status is Placed
            btnCancelOrder.Visible = (status == "Placed");
        }

        private void UpdateTimeline(string currentStatus)
        {
            // Status progression order
            string[] statuses = { "Placed", "Processing", "Shipped", "Delivered" };
            int currentIndex = -1;

            if (currentStatus == "Cancelled")
            {
                // For cancelled, nothing is active
                return;
            }

            for (int i = 0; i < statuses.Length; i++)
            {
                if (statuses[i] == currentStatus)
                {
                    currentIndex = i;
                    break;
                }
            }

            // Placed
            if (currentIndex >= 0)
            {
                if (currentIndex > 0)
                {
                    dotPlaced.Attributes["class"] = "timeline-dot completed";
                    titlePlaced.Attributes["class"] = "timeline-title completed";
                }
                else
                {
                    dotPlaced.Attributes["class"] = "timeline-dot active";
                    titlePlaced.Attributes["class"] = "timeline-title active";
                }
            }
            if (currentIndex > 0)
                linePlaced.Attributes["class"] = "timeline-line completed";

            // Processing
            if (currentIndex >= 1)
            {
                if (currentIndex > 1)
                {
                    dotProcessing.Attributes["class"] = "timeline-dot completed";
                    titleProcessing.Attributes["class"] = "timeline-title completed";
                }
                else
                {
                    dotProcessing.Attributes["class"] = "timeline-dot active";
                    titleProcessing.Attributes["class"] = "timeline-title active";
                }
            }
            if (currentIndex > 1)
                lineProcessing.Attributes["class"] = "timeline-line completed";

            // Shipped
            if (currentIndex >= 2)
            {
                if (currentIndex > 2)
                {
                    dotShipped.Attributes["class"] = "timeline-dot completed";
                    titleShipped.Attributes["class"] = "timeline-title completed";
                }
                else
                {
                    dotShipped.Attributes["class"] = "timeline-dot active";
                    titleShipped.Attributes["class"] = "timeline-title active";
                }
            }
            if (currentIndex > 2)
                lineShipped.Attributes["class"] = "timeline-line completed";

            // Delivered
            if (currentIndex >= 3)
            {
                dotDelivered.Attributes["class"] = "timeline-dot completed";
                titleDelivered.Attributes["class"] = "timeline-title completed";
            }
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            // Verify order belongs to user and is still Placed
            string checkQuery = "SELECT OrderStatus FROM Orders WHERE OrderID = @OrderID AND UserID = @UserID";
            DataRow order = DBHelper.ExecuteSingleRow(checkQuery,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@UserID", userId));

            if (order != null && order["OrderStatus"].ToString() == "Placed")
            {
                // Update order status to Cancelled
                string updateQuery = "UPDATE Orders SET OrderStatus = 'Cancelled' WHERE OrderID = @OrderID";
                DBHelper.ExecuteNonQuery(updateQuery, new SqlParameter("@OrderID", orderId));

                // Restore product stock quantities
                string itemsQuery = "SELECT ProductID, Quantity FROM OrderItems WHERE OrderID = @OrderID";
                DataTable items = DBHelper.ExecuteQuery(itemsQuery, new SqlParameter("@OrderID", orderId));
                foreach (DataRow item in items.Rows)
                {
                    string stockQuery = "UPDATE Products SET StockQuantity = StockQuantity + @Qty WHERE ProductID = @ProductID";
                    DBHelper.ExecuteNonQuery(stockQuery,
                        new SqlParameter("@Qty", Convert.ToInt32(item["Quantity"])),
                        new SqlParameter("@ProductID", Convert.ToInt32(item["ProductID"])));
                }

                // Reload page
                LoadOrderDetails();

                lblMessage.Text = "Order has been cancelled successfully.";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;
            }
        }
    }
}
