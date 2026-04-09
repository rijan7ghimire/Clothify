using System;
using System.Web.UI;

namespace Clothify
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CartCount"] != null)
            {
                lblCartCount.Text = Session["CartCount"].ToString();
            }
            else
            {
                lblCartCount.Text = "0";
            }

            // Hide cart badge if count is zero
            if (lblCartCount.Text == "0")
            {
                lblCartCount.Visible = false;
            }
            else
            {
                lblCartCount.Visible = true;
            }
        }
    }
}
