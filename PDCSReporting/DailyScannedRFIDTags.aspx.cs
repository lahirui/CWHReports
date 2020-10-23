using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class DailyScannedRFIDTags : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                calFromDate.Visible = false;
                calToDate.Visible = false;
                calFromDate.SelectedDate = DateTime.Today;
                calToDate.SelectedDate = DateTime.Today;
                txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
                txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMyModal();", true);
            }
        }

        protected void lbFromDate_Click(object sender, EventArgs e)
        {

            if (calFromDate.Visible)
            {
                calFromDate.Visible = false;
            }
            else
            {
                calFromDate.Visible = true;
            }
        }

        protected void calFromDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
            }
        }

        protected void calFromDate_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
            calFromDate.Visible = false;
        }

        protected void lbToDate_Click(object sender, EventArgs e)
        {
            if (calToDate.Visible)
            {
                calToDate.Visible = false;
            }
            else
            {
                calToDate.Visible = true;
            }
        }

        protected void calToDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
            }
        }

        protected void calToDate_DayRender1(object sender, DayRenderEventArgs e)
        {
            txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
            calToDate.Visible = false;
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string dateFrom, dateTo = null;
            dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");

            rvTeamOutput.ProcessingMode = ProcessingMode.Local;
            rvTeamOutput.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/DailyScannedRFIDTagsDS.rdlc");
            DailyScannedRFIDTagsDS output = dba.getDetailForDailyScannedRFIDTags(dateFrom, dateTo);
            ReportDataSource ds = new ReportDataSource("DailyScannedRFIDTagsDS", output.Tables[0]);
            rvTeamOutput.LocalReport.DataSources.Clear();
            rvTeamOutput.LocalReport.DataSources.Add(ds);

            ReportParameter fromDate = new ReportParameter("FromDate", dateFrom);
            this.rvTeamOutput.LocalReport.SetParameters(new ReportParameter[] { fromDate });
            ReportParameter toDate = new ReportParameter("ToDate", dateTo);
            this.rvTeamOutput.LocalReport.SetParameters(new ReportParameter[] { toDate });
        }
    }
}