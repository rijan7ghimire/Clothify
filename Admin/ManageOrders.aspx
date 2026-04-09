<%@ Page Title="Manage Orders" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="Clothify.Admin.ManageOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .page-title { font-size: 18px; font-weight: 800; color: #111; margin-bottom: 16px; }
        .filter-tabs {
            display: flex; gap: 6px; margin-bottom: 16px; overflow-x: auto;
            padding-bottom: 4px; flex-wrap: wrap;
        }
        .filter-tab {
            padding: 7px 14px; border-radius: 20px; font-size: 12px; font-weight: 600;
            border: 1px solid #ddd; background: #fff; color: #666; cursor: pointer;
            white-space: nowrap; text-decoration: none; text-align: center;
        }
        .filter-tab:hover { border-color: #111; color: #111; }
        .filter-tab.active { background: #111; color: #fff; border-color: #111; }
        .order-card {
            background: #fff; border-radius: 12px; padding: 16px;
            margin-bottom: 12px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0;
        }
        .order-header {
            display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 8px;
        }
        .order-id { font-size: 15px; font-weight: 700; color: #111; }
        .order-customer { font-size: 12px; color: #666; margin-top: 2px; }
        .order-detail { font-size: 12px; color: #666; margin-bottom: 3px; }
        .order-total { font-size: 15px; font-weight: 700; color: #111; }
        .status-badge {
            display: inline-block; padding: 3px 10px; border-radius: 12px;
            font-size: 11px; font-weight: 700; text-transform: uppercase;
        }
        .status-placed { background: #fff3e0; color: #e65100; }
        .status-processing { background: #e3f2fd; color: #1565c0; }
        .status-shipped { background: #f3e5f5; color: #7b1fa2; }
        .status-delivered { background: #e8f5e9; color: #2e7d32; }
        .status-cancelled { background: #ffebee; color: #c62828; }
        .order-actions {
            display: flex; gap: 8px; margin-top: 10px; align-items: center; flex-wrap: wrap;
        }
        .btn {
            padding: 8px 16px; border: none; border-radius: 8px;
            font-size: 13px; font-weight: 600; cursor: pointer; transition: opacity 0.15s;
        }
        .btn:hover { opacity: 0.85; }
        .btn-primary { background: #111; color: #fff; }
        .btn-sm { padding: 6px 12px; font-size: 12px; }
        .btn-secondary { background: #eee; color: #333; }
        .status-select {
            padding: 6px 10px; border: 1px solid #ddd; border-radius: 8px;
            font-size: 12px; background: #fff;
        }
        .alert {
            padding: 12px 16px; border-radius: 10px; margin-bottom: 16px;
            font-size: 13px; font-weight: 500;
        }
        .alert-success { background: #e8f5e9; color: #2e7d32; }
        .alert-danger { background: #ffebee; color: #c62828; }
        .no-data { text-align: center; padding: 24px; color: #999; font-size: 14px; }
        .details-panel {
            background: #fafafa; border-radius: 10px; padding: 14px; margin-top: 12px;
            border: 1px solid #eee;
        }
        .details-title { font-size: 13px; font-weight: 700; color: #111; margin-bottom: 8px; }
        .details-row { font-size: 12px; color: #555; margin-bottom: 4px; }
        .details-row strong { color: #111; }
        .item-row {
            display: flex; justify-content: space-between; padding: 6px 0;
            border-bottom: 1px solid #eee; font-size: 12px; color: #555;
        }
        .item-row:last-child { border-bottom: none; }
    </style>

    <h1 class="page-title">Manage Orders</h1>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <div class="alert" id="divMessage" runat="server"></div>
    </asp:Panel>

    <!-- FILTER TABS -->
    <div class="filter-tabs">
        <asp:LinkButton ID="btnFilterAll" runat="server" CssClass="filter-tab active" Text="All" CommandArgument="All" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilterPlaced" runat="server" CssClass="filter-tab" Text="Placed" CommandArgument="Placed" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilterProcessing" runat="server" CssClass="filter-tab" Text="Processing" CommandArgument="Processing" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilterShipped" runat="server" CssClass="filter-tab" Text="Shipped" CommandArgument="Shipped" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilterDelivered" runat="server" CssClass="filter-tab" Text="Delivered" CommandArgument="Delivered" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilterCancelled" runat="server" CssClass="filter-tab" Text="Cancelled" CommandArgument="Cancelled" OnClick="btnFilter_Click" CausesValidation="false" />
    </div>

    <!-- ORDER LIST -->
    <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand">
        <ItemTemplate>
            <div class="order-card">
                <div class="order-header">
                    <div>
                        <div class="order-id">CN-<%# Eval("OrderID", "{0:D5}") %></div>
                        <div class="order-customer"><%# Eval("CustomerName") %></div>
                    </div>
                    <span class='<%# "status-badge status-" + Eval("OrderStatus").ToString().ToLower() %>'>
                        <%# Eval("OrderStatus") %>
                    </span>
                </div>
                <div class="order-detail">Date: <%# Eval("OrderDate", "{0:yyyy-MM-dd}") %></div>
                <div class="order-detail">Payment: <%# Eval("PaymentMethod") %></div>
                <div class="order-total">Rs. <%# Convert.ToDecimal(Eval("TotalAmount")).ToString("N0") %></div>
                <div class="order-actions">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="status-select">
                        <asp:ListItem Text="Placed" Value="Placed" />
                        <asp:ListItem Text="Processing" Value="Processing" />
                        <asp:ListItem Text="Shipped" Value="Shipped" />
                        <asp:ListItem Text="Delivered" Value="Delivered" />
                        <asp:ListItem Text="Cancelled" Value="Cancelled" />
                    </asp:DropDownList>
                    <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" CssClass="btn btn-primary btn-sm"
                        CommandName="UpdateStatus" CommandArgument='<%# Eval("OrderID") %>' CausesValidation="false" />
                    <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CssClass="btn btn-secondary btn-sm"
                        CommandName="ViewDetails" CommandArgument='<%# Eval("OrderID") %>' CausesValidation="false" />
                </div>

                <!-- ORDER DETAILS PANEL -->
                <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                    <div class="details-panel">
                        <div class="details-title">Order Items</div>
                        <asp:Repeater ID="rptOrderItems" runat="server">
                            <ItemTemplate>
                                <div class="item-row">
                                    <span><%# Eval("ProductName") %> x <%# Eval("Quantity") %></span>
                                    <span>Rs. <%# Convert.ToDecimal(Eval("SubTotal")).ToString("N0") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="details-title" style="margin-top:12px;">Shipping Address</div>
                        <div class="details-row"><strong>Name:</strong> <asp:Literal ID="litShipName" runat="server" /></div>
                        <div class="details-row"><strong>Phone:</strong> <asp:Literal ID="litShipPhone" runat="server" /></div>
                        <div class="details-row"><strong>Address:</strong> <asp:Literal ID="litShipAddress" runat="server" /></div>
                        <div class="details-row"><strong>Landmark:</strong> <asp:Literal ID="litShipLandmark" runat="server" /></div>

                        <div class="details-title" style="margin-top:12px;">Payment</div>
                        <div class="details-row"><strong>Method:</strong> <asp:Literal ID="litPayMethod" runat="server" /></div>
                        <div class="details-row"><strong>Status:</strong> <asp:Literal ID="litPayStatus" runat="server" /></div>
                        <div class="details-row"><strong>Date:</strong> <asp:Literal ID="litPayDate" runat="server" /></div>
                        <div class="details-row"><strong>Delivery Fee:</strong> <asp:Literal ID="litDeliveryFee" runat="server" /></div>
                    </div>
                </asp:Panel>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
        <div class="no-data">No orders found.</div>
    </asp:Panel>
</asp:Content>
