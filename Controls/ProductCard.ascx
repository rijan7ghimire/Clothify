<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductCard.ascx.cs" Inherits="Clothify.Controls.ProductCardControl" %>

<div class="product-card">
    <a href='<%# ResolveUrl("~/ProductDetail.aspx?id=" + ProductID) %>'>
        <img src='<%# ResolveUrl("~" + ImageURL) %>' alt='<%# ProductName %>' class="product-image"
             onerror="this.src='https://via.placeholder.com/300x300?text=No+Image'" />
    </a>
    <div class="product-info">
        <a href='<%# ResolveUrl("~/ProductDetail.aspx?id=" + ProductID) %>' class="product-link">
            <h3 class="product-name"><%# ProductName %></h3>
        </a>
        <p class="product-description"><%# ProductDescription != null && ProductDescription.Length > 60 ? ProductDescription.Substring(0, 60) + "..." : ProductDescription %></p>
        <span class="price">Rs. <%# string.Format("{0:N0}", Price) %></span>
        <asp:Button ID="btnAddToCart" runat="server" Text="ADD TO CART" CssClass="btn-primary btn-add-cart" OnClick="btnAddToCart_Click" />
    </div>
</div>
