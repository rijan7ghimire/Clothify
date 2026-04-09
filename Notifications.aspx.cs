using System;
using System.Data;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }

        private void LoadNotifications()
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            DataTable dt = DBHelper.ExecuteQuery(
                @"SELECT TOP 20 OrderID, OrderDate, OrderStatus, TotalAmount
                  FROM Orders
                  WHERE UserID = @UserID
                  ORDER BY OrderDate DESC",
                new SqlParameter("@UserID", userId));

            if (dt.Rows.Count > 0)
            {
                rptNotifications.DataSource = dt;
                rptNotifications.DataBind();
                pnlEmpty.Visible = false;
            }
            else
            {
                rptNotifications.DataSource = null;
                rptNotifications.DataBind();
                pnlEmpty.Visible = true;
                pnlNotifications.Visible = false;
            }
        }

        protected string GetStatusMessage(string status, string orderId)
        {
            string orderNum = "CN-" + orderId.PadLeft(5, '0');
            switch (status)
            {
                case "Placed":
                    return "Your order " + orderNum + " has been placed successfully and is awaiting confirmation.";
                case "Processing":
                    return "Your order " + orderNum + " is being processed and prepared for shipment.";
                case "Shipped":
                    return "Your order " + orderNum + " has been shipped and is on its way to you.";
                case "Delivered":
                    return "Your order " + orderNum + " has been delivered. Thank you for shopping with us!";
                case "Cancelled":
                    return "Your order " + orderNum + " has been cancelled.";
                default:
                    return "Order " + orderNum + " status updated to " + status + ".";
            }
        }
    }
}
