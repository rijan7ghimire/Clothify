using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string emailOrPhone = txtEmailOrPhone.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(emailOrPhone) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter your email/phone and password.");
                return;
            }

            try
            {
                string query = "SELECT UserID, FullName, Email, Phone, PasswordHash, RoleID FROM Users WHERE Email = @Input OR Phone = @Input";
                DataRow user = DBHelper.ExecuteSingleRow(query, new SqlParameter("@Input", emailOrPhone));

                if (user == null)
                {
                    ShowError("Invalid email/phone or password.");
                    return;
                }

                string storedHash = user["PasswordHash"].ToString();

                if (!PasswordHelper.VerifyPassword(password, storedHash))
                {
                    ShowError("Invalid email/phone or password.");
                    return;
                }

                int userId = Convert.ToInt32(user["UserID"]);
                string fullName = user["FullName"].ToString();
                string email = user["Email"].ToString();
                int roleId = Convert.ToInt32(user["RoleID"]);
                string roleName = roleId == 1 ? "Admin" : "Customer";

                // Create authentication ticket with role in UserData
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                          // version
                    email,                      // name
                    DateTime.Now,               // issueDate
                    DateTime.Now.AddMinutes(30),// expiration
                    false,                      // isPersistent
                    roleName,                   // userData (role)
                    FormsAuthentication.FormsCookiePath
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);

                // Set session variables
                Session["UserID"] = userId;
                Session["FullName"] = fullName;
                Session["Email"] = email;
                Session["RoleID"] = roleId;

                // Redirect based on role
                if (roleId == 1)
                {
                    Response.Redirect("~/Admin/Dashboard.aspx");
                }
                else
                {
                    Response.Redirect("~/Default.aspx");
                }
            }
            catch (Exception ex)
            {
                ShowError("An error occurred. Please try again.");
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}
