using System;
using System.Web.UI;

namespace Clothify.Controls
{
    public partial class StarRatingControl : UserControl
    {
        public int Rating { get; set; }

        public string GetStars()
        {
            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                stars += (i <= Rating) ? "★" : "☆";
            }
            return stars;
        }
    }
}
