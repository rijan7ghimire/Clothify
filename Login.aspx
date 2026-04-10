<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Clothify.Login" %>

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

        .auth-links {
            text-align: center;
            margin-top: 24px;
        }

        .auth-links a {
            color: #1a1a1a;
            text-decoration: none;
            font-size: 14px;
            font-weight: 500;
        }

        .auth-links a:hover {
            text-decoration: underline;
        }

        .auth-links .separator {
            color: #ccc;
            margin: 0 12px;
        }

        .error-message {
            color: #e74c3c;
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
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <h1 class="auth-title">Welcome to Clothify</h1>
        <p class="auth-subtitle">Sign in to continue shopping</p>

        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>

        <div class="form-group">
            <label for="<%= txtEmailOrPhone.ClientID %>">EMAIL OR PHONE</label>
            <asp:TextBox ID="txtEmailOrPhone" runat="server" CssClass="form-control" placeholder="Enter your email or phone"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmailOrPhone" runat="server" ControlToValidate="txtEmailOrPhone"
                ErrorMessage="Email or phone is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtPassword.ClientID %>">PASSWORD</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="Password is required" CssClass="validation-error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="LOGIN" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />

        <div class="auth-links">
            <a href="Register.aspx">Create Account</a>
            <span class="separator">|</span>
            <a href="#">Forgot Password?</a>
        </div>
    </div>
</asp:Content>
