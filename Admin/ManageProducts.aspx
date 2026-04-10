<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ManageProducts.aspx.cs" Inherits="Clothify.Admin.ManageProducts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .page-title { font-size: 18px; font-weight: 800; color: #111; margin-bottom: 16px; }
        .card {
            background: #fff; border-radius: 12px; padding: 16px;
            margin-bottom: 12px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0;
        }
        .card-title { font-size: 14px; font-weight: 700; color: #111; margin-bottom: 12px; }
        .form-group { margin-bottom: 12px; }
        .form-group label {
            display: block; font-size: 12px; font-weight: 600;
            color: #555; margin-bottom: 4px; text-transform: uppercase; letter-spacing: 0.5px;
        }
        .form-control {
            width: 100%; padding: 10px 12px; border: 1px solid #ddd;
            border-radius: 8px; font-size: 14px; box-sizing: border-box; outline: none;
        }
        .form-control:focus { border-color: #111; }
        .btn {
            padding: 10px 20px; border: none; border-radius: 8px;
            font-size: 13px; font-weight: 600; cursor: pointer; transition: opacity 0.15s;
        }
        .btn:hover { opacity: 0.85; }
        .btn-primary { background: #111; color: #fff; }
        .btn-danger { background: #e53935; color: #fff; }
        .btn-sm { padding: 6px 12px; font-size: 12px; }
        .btn-secondary { background: #eee; color: #333; }
        .alert {
            padding: 12px 16px; border-radius: 10px; margin-bottom: 16px;
            font-size: 13px; font-weight: 500;
        }
        .alert-success { background: #e8f5e9; color: #2e7d32; }
        .alert-danger { background: #ffebee; color: #c62828; }
        .product-card {
            display: flex; gap: 12px; align-items: flex-start;
        }
        .product-thumb {
            width: 60px; height: 60px; border-radius: 8px;
            object-fit: cover; background: #f5f5f5; flex-shrink: 0;
        }
        .product-info { flex: 1; }
        .product-name { font-size: 14px; font-weight: 700; color: #111; margin-bottom: 2px; }
        .product-detail { font-size: 12px; color: #666; margin-bottom: 2px; }
        .product-price { font-size: 14px; font-weight: 700; color: #111; }
        .product-actions {
            display: flex; gap: 8px; margin-top: 10px; flex-wrap: wrap;
        }
        .no-data { text-align: center; padding: 24px; color: #999; font-size: 14px; }
        .toggle-btn {
            background: #111; color: #fff; padding: 10px 20px; border: none;
            border-radius: 8px; font-size: 13px; font-weight: 600; cursor: pointer;
            margin-bottom: 16px;
        }
        .edit-panel { background: #fafafa; border-radius: 10px; padding: 14px; margin-top: 10px; }
    </style>

    <h1 class="page-title">Manage Products</h1>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <div class="alert" id="divMessage" runat="server"></div>
    </asp:Panel>

    <!-- Toggle Add Product Form -->
    <asp:Button ID="btnToggleAdd" runat="server" Text="+ Add New Product" CssClass="toggle-btn" OnClick="btnToggleAdd_Click" CausesValidation="false" />

    <!-- ADD NEW PRODUCT -->
    <asp:Panel ID="pnlAddProduct" runat="server" Visible="false">
        <div class="card">
            <div class="card-title">ADD NEW PRODUCT</div>
            <div class="form-group">
                <label>Product Name</label>
                <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Enter product name" />
            </div>
            <div class="form-group">
                <label>Description</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter product description" />
            </div>
            <div class="form-group">
                <label>Price (NPR)</label>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="0.00" />
            </div>
            <div class="form-group">
                <label>Stock Quantity</label>
                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" placeholder="0" />
            </div>
            <div class="form-group">
                <label>Image URL</label>
                <asp:TextBox ID="txtImageURL" runat="server" CssClass="form-control" placeholder="https://..." />
            </div>
            <div class="form-group">
                <label>Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnAddProduct" runat="server" Text="ADD PRODUCT" CssClass="btn btn-primary" OnClick="btnAddProduct_Click" />
            <asp:Button ID="btnCancelAdd" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancelAdd_Click" CausesValidation="false" style="margin-left:8px;" />
        </div>
    </asp:Panel>

    <!-- EDIT PRODUCT PANEL -->
    <asp:Panel ID="pnlEditProduct" runat="server" Visible="false">
        <div class="card">
            <div class="card-title">EDIT PRODUCT</div>
            <asp:HiddenField ID="hfEditProductID" runat="server" />
            <div class="form-group">
                <label>Product Name</label>
                <asp:TextBox ID="txtEditProductName" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Description</label>
                <asp:TextBox ID="txtEditDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
            </div>
            <div class="form-group">
                <label>Price (NPR)</label>
                <asp:TextBox ID="txtEditPrice" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Stock Quantity</label>
                <asp:TextBox ID="txtEditStock" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Image URL</label>
                <asp:TextBox ID="txtEditImageURL" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Category</label>
                <asp:DropDownList ID="ddlEditCategory" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnSaveEdit" runat="server" Text="SAVE CHANGES" CssClass="btn btn-primary" OnClick="btnSaveEdit_Click" />
            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancelEdit_Click" CausesValidation="false" style="margin-left:8px;" />
        </div>
    </asp:Panel>

    <!-- PRODUCT LIST -->
    <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
        <ItemTemplate>
            <div class="card">
                <div class="product-card">
                    <img src='<%# Eval("ImageURL") %>' alt="" class="product-thumb" onerror="this.src='data:image/svg+xml,<svg xmlns=%22http://www.w3.org/2000/svg%22 width=%2260%22 height=%2260%22><rect fill=%22%23f0f0f0%22 width=%2260%22 height=%2260%22/></svg>';" />
                    <div class="product-info">
                        <div class="product-name"><%# Eval("ProductName") %></div>
                        <div class="product-detail">ID: <%# Eval("ProductID") %> | Category: <%# Eval("CategoryName") %></div>
                        <div class="product-detail">Stock: <%# Eval("StockQuantity") %></div>
                        <div class="product-price">Rs. <%# Convert.ToDecimal(Eval("Price")).ToString("N0") %></div>
                    </div>
                </div>
                <div class="product-actions">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-sm"
                        CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>' CausesValidation="false" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                        CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>'
                        OnClientClick="return confirm('Are you sure you want to delete this product?');" CausesValidation="false" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
        <div class="no-data">No products found.</div>
    </asp:Panel>
</asp:Content>
