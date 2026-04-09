using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify
{
    public partial class _Default : System.Web.UI.Page
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
            rptCategories.DataSource = dt;
            rptCategories.DataBind();
        }

        private void LoadProducts()
        {
            string query;
            string catId = Request.QueryString["cat"];

            if (!string.IsNullOrEmpty(catId))
            {
                query = "SELECT ProductID, ProductName, Description, Price, ImageURL FROM Products WHERE CategoryID = @CategoryID ORDER BY ProductID DESC";
                DataTable dt = DBHelper.ExecuteQuery(query, new SqlParameter("@CategoryID", int.Parse(catId)));
                BindProducts(dt);
            }
            else
            {
                query = "SELECT ProductID, ProductName, Description, Price, ImageURL FROM Products ORDER BY ProductID DESC";
                DataTable dt = DBHelper.ExecuteQuery(query);
                BindProducts(dt);
            }
        }

        private void BindProducts(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                rptProducts.DataSource = dt;
                rptProducts.DataBind();
                pnlNoProducts.Visible = false;
            }
            else
            {
                rptProducts.DataSource = null;
                rptProducts.DataBind();
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
        }

        protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
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

            // Refresh the page to reflect changes
            Response.Redirect(Request.RawUrl);
        }
    }
}
