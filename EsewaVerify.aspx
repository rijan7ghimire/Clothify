<%@ Page Title="Payment Verification" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="EsewaVerify.aspx.cs" Inherits="Clothify.EsewaVerify" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .verify-container { max-width: 500px; margin: 0 auto; padding: 40px 16px 100px; text-align: center; }
        .verify-card { background: #fff; border: 1px solid #eee; border-radius: 16px; padding: 32px 24px; }
        .verify-icon { font-size: 56px; margin-bottom: 16px; }
        .verify-icon.success { color: #60BB46; }
        .verify-icon.failed { color: #e74c3c; }
        .verify-title { font-size: 20px; font-weight: 700; color: #1a1a1a; margin-bottom: 8px; letter-spacing: 1px; }
        .verify-message { font-size: 14px; color: #666; margin-bottom: 24px; line-height: 1.5; }
        .verify-details { background: #f9f9f9; border-radius: 8px; padding: 16px; margin-bottom: 24px; text-align: left; }
        .verify-row { display: flex; justify-content: space-between; padding: 6px 0; font-size: 13px; }
        .verify-row .label { color: #888; }
        .verify-row .value { font-weight: 600; color: #1a1a1a; }
        .btn-primary { display: block; width: 100%; padding: 14px; font-size: 14px; font-weight: 700; color: #fff; background: #1a1a1a; border: none; border-radius: 10px; cursor: pointer; letter-spacing: 1px; text-transform: uppercase; text-decoration: none; text-align: center; }
        .btn-retry { display: block; width: 100%; padding: 14px; font-size: 14px; font-weight: 600; color: #e74c3c; background: none; border: 1px solid #e74c3c; border-radius: 10px; cursor: pointer; margin-top: 10px; text-decoration: none; text-align: center; }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="verify-container">
        <!-- Success Panel -->
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false">
            <div class="verify-card">
                <div class="verify-icon success">&#10004;</div>
                <div class="verify-title">PAYMENT SUCCESSFUL</div>
                <div class="verify-message">
                    Your eSewa payment has been verified and your order is confirmed!
                </div>
                <div class="verify-details">
                    <div class="verify-row">
                        <span class="label">Order Number</span>
                        <span class="value"><asp:Label ID="lblSuccessOrder" runat="server" /></span>
                    </div>
                    <div class="verify-row">
                        <span class="label">Amount Paid</span>
                        <span class="value"><asp:Label ID="lblSuccessAmount" runat="server" /></span>
                    </div>
                    <div class="verify-row">
                        <span class="label">Payment Method</span>
                        <span class="value" style="color:#60BB46;">eSewa</span>
                    </div>
                    <div class="verify-row">
                        <span class="label">Status</span>
                        <span class="value" style="color:#60BB46;">Paid</span>
                    </div>
                </div>
                <a href="Orders.aspx" class="btn-primary">VIEW MY ORDERS</a>
            </div>
        </asp:Panel>

        <!-- Failed Panel -->
        <asp:Panel ID="pnlFailed" runat="server" Visible="false">
            <div class="verify-card">
                <div class="verify-icon failed">&#10008;</div>
                <div class="verify-title">PAYMENT FAILED</div>
                <div class="verify-message">
                    <asp:Label ID="lblFailedMessage" runat="server" Text="Your eSewa payment could not be completed. Please try again." />
                </div>
                <a href="Orders.aspx" class="btn-primary">VIEW MY ORDERS</a>
                <a href="Checkout.aspx" class="btn-retry">TRY AGAIN</a>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
