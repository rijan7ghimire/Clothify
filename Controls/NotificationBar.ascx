<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationBar.ascx.cs" Inherits="Clothify.Controls.NotificationBarControl" %>

<asp:Panel ID="pnlNotification" runat="server" Visible="false">
    <div class='alert alert-<%# MessageType %>'>
        <asp:Label ID="lblMessage" runat="server" />
    </div>
</asp:Panel>
