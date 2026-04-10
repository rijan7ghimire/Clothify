<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderStatusBadge.ascx.cs" Inherits="Clothify.Controls.OrderStatusBadgeControl" %>

<span class='status-badge status-<%# Status != null ? Status.ToLower() : "" %>'><%# Status %></span>
