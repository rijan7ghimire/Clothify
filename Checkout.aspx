<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Clothify.Checkout" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .checkout-container {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px 16px 100px;
        }

        .page-header {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 24px;
        }

        .back-arrow {
            text-decoration: none;
            color: #1a1a1a;
            font-size: 20px;
            display: flex;
            align-items: center;
        }

        .page-title {
            font-size: 20px;
            font-weight: 700;
            color: #1a1a1a;
            letter-spacing: 1px;
        }

        .checkout-steps {
            display: flex;
            justify-content: space-between;
            margin-bottom: 32px;
            padding: 0 8px;
        }

        .checkout-step {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 12px;
            font-weight: 600;
            color: #ccc;
            letter-spacing: 0.5px;
        }

        .checkout-step.active {
            color: #1a1a1a;
        }

        .checkout-step .step-number {
            font-size: 11px;
            font-weight: 700;
        }

        .section-title {
            font-size: 14px;
            font-weight: 700;
            color: #1a1a1a;
            letter-spacing: 1px;
            margin-bottom: 16px;
            text-transform: uppercase;
        }

        .form-group {
            margin-bottom: 16px;
        }

        .form-group label {
            display: block;
            font-size: 11px;
            font-weight: 600;
            color: #555;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 6px;
        }

        .form-control {
            width: 100%;
            padding: 12px 14px;
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

        .grid-2 {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 12px;
        }

        .payment-section {
            margin-top: 28px;
            margin-bottom: 28px;
        }

        .payment-options {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .payment-option {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 14px 16px;
            border: 1px solid #ddd;
            border-radius: 8px;
            cursor: pointer;
            transition: border-color 0.2s, background-color 0.2s;
            font-size: 13px;
            font-weight: 600;
            color: #333;
            letter-spacing: 0.5px;
        }

        .payment-option:hover {
            border-color: #1a1a1a;
        }

        .payment-option.selected {
            border-color: #1a1a1a;
            background-color: #f5f5f5;
        }

        .payment-option input[type="radio"] {
            accent-color: #1a1a1a;
            width: 16px;
            height: 16px;
        }

        .order-summary {
            background-color: #f9f9f9;
            border-radius: 12px;
            padding: 20px;
            margin-bottom: 24px;
        }

        .summary-row {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 8px 0;
            font-size: 14px;
            color: #555;
        }

        .summary-row.total {
            border-top: 1px solid #ddd;
            margin-top: 8px;
            padding-top: 14px;
            font-weight: 700;
            font-size: 16px;
            color: #1a1a1a;
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
    <div class="checkout-container">
        <!-- Header -->
        <div class="page-header">
            <a href="Cart.aspx" class="back-arrow">&#8592;</a>
            <h1 class="page-title">CHECKOUT</h1>
        </div>

        <!-- Checkout Steps -->
        <div class="checkout-steps">
            <div class="checkout-step active">
                <span class="step-number">01</span> SHIPPING
            </div>
            <div class="checkout-step">
                <span class="step-number">02</span> PAYMENT
            </div>
            <div class="checkout-step">
                <span class="step-number">03</span> REVIEW
            </div>
        </div>

        <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false"></asp:Label>

        <!-- Shipping Address -->
        <h2 class="section-title">Shipping Address</h2>

        <div class="form-group">
            <label>FULL NAME</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter your full name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                ErrorMessage="Full name is required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" />
        </div>

        <div class="form-group">
            <label>PHONE NUMBER</label>
            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter your phone number"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                ErrorMessage="Phone number is required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" />
        </div>

        <div class="form-group">
            <label>PROVINCE</label>
            <asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control">
                <asp:ListItem Text="Select Province" Value="" />
                <asp:ListItem Text="Koshi" Value="Koshi" />
                <asp:ListItem Text="Madhesh" Value="Madhesh" />
                <asp:ListItem Text="Bagmati" Value="Bagmati" />
                <asp:ListItem Text="Gandaki" Value="Gandaki" />
                <asp:ListItem Text="Lumbini" Value="Lumbini" />
                <asp:ListItem Text="Karnali" Value="Karnali" />
                <asp:ListItem Text="Sudurpashchim" Value="Sudurpashchim" />
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince"
                ErrorMessage="Province is required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" InitialValue="" />
        </div>

        <div class="grid-2">
            <div class="form-group">
                <label>DISTRICT</label>
                <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control" placeholder="District"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="txtDistrict"
                    ErrorMessage="Required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" />
            </div>
            <div class="form-group">
                <label>MUNICIPALITY</label>
                <asp:TextBox ID="txtMunicipality" runat="server" CssClass="form-control" placeholder="Municipality"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvMunicipality" runat="server" ControlToValidate="txtMunicipality"
                    ErrorMessage="Required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" />
            </div>
        </div>

        <div class="grid-2">
            <div class="form-group">
                <label>WARD NO.</label>
                <asp:TextBox ID="txtWardNo" runat="server" CssClass="form-control" placeholder="Ward No."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvWardNo" runat="server" ControlToValidate="txtWardNo"
                    ErrorMessage="Required" CssClass="validation-error" Display="Dynamic" ValidationGroup="checkout" />
            </div>
            <div class="form-group">
                <label>LANDMARK</label>
                <asp:TextBox ID="txtLandmark" runat="server" CssClass="form-control" placeholder="Landmark"></asp:TextBox>
            </div>
        </div>

        <!-- Payment Method -->
        <div class="payment-section">
            <h2 class="section-title">Payment Method</h2>
            <div class="payment-options">
                <label class="payment-option">
                    <asp:RadioButton ID="rbCOD" runat="server" GroupName="PaymentMethod" Checked="true" />
                    CASH ON DELIVERY
                </label>
                <label class="payment-option">
                    <asp:RadioButton ID="rbEsewa" runat="server" GroupName="PaymentMethod" />
                    ESEWA
                </label>
                <label class="payment-option">
                    <asp:RadioButton ID="rbKhalti" runat="server" GroupName="PaymentMethod" />
                    KHALTI
                </label>
                <label class="payment-option">
                    <asp:RadioButton ID="rbBankTransfer" runat="server" GroupName="PaymentMethod" />
                    BANK TRANSFER
                </label>
            </div>
        </div>

        <!-- Order Summary -->
        <h2 class="section-title">Order Summary</h2>
        <div class="order-summary">
            <div class="summary-row">
                <span>Subtotal</span>
                <span><asp:Label ID="lblSubtotal" runat="server" Text="Rs. 0"></asp:Label></span>
            </div>
            <div class="summary-row">
                <span>Delivery Fee</span>
                <span><asp:Label ID="lblDeliveryFee" runat="server" Text="Rs. 150"></asp:Label></span>
            </div>
            <div class="summary-row total">
                <span>Total Price</span>
                <span><asp:Label ID="lblTotal" runat="server" Text="Rs. 0"></asp:Label></span>
            </div>
        </div>

        <!-- Place Order Button -->
        <asp:Button ID="btnPlaceOrder" runat="server" Text="PLACE ORDER" CssClass="btn btn-primary btn-block"
            OnClick="btnPlaceOrder_Click" ValidationGroup="checkout" />
    </div>

    <script type="text/javascript">
        document.querySelectorAll('.payment-option').forEach(function (option) {
            option.addEventListener('click', function () {
                document.querySelectorAll('.payment-option').forEach(function (o) {
                    o.classList.remove('selected');
                });
                this.classList.add('selected');
                var radio = this.querySelector('input[type="radio"]');
                if (radio) radio.checked = true;
            });
            var radio = option.querySelector('input[type="radio"]');
            if (radio && radio.checked) {
                option.classList.add('selected');
            }
        });
    </script>
</asp:Content>
