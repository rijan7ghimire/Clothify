using System;
using System.Web.UI;

namespace Clothify.Controls
{
    public partial class NotificationBarControl : UserControl
    {
        public string Message { get; set; }
        private string _messageType = "info";
        public string MessageType { get { return _messageType; } set { _messageType = value; } }

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
