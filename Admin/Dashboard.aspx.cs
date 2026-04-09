using System;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardStats();
            }
        }

        private void LoadDashboardStats()
        {
            // Total Orders
            object totalOrders = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Orders");
            litTotalOrders.Text = Convert.ToInt32(totalOrders).ToString();

            // Pending Orders (Placed or Processing)
            object pendingOrders = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Orders WHERE OrderStatus IN ('Placed', 'Processing')");
            litPendingOrders.Text = Convert.ToInt32(pendingOrders).ToString();

            // Delivered Orders
            object deliveredOrders = DBHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Orders WHERE OrderStatus = 'Delivered'");
            litDeliveredOrders.Text = Convert.ToInt32(deliveredOrders).ToString();

            // Total Users
            object totalUsers = DBHelper.ExecuteScalar("SELECT COUNT(*) FROM Users");
            litTotalUsers.Text = Convert.ToInt32(totalUsers).ToString();

            // Last Sync Time
            litLastSync.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
