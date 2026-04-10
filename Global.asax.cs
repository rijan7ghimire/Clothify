using System;
using System.Web;
using System.Web.Security;

namespace Clothify
{
    public partial class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Application startup
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = identity.Ticket;
                string role = ticket.UserData;
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(identity, new string[] { role });
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Error handling
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Session started
        }
    }
}
