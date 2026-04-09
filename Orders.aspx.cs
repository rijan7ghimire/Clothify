using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Orders : System.Web.UI.Page
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
                // Show success message if redirected from checkout
                if (Request.QueryString["msg"] == "success")
                {
                    pnlSuccess.Visible = true;
                }

                string statusFilter = Request.QueryString["status"];
                if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                {
                    SetActiveTab(statusFilter);
                    LoadOrders(statusFilter);
                }
                else
                {
                    LoadOrders("All");
                }
            }
        }

        protected void FilterOrders_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string status = btn.CommandArgument;

            SetActiveTab(status);
            LoadOrders(status);
        }

        private void SetActiveTab(string status)
        {
            lnkAll.CssClass = "filter-tab";
            lnkPlaced.CssClass = "filter-tab";
            lnkProcessing.CssClass = "filter-tab";
            lnkShipped.CssClass = "filter-tab";
            lnkDelivered.CssClass = "filter-tab";

            switch (status)
            {
                case "Placed":
                    lnkPlaced.CssClass = "filter-tab active";
                    break;
                case "Processing":
                    lnkProcessing.CssClass = "filter-tab active";
                    break;
                case "Shipped":
                    lnkShipped.CssClass = "filter-tab active";
                    break;
                case "Delivered":
                    lnkDelivered.CssClass = "filter-tab active";
                    break;
                default:
                    lnkAll.CssClass = "filter-tab active";
                    break;
            }
        }

        private void LoadOrders(string status)
        {
            int userId = Convert.ToInt32(Session["UserID"]);

            string query;
            DataTable dt;

            if (status == "All")
            {
                query = "SELECT OrderID, OrderDate, TotalAmount, OrderStatus FROM Orders WHERE UserID = @UserID ORDER BY OrderDate DESC";
                dt = DBHelper.ExecuteQuery(query, new SqlParameter("@UserID", userId));
            }
            else
            {
                query = "SELECT OrderID, OrderDate, TotalAmount, OrderStatus FROM Orders WHERE UserID = @UserID AND OrderStatus = @Status ORDER BY OrderDate DESC";
                dt = DBHelper.ExecuteQuery(query,
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@Status", status));
            }

            if (dt.Rows.Count > 0)
            {
                rptOrders.DataSource = dt;
                rptOrders.DataBind();
                pnlEmpty.Visible = false;
            }
            else
            {
                rptOrders.DataSource = null;
                rptOrders.DataBind();
                pnlEmpty.Visible = true;
            }
        }
    }
}
