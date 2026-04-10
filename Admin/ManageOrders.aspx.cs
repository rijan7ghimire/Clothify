using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Filter"] = "All";
                LoadOrders("All");
            }
        }

        private void LoadOrders(string statusFilter)
        {
            string query = @"SELECT o.OrderID, o.OrderDate, o.TotalAmount, o.OrderStatus, o.DeliveryFee,
                             o.ShippingFullName, o.ShippingPhone, o.Province, o.District, o.Municipality,
                             o.WardNo, o.Landmark, u.FullName AS CustomerName,
                             ISNULL(p.PaymentMethod, 'N/A') AS PaymentMethod,
                             ISNULL(p.PaymentStatus, 'N/A') AS PaymentStatus,
                             p.PaymentDate
                             FROM Orders o
                             LEFT JOIN Users u ON o.UserID = u.UserID
                             LEFT JOIN Payments p ON o.OrderID = p.OrderID";

            DataTable dt;

            if (statusFilter != "All")
            {
                query += " WHERE o.OrderStatus = @Status";
                query += " ORDER BY o.OrderDate DESC";
                dt = DBHelper.ExecuteQuery(query, new SqlParameter("@Status", statusFilter));
            }
            else
            {
                query += " ORDER BY o.OrderDate DESC";
                dt = DBHelper.ExecuteQuery(query);
            }

            if (dt.Rows.Count > 0)
            {
                rptOrders.DataSource = dt;
                rptOrders.DataBind();
                pnlNoData.Visible = false;

                // Set selected status for each order
                for (int i = 0; i < rptOrders.Items.Count; i++)
                {
                    DropDownList ddl = (DropDownList)rptOrders.Items[i].FindControl("ddlStatus");
                    if (ddl != null)
                    {
                        string currentStatus = dt.Rows[i]["OrderStatus"].ToString();
                        if (ddl.Items.FindByValue(currentStatus) != null)
                            ddl.SelectedValue = currentStatus;
                    }
                }
            }
            else
            {
                rptOrders.DataSource = null;
                rptOrders.DataBind();
                pnlNoData.Visible = true;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string filter = btn.CommandArgument;
            ViewState["Filter"] = filter;

            // Update active tab styling
            btnFilterAll.CssClass = "filter-tab";
            btnFilterPlaced.CssClass = "filter-tab";
            btnFilterProcessing.CssClass = "filter-tab";
            btnFilterShipped.CssClass = "filter-tab";
            btnFilterDelivered.CssClass = "filter-tab";
            btnFilterCancelled.CssClass = "filter-tab";
            btn.CssClass = "filter-tab active";

            LoadOrders(filter);
        }

        protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int orderId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "UpdateStatus")
            {
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                if (ddlStatus != null)
                {
                    string newStatus = ddlStatus.SelectedValue;
                    DBHelper.ExecuteNonQuery(
                        "UPDATE Orders SET OrderStatus = @Status WHERE OrderID = @OrderID",
                        new SqlParameter("@Status", newStatus),
                        new SqlParameter("@OrderID", orderId));

                    // Send email notification about status update
                    string orderNumber = "CN-" + orderId.ToString("D5");
                    DataRow orderRow = DBHelper.ExecuteSingleRow(
                        @"SELECT o.ShippingFullName, u.Email
                          FROM Orders o
                          LEFT JOIN Users u ON o.UserID = u.UserID
                          WHERE o.OrderID = @OrderID",
                        new SqlParameter("@OrderID", orderId));

                    if (orderRow != null)
                    {
                        string customerEmail = orderRow["Email"] != DBNull.Value ? orderRow["Email"].ToString() : "";
                        string customerName = orderRow["ShippingFullName"].ToString();

                        if (!string.IsNullOrEmpty(customerEmail))
                        {
                            EmailHelper.SendOrderStatusUpdate(customerEmail, customerName, orderNumber, newStatus);
                        }
                    }

                    ShowMessage("Order CN-" + orderId.ToString("D5") + " status updated to " + newStatus + ".", true);
                    LoadOrders(ViewState["Filter"].ToString());
                }
            }
            else if (e.CommandName == "ViewDetails")
            {
                Panel pnlDetails = (Panel)e.Item.FindControl("pnlDetails");
                if (pnlDetails != null)
                {
                    pnlDetails.Visible = !pnlDetails.Visible;

                    if (pnlDetails.Visible)
                    {
                        // Load order items
                        DataTable dtItems = DBHelper.ExecuteQuery(
                            @"SELECT oi.Quantity, oi.UnitPrice, (oi.Quantity * oi.UnitPrice) AS SubTotal, p.ProductName
                              FROM OrderItems oi
                              INNER JOIN Products p ON oi.ProductID = p.ProductID
                              WHERE oi.OrderID = @OrderID",
                            new SqlParameter("@OrderID", orderId));

                        Repeater rptOrderItems = (Repeater)e.Item.FindControl("rptOrderItems");
                        if (rptOrderItems != null)
                        {
                            rptOrderItems.DataSource = dtItems;
                            rptOrderItems.DataBind();
                        }

                        // Load order details for shipping
                        DataRow orderRow = DBHelper.ExecuteSingleRow(
                            @"SELECT o.*, ISNULL(p.PaymentMethod, 'N/A') AS PayMethod,
                              ISNULL(p.PaymentStatus, 'N/A') AS PayStatus, p.PaymentDate
                              FROM Orders o LEFT JOIN Payments p ON o.OrderID = p.OrderID
                              WHERE o.OrderID = @OrderID",
                            new SqlParameter("@OrderID", orderId));

                        if (orderRow != null)
                        {
                            Literal litShipName = (Literal)e.Item.FindControl("litShipName");
                            Literal litShipPhone = (Literal)e.Item.FindControl("litShipPhone");
                            Literal litShipAddress = (Literal)e.Item.FindControl("litShipAddress");
                            Literal litShipLandmark = (Literal)e.Item.FindControl("litShipLandmark");
                            Literal litPayMethod = (Literal)e.Item.FindControl("litPayMethod");
                            Literal litPayStatus = (Literal)e.Item.FindControl("litPayStatus");
                            Literal litPayDate = (Literal)e.Item.FindControl("litPayDate");
                            Literal litDeliveryFee = (Literal)e.Item.FindControl("litDeliveryFee");

                            if (litShipName != null)
                                litShipName.Text = orderRow["ShippingFullName"].ToString();
                            if (litShipPhone != null)
                                litShipPhone.Text = orderRow["ShippingPhone"].ToString();
                            if (litShipAddress != null)
                                litShipAddress.Text = string.Format("{0}, Ward {1}, {2}, {3}, Province {4}",
                                    orderRow["Municipality"], orderRow["WardNo"], orderRow["District"],
                                    orderRow["Municipality"], orderRow["Province"]);
                            if (litShipLandmark != null)
                                litShipLandmark.Text = orderRow["Landmark"].ToString();
                            if (litPayMethod != null)
                                litPayMethod.Text = orderRow["PayMethod"].ToString();
                            if (litPayStatus != null)
                                litPayStatus.Text = orderRow["PayStatus"].ToString();
                            if (litPayDate != null)
                            {
                                if (orderRow["PaymentDate"] != DBNull.Value)
                                    litPayDate.Text = Convert.ToDateTime(orderRow["PaymentDate"]).ToString("yyyy-MM-dd");
                                else
                                    litPayDate.Text = "N/A";
                            }
                            if (litDeliveryFee != null)
                            {
                                if (orderRow["DeliveryFee"] != DBNull.Value)
                                    litDeliveryFee.Text = "Rs. " + Convert.ToDecimal(orderRow["DeliveryFee"]).ToString("N0");
                                else
                                    litDeliveryFee.Text = "Rs. 0";
                            }
                        }
                    }
                }
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            divMessage.InnerText = message;
            divMessage.Attributes["class"] = isSuccess ? "alert alert-success" : "alert alert-danger";
        }
    }
}
