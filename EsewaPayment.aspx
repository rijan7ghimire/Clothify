<%@ Page Title="eSewa Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="EsewaPayment.aspx.cs" Inherits="Clothify.EsewaPayment" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .payment-container { max-width: 500px; margin: 0 auto; padding: 20px 16px 100px; }
        .payment-card { background: #fff; border: 1px solid #eee; border-radius: 16px; padding: 24px; margin-bottom: 20px; }
        .esewa-logo { text-align: center; margin-bottom: 20px; }
        .esewa-logo span { display: inline-block; background: #60BB46; color: #fff; font-size: 24px; font-weight: 800; padding: 12px 24px; border-radius: 12px; letter-spacing: 2px; }
        .payment-info { margin-bottom: 20px; }
        .payment-info-row { display: flex; justify-content: space-between; padding: 10px 0; font-size: 14px; border-bottom: 1px solid #f0f0f0; }
        .payment-info-row:last-child { border-bottom: none; }
        .payment-info-row .label { color: #888; }
        .payment-info-row .value { font-weight: 700; color: #1a1a1a; }
        .payment-info-row.total { border-top: 2px solid #1a1a1a; margin-top: 8px; padding-top: 14px; font-size: 16px; }
        .payment-info-row.total .value { color: #60BB46; }
        .esewa-note { background: #f0f9eb; border: 1px solid #60BB46; border-radius: 8px; padding: 12px 16px; margin-bottom: 20px; font-size: 13px; color: #2d7a0a; }
        .esewa-note strong { display: block; margin-bottom: 4px; }
        .btn-esewa { display: block; width: 100%; padding: 16px; font-size: 16px; font-weight: 700; color: #fff; background: #60BB46; border: none; border-radius: 10px; cursor: pointer; letter-spacing: 1px; text-transform: uppercase; transition: background 0.2s; }
        .btn-esewa:hover { background: #4da636; }
        .btn-cancel { display: block; width: 100%; padding: 14px; font-size: 14px; font-weight: 600; color: #888; background: none; border: 1px solid #ddd; border-radius: 10px; cursor: pointer; margin-top: 10px; text-align: center; text-decoration: none; letter-spacing: 0.5px; }
        .btn-cancel:hover { background: #f5f5f5; }
        .test-creds { background: #fff3e0; border: 1px solid #f57c00; border-radius: 8px; padding: 12px 16px; margin-bottom: 20px; font-size: 12px; color: #e65100; }
        .test-creds strong { display: block; margin-bottom: 4px; font-size: 13px; }
        .secure-badge { text-align: center; font-size: 12px; color: #999; margin-top: 16px; }
        .secure-badge span { color: #60BB46; }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="payment-container">
        <!-- Header -->
        <div class="page-header" style="display:flex; align-items:center; gap:12px; margin-bottom:24px;">
            <a href="javascript:history.back()" style="text-decoration:none; color:#1a1a1a; font-size:20px;">&#8592;</a>
            <h1 style="font-size:20px; font-weight:700; letter-spacing:1px;">ESEWA PAYMENT</h1>
        </div>

        <div class="payment-card">
            <!-- eSewa Logo -->
            <div class="esewa-logo">
                <span>eSewa</span>
            </div>

            <!-- Order Details -->
            <div class="payment-info">
                <div class="payment-info-row">
                    <span class="label">Order Number</span>
                    <span class="value"><asp:Label ID="lblOrderNumber" runat="server" /></span>
                </div>
                <div class="payment-info-row">
                    <span class="label">Subtotal</span>
                    <span class="value"><asp:Label ID="lblSubtotal" runat="server" /></span>
                </div>
                <div class="payment-info-row">
                    <span class="label">Delivery Fee</span>
                    <span class="value">Rs. 150</span>
                </div>
                <div class="payment-info-row">
                    <span class="label">Tax</span>
                    <span class="value">Rs. 0</span>
                </div>
                <div class="payment-info-row total">
                    <span class="label">Total Amount</span>
                    <span class="value"><asp:Label ID="lblTotalAmount" runat="server" /></span>
                </div>
            </div>

            <!-- Test Credentials -->
            <div class="test-creds">
                <strong>Test Mode - eSewa Sandbox</strong>
                eSewa ID: <strong>9806800001/2/3/4/5</strong><br />
                Password: <strong>Nepal@123</strong><br />
                MPIN: <strong>1122</strong> | Token: <strong>123456</strong>
            </div>

            <!-- eSewa Note -->
            <div class="esewa-note">
                <strong>Secure Payment via eSewa</strong>
                You will be redirected to eSewa's payment page to complete your transaction securely.
            </div>

            <!-- eSewa Payment Form -->
            <asp:Panel ID="pnlEsewaForm" runat="server">
                <div id="esewaFormContainer">
                    <asp:HiddenField ID="hfAmount" runat="server" />
                    <asp:HiddenField ID="hfTaxAmount" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTotalAmount" runat="server" />
                    <asp:HiddenField ID="hfTransactionUuid" runat="server" />
                    <asp:HiddenField ID="hfProductCode" runat="server" Value="EPAYTEST" />
                    <asp:HiddenField ID="hfSignedFieldNames" runat="server" />
                    <asp:HiddenField ID="hfSignature" runat="server" />

                    <asp:Button ID="btnPayWithEsewa" runat="server" Text="PAY WITH ESEWA" CssClass="btn-esewa"
                        OnClick="btnPayWithEsewa_Click" />
                </div>
            </asp:Panel>

            <a href="Checkout.aspx" class="btn-cancel">CANCEL & RETURN TO CHECKOUT</a>
        </div>

        <div class="secure-badge">
            <span>&#128274;</span> Secured by eSewa Payment Gateway
        </div>
    </div>
</asp:Content>
