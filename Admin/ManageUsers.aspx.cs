using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers(string search = "")
        {
            string query;
            DataTable dt;

            if (!string.IsNullOrEmpty(search))
            {
                query = @"SELECT u.UserID, u.FullName, u.Email, u.PhoneNumber, u.RoleID, u.CreatedAt, r.RoleName
                          FROM Users u INNER JOIN Roles r ON u.RoleID = r.RoleID
                          WHERE u.FullName LIKE @Search OR u.Email LIKE @Search OR u.PhoneNumber LIKE @Search
                          ORDER BY u.CreatedAt DESC";
                dt = DBHelper.ExecuteQuery(query, new SqlParameter("@Search", "%" + search + "%"));
            }
            else
            {
                query = @"SELECT u.UserID, u.FullName, u.Email, u.PhoneNumber, u.RoleID, u.CreatedAt, r.RoleName
                          FROM Users u INNER JOIN Roles r ON u.RoleID = r.RoleID
                          ORDER BY u.CreatedAt DESC";
                dt = DBHelper.ExecuteQuery(query);
            }

            gvUsers.DataSource = dt;
            gvUsers.DataBind();

            // Set selected role for each row in edit mode
            foreach (GridViewRow row in gvUsers.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow && (row.RowState & DataControlRowState.Edit) != 0)
                {
                    DropDownList ddl = (DropDownList)row.FindControl("ddlRole");
                    if (ddl != null)
                    {
                        string roleId = dt.Rows[row.DataItemIndex]["RoleID"].ToString();
                        if (ddl.Items.FindByValue(roleId) != null)
                            ddl.SelectedValue = roleId;
                    }
                }
            }
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            string fullName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string phone = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            DropDownList ddlRole = (DropDownList)row.FindControl("ddlRole");
            int newRoleId = Convert.ToInt32(ddlRole.SelectedValue);

            string query = @"UPDATE Users SET FullName = @FullName, Email = @Email, PhoneNumber = @Phone, RoleID = @RoleID WHERE UserID = @UserID";
            DBHelper.ExecuteNonQuery(query,
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@Email", email),
                new SqlParameter("@Phone", phone),
                new SqlParameter("@RoleID", newRoleId),
                new SqlParameter("@UserID", userId));

            gvUsers.EditIndex = -1;
            ShowMessage("User updated successfully.", true);
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            // Prevent deleting own account
            if (Session["UserID"] != null && Convert.ToInt32(Session["UserID"]) == userId)
            {
                ShowMessage("You cannot delete your own account.", false);
                return;
            }

            string query = "DELETE FROM Users WHERE UserID = @UserID";
            DBHelper.ExecuteNonQuery(query, new SqlParameter("@UserID", userId));

            ShowMessage("User deleted successfully.", true);
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadUsers();
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            divMessage.InnerText = message;
            divMessage.Attributes["class"] = isSuccess ? "alert alert-success" : "alert alert-danger";
        }
    }
}
