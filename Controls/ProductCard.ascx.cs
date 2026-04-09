using System;
using System.Web.UI;

namespace Clothify.Controls
{
    public partial class ProductCardControl : UserControl
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }

        public event EventHandler AddToCartClicked;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (AddToCartClicked != null)
                AddToCartClicked(this, EventArgs.Empty);
        }
    }
}
