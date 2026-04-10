using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            // Server-side validation
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password))
            {
                ShowError("All fields are required.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowError("Passwords do not match.");
                return;
            }

            try
            {
                // Check if email already exists
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                object result = DBHelper.ExecuteScalar(checkQuery, new SqlParameter("@Email", email));
                int count = Convert.ToInt32(result);

                if (count > 0)
                {
                    ShowError("An account with this email already exists.");
                    return;
                }

                // Hash password
                string hashedPassword = PasswordHelper.HashPassword(password);

                // Insert new user with RoleID=2 (Customer)
                string insertQuery = @"INSERT INTO Users (FullName, Email, PhoneNumber, PasswordHash, RoleID, CreatedAt)
                                       VALUES (@FullName, @Email, @PhoneNumber, @PasswordHash, 2, GETDATE());
                                       SELECT SCOPE_IDENTITY();";

                object newIdObj = DBHelper.ExecuteScalar(insertQuery,
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@PhoneNumber", phone),
                    new SqlParameter("@PasswordHash", hashedPassword)
                );

                int userId = Convert.ToInt32(newIdObj);

                // Auto-login after registration
                string roleName = "Customer";

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    email,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    roleName,
                    FormsAuthentication.FormsCookiePath
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                // Set session variables
                Session["UserID"] = userId;
                Session["FullName"] = fullName;
                Session["Email"] = email;
                Session["RoleID"] = 2;

                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                ShowError("An error occurred during registration. Please try again.");
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
