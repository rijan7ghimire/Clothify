<%@ Page Title="Feedback" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Clothify.Feedback" %>
<%@ Register Src="~/Controls/StarRating.ascx" TagPrefix="uc" TagName="StarRating" %>
<%@ Register Src="~/Controls/NotificationBar.ascx" TagPrefix="uc" TagName="NotificationBar" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .feedback-container {
            max-width: 500px;
            margin: 0 auto;
            padding: 20px 16px 100px;
        }

        .page-header {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 28px;
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

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            font-size: 11px;
            font-weight: 600;
            color: #555;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            margin-bottom: 8px;
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

        textarea.form-control {
            min-height: 120px;
            resize: vertical;
            font-family: inherit;
        }

        .rating-buttons {
            display: flex;
            gap: 10px;
        }

        .rating-btn {
            width: 48px;
            height: 48px;
            border: 1px solid #ddd;
            border-radius: 8px;
            background-color: #f9f9f9;
            font-size: 16px;
            font-weight: 600;
            color: #555;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.2s;
        }

        .rating-btn:hover {
            border-color: #1a1a1a;
            background-color: #f0f0f0;
        }

        .rating-btn.active {
            background-color: #1a1a1a;
            color: #fff;
            border-color: #1a1a1a;
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

        .success-message {
            background-color: #e8f5e9;
            color: #2e7d32;
            padding: 12px 16px;
            border-radius: 8px;
            font-size: 14px;
            font-weight: 500;
            margin-bottom: 20px;
            text-align: center;
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
    <div class="feedback-container">
        <!-- Header -->
        <div class="page-header">
            <a href="Default.aspx" class="back-arrow">&#8592;</a>
            <h1 class="page-title">FEEDBACK</h1>
        </div>

        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

        <asp:Panel ID="pnlFeedbackForm" runat="server">
            <!-- Select Product -->
            <div class="form-group">
                <label>SELECT PRODUCT</label>
                <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="ddlProduct"
                    ErrorMessage="Please select a product" CssClass="validation-error" Display="Dynamic"
                    ValidationGroup="feedback" InitialValue="" />
            </div>

            <!-- Rating -->
            <div class="form-group">
                <label>OVERALL RATING (1-5)</label>
                <div class="rating-buttons">
                    <button type="button" class="rating-btn" data-value="1" onclick="selectRating(1)">1</button>
                    <button type="button" class="rating-btn" data-value="2" onclick="selectRating(2)">2</button>
                    <button type="button" class="rating-btn" data-value="3" onclick="selectRating(3)">3</button>
                    <button type="button" class="rating-btn" data-value="4" onclick="selectRating(4)">4</button>
                    <button type="button" class="rating-btn" data-value="5" onclick="selectRating(5)">5</button>
                </div>
                <asp:HiddenField ID="hdnRating" runat="server" Value="" />
                <asp:Label ID="lblRatingError" runat="server" CssClass="validation-error" Visible="false">Please select a rating</asp:Label>
            </div>

            <!-- Comments -->
            <div class="form-group">
                <label>COMMENTS</label>
                <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" TextMode="MultiLine"
                    placeholder="Share your experience with this product..."></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvComment" runat="server" ControlToValidate="txtComment"
                    ErrorMessage="Please enter your comments" CssClass="validation-error" Display="Dynamic"
                    ValidationGroup="feedback" />
            </div>

            <!-- Submit Button -->
            <asp:Button ID="btnSubmitFeedback" runat="server" Text="SUBMIT FEEDBACK" CssClass="btn btn-primary btn-block"
                OnClick="btnSubmitFeedback_Click" ValidationGroup="feedback" />
        </asp:Panel>
    </div>

    <script type="text/javascript">
        function selectRating(value) {
            document.getElementById('<%= hdnRating.ClientID %>').value = value;
            var buttons = document.querySelectorAll('.rating-btn');
            buttons.forEach(function (btn) {
                btn.classList.remove('active');
                if (parseInt(btn.getAttribute('data-value')) <= value) {
                    btn.classList.add('active');
                }
            });
        }

        // Restore rating state on postback
        document.addEventListener('DOMContentLoaded', function () {
            var hiddenField = document.getElementById('<%= hdnRating.ClientID %>');
            if (hiddenField && hiddenField.value) {
                selectRating(parseInt(hiddenField.value));
            }
        });
    </script>
</asp:Content>
