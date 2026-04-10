using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;
using Clothify.App_Code;

namespace Clothify
{
    public partial class EsewaVerify : System.Web.UI.Page
    {
        private const string ESEWA_MERCHANT_CODE = "EPAYTEST";
        private const string ESEWA_VERIFY_URL = "https://rc-epay.esewa.com.np/api/epay/txn/status/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string status = Request.QueryString["status"];
                string encodedData = Request.QueryString["data"];

                if (status == "failed")
                {
                    HandleFailure("Payment was cancelled or failed.");
                    return;
                }

                if (!string.IsNullOrEmpty(encodedData))
                {
                    HandleEsewaCallback(encodedData);
                }
                else
                {
                    HandleSuccess();
                }
            }
        }

        private void HandleEsewaCallback(string encodedData)
        {
            try
            {
                // Decode Base64 response from eSewa
                byte[] data = Convert.FromBase64String(encodedData);
                string jsonResponse = Encoding.UTF8.GetString(data);

                // Simple JSON parsing without Newtonsoft
                // Response format: {"transaction_code":"...","status":"COMPLETE","total_amount":"...","transaction_uuid":"...","product_code":"EPAYTEST","signed_field_names":"...","signature":"..."}
                string transactionStatus = ExtractJsonValue(jsonResponse, "status");
                string totalAmount = ExtractJsonValue(jsonResponse, "total_amount");
                string transactionUuid = ExtractJsonValue(jsonResponse, "transaction_uuid");

                if (transactionStatus == "COMPLETE")
                {
                    // Update payment status in database
                    if (Session["PendingOrderID"] != null)
                    {
                        int orderId = Convert.ToInt32(Session["PendingOrderID"]);

                        DBHelper.ExecuteNonQuery(
                            "UPDATE Payments SET PaymentStatus = 'Paid' WHERE OrderID = @OrderID",
                            new SqlParameter("@OrderID", orderId));

                        string orderNumber = "CN-" + orderId.ToString("D5");
                        lblSuccessOrder.Text = orderNumber;
                        lblSuccessAmount.Text = "Rs. " + Convert.ToDecimal(Session["PendingOrderTotal"]).ToString("N0");

                        // Send email notification
                        string customerEmail = "";
                        string customerName = "";
                        DataRow userRow = DBHelper.ExecuteSingleRow(
                            "SELECT u.Email, o.ShippingFullName FROM Orders o INNER JOIN Users u ON o.UserID = u.UserID WHERE o.OrderID = @OrderID",
                            new SqlParameter("@OrderID", orderId));
                        if (userRow != null)
                        {
                            customerEmail = userRow["Email"].ToString();
                            customerName = userRow["ShippingFullName"].ToString();
                        }

                        if (!string.IsNullOrEmpty(customerEmail))
                        {
                            EmailHelper.SendOrderConfirmation(customerEmail, customerName, orderNumber,
                                Convert.ToDecimal(Session["PendingOrderTotal"]));
                        }

                        // Clear pending order session
                        Session["PendingOrderID"] = null;
                        Session["PendingOrderTotal"] = null;
                        Session["EsewaTransactionUuid"] = null;
                        Session["CartItems"] = null;
                        Session["CartCount"] = 0;

                        pnlSuccess.Visible = true;
                        pnlFailed.Visible = false;
                    }
                    else
                    {
                        HandleSuccess();
                    }
                }
                else
                {
                    HandleFailure("Payment verification failed. Status: " + transactionStatus);
                }
            }
            catch (Exception ex)
            {
                HandleFailure("Could not verify payment. Please contact support.");
            }
        }

        private void HandleSuccess()
        {
            if (Session["PendingOrderID"] != null)
            {
                int orderId = Convert.ToInt32(Session["PendingOrderID"]);
                string orderNumber = "CN-" + orderId.ToString("D5");

                DBHelper.ExecuteNonQuery(
                    "UPDATE Payments SET PaymentStatus = 'Paid' WHERE OrderID = @OrderID",
                    new SqlParameter("@OrderID", orderId));

                lblSuccessOrder.Text = orderNumber;
                lblSuccessAmount.Text = "Rs. " + Convert.ToDecimal(Session["PendingOrderTotal"]).ToString("N0");

                Session["PendingOrderID"] = null;
                Session["PendingOrderTotal"] = null;
                Session["CartItems"] = null;
                Session["CartCount"] = 0;
            }

            pnlSuccess.Visible = true;
            pnlFailed.Visible = false;
        }

        private void HandleFailure(string message)
        {
            // If payment failed, cancel the pending order
            if (Session["PendingOrderID"] != null)
            {
                int orderId = Convert.ToInt32(Session["PendingOrderID"]);

                DBHelper.ExecuteNonQuery(
                    "UPDATE Orders SET OrderStatus = 'Cancelled' WHERE OrderID = @OrderID",
                    new SqlParameter("@OrderID", orderId));

                DBHelper.ExecuteNonQuery(
                    "UPDATE Payments SET PaymentStatus = 'Failed' WHERE OrderID = @OrderID",
                    new SqlParameter("@OrderID", orderId));

                // Restore stock
                DataTable items = DBHelper.ExecuteQuery(
                    "SELECT ProductID, Quantity FROM OrderItems WHERE OrderID = @OrderID",
                    new SqlParameter("@OrderID", orderId));

                foreach (DataRow item in items.Rows)
                {
                    DBHelper.ExecuteNonQuery(
                        "UPDATE Products SET StockQuantity = StockQuantity + @Qty WHERE ProductID = @ProductID",
                        new SqlParameter("@Qty", item["Quantity"]),
                        new SqlParameter("@ProductID", item["ProductID"]));
                }

                Session["PendingOrderID"] = null;
                Session["PendingOrderTotal"] = null;
            }

            lblFailedMessage.Text = message;
            pnlFailed.Visible = true;
            pnlSuccess.Visible = false;
        }

        private string ExtractJsonValue(string json, string key)
        {
            string searchKey = "\"" + key + "\":\"";
            int startIndex = json.IndexOf(searchKey);
            if (startIndex < 0) return "";
            startIndex += searchKey.Length;
            int endIndex = json.IndexOf("\"", startIndex);
            if (endIndex < 0) return "";
            return json.Substring(startIndex, endIndex - startIndex);
        }
    }
}
