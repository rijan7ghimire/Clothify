<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Clothify.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .dashboard-container { padding: 0 0 24px 0; }
        .overview-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 16px;
        }
        .overview-header h2 {
            font-size: 14px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #333;
            margin: 0;
        }
        .status-live {
            background: #e8f5e9;
            color: #2e7d32;
            font-size: 11px;
            font-weight: 700;
            padding: 4px 12px;
            border-radius: 20px;
            letter-spacing: 1px;
        }
        .admin-stats {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 12px;
            margin-bottom: 24px;
        }
        .stat-card {
            background: #fff;
            border-radius: 12px;
            padding: 16px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06);
            border: 1px solid #f0f0f0;
        }
        .stat-value {
            font-size: 28px;
            font-weight: 800;
            color: #111;
            margin-bottom: 2px;
        }
        .stat-label {
            font-size: 11px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: #888;
            margin-bottom: 6px;
        }
        .stat-change {
            font-size: 11px;
            font-weight: 600;
            color: #2e7d32;
        }
        .stat-change.warning { color: #e65100; }
        .section-title {
            font-size: 14px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #333;
            margin-bottom: 12px;
        }
        .management-section {
            margin-bottom: 24px;
        }
        .management-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            background: #fff;
            border-radius: 12px;
            padding: 14px 16px;
            margin-bottom: 8px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06);
            border: 1px solid #f0f0f0;
            text-decoration: none;
            color: inherit;
            transition: box-shadow 0.15s;
        }
        .management-item:hover {
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .management-item .item-info h3 {
            font-size: 14px;
            font-weight: 700;
            color: #111;
            margin: 0 0 2px 0;
        }
        .management-item .item-info p {
            font-size: 12px;
            color: #888;
            margin: 0;
        }
        .management-item .arrow {
            color: #ccc;
            font-size: 18px;
        }
        .system-status {
            background: #fff;
            border-radius: 12px;
            padding: 16px;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06);
            border: 1px solid #f0f0f0;
        }
        .system-status .status-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .system-ok {
            background: #e8f5e9;
            color: #2e7d32;
            font-size: 12px;
            font-weight: 700;
            padding: 4px 14px;
            border-radius: 20px;
        }
        .sync-time {
            font-size: 11px;
            color: #999;
            margin-top: 8px;
        }
    </style>

    <div class="dashboard-container">
        <!-- OVERVIEW -->
        <div class="overview-header">
            <h2>OVERVIEW</h2>
            <span class="status-live">STATUS: LIVE</span>
        </div>

        <!-- STAT CARDS -->
        <div class="admin-stats">
            <div class="stat-card">
                <div class="stat-value"><asp:Literal ID="litTotalOrders" runat="server" Text="0" /></div>
                <div class="stat-label">TOTAL ORDERS</div>
                <div class="stat-change">All time</div>
            </div>
            <div class="stat-card">
                <div class="stat-value"><asp:Literal ID="litPendingOrders" runat="server" Text="0" /></div>
                <div class="stat-label">PENDING</div>
                <div class="stat-change warning">Action required</div>
            </div>
            <div class="stat-card">
                <div class="stat-value"><asp:Literal ID="litDeliveredOrders" runat="server" Text="0" /></div>
                <div class="stat-label">DELIVERED</div>
                <div class="stat-change">Completed</div>
            </div>
            <div class="stat-card">
                <div class="stat-value"><asp:Literal ID="litTotalUsers" runat="server" Text="0" /></div>
                <div class="stat-label">TOTAL USERS</div>
                <div class="stat-change">Registered</div>
            </div>
        </div>

        <!-- MANAGEMENT -->
        <div class="management-section">
            <div class="section-title">MANAGEMENT</div>

            <a href="ManageUsers.aspx" class="management-item">
                <div class="item-info">
                    <h3>MANAGE USERS</h3>
                    <p>List, edit, and role assignment</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>

            <a href="ManageProducts.aspx" class="management-item">
                <div class="item-info">
                    <h3>MANAGE PRODUCTS</h3>
                    <p>Inventory control and price management</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>

            <a href="ManageCategories.aspx" class="management-item">
                <div class="item-info">
                    <h3>MANAGE CATEGORIES</h3>
                    <p>Catalog structure and organization</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>

            <a href="ManageOrders.aspx" class="management-item">
                <div class="item-info">
                    <h3>MANAGE ORDERS</h3>
                    <p>Processing, logistics, and returns</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>

            <a href="ManageFeedback.aspx" class="management-item">
                <div class="item-info">
                    <h3>FEEDBACK</h3>
                    <p>User reviews and support tickets</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>

            <a href="Reports.aspx" class="management-item">
                <div class="item-info">
                    <h3>VIEW REPORTS</h3>
                    <p>Sales analytics and performance dashboard</p>
                </div>
                <span class="arrow">&#8250;</span>
            </a>
        </div>

        <!-- SYSTEM STATUS -->
        <div class="section-title">SYSTEM STATUS</div>
        <div class="system-status">
            <div class="status-row">
                <span style="font-size:14px; font-weight:600; color:#333;">System Health</span>
                <span class="system-ok">OK</span>
            </div>
            <div class="sync-time">Last sync: <asp:Literal ID="litLastSync" runat="server" /></div>
        </div>
    </div>
</asp:Content>
