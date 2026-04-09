<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderStatusBadge.ascx.cs" Inherits="Clothify.Controls.OrderStatusBadgeControl" %>

<span class='status-badge status-<%# Status != null ? Status.ToLower() : "" %>'><%# Status %></span>
