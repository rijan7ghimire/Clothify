<%@ Page Title="Sales Reports" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="Clothify.Admin.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .reports-container { padding: 0 0 24px 0; }
        .reports-header { margin-bottom: 16px; }
        .reports-header h2 { font-size: 14px; font-weight: 700; text-transform: uppercase; letter-spacing: 1px; color: #333; margin: 0; }
        .report-stats { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-bottom: 24px; }
        .report-card { background: #fff; border-radius: 12px; padding: 16px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0; }
        .report-card-value { font-size: 24px; font-weight: 800; color: #111; margin-bottom: 2px; }
        .report-card-label { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; color: #888; }
        .section-title { font-size: 14px; font-weight: 700; text-transform: uppercase; letter-spacing: 1px; color: #333; margin-bottom: 12px; }
        .monthly-summary { display: grid; grid-template-columns: 1fr 1fr; gap: 12px; margin-bottom: 24px; }
        .monthly-card { background: #fff; border-radius: 12px; padding: 16px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0; }
        .monthly-card h4 { font-size: 11px; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; color: #888; margin: 0 0 6px 0; }
        .monthly-card .value { font-size: 20px; font-weight: 800; color: #111; }
        .table-section { margin-bottom: 24px; }
        .data-table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 12px; overflow: hidden; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0; }
        .data-table th { background: #1a1a1a; color: #fff; padding: 10px 12px; font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; text-align: left; }
        .data-table td { padding: 10px 12px; font-size: 13px; color: #333; border-bottom: 1px solid #f0f0f0; }
        .data-table tr:last-child td { border-bottom: none; }
        .status-badge { display: inline-block; padding: 3px 8px; border-radius: 12px; font-size: 10px; font-weight: 700; text-transform: uppercase; }
        .status-placed { background: #e8f0fe; color: #1a73e8; }
        .status-processing { background: #fff3e0; color: #f57c00; }
        .status-shipped { background: #e0f2f1; color: #00897b; }
        .status-delivered { background: #e8f5e9; color: #2e7d32; }
        .status-cancelled { background: #fce4ec; color: #c62828; }
        .back-link { display: inline-block; margin-bottom: 16px; font-size: 13px; font-weight: 600; color: #1a1a1a; text-decoration: none; }
        .back-link:hover { text-decoration: underline; }
    </style>

    <div class="reports-container">
        <a href="Dashboard.aspx" class="back-link">&#8592; Back to Dashboard</a>

        <div class="reports-header">
            <h2>SALES REPORTS</h2>
        </div>

        <!-- Summary Cards -->
        <div class="report-stats">
            <div class="report-card">
                <div class="report-card-value"><asp:Literal ID="litTotalRevenue" runat="server" Text="Rs. 0" /></div>
                <div class="report-card-label">TOTAL REVENUE</div>
            </div>
            <div class="report-card">
                <div class="report-card-value"><asp:Literal ID="litTotalOrders" runat="server" Text="0" /></div>
                <div class="report-card-label">TOTAL ORDERS</div>
            </div>
            <div class="report-card">
                <div class="report-card-value"><asp:Literal ID="litTotalCustomers" runat="server" Text="0" /></div>
                <div class="report-card-label">TOTAL CUSTOMERS</div>
            </div>
            <div class="report-card">
                <div class="report-card-value"><asp:Literal ID="litAvgOrderValue" runat="server" Text="Rs. 0" /></div>
                <div class="report-card-label">AVG ORDER VALUE</div>
            </div>
        </div>

        <!-- Monthly Sales Summary -->
        <div class="section-title">MONTHLY COMPARISON</div>
        <div class="monthly-summary">
            <div class="monthly-card">
                <h4>THIS MONTH</h4>
                <div class="value"><asp:Literal ID="litThisMonth" runat="server" Text="Rs. 0" /></div>
            </div>
            <div class="monthly-card">
                <h4>LAST MONTH</h4>
                <div class="value"><asp:Literal ID="litLastMonth" runat="server" Text="Rs. 0" /></div>
            </div>
        </div>

        <!-- Recent Orders -->
        <div class="table-section">
            <div class="section-title">RECENT ORDERS</div>
            <asp:GridView ID="gvRecentOrders" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No orders found.">
                <Columns>
                    <asp:TemplateField HeaderText="ORDER #">
                        <ItemTemplate>CN-<%# Eval("OrderID", "{0:D5}") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrderDate" HeaderText="DATE" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:TemplateField HeaderText="AMOUNT">
                        <ItemTemplate>Rs. <%# Convert.ToDecimal(Eval("TotalAmount")).ToString("N0") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="STATUS">
                        <ItemTemplate>
                            <span class='status-badge status-<%# Eval("OrderStatus").ToString().ToLower() %>'><%# Eval("OrderStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <!-- Top Selling Products -->
        <div class="table-section">
            <div class="section-title">TOP SELLING PRODUCTS</div>
            <asp:GridView ID="gvTopProducts" runat="server" AutoGenerateColumns="false" CssClass="data-table"
                GridLines="None" ShowHeaderWhenEmpty="true" EmptyDataText="No products found.">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="PRODUCT" />
                    <asp:BoundField DataField="TotalQty" HeaderText="QTY SOLD" />
                    <asp:TemplateField HeaderText="REVENUE">
                        <ItemTemplate>Rs. <%# Convert.ToDecimal(Eval("TotalRevenue")).ToString("N0") %></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
