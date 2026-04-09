using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class ManageProducts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();
            }
        }

        private void LoadCategories()
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName");

            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", "0"));

            ddlEditCategory.DataSource = dt;
            ddlEditCategory.DataTextField = "CategoryName";
            ddlEditCategory.DataValueField = "CategoryID";
            ddlEditCategory.DataBind();
            ddlEditCategory.Items.Insert(0, new ListItem("-- Select Category --", "0"));
        }

        private void LoadProducts()
        {
            string query = @"SELECT p.ProductID, p.ProductName, p.Description, p.Price, p.StockQuantity,
                             p.ImageURL, p.CategoryID, c.CategoryName
                             FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                             ORDER BY p.ProductID DESC";
            DataTable dt = DBHelper.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                rptProducts.DataSource = dt;
                rptProducts.DataBind();
                pnlNoData.Visible = false;
            }
            else
            {
                rptProducts.DataSource = null;
                rptProducts.DataBind();
                pnlNoData.Visible = true;
            }
        }

        protected void btnToggleAdd_Click(object sender, EventArgs e)
        {
            pnlAddProduct.Visible = !pnlAddProduct.Visible;
            pnlEditProduct.Visible = false;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAddProduct.Visible = false;
            ClearAddForm();
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtProductName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string stockText = txtStock.Text.Trim();
            string imageUrl = txtImageURL.Text.Trim();
            string categoryId = ddlCategory.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText) || string.IsNullOrEmpty(stockText) || categoryId == "0")
            {
                ShowMessage("Please fill in all required fields (Name, Price, Stock, Category).", false);
                return;
            }

            decimal price;
            int stock;
            if (!decimal.TryParse(priceText, out price) || !int.TryParse(stockText, out stock))
            {
                ShowMessage("Please enter valid Price and Stock values.", false);
                return;
            }

            string query = @"INSERT INTO Products (ProductName, Description, Price, StockQuantity, ImageURL, CategoryID)
                             VALUES (@ProductName, @Description, @Price, @StockQuantity, @ImageURL, @CategoryID)";
            DBHelper.ExecuteNonQuery(query,
                new SqlParameter("@ProductName", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@Price", price),
                new SqlParameter("@StockQuantity", stock),
                new SqlParameter("@ImageURL", imageUrl),
                new SqlParameter("@CategoryID", Convert.ToInt32(categoryId)));

            ShowMessage("Product added successfully.", true);
            ClearAddForm();
            pnlAddProduct.Visible = false;
            LoadProducts();
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditProduct")
            {
                DataRow row = DBHelper.ExecuteSingleRow(
                    "SELECT * FROM Products WHERE ProductID = @ProductID",
                    new SqlParameter("@ProductID", productId));

                if (row != null)
                {
                    hfEditProductID.Value = productId.ToString();
                    txtEditProductName.Text = row["ProductName"].ToString();
                    txtEditDescription.Text = row["Description"].ToString();
                    txtEditPrice.Text = Convert.ToDecimal(row["Price"]).ToString("F2");
                    txtEditStock.Text = row["StockQuantity"].ToString();
                    txtEditImageURL.Text = row["ImageURL"].ToString();

                    if (ddlEditCategory.Items.FindByValue(row["CategoryID"].ToString()) != null)
                        ddlEditCategory.SelectedValue = row["CategoryID"].ToString();

                    pnlEditProduct.Visible = true;
                    pnlAddProduct.Visible = false;
                }
            }
            else if (e.CommandName == "DeleteProduct")
            {
                DBHelper.ExecuteNonQuery("DELETE FROM Products WHERE ProductID = @ProductID",
                    new SqlParameter("@ProductID", productId));

                ShowMessage("Product deleted successfully.", true);
                LoadProducts();
            }
        }

        protected void btnSaveEdit_Click(object sender, EventArgs e)
        {
            int productId = Convert.ToInt32(hfEditProductID.Value);
            string name = txtEditProductName.Text.Trim();
            string description = txtEditDescription.Text.Trim();
            string priceText = txtEditPrice.Text.Trim();
            string stockText = txtEditStock.Text.Trim();
            string imageUrl = txtEditImageURL.Text.Trim();
            string categoryId = ddlEditCategory.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(priceText) || string.IsNullOrEmpty(stockText) || categoryId == "0")
            {
                ShowMessage("Please fill in all required fields.", false);
                return;
            }

            decimal price;
            int stock;
            if (!decimal.TryParse(priceText, out price) || !int.TryParse(stockText, out stock))
            {
                ShowMessage("Please enter valid Price and Stock values.", false);
                return;
            }

            string query = @"UPDATE Products SET ProductName=@ProductName, Description=@Description,
                             Price=@Price, StockQuantity=@StockQuantity, ImageURL=@ImageURL, CategoryID=@CategoryID
                             WHERE ProductID=@ProductID";
            DBHelper.ExecuteNonQuery(query,
                new SqlParameter("@ProductName", name),
                new SqlParameter("@Description", description),
                new SqlParameter("@Price", price),
                new SqlParameter("@StockQuantity", stock),
                new SqlParameter("@ImageURL", imageUrl),
                new SqlParameter("@CategoryID", Convert.ToInt32(categoryId)),
                new SqlParameter("@ProductID", productId));

            ShowMessage("Product updated successfully.", true);
            pnlEditProduct.Visible = false;
            LoadProducts();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            pnlEditProduct.Visible = false;
        }

        private void ClearAddForm()
        {
            txtProductName.Text = "";
            txtDescription.Text = "";
            txtPrice.Text = "";
            txtStock.Text = "";
            txtImageURL.Text = "";
            ddlCategory.SelectedIndex = 0;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            divMessage.InnerText = message;
            divMessage.Attributes["class"] = isSuccess ? "alert alert-success" : "alert alert-danger";
        }
    }
}
