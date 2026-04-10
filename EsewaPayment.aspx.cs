using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Clothify.App_Code;

namespace Clothify
{
    public partial class EsewaPayment : System.Web.UI.Page
    {
        // eSewa Sandbox credentials
        private const string ESEWA_MERCHANT_CODE = "EPAYTEST";
        private const string ESEWA_SECRET_KEY = "8gBm/:&EnhH.1/q";
        private const string ESEWA_PAYMENT_URL = "https://rc-epay.esewa.com.np/api/epay/main/v2/form";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Check if we have a pending order
                if (Session["PendingOrderID"] == null || Session["PendingOrderTotal"] == null)
                {
                    Response.Redirect("~/Checkout.aspx");
                    return;
                }

                int orderId = Convert.ToInt32(Session["PendingOrderID"]);
                decimal totalAmount = Convert.ToDecimal(Session["PendingOrderTotal"]);
                decimal subtotal = totalAmount - 150m;

                string orderNumber = "CN-" + orderId.ToString("D5");
                string transactionUuid = orderNumber + "-" + DateTime.Now.Ticks;

                lblOrderNumber.Text = orderNumber;
                lblSubtotal.Text = "Rs. " + subtotal.ToString("N0");
                lblTotalAmount.Text = "Rs. " + totalAmount.ToString("N0");

                hfAmount.Value = totalAmount.ToString("F2");
                hfTaxAmount.Value = "0";
                hfTotalAmount.Value = totalAmount.ToString("F2");
                hfTransactionUuid.Value = transactionUuid;
                hfProductCode.Value = ESEWA_MERCHANT_CODE;

                // Generate signature
                string signedFieldNames = "total_amount,transaction_uuid,product_code";
                string signData = "total_amount=" + totalAmount.ToString("F2")
                    + ",transaction_uuid=" + transactionUuid
                    + ",product_code=" + ESEWA_MERCHANT_CODE;

                string signature = GenerateSignature(signData, ESEWA_SECRET_KEY);
                hfSignedFieldNames.Value = signedFieldNames;
                hfSignature.Value = signature;

                // Store transaction UUID in session for verification
                Session["EsewaTransactionUuid"] = transactionUuid;
            }
        }

        protected void btnPayWithEsewa_Click(object sender, EventArgs e)
        {
            string amount = hfAmount.Value;
            string taxAmount = hfTaxAmount.Value;
            string totalAmount = hfTotalAmount.Value;
            string transactionUuid = hfTransactionUuid.Value;
            string productCode = hfProductCode.Value;
            string signature = hfSignature.Value;
            string signedFieldNames = hfSignedFieldNames.Value;

            string successUrl = Request.Url.GetLeftPart(UriPartial.Authority)
                + ResolveUrl("~/EsewaVerify.aspx");
            string failureUrl = Request.Url.GetLeftPart(UriPartial.Authority)
                + ResolveUrl("~/EsewaVerify.aspx?status=failed");

            // Build auto-submit HTML form
            string html = "<html><body onload='document.forms[0].submit()'>";
            html += "<form method='POST' action='" + ESEWA_PAYMENT_URL + "'>";
            html += "<input type='hidden' name='amount' value='" + amount + "' />";
            html += "<input type='hidden' name='tax_amount' value='" + taxAmount + "' />";
            html += "<input type='hidden' name='total_amount' value='" + totalAmount + "' />";
            html += "<input type='hidden' name='transaction_uuid' value='" + transactionUuid + "' />";
            html += "<input type='hidden' name='product_code' value='" + productCode + "' />";
            html += "<input type='hidden' name='product_service_charge' value='0' />";
            html += "<input type='hidden' name='product_delivery_charge' value='0' />";
            html += "<input type='hidden' name='success_url' value='" + successUrl + "' />";
            html += "<input type='hidden' name='failure_url' value='" + failureUrl + "' />";
            html += "<input type='hidden' name='signed_field_names' value='" + signedFieldNames + "' />";
            html += "<input type='hidden' name='signature' value='" + signature + "' />";
            html += "<p style='text-align:center; font-family:Arial; margin-top:50px;'>Redirecting to eSewa...</p>";
            html += "</form></body></html>";

            Response.Clear();
            Response.Write(html);
            Response.End();
        }

        private string GenerateSignature(string data, string secretKey)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(dataBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
