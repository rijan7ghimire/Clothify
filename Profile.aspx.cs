using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);

            string query = "SELECT FullName, Email, PhoneNumber FROM Users WHERE UserID = @UserID";
            DataRow user = DBHelper.ExecuteSingleRow(query, new SqlParameter("@UserID", userId));

            if (user != null)
            {
                lblFullName.Text = user["FullName"].ToString();
                lblEmail.Text = user["Email"].ToString();
                lblPhone.Text = user["PhoneNumber"] != DBNull.Value ? user["PhoneNumber"].ToString() : "";
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
