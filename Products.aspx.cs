using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadProducts();

                string search = Request.QueryString["search"];
                if (!string.IsNullOrEmpty(search))
                {
                    txtSearch.Text = search;
                }
            }
        }

        private void LoadCategories()
        {
            DataTable dt = DBHelper.ExecuteQuery("SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName");
            rptCategories.DataSource = dt;
            rptCategories.DataBind();
        }

        private void LoadProducts()
        {
            string search = Request.QueryString["search"];
            string catId = Request.QueryString["cat"];

            string query = "SELECT ProductID, ProductName, Description, Price, ImageURL FROM Products WHERE 1=1";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(search))
            {
                query += " AND (ProductName LIKE @Search OR Description LIKE @Search)";
                parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
                lblSearchInfo.Text = "Search results for: \"" + Server.HtmlEncode(search) + "\"";
                lblSearchInfo.Visible = true;
            }

            if (!string.IsNullOrEmpty(catId))
            {
                query += " AND CategoryID = @CategoryID";
                parameters.Add(new SqlParameter("@CategoryID", int.Parse(catId)));
            }

            query += " ORDER BY ProductID DESC";

            DataTable dt = DBHelper.ExecuteQuery(query, parameters.ToArray());

            if (dt.Rows.Count > 0)
            {
                dlProducts.DataSource = dt;
                dlProducts.DataBind();
                pnlNoProducts.Visible = false;
            }
            else
            {
                dlProducts.DataSource = null;
                dlProducts.DataBind();
                pnlNoProducts.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                Response.Redirect("~/Products.aspx?search=" + Server.UrlEncode(searchTerm));
            }
            else
            {
                Response.Redirect("~/Products.aspx");
            }
        }

        protected void dlProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "AddToCart")
            {
                int productId = int.Parse(e.CommandArgument.ToString());
                AddToCart(productId);
            }
        }

        private void AddToCart(int productId)
        {
            DataRow row = DBHelper.ExecuteSingleRow(
                "SELECT ProductID, ProductName, Price, ImageURL FROM Products WHERE ProductID = @ProductID",
                new SqlParameter("@ProductID", productId));

            if (row == null) return;

            List<CartItem> cart = Session["CartItems"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            CartItem existing = cart.Find(c => c.ProductID == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    ImageURL = row["ImageURL"].ToString(),
                    Quantity = 1
                });
            }

            Session["CartItems"] = cart;
            Session["CartCount"] = cart.Count;

            Response.Redirect(Request.RawUrl);
        }
    }
}
