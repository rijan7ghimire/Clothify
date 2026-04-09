using System;
using System.Data;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmNewPassword = txtConfirmNewPassword.Text;

            // Server-side validation
            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                ShowError("All fields are required.");
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                ShowError("New passwords do not match.");
                return;
            }

            if (newPassword.Length < 6)
            {
                ShowError("New password must be at least 6 characters long.");
                return;
            }

            try
            {
                int userId = Convert.ToInt32(Session["UserID"]);

                // Get current password hash
                string query = "SELECT PasswordHash FROM Users WHERE UserID = @UserID";
                DataRow user = DBHelper.ExecuteSingleRow(query, new SqlParameter("@UserID", userId));

                if (user == null)
                {
                    ShowError("User not found.");
                    return;
                }

                string storedHash = user["PasswordHash"].ToString();

                // Verify current password
                if (!PasswordHelper.VerifyPassword(currentPassword, storedHash))
                {
                    ShowError("Current password is incorrect.");
                    return;
                }

                // Hash new password and update
                string newHash = PasswordHelper.HashPassword(newPassword);

                string updateQuery = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";
                DBHelper.ExecuteNonQuery(updateQuery,
                    new SqlParameter("@PasswordHash", newHash),
                    new SqlParameter("@UserID", userId)
                );

                lblSuccess.Text = "Password changed successfully.";
                lblSuccess.Visible = true;
                lblError.Visible = false;

                // Clear the password fields
                txtCurrentPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmNewPassword.Text = "";
            }
            catch (Exception ex)
            {
                ShowError("An error occurred while changing your password. Please try again.");
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
