<%@ Page Title="Edit Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="Clothify.EditProfile" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .auth-container {
            max-width: 400px;
            margin: 0 auto;
            padding: 40px 20px;
        }

        .auth-title {
            font-size: 24px;
            font-weight: 700;
            text-align: center;
            margin-bottom: 4px;
            color: #1a1a1a;
        }

        .auth-subtitle {
            font-size: 14px;
            color: #888;
            text-align: center;
            margin-bottom: 32px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            font-size: 12px;
            font-weight: 600;
            color: #555;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 8px;
        }

        .form-control {
            width: 100%;
            padding: 12px 16px;
            border: 1px solid #ddd;
            border-radius: 8px;
            font-size: 14px;
            color: #333;
            background-color: #f9f9f9;
            box-sizing: border-box;
            transition: border-color 0.2s;
        }

        .form-control:focus {
            outline: none;
            border-color: #1a1a1a;
            background-color: #fff;
        }

        .form-control-readonly {
            background-color: #e9e9e9;
            color: #888;
            cursor: not-allowed;
        }

        .btn {
            display: inline-block;
            padding: 14px 24px;
            font-size: 14px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.2s;
        }

        .btn-primary {
            background-color: #1a1a1a;
            color: #fff;
        }

        .btn-primary:hover {
            background-color: #333;
        }

        .btn-block {
            display: block;
            width: 100%;
        }

        .error-message {
            color: #e74c3c;
            font-size: 13px;
            text-align: center;
            margin-bottom: 16px;
        }

        .success-message {
            color: #27ae60;
            font-size: 13px;
            text-align: center;
            margin-bottom: 16px;
        }

        .validation-error {
            color: #e74c3c;
            font-size: 12px;
            margin-top: 4px;
            display: block;
        }

        .back-link {
            display: block;
            text-align: center;
            margin-top: 20px;
            color: #1a1a1a;
            text-decoration: none;
            font-size: 14px;
            font-weight: 500;
        }

        .back-link:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <h1 class="auth-title">Edit Profile</h1>
        <p class="auth-subtitle">Update your personal information</p>

        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false"></asp:Label>

        <div class="form-group">
            <label for="<%= txtFullName.ClientID %>">FULL NAME</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter your full name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                ErrorMessage="Full name is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtEmail.ClientID %>">EMAIL</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-readonly" ReadOnly="true"></asp:TextBox>
        </div>

        <div class="form-group">
            <label for="<%= txtPhone.ClientID %>">PHONE NUMBER</label>
            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter your phone number"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                ErrorMessage="Phone number is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="SAVE CHANGES" CssClass="btn btn-primary btn-block" OnClick="btnSave_Click" />

        <a href="Profile.aspx" class="back-link">&larr; Back to Profile</a>
    </div>
</asp:Content>
