<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="Clothify.Notifications" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .notifications-container { max-width: 500px; margin: 0 auto; padding: 20px 16px 100px; }
        .notif-card { background: #fff; border: 1px solid #eee; border-radius: 12px; padding: 16px; margin-bottom: 12px; transition: box-shadow 0.2s; }
        .notif-card:hover { box-shadow: 0 2px 8px rgba(0,0,0,0.06); }
        .notif-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 6px; }
        .notif-order { font-size: 15px; font-weight: 700; color: #1a1a1a; }
        .notif-date { font-size: 12px; color: #999; }
        .notif-status { display: inline-block; padding: 4px 10px; border-radius: 20px; font-size: 10px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; }
        .notif-message { font-size: 13px; color: #555; margin-top: 8px; }
        .status-placed { background: #e8f0fe; color: #1a73e8; }
        .status-processing { background: #fff3e0; color: #f57c00; }
        .status-shipped { background: #e0f2f1; color: #00897b; }
        .status-delivered { background: #e8f5e9; color: #2e7d32; }
        .status-cancelled { background: #fce4ec; color: #c62828; }
        .empty-state { text-align: center; padding: 60px 20px; color: #999; }
        .empty-state-icon { font-size: 48px; margin-bottom: 16px; }
        .empty-state-title { font-size: 16px; font-weight: 600; color: #555; margin-bottom: 8px; }
        .empty-state-text { font-size: 14px; color: #999; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="notifications-container">
        <div class="page-header">
            <a href="javascript:history.back()" class="btn-back">&lt; NOTIFICATIONS</a>
        </div>

        <asp:Panel ID="pnlNotifications" runat="server">
            <asp:Repeater ID="rptNotifications" runat="server">
                <ItemTemplate>
                    <div class="notif-card">
                        <div class="notif-header">
                            <span class="notif-order">ORDER #CN-<%# Eval("OrderID", "{0:D5}") %></span>
                            <span class="notif-date"><%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %></span>
                        </div>
                        <span class='notif-status status-<%# Eval("OrderStatus").ToString().ToLower() %>'>
                            <%# Eval("OrderStatus") %>
                        </span>
                        <div class="notif-message">
                            <%# GetStatusMessage(Eval("OrderStatus").ToString(), Eval("OrderID").ToString()) %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>

        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
            <div class="empty-state-icon">&#128276;</div>
            <div class="empty-state-title">No notifications</div>
            <div class="empty-state-text">Your order updates will appear here.</div>
        </asp:Panel>
    </div>
</asp:Content>
