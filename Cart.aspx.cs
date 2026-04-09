using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Cart : System.Web.UI.Page
    {
        private const decimal DELIVERY_FEE = 150.00m;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            List<CartItem> cart = Session["CartItems"] as List<CartItem>;

            if (cart == null || cart.Count == 0)
            {
                pnlCart.Visible = false;
                pnlEmptyCart.Visible = true;
                return;
            }

            pnlCart.Visible = true;
            pnlEmptyCart.Visible = false;

            // Bind cart items
            rptCartItems.DataSource = cart;
            rptCartItems.DataBind();

            // Item count
            int totalItems = cart.Sum(c => c.Quantity);
            lblItemCount.Text = totalItems + " ITEM" + (totalItems != 1 ? "S" : "") + " IN CART";

            // Calculate totals
            decimal subtotal = cart.Sum(c => c.Total);
            decimal total = subtotal + DELIVERY_FEE;

            lblSubtotal.Text = "Rs. " + string.Format("{0:N0}", subtotal);
            lblDeliveryFee.Text = "Rs. " + string.Format("{0:N0}", DELIVERY_FEE);
            lblTotal.Text = "Rs. " + string.Format("{0:N0}", total);
        }

        protected void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int productId = int.Parse(e.CommandArgument.ToString());
            List<CartItem> cart = Session["CartItems"] as List<CartItem>;

            if (cart == null) return;

            CartItem item = cart.Find(c => c.ProductID == productId);
            if (item == null) return;

            switch (e.CommandName)
            {
                case "Increase":
                    item.Quantity++;
                    break;

                case "Decrease":
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                    }
                    else
                    {
                        cart.Remove(item);
                    }
                    break;

                case "Remove":
                    cart.Remove(item);
                    break;
            }

            Session["CartItems"] = cart;
            Session["CartCount"] = cart.Count;

            LoadCart();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Checkout.aspx");
        }
    }
}
