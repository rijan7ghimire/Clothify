<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Clothify.Cart" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Header -->
    <div class="page-header">
        <a href="javascript:history.back()" class="btn-back">&lt; BACK</a>
        <h2 class="page-title">SHOPPING CART</h2>
    </div>

    <!-- Cart with items -->
    <asp:Panel ID="pnlCart" runat="server">

        <div class="cart-count">
            <asp:Label ID="lblItemCount" runat="server" />
        </div>

        <!-- Cart Items -->
        <div class="cart-items">
            <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
                <ItemTemplate>
                    <div class="cart-item">
                        <img src='<%# ResolveUrl("~" + Eval("ImageURL")) %>' alt='<%# Eval("ProductName") %>' class="cart-item-image" />
                        <div class="cart-item-details">
                            <h4 class="cart-item-name"><%# Eval("ProductName") %></h4>
                            <span class="cart-item-price">Rs. <%# string.Format("{0:N0}", Eval("Price")) %></span>
                            <div class="quantity-control">
                                <asp:Button ID="btnDecrease" runat="server" Text="-" CssClass="quantity-btn"
                                    CommandName="Decrease" CommandArgument='<%# Eval("ProductID") %>' />
                                <span class="quantity-value"><%# Eval("Quantity") %></span>
                                <asp:Button ID="btnIncrease" runat="server" Text="+" CssClass="quantity-btn"
                                    CommandName="Increase" CommandArgument='<%# Eval("ProductID") %>' />
                            </div>
                        </div>
                        <asp:Button ID="btnRemove" runat="server" Text="REMOVE" CssClass="btn-remove"
                            CommandName="Remove" CommandArgument='<%# Eval("ProductID") %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Order Summary -->
        <div class="order-summary">
            <h3>Order Summary</h3>
            <div class="summary-row">
                <span>SUBTOTAL</span>
                <asp:Label ID="lblSubtotal" runat="server" />
            </div>
            <div class="summary-row">
                <span>DELIVERY FEE</span>
                <asp:Label ID="lblDeliveryFee" runat="server" />
            </div>
            <div class="summary-row summary-total">
                <span>TOTAL PRICE</span>
                <asp:Label ID="lblTotal" runat="server" />
            </div>
        </div>

        <!-- Checkout Button -->
        <asp:Button ID="btnCheckout" runat="server" Text="PROCEED TO CHECKOUT" CssClass="btn-primary btn-block" OnClick="btnCheckout_Click" />

    </asp:Panel>

    <!-- Empty Cart State -->
    <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false" CssClass="empty-state">
        <h3>Your cart is empty</h3>
        <p>Looks like you haven't added any items to your cart yet.</p>
        <a href="<%: ResolveUrl("~/Products.aspx") %>" class="btn-primary btn-block">BROWSE PRODUCTS</a>
    </asp:Panel>

</asp:Content>
