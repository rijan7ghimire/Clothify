using System;
using System.Data;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify
{
    public partial class EditProfile : System.Web.UI.Page
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

            string query = "SELECT FullName, Email, Phone FROM Users WHERE UserID = @UserID";
            DataRow user = DBHelper.ExecuteSingleRow(query, new SqlParameter("@UserID", userId));

            if (user != null)
            {
                txtFullName.Text = user["FullName"].ToString();
                txtEmail.Text = user["Email"].ToString();
                txtPhone.Text = user["Phone"] != DBNull.Value ? user["Phone"].ToString() : "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phone))
            {
                ShowError("Full name and phone number are required.");
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                string updateQuery = "UPDATE Users SET FullName = @FullName, Phone = @Phone WHERE UserID = @UserID";
                DBHelper.ExecuteNonQuery(updateQuery,
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@UserID", userId)
                );

                // Update session
                Session["FullName"] = fullName;

                lblSuccess.Text = "Profile updated successfully.";
                lblSuccess.Visible = true;
                lblError.Visible = false;
            }
            catch (Exception ex)
            {
                ShowError("An error occurred while updating your profile. Please try again.");
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            lblSuccess.Visible = false;
        }
    }
}
