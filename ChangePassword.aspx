<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Clothify.ChangePassword" %>

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
        <h1 class="auth-title">Change Password</h1>
        <p class="auth-subtitle">Keep your account secure</p>

        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>
        <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false"></asp:Label>

        <div class="form-group">
            <label for="<%= txtCurrentPassword.ClientID %>">CURRENT PASSWORD</label>
            <asp:TextBox ID="txtCurrentPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter current password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword"
                ErrorMessage="Current password is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtNewPassword.ClientID %>">NEW PASSWORD</label>
            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter new password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword"
                ErrorMessage="New password is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtConfirmNewPassword.ClientID %>">CONFIRM NEW PASSWORD</label>
            <asp:TextBox ID="txtConfirmNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm new password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvConfirmNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
                ErrorMessage="Please confirm your new password" CssClass="validation-error" Display="Dynamic" />
            <asp:CompareValidator ID="cvNewPassword" runat="server" ControlToValidate="txtConfirmNewPassword"
                ControlToCompare="txtNewPassword" ErrorMessage="Passwords do not match" CssClass="validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnChangePassword" runat="server" Text="CHANGE PASSWORD" CssClass="btn btn-primary btn-block" OnClick="btnChangePassword_Click" />

        <a href="Profile.aspx" class="back-link">&larr; Back to Profile</a>
    </div>
</asp:Content>
