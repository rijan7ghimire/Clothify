using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class ManageCategories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT CategoryID, CategoryName, Description FROM Categories ORDER BY CategoryID DESC");

            if (dt.Rows.Count > 0)
            {
                rptCategories.DataSource = dt;
                rptCategories.DataBind();
                pnlNoData.Visible = false;
            }
            else
            {
                rptCategories.DataSource = null;
                rptCategories.DataBind();
                pnlNoData.Visible = true;
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text.Trim();
            string description = txtCategoryDesc.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowMessage("Please enter a category name.", false);
                return;
            }

            string query = "INSERT INTO Categories (CategoryName, Description) VALUES (@CategoryName, @Description)";
            DBHelper.ExecuteNonQuery(query,
                new SqlParameter("@CategoryName", name),
                new SqlParameter("@Description", description));

            ShowMessage("Category added successfully.", true);
            txtCategoryName.Text = "";
            txtCategoryDesc.Text = "";
            LoadCategories();
        }

        protected void rptCategories_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int categoryId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditCategory")
            {
                DataRow row = DBHelper.ExecuteSingleRow(
                    "SELECT * FROM Categories WHERE CategoryID = @CategoryID",
                    new SqlParameter("@CategoryID", categoryId));

                if (row != null)
                {
                    hfEditCategoryID.Value = categoryId.ToString();
                    txtEditCategoryName.Text = row["CategoryName"].ToString();
                    txtEditCategoryDesc.Text = row["Description"].ToString();
                    pnlEditCategory.Visible = true;
                }
            }
            else if (e.CommandName == "DeleteCategory")
            {
                // Check if category has products
                object count = DBHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID",
                    new SqlParameter("@CategoryID", categoryId));

                if (Convert.ToInt32(count) > 0)
                {
                    ShowMessage("Cannot delete category. It has associated products.", false);
                    return;
                }

                DBHelper.ExecuteNonQuery("DELETE FROM Categories WHERE CategoryID = @CategoryID",
                    new SqlParameter("@CategoryID", categoryId));

                ShowMessage("Category deleted successfully.", true);
                LoadCategories();
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int categoryId = Convert.ToInt32(hfEditCategoryID.Value);
            string name = txtEditCategoryName.Text.Trim();
            string description = txtEditCategoryDesc.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ShowMessage("Please enter a category name.", false);
                return;
            }

            string query = "UPDATE Categories SET CategoryName=@CategoryName, Description=@Description WHERE CategoryID=@CategoryID";
            DBHelper.ExecuteNonQuery(query,
                new SqlParameter("@CategoryName", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@CategoryID", categoryId));

            ShowMessage("Category updated successfully.", true);
            pnlEditCategory.Visible = false;
            LoadCategories();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditCategory.Visible = false;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            divMessage.InnerText = message;
            divMessage.Attributes["class"] = isSuccess ? "alert alert-success" : "alert alert-danger";
        }
    }
}
