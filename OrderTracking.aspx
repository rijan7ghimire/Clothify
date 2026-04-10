<%@ Page Title="Order Tracking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="OrderTracking.aspx.cs" Inherits="Clothify.OrderTracking" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .tracking-container {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px 16px 100px;
        }

        .page-header {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 24px;
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

        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .order-number {
            font-size: 18px;
            font-weight: 700;
            color: #1a1a1a;
        }

        .status-badge {
            display: inline-block;
            padding: 5px 12px;
            border-radius: 20px;
            font-size: 11px;
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

        .section-title {
            font-size: 14px;
            font-weight: 700;
            color: #1a1a1a;
            letter-spacing: 1px;
            text-transform: uppercase;
            margin-bottom: 16px;
            margin-top: 28px;
        }

        .detail-card {
            background: #f9f9f9;
            border-radius: 12px;
            padding: 16px;
            margin-bottom: 16px;
        }

        .detail-row {
            display: flex;
            justify-content: space-between;
            padding: 6px 0;
            font-size: 14px;
        }

        .detail-label {
            color: #888;
        }

        .detail-value {
            color: #1a1a1a;
            font-weight: 500;
            text-align: right;
        }

        /* Status Timeline */
        .status-timeline {
            padding: 20px 0;
        }

        .timeline-step {
            display: flex;
            align-items: flex-start;
            gap: 16px;
            position: relative;
            padding-bottom: 28px;
        }

        .timeline-step:last-child {
            padding-bottom: 0;
        }

        .timeline-indicator {
            display: flex;
            flex-direction: column;
            align-items: center;
            position: relative;
            z-index: 1;
        }

        .timeline-dot {
            width: 20px;
            height: 20px;
            border-radius: 50%;
            background-color: #e0e0e0;
            border: 3px solid #e0e0e0;
            flex-shrink: 0;
        }

        .timeline-dot.active {
            background-color: #1a1a1a;
            border-color: #1a1a1a;
        }

        .timeline-dot.completed {
            background-color: #2e7d32;
            border-color: #2e7d32;
        }

        .timeline-line {
            width: 2px;
            height: 28px;
            background-color: #e0e0e0;
            position: absolute;
            top: 20px;
            left: 9px;
        }

        .timeline-line.completed {
            background-color: #2e7d32;
        }

        .timeline-content {
            padding-top: 1px;
        }

        .timeline-title {
            font-size: 14px;
            font-weight: 600;
            color: #ccc;
        }

        .timeline-title.active {
            color: #1a1a1a;
        }

        .timeline-title.completed {
            color: #2e7d32;
        }

        /* Order Items */
        .order-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px 0;
            border-bottom: 1px solid #f0f0f0;
        }

        .order-item:last-child {
            border-bottom: none;
        }

        .order-item-info {
            flex: 1;
        }

        .order-item-name {
            font-size: 14px;
            font-weight: 600;
            color: #1a1a1a;
            margin-bottom: 2px;
        }

        .order-item-qty {
            font-size: 12px;
            color: #888;
        }

        .order-item-price {
            font-size: 14px;
            font-weight: 600;
            color: #1a1a1a;
        }

        .btn-cancel {
            display: block;
            width: 100%;
            padding: 14px 24px;
            font-size: 14px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            border: 1px solid #e74c3c;
            border-radius: 8px;
            cursor: pointer;
            background-color: #fff;
            color: #e74c3c;
            margin-top: 24px;
            transition: background-color 0.2s;
        }

        .btn-cancel:hover {
            background-color: #fce4ec;
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

        .error-message {
            color: #e74c3c;
            font-size: 13px;
            text-align: center;
            margin-bottom: 16px;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tracking-container">
        <!-- Header -->
        <div class="page-header">
            <a href="Orders.aspx" class="back-arrow">&#8592;</a>
            <h1 class="page-title">ORDER DETAILS</h1>
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

        <asp:Panel ID="pnlOrderDetails" runat="server">
            <!-- Order Number & Status -->
            <div class="order-header">
                <span class="order-number">ORDER #CN-<asp:Label ID="lblOrderNumber" runat="server"></asp:Label></span>
                <asp:Label ID="lblStatus" runat="server" CssClass="status-badge"></asp:Label>
            </div>

            <!-- Order Details -->
            <h2 class="section-title">Order Details</h2>
            <div class="detail-card">
                <div class="detail-row">
                    <span class="detail-label">Date</span>
                    <span class="detail-value"><asp:Label ID="lblOrderDate" runat="server"></asp:Label></span>
                </div>
                <div class="detail-row">
                    <span class="detail-label">Total</span>
                    <span class="detail-value"><asp:Label ID="lblTotal" runat="server"></asp:Label></span>
                </div>
                <div class="detail-row">
                    <span class="detail-label">Payment Method</span>
                    <span class="detail-value"><asp:Label ID="lblPaymentMethod" runat="server"></asp:Label></span>
                </div>
                <div class="detail-row">
                    <span class="detail-label">Payment Status</span>
                    <span class="detail-value"><asp:Label ID="lblPaymentStatus" runat="server"></asp:Label></span>
                </div>
            </div>

            <!-- Shipping Address -->
            <h2 class="section-title">Shipping Address</h2>
            <div class="detail-card">
                <div class="detail-row">
                    <span class="detail-label">Name</span>
                    <span class="detail-value"><asp:Label ID="lblShipName" runat="server"></asp:Label></span>
                </div>
                <div class="detail-row">
                    <span class="detail-label">Phone</span>
                    <span class="detail-value"><asp:Label ID="lblShipPhone" runat="server"></asp:Label></span>
                </div>
                <div class="detail-row">
                    <span class="detail-label">Address</span>
                    <span class="detail-value"><asp:Label ID="lblShipAddress" runat="server"></asp:Label></span>
                </div>
            </div>

            <!-- Status Timeline -->
            <h2 class="section-title">Order Status</h2>
            <div class="status-timeline">
                <div class="timeline-step">
                    <div class="timeline-indicator">
                        <div class='timeline-dot' id="dotPlaced" runat="server"></div>
                        <div class='timeline-line' id="linePlaced" runat="server"></div>
                    </div>
                    <div class="timeline-content">
                        <div class='timeline-title' id="titlePlaced" runat="server">Placed</div>
                    </div>
                </div>
                <div class="timeline-step">
                    <div class="timeline-indicator">
                        <div class='timeline-dot' id="dotProcessing" runat="server"></div>
                        <div class='timeline-line' id="lineProcessing" runat="server"></div>
                    </div>
                    <div class="timeline-content">
                        <div class='timeline-title' id="titleProcessing" runat="server">Processing</div>
                    </div>
                </div>
                <div class="timeline-step">
                    <div class="timeline-indicator">
                        <div class='timeline-dot' id="dotShipped" runat="server"></div>
                        <div class='timeline-line' id="lineShipped" runat="server"></div>
                    </div>
                    <div class="timeline-content">
                        <div class='timeline-title' id="titleShipped" runat="server">Shipped</div>
                    </div>
                </div>
                <div class="timeline-step">
                    <div class="timeline-indicator">
                        <div class='timeline-dot' id="dotDelivered" runat="server"></div>
                    </div>
                    <div class="timeline-content">
                        <div class='timeline-title' id="titleDelivered" runat="server">Delivered</div>
                    </div>
                </div>
            </div>

            <!-- Order Items -->
            <h2 class="section-title">Order Items</h2>
            <div class="detail-card">
                <asp:Repeater ID="rptOrderItems" runat="server">
                    <ItemTemplate>
                        <div class="order-item">
                            <div class="order-item-info">
                                <div class="order-item-name"><%# Eval("ProductName") %></div>
                                <div class="order-item-qty">Qty: <%# Eval("Quantity") %></div>
                            </div>
                            <div class="order-item-price">Rs. <%# Convert.ToDecimal(Eval("UnitPrice")).ToString("N0") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <!-- Cancel Button (only for Placed orders) -->
            <asp:Button ID="btnCancelOrder" runat="server" Text="CANCEL ORDER" CssClass="btn-cancel"
                OnClick="btnCancelOrder_Click" Visible="false"
                OnClientClick="return confirm('Are you sure you want to cancel this order?');" />
        </asp:Panel>
    </div>
</asp:Content>
