using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Clothify.App_Code;

namespace Clothify.Admin
{
    public partial class ManageFeedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RatingFilter"] = 0;
                LoadFeedback(0);
            }
        }

        private void LoadFeedback(int ratingFilter)
        {
            string query = @"SELECT f.FeedbackID, f.Rating, f.Comment, f.FeedbackDate,
                             u.FullName AS CustomerName, p.ProductName
                             FROM Feedback f
                             INNER JOIN Users u ON f.UserID = u.UserID
                             INNER JOIN Products p ON f.ProductID = p.ProductID";

            DataTable dt;

            if (ratingFilter > 0)
            {
                query += " WHERE f.Rating = @Rating";
                query += " ORDER BY f.FeedbackDate DESC";
                dt = DBHelper.ExecuteQuery(query, new SqlParameter("@Rating", ratingFilter));
            }
            else
            {
                query += " ORDER BY f.FeedbackDate DESC";
                dt = DBHelper.ExecuteQuery(query);
            }

            if (dt.Rows.Count > 0)
            {
                rptFeedback.DataSource = dt;
                rptFeedback.DataBind();
                pnlNoData.Visible = false;
            }
            else
            {
                rptFeedback.DataSource = null;
                rptFeedback.DataBind();
                pnlNoData.Visible = true;
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int rating = Convert.ToInt32(btn.CommandArgument);
            ViewState["RatingFilter"] = rating;

            // Update active tab styling
            btnFilterAll.CssClass = "filter-tab";
            btnFilter5.CssClass = "filter-tab";
            btnFilter4.CssClass = "filter-tab";
            btnFilter3.CssClass = "filter-tab";
            btnFilter2.CssClass = "filter-tab";
            btnFilter1.CssClass = "filter-tab";
            btn.CssClass = "filter-tab active";

            LoadFeedback(rating);
        }

        protected void rptFeedback_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteFeedback")
            {
                int feedbackId = Convert.ToInt32(e.CommandArgument);

                DBHelper.ExecuteNonQuery("DELETE FROM Feedback WHERE FeedbackID = @FeedbackID",
                    new SqlParameter("@FeedbackID", feedbackId));

                ShowMessage("Feedback deleted successfully.", true);

                int ratingFilter = ViewState["RatingFilter"] != null ? Convert.ToInt32(ViewState["RatingFilter"]) : 0;
                LoadFeedback(ratingFilter);
            }
        }

        protected string GenerateStars(int rating)
        {
            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating)
                    stars += "<span>&#9733;</span>";
                else
                    stars += "<span class='empty'>&#9733;</span>";
            }
            return stars;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            divMessage.InnerText = message;
            divMessage.Attributes["class"] = isSuccess ? "alert alert-success" : "alert alert-danger";
        }
    }
}
