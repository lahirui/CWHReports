using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            YearLabel.Text = "&#169 " + DateTime.Now.Year.ToString() + " CMSL IT. All rights reserved.";
        }

        protected void ReportsLinkButton_Click(object sender, EventArgs e)
        {
            //if (reportDiv.Visible)
            //{
            //    reportDiv.Visible = false;
            //    chartDiv.Visible = false;
            //}

            //else
            //{
            //    reportDiv.Visible = true;
            //    chartDiv.Visible = false;
            //}
        }

        protected void chartsLinkButton_Click(object sender, EventArgs e)
        {
            //if (chartDiv.Visible)
            //{
            //    chartDiv.Visible = false;
            //    reportDiv.Visible = false;
            //}
            //else
            //{
            //    chartDiv.Visible = true;
            //    reportDiv.Visible = false;
            //}
        }

        protected void TicketLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PrintTicket.aspx");
        }

        protected void BundleDetailsLinkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BundleDetails.aspx");
        }
    }
}