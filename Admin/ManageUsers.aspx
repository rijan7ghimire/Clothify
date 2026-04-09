<%@ Page Title="Manage Users" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="Clothify.Admin.ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .page-title {
            font-size: 18px;
            font-weight: 800;
            color: #111;
            margin-bottom: 16px;
        }
        .search-box {
            margin-bottom: 16px;
        }
        .search-box .form-control {
            width: 100%;
            padding: 10px 14px;
            border: 1px solid #ddd;
            border-radius: 10px;
            font-size: 14px;
            box-sizing: border-box;
            outline: none;
        }
        .search-box .form-control:focus { border-color: #111; }
        .btn {
            padding: 8px 16px;
            border: none;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 600;
            cursor: pointer;
            transition: opacity 0.15s;
        }
        .btn:hover { opacity: 0.85; }
        .btn-primary { background: #111; color: #fff; }
        .btn-danger { background: #e53935; color: #fff; }
        .btn-sm { padding: 6px 12px; font-size: 12px; }
        .alert {
            padding: 12px 16px;
            border-radius: 10px;
            margin-bottom: 16px;
            font-size: 13px;
            font-weight: 500;
        }
        .alert-success { background: #e8f5e9; color: #2e7d32; }
        .alert-danger { background: #ffebee; color: #c62828; }
        .role-badge {
            display: inline-block;
            padding: 2px 10px;
            border-radius: 12px;
            font-size: 11px;
            font-weight: 700;
        }
        .role-admin { background: #e3f2fd; color: #1565c0; }
        .role-customer { background: #f3e5f5; color: #7b1fa2; }
        .role-select {
            padding: 6px 10px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 12px;
            background: #fff;
        }
        .no-data { text-align: center; padding: 24px; color: #999; font-size: 14px; }
        .grid-table { border-collapse: collapse; border-radius: 12px; overflow: hidden; box-shadow: 0 1px 4px rgba(0,0,0,0.06); }
        .grid-table th, .grid-table td { padding: 10px 14px; font-size: 13px; text-align: left; border-bottom: 1px solid #f0f0f0; }
        .grid-table th { font-weight: 700; }
        .grid-table input[type="text"] { padding: 6px 10px; border: 1px solid #ddd; border-radius: 8px; font-size: 12px; width: 100%; box-sizing: border-box; }
    </style>

    <h1 class="page-title">Manage Users</h1>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <div class="alert" id="divMessage" runat="server"></div>
    </asp:Panel>

    <!-- Search -->
    <div class="search-box">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name, email, or phone..." />
    </div>
    <div style="margin-bottom:16px;">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-sm" OnClick="btnClear_Click" style="background:#eee;color:#333;margin-left:6px;" />
    </div>

    <!-- User List (GridView) -->
    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
        OnRowEditing="gvUsers_RowEditing" OnRowUpdating="gvUsers_RowUpdating"
        OnRowDeleting="gvUsers_RowDeleting" OnRowCancelingEdit="gvUsers_RowCancelingEdit"
        CssClass="grid-table" GridLines="None" Width="100%"
        HeaderStyle-BackColor="#111" HeaderStyle-ForeColor="#fff" HeaderStyle-Font-Size="13"
        RowStyle-BackColor="#fff" AlternatingRowStyle-BackColor="#fafafa"
        EditRowStyle-BackColor="#fffde7">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="ID" ReadOnly="True" ItemStyle-Width="50px" />
            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate>
                    <span class='<%# Convert.ToString(Eval("RoleName")).ToLower() == "admin" ? "role-badge role-admin" : "role-badge role-customer" %>'>
                        <%# Eval("RoleName") %>
                    </span>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="role-select">
                        <asp:ListItem Text="Customer" Value="2" />
                        <asp:ListItem Text="Admin" Value="1" />
                    </asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="CreatedAt" HeaderText="Joined" DataFormatString="{0:yyyy-MM-dd}" ReadOnly="True" />
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                EditText="Edit" DeleteText="Delete" UpdateText="Save" CancelText="Cancel"
                ItemStyle-Width="140px"
                ControlStyle-CssClass="btn btn-sm"
                ButtonType="Button" />
        </Columns>
        <EmptyDataTemplate>
            <div class="no-data">No users found.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
