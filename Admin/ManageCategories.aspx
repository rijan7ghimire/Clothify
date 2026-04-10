<%@ Page Title="Manage Categories" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ManageCategories.aspx.cs" Inherits="Clothify.Admin.ManageCategories" %>

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
        .cat-header {
            display: flex; justify-content: space-between; align-items: flex-start;
        }
        .cat-name { font-size: 15px; font-weight: 700; color: #111; }
        .cat-id { font-size: 11px; color: #999; }
        .cat-desc { font-size: 13px; color: #666; margin-top: 4px; }
        .cat-actions { display: flex; gap: 8px; margin-top: 10px; }
        .no-data { text-align: center; padding: 24px; color: #999; font-size: 14px; }
    </style>

    <h1 class="page-title">Manage Categories</h1>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <div class="alert" id="divMessage" runat="server"></div>
    </asp:Panel>

    <!-- ADD CATEGORY -->
    <div class="card">
        <div class="card-title">ADD CATEGORY</div>
        <div class="form-group">
            <label>Category Name</label>
            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Enter category name" />
        </div>
        <div class="form-group">
            <label>Description</label>
            <asp:TextBox ID="txtCategoryDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Enter description" />
        </div>
        <asp:Button ID="btnAddCategory" runat="server" Text="ADD CATEGORY" CssClass="btn btn-primary" OnClick="btnAddCategory_Click" />
    </div>

    <!-- EDIT CATEGORY PANEL -->
    <asp:Panel ID="pnlEditCategory" runat="server" Visible="false">
        <div class="card">
            <div class="card-title">EDIT CATEGORY</div>
            <asp:HiddenField ID="hfEditCategoryID" runat="server" />
            <div class="form-group">
                <label>Category Name</label>
                <asp:TextBox ID="txtEditCategoryName" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label>Description</label>
                <asp:TextBox ID="txtEditCategoryDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
            </div>
            <asp:Button ID="btnSaveEdit" runat="server" Text="SAVE CHANGES" CssClass="btn btn-primary" OnClick="btnSaveEdit_Click" />
            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancelEdit_Click" CausesValidation="false" style="margin-left:8px;" />
        </div>
    </asp:Panel>

    <!-- CATEGORY LIST -->
    <asp:Repeater ID="rptCategories" runat="server" OnItemCommand="rptCategories_ItemCommand">
        <ItemTemplate>
            <div class="card">
                <div class="cat-header">
                    <div>
                        <div class="cat-name"><%# Eval("CategoryName") %></div>
                        <div class="cat-id">ID: <%# Eval("CategoryID") %></div>
                    </div>
                </div>
                <div class="cat-desc"><%# Eval("Description") %></div>
                <div class="cat-actions">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary btn-sm"
                        CommandName="EditCategory" CommandArgument='<%# Eval("CategoryID") %>' CausesValidation="false" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                        CommandName="DeleteCategory" CommandArgument='<%# Eval("CategoryID") %>'
                        OnClientClick="return confirm('Are you sure you want to delete this category?');" CausesValidation="false" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
        <div class="no-data">No categories found.</div>
    </asp:Panel>
</asp:Content>
