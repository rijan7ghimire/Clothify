<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="ProductDetail.aspx.cs" Inherits="Clothify.ProductDetail" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Back Button -->
    <div class="page-header">
        <a href="javascript:history.back()" class="btn-back">&lt; Product Details</a>
    </div>

    <asp:Panel ID="pnlProduct" runat="server">
        <!-- Product Image -->
        <div class="product-detail-image-container">
            <asp:Image ID="imgProduct" runat="server" CssClass="product-detail-image" />
        </div>

        <!-- Product Info -->
        <div class="product-detail-info">
            <h1 class="product-detail-name">
                <asp:Label ID="lblProductName" runat="server" />
            </h1>
            <div class="product-detail-price">
                <asp:Label ID="lblPrice" runat="server" />
            </div>

            <!-- Description -->
            <div class="product-detail-section">
                <h3>Product Description</h3>
                <p class="product-detail-description">
                    <asp:Label ID="lblDescription" runat="server" />
                </p>
            </div>

            <!-- DetailView for Product Specifications -->
            <div class="product-detail-section">
                <h3>Product Specifications</h3>
                <asp:DetailsView ID="dvProduct" runat="server" AutoGenerateRows="False"
                    CssClass="detail-view-table" GridLines="None" Width="100%">
                    <FieldHeaderStyle Font-Bold="True" Width="40%" CssClass="dv-header" />
                    <RowStyle CssClass="dv-row" />
                    <AlternatingRowStyle CssClass="dv-row-alt" />
                    <Fields>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="Rs. {0:N0}" />
                        <asp:BoundField DataField="StockQuantity" HeaderText="Stock Available" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                    </Fields>
                </asp:DetailsView>
                <style>
                    .detail-view-table { border-collapse: collapse; margin-top: 8px; border-radius: 10px; overflow: hidden; box-shadow: 0 1px 4px rgba(0,0,0,0.06); }
                    .detail-view-table td { padding: 10px 14px; font-size: 13px; border-bottom: 1px solid #f0f0f0; }
                    .dv-header { background: #f9f9f9; color: #333; }
                    .dv-row { background: #fff; }
                    .dv-row-alt { background: #fafafa; }
                </style>
            </div>

            <!-- Quantity Selector -->
            <div class="quantity-control">
                <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="quantity-btn" OnClick="btnDecrease_Click" />
                <asp:TextBox ID="txtQuantity" runat="server" Text="1" CssClass="quantity-input" ReadOnly="true" />
                <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="quantity-btn" OnClick="btnIncrease_Click" />
            </div>

            <!-- Add to Cart & Wishlist -->
            <div style="display:flex; gap:10px; margin-bottom:8px;">
                <asp:Button ID="btnAddToCart" runat="server" Text="ADD TO CART" CssClass="btn-primary btn-block" OnClick="btnAddToCart_Click" />
                <asp:Button ID="btnAddToWishlist" runat="server" Text="&#9825; WISHLIST" CssClass="btn-primary btn-block"
                    OnClick="btnAddToWishlist_Click" style="background:#fff; color:#1a1a1a; border:2px solid #1a1a1a;" />
            </div>
        </div>

        <!-- Ratings & Reviews -->
        <div class="reviews-section">
            <h3>Ratings &amp; Reviews</h3>
            <asp:Label ID="lblAvgRating" runat="server" CssClass="avg-rating" />

            <asp:Repeater ID="rptReviews" runat="server">
                <ItemTemplate>
                    <div class="review-card">
                        <div class="review-header">
                            <span class="review-user"><%# Eval("FullName") %></span>
                            <span class="review-date"><%# Eval("FeedbackDate", "{0:MMM dd, yyyy}") %></span>
                        </div>
                        <div class="review-rating">
                            <%# GetStars(Convert.ToInt32(Eval("Rating"))) %>
                        </div>
                        <p class="review-comment"><%# Eval("Comment") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoReviews" runat="server" Visible="false" CssClass="no-reviews">
                <p>No reviews yet. Be the first to review this product!</p>
            </asp:Panel>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlNotFound" runat="server" Visible="false" CssClass="empty-state">
        <p>Product not found.</p>
        <a href="<%: ResolveUrl("~/Products.aspx") %>" class="btn-primary">Browse Products</a>
    </asp:Panel>

</asp:Content>
