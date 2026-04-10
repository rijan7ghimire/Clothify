<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="Clothify.UserProfile" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .profile-container {
            max-width: 400px;
            margin: 0 auto;
            padding: 30px 20px;
        }

        .profile-header {
            text-align: center;
            margin-bottom: 32px;
        }

        .profile-avatar {
            width: 80px;
            height: 80px;
            border-radius: 50%;
            background-color: #e0e0e0;
            margin: 0 auto 16px;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        .profile-avatar svg {
            width: 40px;
            height: 40px;
            color: #999;
        }

        .profile-name {
            font-size: 20px;
            font-weight: 700;
            color: #1a1a1a;
            margin-bottom: 4px;
        }

        .profile-email {
            font-size: 14px;
            color: #888;
            margin-bottom: 2px;
        }

        .profile-phone {
            font-size: 14px;
            color: #888;
        }

        .profile-menu {
            margin-top: 24px;
            border-top: 1px solid #eee;
        }

        .profile-menu-item {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 16px 0;
            border-bottom: 1px solid #eee;
            text-decoration: none;
            color: #1a1a1a;
            font-size: 15px;
            font-weight: 500;
            transition: color 0.2s;
        }

        .profile-menu-item:hover {
            color: #555;
        }

        .profile-menu-item .arrow {
            color: #ccc;
            font-size: 18px;
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

        .btn-danger {
            background-color: #e74c3c;
            color: #fff;
        }

        .btn-danger:hover {
            background-color: #c0392b;
        }

        .btn-block {
            display: block;
            width: 100%;
        }

        .logout-section {
            margin-top: 32px;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile-container">
        <div class="profile-header">
            <div class="profile-avatar">
                <svg xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round">
                    <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                    <circle cx="12" cy="7" r="4"></circle>
                </svg>
            </div>
            <div class="profile-name">
                <asp:Label ID="lblFullName" runat="server" Text=""></asp:Label>
            </div>
            <div class="profile-email">
                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
            </div>
            <div class="profile-phone">
                <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
            </div>
        </div>

        <div class="profile-menu">
            <a href="#" class="profile-menu-item">
                <span>SAVED ADDRESSES</span>
                <span class="arrow">&rarr;</span>
            </a>
            <a href="EditProfile.aspx" class="profile-menu-item">
                <span>EDIT PROFILE</span>
                <span class="arrow">&rarr;</span>
            </a>
            <a href="ChangePassword.aspx" class="profile-menu-item">
                <span>CHANGE PASSWORD</span>
                <span class="arrow">&rarr;</span>
            </a>
        </div>

        <div class="logout-section">
            <asp:Button ID="btnLogout" runat="server" Text="LOGOUT" CssClass="btn btn-danger btn-block" OnClick="btnLogout_Click" CausesValidation="false" />
        </div>
    </div>
</asp:Content>
