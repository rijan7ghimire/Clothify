<%@ Page Title="Wishlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs" Inherits="Clothify.Wishlist" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wishlist-container { max-width: 500px; margin: 0 auto; padding: 20px 16px 100px; }
        .wishlist-count { font-size: 12px; font-weight: 600; color: #888; letter-spacing: 0.5px; text-transform: uppercase; margin-bottom: 16px; }
        .wishlist-item { display: flex; align-items: center; background: #fff; border: 1px solid #eee; border-radius: 12px; padding: 12px; margin-bottom: 12px; gap: 12px; }
        .wishlist-item:hover { box-shadow: 0 2px 8px rgba(0,0,0,0.06); }
        .wishlist-item-image { width: 80px; height: 80px; object-fit: cover; border-radius: 8px; background: #f5f5f5; }
        .wishlist-item-details { flex: 1; min-width: 0; }
        .wishlist-item-name { font-size: 14px; font-weight: 700; color: #1a1a1a; margin: 0 0 4px 0; }
        .wishlist-item-price { font-size: 14px; font-weight: 600; color: #333; margin-bottom: 8px; }
        .wishlist-actions { display: flex; gap: 8px; }
        .btn-add-cart { padding: 6px 12px; font-size: 11px; font-weight: 700; color: #fff; background: #1a1a1a; border: none; border-radius: 6px; cursor: pointer; letter-spacing: 0.5px; }
        .btn-remove-wish { padding: 6px 12px; font-size: 11px; font-weight: 700; color: #c62828; background: #fce4ec; border: none; border-radius: 6px; cursor: pointer; letter-spacing: 0.5px; }
        .empty-state { text-align: center; padding: 60px 20px; color: #999; }
        .empty-state-icon { font-size: 48px; margin-bottom: 16px; }
        .empty-state-title { font-size: 16px; font-weight: 600; color: #555; margin-bottom: 8px; }
        .empty-state-text { font-size: 14px; color: #999; margin-bottom: 20px; }
        .btn-browse { display: inline-block; padding: 12px 24px; font-size: 13px; font-weight: 700; color: #fff; background: #1a1a1a; border: none; border-radius: 8px; text-decoration: none; letter-spacing: 1px; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="wishlist-container">
        <div class="page-header">
            <a href="javascript:history.back()" class="btn-back">&lt; WISHLIST</a>
        </div>

        <asp:Panel ID="pnlWishlist" runat="server">
            <div class="wishlist-count">
                <asp:Label ID="lblWishlistCount" runat="server" />
            </div>
            <asp:Repeater ID="rptWishlist" runat="server" OnItemCommand="rptWishlist_ItemCommand">
                <ItemTemplate>
                    <div class="wishlist-item">
                        <img src='<%# ResolveUrl("~" + Eval("ImageURL")) %>' alt='<%# Eval("ProductName") %>' class="wishlist-item-image" />
                        <div class="wishlist-item-details">
                            <h4 class="wishlist-item-name"><%# Eval("ProductName") %></h4>
                            <div class="wishlist-item-price">Rs. <%# string.Format("{0:N0}", Eval("Price")) %></div>
                            <div class="wishlist-actions">
                                <asp:Button ID="btnAddToCart" runat="server" Text="ADD TO CART" CssClass="btn-add-cart"
                                    CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' />
                                <asp:Button ID="btnRemove" runat="server" Text="REMOVE" CssClass="btn-remove-wish"
                                    CommandName="Remove" CommandArgument='<%# Eval("ProductID") %>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>

        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
            <div class="empty-state-icon">&#9825;</div>
            <div class="empty-state-title">Your wishlist is empty</div>
            <div class="empty-state-text">Save items you love to your wishlist and revisit them anytime.</div>
            <a href="<%: ResolveUrl("~/Products.aspx") %>" class="btn-browse">BROWSE PRODUCTS</a>
        </asp:Panel>
    </div>
</asp:Content>
