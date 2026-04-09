using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Wishlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadWishlist();
            }
        }

        private void LoadWishlist()
        {
            List<int> wishlist = Session["Wishlist"] as List<int>;

            if (wishlist == null || wishlist.Count == 0)
            {
                pnlWishlist.Visible = false;
                pnlEmpty.Visible = true;
                return;
            }

            pnlWishlist.Visible = true;
            pnlEmpty.Visible = false;

            string ids = string.Join(",", wishlist);
            DataTable dt = DBHelper.ExecuteQuery(
                "SELECT ProductID, ProductName, Price, ImageURL FROM Products WHERE ProductID IN (" + ids + ")");

            if (dt.Rows.Count == 0)
            {
                pnlWishlist.Visible = false;
                pnlEmpty.Visible = true;
                return;
            }

            rptWishlist.DataSource = dt;
            rptWishlist.DataBind();

            lblWishlistCount.Text = dt.Rows.Count + " ITEM" + (dt.Rows.Count != 1 ? "S" : "") + " IN WISHLIST";
        }

        protected void rptWishlist_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = int.Parse(e.CommandArgument.ToString());
            List<int> wishlist = Session["Wishlist"] as List<int>;
            if (wishlist == null) wishlist = new List<int>();

            switch (e.CommandName)
            {
                case "AddToCart":
                    DataRow row = DBHelper.ExecuteSingleRow(
                        "SELECT ProductID, ProductName, Price, ImageURL FROM Products WHERE ProductID = @ProductID",
                        new SqlParameter("@ProductID", productId));

                    if (row != null)
                    {
                        List<CartItem> cart = Session["CartItems"] as List<CartItem>;
                        if (cart == null) cart = new List<CartItem>();

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
                    }

                    wishlist.Remove(productId);
                    Session["Wishlist"] = wishlist;
                    Response.Redirect("~/Cart.aspx");
                    break;

                case "Remove":
                    wishlist.Remove(productId);
                    Session["Wishlist"] = wishlist;
                    LoadWishlist();
                    break;
            }
        }
    }
}
