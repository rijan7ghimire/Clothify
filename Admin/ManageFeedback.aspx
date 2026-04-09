<%@ Page Title="Manage Feedback" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeBehind="ManageFeedback.aspx.cs" Inherits="Clothify.Admin.ManageFeedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AdminContent" runat="server">
    <style>
        .page-title { font-size: 18px; font-weight: 800; color: #111; margin-bottom: 16px; }
        .filter-tabs {
            display: flex; gap: 6px; margin-bottom: 16px; overflow-x: auto;
            padding-bottom: 4px; flex-wrap: wrap;
        }
        .filter-tab {
            padding: 7px 14px; border-radius: 20px; font-size: 12px; font-weight: 600;
            border: 1px solid #ddd; background: #fff; color: #666; cursor: pointer;
            white-space: nowrap; text-decoration: none; text-align: center;
        }
        .filter-tab:hover { border-color: #111; color: #111; }
        .filter-tab.active { background: #111; color: #fff; border-color: #111; }
        .card {
            background: #fff; border-radius: 12px; padding: 16px;
            margin-bottom: 12px; box-shadow: 0 1px 4px rgba(0,0,0,0.06); border: 1px solid #f0f0f0;
        }
        .feedback-header {
            display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 6px;
        }
        .feedback-id { font-size: 11px; color: #999; }
        .feedback-customer { font-size: 14px; font-weight: 700; color: #111; }
        .feedback-product { font-size: 12px; color: #666; margin-bottom: 4px; }
        .feedback-comment {
            font-size: 13px; color: #444; margin: 8px 0;
            background: #fafafa; padding: 10px 12px; border-radius: 8px;
            line-height: 1.5;
        }
        .feedback-date { font-size: 11px; color: #999; }
        .stars { color: #f9a825; font-size: 16px; letter-spacing: 1px; }
        .stars .empty { color: #ddd; }
        .btn {
            padding: 8px 16px; border: none; border-radius: 8px;
            font-size: 13px; font-weight: 600; cursor: pointer; transition: opacity 0.15s;
        }
        .btn:hover { opacity: 0.85; }
        .btn-danger { background: #e53935; color: #fff; }
        .btn-sm { padding: 6px 12px; font-size: 12px; }
        .alert {
            padding: 12px 16px; border-radius: 10px; margin-bottom: 16px;
            font-size: 13px; font-weight: 500;
        }
        .alert-success { background: #e8f5e9; color: #2e7d32; }
        .alert-danger { background: #ffebee; color: #c62828; }
        .no-data { text-align: center; padding: 24px; color: #999; font-size: 14px; }
        .feedback-footer {
            display: flex; justify-content: space-between; align-items: center; margin-top: 8px;
        }
    </style>

    <h1 class="page-title">Manage Feedback</h1>

    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
        <div class="alert" id="divMessage" runat="server"></div>
    </asp:Panel>

    <!-- FILTER BY RATING -->
    <div class="filter-tabs">
        <asp:LinkButton ID="btnFilterAll" runat="server" CssClass="filter-tab active" Text="All" CommandArgument="0" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilter5" runat="server" CssClass="filter-tab" Text="5 Stars" CommandArgument="5" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilter4" runat="server" CssClass="filter-tab" Text="4 Stars" CommandArgument="4" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilter3" runat="server" CssClass="filter-tab" Text="3 Stars" CommandArgument="3" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilter2" runat="server" CssClass="filter-tab" Text="2 Stars" CommandArgument="2" OnClick="btnFilter_Click" CausesValidation="false" />
        <asp:LinkButton ID="btnFilter1" runat="server" CssClass="filter-tab" Text="1 Star" CommandArgument="1" OnClick="btnFilter_Click" CausesValidation="false" />
    </div>

    <!-- FEEDBACK LIST -->
    <asp:Repeater ID="rptFeedback" runat="server" OnItemCommand="rptFeedback_ItemCommand">
        <ItemTemplate>
            <div class="card">
                <div class="feedback-header">
                    <div>
                        <div class="feedback-customer"><%# Eval("CustomerName") %></div>
                        <div class="feedback-id">Feedback #<%# Eval("FeedbackID") %></div>
                    </div>
                    <div class="stars">
                        <%# GenerateStars(Convert.ToInt32(Eval("Rating"))) %>
                    </div>
                </div>
                <div class="feedback-product">Product: <%# Eval("ProductName") %></div>
                <div class="feedback-comment"><%# Server.HtmlEncode(Eval("Comment").ToString()) %></div>
                <div class="feedback-footer">
                    <span class="feedback-date"><%# Eval("FeedbackDate", "{0:yyyy-MM-dd}") %></span>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                        CommandName="DeleteFeedback" CommandArgument='<%# Eval("FeedbackID") %>'
                        OnClientClick="return confirm('Are you sure you want to delete this feedback?');" CausesValidation="false" />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Panel ID="pnlNoData" runat="server" Visible="false">
        <div class="no-data">No feedback found.</div>
    </asp:Panel>
</asp:Content>
