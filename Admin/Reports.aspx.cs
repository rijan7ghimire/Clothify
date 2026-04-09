using System;
using System.Data;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSummaryCards();
                LoadMonthlySummary();
                LoadRecentOrders();
                LoadTopProducts();
            }
        }

        private void LoadSummaryCards()
        {
            object totalRevenue = DBHelper.ExecuteScalar("SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders");
            litTotalRevenue.Text = "Rs. " + Convert.ToDecimal(totalRevenue).ToString("N0");

            object totalOrders = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Orders");
            litTotalOrders.Text = Convert.ToInt32(totalOrders).ToString();

            object totalCustomers = DBHelper.ExecuteScalar("SELECT COUNT(DISTINCT UserID) FROM Orders");
            litTotalCustomers.Text = Convert.ToInt32(totalCustomers).ToString();

            object avgOrder = DBHelper.ExecuteScalar("SELECT ISNULL(AVG(TotalAmount), 0) FROM Orders");
            litAvgOrderValue.Text = "Rs. " + Convert.ToDecimal(avgOrder).ToString("N0");
        }

        private void LoadMonthlySummary()
        {
            object thisMonth = DBHelper.ExecuteScalar(
                @"SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders
                  WHERE MONTH(OrderDate) = MONTH(GETDATE()) AND YEAR(OrderDate) = YEAR(GETDATE())");
            litThisMonth.Text = "Rs. " + Convert.ToDecimal(thisMonth).ToString("N0");

            object lastMonth = DBHelper.ExecuteScalar(
                @"SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders
                  WHERE MONTH(OrderDate) = MONTH(DATEADD(MONTH, -1, GETDATE()))
                  AND YEAR(OrderDate) = YEAR(DATEADD(MONTH, -1, GETDATE()))");
            litLastMonth.Text = "Rs. " + Convert.ToDecimal(lastMonth).ToString("N0");
        }

        private void LoadRecentOrders()
        {
            DataTable dt = DBHelper.ExecuteQuery(
                "SELECT TOP 10 OrderID, OrderDate, TotalAmount, OrderStatus FROM Orders ORDER BY OrderDate DESC");
            gvRecentOrders.DataSource = dt;
            gvRecentOrders.DataBind();
        }

        private void LoadTopProducts()
        {
            DataTable dt = DBHelper.ExecuteQuery(
                @"SELECT TOP 5 p.ProductName, SUM(oi.Quantity) AS TotalQty,
                  SUM(oi.Quantity * oi.UnitPrice) AS TotalRevenue
                  FROM OrderItems oi
                  INNER JOIN Products p ON oi.ProductID = p.ProductID
                  GROUP BY p.ProductName
                  ORDER BY TotalQty DESC");
            gvTopProducts.DataSource = dt;
            gvTopProducts.DataBind();
        }
    }
}
