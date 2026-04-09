using System;
using System.Data;
using System.Data.SqlClient;
using Clothify.App_Code;

namespace Clothify
{
    public partial class Feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            string query = "SELECT ProductID, ProductName FROM Products ORDER BY ProductName";
            DataTable dt = DBHelper.ExecuteQuery(query);

            ddlProduct.Items.Clear();
            ddlProduct.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select a Product --", ""));

            foreach (DataRow row in dt.Rows)
            {
                ddlProduct.Items.Add(new System.Web.UI.WebControls.ListItem(
                    row["ProductName"].ToString(),
                    row["ProductID"].ToString()
                ));
            }
        }

        protected void btnSubmitFeedback_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Validate rating
            string ratingValue = hdnRating.Value;
            int rating;
            if (string.IsNullOrEmpty(ratingValue) || !int.TryParse(ratingValue, out rating) || rating < 1 || rating > 5)
            {
                lblRatingError.Visible = true;
                return;
            }

            lblRatingError.Visible = false;

            int userId = Convert.ToInt32(Session["UserID"]);
            int productId = Convert.ToInt32(ddlProduct.SelectedValue);
            string comment = txtComment.Text.Trim();

            try
            {
                string query = @"INSERT INTO Feedback (Rating, Comment, FeedbackDate, UserID, ProductID)
                                 VALUES (@Rating, @Comment, @FeedbackDate, @UserID, @ProductID)";

                DBHelper.ExecuteNonQuery(query,
                    new SqlParameter("@Rating", rating),
                    new SqlParameter("@Comment", comment),
                    new SqlParameter("@FeedbackDate", DateTime.Now),
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@ProductID", productId)
                );

                // Show success message
                lblMessage.Text = "Thank you! Your feedback has been submitted successfully.";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;

                // Reset form
                ddlProduct.SelectedIndex = 0;
                hdnRating.Value = "";
                txtComment.Text = "";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred while submitting your feedback. Please try again.";
                lblMessage.CssClass = "error-message";
                lblMessage.Visible = true;
            }
        }
    }
}
