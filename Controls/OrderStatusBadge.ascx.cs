using System;
using System.Web.UI;

namespace Clothify.Controls
{
    public partial class OrderStatusBadgeControl : UserControl
    {
        public string Status { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }
    }
}
