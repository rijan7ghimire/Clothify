using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using Clothify.App_Code;

namespace Clothify
{
    public partial class ProductDetail : System.Web.UI.Page
    {
        private int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            string idParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out productId))
            {
                pnlProduct.Visible = false;
                pnlNotFound.Visible = true;
                return;
            }

            if (!IsPostBack)
            {
                LoadProduct();
                LoadProductDetailView();
                LoadReviews();
            }
        }

        private void LoadProduct()
        {
            DataRow row = DBHelper.ExecuteSingleRow(
                "SELECT ProductID, ProductName, Description, Price, ImageURL FROM Products WHERE ProductID = @ProductID",
                new SqlParameter("@ProductID", productId));

            if (row == null)
            {
                pnlProduct.Visible = false;
                pnlNotFound.Visible = true;
                return;
            }

            lblProductName.Text = row["ProductName"].ToString();
            lblPrice.Text = "Rs. " + string.Format("{0:N0}", row["Price"]);
            lblDescription.Text = row["Description"].ToString();
            imgProduct.ImageUrl = ResolveUrl("~" + row["ImageURL"].ToString());
            imgProduct.AlternateText = row["ProductName"].ToString();

            Page.Title = row["ProductName"].ToString() + " - Clothify";
        }

        private void LoadProductDetailView()
        {
            DataTable dt = DBHelper.ExecuteQuery(
                @"SELECT p.ProductName, p.Description, p.Price, p.StockQuantity,
                         ISNULL(c.CategoryName, 'N/A') AS CategoryName
                  FROM Products p
                  LEFT JOIN Categories c ON p.CategoryID = c.CategoryID
                  WHERE p.ProductID = @ProductID",
                new SqlParameter("@ProductID", productId));

            if (dt.Rows.Count > 0)
            {
                dvProduct.DataSource = dt;
                dvProduct.DataBind();
            }
        }

        private void LoadReviews()
        {
            DataTable dt = DBHelper.ExecuteQuery(
                @"SELECT f.Rating, f.Comment, f.FeedbackDate, u.FullName
                  FROM Feedback f
                  INNER JOIN Users u ON f.UserID = u.UserID
                  WHERE f.ProductID = @ProductID
                  ORDER BY f.FeedbackDate DESC",
                new SqlParameter("@ProductID", productId));

            if (dt.Rows.Count > 0)
            {
                rptReviews.DataSource = dt;
                rptReviews.DataBind();
                pnlNoReviews.Visible = false;

                // Calculate average rating
                object avgResult = DBHelper.ExecuteScalar(
                    "SELECT AVG(CAST(Rating AS FLOAT)) FROM Feedback WHERE ProductID = @ProductID",
                    new SqlParameter("@ProductID", productId));

                if (avgResult != null && avgResult != DBNull.Value)
                {
                    double avgRating = Convert.ToDouble(avgResult);
                    lblAvgRating.Text = string.Format("{0:F1} out of 5 ({1} review{2})",
                        avgRating, dt.Rows.Count, dt.Rows.Count > 1 ? "s" : "");
                }
            }
            else
            {
                rptReviews.DataSource = null;
                rptReviews.DataBind();
                pnlNoReviews.Visible = true;
                lblAvgRating.Visible = false;
            }
        }

        protected string GetStars(int rating)
        {
            string stars = "";
            for (int i = 0; i < 5; i++)
            {
                if (i < rating)
                    stars += "<span class=\"star filled\">&#9733;</span>";
                else
                    stars += "<span class=\"star\">&#9733;</span>";
            }
            return stars;
        }

        protected void btnDecrease_Click(object sender, EventArgs e)
        {
            int qty = int.Parse(txtQuantity.Text);
            if (qty > 1)
            {
                txtQuantity.Text = (qty - 1).ToString();
            }
        }

        protected void btnIncrease_Click(object sender, EventArgs e)
        {
            int qty = int.Parse(txtQuantity.Text);
            txtQuantity.Text = (qty + 1).ToString();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            string idParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out productId))
                return;

            int quantity = int.Parse(txtQuantity.Text);

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
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    ImageURL = row["ImageURL"].ToString(),
                    Quantity = quantity
                });
            }

            Session["CartItems"] = cart;
            Session["CartCount"] = cart.Count;

            Response.Redirect("~/Cart.aspx");
        }

        protected void btnAddToWishlist_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int prodId;
            string idParam = Request.QueryString["id"];
            if (string.IsNullOrEmpty(idParam) || !int.TryParse(idParam, out prodId))
                return;

            List<int> wishlist = Session["Wishlist"] as List<int>;
            if (wishlist == null)
                wishlist = new List<int>();

            if (!wishlist.Contains(prodId))
                wishlist.Add(prodId);

            Session["Wishlist"] = wishlist;
            Response.Redirect("~/Wishlist.aspx");
        }
    }
}
