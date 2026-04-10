using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Clothify.App_Code
{
    public class EmailHelper
    {
        public static bool SendOrderConfirmation(string toEmail, string customerName, string orderNumber, decimal totalAmount)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser)) return false;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUser, "Clothify");
                mail.To.Add(toEmail);
                mail.Subject = "Order Confirmation - " + orderNumber;
                mail.IsBodyHtml = true;
                mail.Body = string.Format(@"
                <div style='font-family: Arial; max-width: 480px; margin: 0 auto; padding: 20px;'>
                    <h2 style='text-align: center;'>CLOTHIFY</h2>
                    <hr/>
                    <h3>Order Confirmed!</h3>
                    <p>Dear {0},</p>
                    <p>Thank you for your order. Your order <strong>{1}</strong> has been placed successfully.</p>
                    <p><strong>Total Amount: Rs. {2}</strong></p>
                    <p>You can track your order status in the Orders section of your account.</p>
                    <hr/>
                    <p style='color: #888; font-size: 12px; text-align: center;'>Clothify - Fashion for Nepal</p>
                </div>", customerName, orderNumber, totalAmount.ToString("N0"));

                SmtpClient smtp = new SmtpClient(smtpHost, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendOrderStatusUpdate(string toEmail, string customerName, string orderNumber, string newStatus)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "";
                string smtpPass = ConfigurationManager.AppSettings["SmtpPass"] ?? "";

                if (string.IsNullOrEmpty(smtpUser)) return false;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(smtpUser, "Clothify");
                mail.To.Add(toEmail);
                mail.Subject = "Order Update - " + orderNumber;
                mail.IsBodyHtml = true;
                mail.Body = string.Format(@"
                <div style='font-family: Arial; max-width: 480px; margin: 0 auto; padding: 20px;'>
                    <h2 style='text-align: center;'>CLOTHIFY</h2>
                    <hr/>
                    <h3>Order Status Updated</h3>
                    <p>Dear {0},</p>
                    <p>Your order <strong>{1}</strong> status has been updated to: <strong>{2}</strong></p>
                    <p>You can view the full details in your account.</p>
                    <hr/>
                    <p style='color: #888; font-size: 12px; text-align: center;'>Clothify - Fashion for Nepal</p>
                </div>", customerName, orderNumber, newStatus);

                SmtpClient smtp = new SmtpClient(smtpHost, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}