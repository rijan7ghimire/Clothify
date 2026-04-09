<%@ Page Title="Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="Clothify.Orders" %>
<%@ Register Src="~/Controls/OrderStatusBadge.ascx" TagPrefix="uc" TagName="OrderStatusBadge" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .orders-container {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px 16px 100px;
        }

        .page-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 20px;
        }

        .page-header-left {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .back-arrow {
            text-decoration: none;
            color: #1a1a1a;
            font-size: 20px;
            display: flex;
            align-items: center;
        }

        .page-title {
            font-size: 20px;
            font-weight: 700;
            color: #1a1a1a;
            letter-spacing: 1px;
        }

        .filter-tabs {
            display: flex;
            gap: 0;
            margin-bottom: 24px;
            border-bottom: 1px solid #eee;
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .filter-tab {
            padding: 10px 16px;
            font-size: 12px;
            font-weight: 600;
            color: #999;
            text-decoration: none;
            letter-spacing: 0.5px;
            border-bottom: 2px solid transparent;
            white-space: nowrap;
            transition: color 0.2s, border-color 0.2s;
            background: none;
            border-top: none;
            border-left: none;
            border-right: none;
            cursor: pointer;
        }

        .filter-tab:hover {
            color: #555;
        }

        .filter-tab.active {
            color: #1a1a1a;
            border-bottom-color: #1a1a1a;
        }

        .order-card {
            background: #fff;
            border: 1px solid #eee;
            border-radius: 12px;
            padding: 16px;
            margin-bottom: 12px;
            transition: box-shadow 0.2s;
        }

        .order-card:hover {
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
        }

        .order-card-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 8px;
        }

        .status-badge {
            display: inline-block;
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 10px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .status-placed {
            background-color: #e8f0fe;
            color: #1a73e8;
        }

        .status-processing {
            background-color: #fff3e0;
            color: #f57c00;
        }

        .status-shipped {
            background-color: #e0f2f1;
            color: #00897b;
        }

        .status-delivered {
            background-color: #e8f5e9;
            color: #2e7d32;
        }

        .status-cancelled {
            background-color: #fce4ec;
            color: #c62828;
        }

        .order-number {
            font-size: 15px;
            font-weight: 700;
            color: #1a1a1a;
            margin-bottom: 4px;
        }

        .order-meta {
            font-size: 13px;
            color: #888;
        }

        .order-meta .separator {
            margin: 0 8px;
            color: #ddd;
        }

        .order-card-footer {
            display: flex;
            justify-content: flex-end;
            margin-top: 12px;
        }

        .btn-view {
            padding: 8px 16px;
            font-size: 12px;
            font-weight: 600;
            color: #1a1a1a;
            background: #f5f5f5;
            border: 1px solid #eee;
            border-radius: 6px;
            text-decoration: none;
            letter-spacing: 0.5px;
            text-transform: uppercase;
            transition: background-color 0.2s;
        }

        .btn-view:hover {
            background: #eee;
        }

        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #999;
        }

        .empty-state-icon {
            font-size: 48px;
            margin-bottom: 16px;
        }

        .empty-state-title {
            font-size: 16px;
            font-weight: 600;
            color: #555;
            margin-bottom: 8px;
        }

        .empty-state-text {
            font-size: 14px;
            color: #999;
        }

        .success-message {
            background-color: #e8f5e9;
            color: #2e7d32;
            padding: 12px 16px;
            border-radius: 8px;
            font-size: 14px;
            font-weight: 500;
            margin-bottom: 20px;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="orders-container">
        <!-- Header -->
        <div class="page-header">
            <div class="page-header-left">
                <a href="Default.aspx" class="back-arrow">&#8592;</a>
                <h1 class="page-title">ORDER HISTORY</h1>
            </div>
        </div>

        <!-- Success Message -->
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="success-message">
            Your order has been placed successfully!
        </asp:Panel>

        <!-- Filter Tabs -->
        <div class="filter-tabs">
            <asp:LinkButton ID="lnkAll" runat="server" CssClass="filter-tab active" OnClick="FilterOrders_Click" CommandArgument="All">ALL</asp:LinkButton>
            <asp:LinkButton ID="lnkPlaced" runat="server" CssClass="filter-tab" OnClick="FilterOrders_Click" CommandArgument="Placed">PLACED</asp:LinkButton>
            <asp:LinkButton ID="lnkProcessing" runat="server" CssClass="filter-tab" OnClick="FilterOrders_Click" CommandArgument="Processing">PROCESSING</asp:LinkButton>
            <asp:LinkButton ID="lnkShipped" runat="server" CssClass="filter-tab" OnClick="FilterOrders_Click" CommandArgument="Shipped">SHIPPED</asp:LinkButton>
            <asp:LinkButton ID="lnkDelivered" runat="server" CssClass="filter-tab" OnClick="FilterOrders_Click" CommandArgument="Delivered">DELIVERED</asp:LinkButton>
        </div>

        <!-- Order List -->
        <asp:Repeater ID="rptOrders" runat="server">
            <ItemTemplate>
                <div class="order-card">
                    <div class="order-card-header">
                        <uc:OrderStatusBadge ID="ucStatus" runat="server" Status='<%# Eval("OrderStatus").ToString() %>' />
                    </div>
                    <div class="order-number">ORDER #CN-<%# Eval("OrderID", "{0:D5}") %></div>
                    <div class="order-meta">
                        <%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd MMM yyyy") %>
                        <span class="separator">|</span>
                        Rs. <%# Convert.ToDecimal(Eval("TotalAmount")).ToString("N0") %>
                    </div>
                    <div class="order-card-footer">
                        <a href='OrderTracking.aspx?id=<%# Eval("OrderID") %>' class="btn-view">VIEW DETAILS</a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <!-- Empty State -->
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
            <div class="empty-state-icon">&#128230;</div>
            <div class="empty-state-title">No orders found</div>
            <div class="empty-state-text">Your orders will appear here once you make a purchase.</div>
        </asp:Panel>
    </div>
</asp:Content>
