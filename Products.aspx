<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Products.aspx.cs" Inherits="Clothify.Products" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Search Bar -->
    <div class="search-container">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="search-bar" placeholder="Search products..." />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-search" OnClick="btnSearch_Click" />
    </div>

    <!-- Category Tabs -->
    <div class="category-tabs">
        <a href="<%: ResolveUrl("~/Products.aspx") %>" class="category-tab <%= string.IsNullOrEmpty(Request.QueryString["cat"]) && string.IsNullOrEmpty(Request.QueryString["search"]) ? "active" : "" %>">All</a>
        <asp:Repeater ID="rptCategories" runat="server">
            <ItemTemplate>
                <a href='<%# ResolveUrl("~/Products.aspx?cat=" + Eval("CategoryID")) %>'
                   class='category-tab <%# Request.QueryString["cat"] == Eval("CategoryID").ToString() ? "active" : "" %>'>
                    <%# Eval("CategoryName") %>
                </a>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- Search Results Label -->
    <asp:Label ID="lblSearchInfo" runat="server" CssClass="search-info" Visible="false" />

    <!-- Product Grid (DataList) -->
    <asp:DataList ID="dlProducts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
        OnItemCommand="dlProducts_ItemCommand" CssClass="product-grid" ItemStyle-VerticalAlign="Top">
        <ItemTemplate>
            <div class="product-card">
                <a href='<%# ResolveUrl("~/ProductDetail.aspx?id=" + Eval("ProductID")) %>'>
                    <img src='<%# ResolveUrl("~" + Eval("ImageURL")) %>' alt='<%# Eval("ProductName") %>' class="product-image" />
                </a>
                <div class="product-info">
                    <a href='<%# ResolveUrl("~/ProductDetail.aspx?id=" + Eval("ProductID")) %>' class="product-link">
                        <h3 class="product-name"><%# Eval("ProductName") %></h3>
                    </a>
                    <p class="product-description"><%# Eval("Description") != null && Eval("Description").ToString().Length > 60 ? Eval("Description").ToString().Substring(0, 60) + "..." : Eval("Description") %></p>
                    <span class="price">Rs. <%# string.Format("{0:N0}", Eval("Price")) %></span>
                    <asp:Button ID="btnAddToCart" runat="server" Text="ADD TO CART" CssClass="btn-primary btn-add-cart"
                        CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' />
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>

    <asp:Panel ID="pnlNoProducts" runat="server" Visible="false" CssClass="empty-state">
        <p>No products found.</p>
    </asp:Panel>

</asp:Content>
