using System;
using System.Web.UI;

namespace Clothify.Controls
{
    public partial class NotificationBarControl : UserControl
    {
        public string Message { get; set; }
        public string MessageType { get; set; } = "info";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                pnlNotification.Visible = true;
                lblMessage.Text = Message;
                DataBind();
            }
        }

        public void Show(string message, string type = "info")
        {
            Message = message;
            MessageType = type;
            pnlNotification.Visible = true;
            lblMessage.Text = message;
            DataBind();
        }
    }
}
