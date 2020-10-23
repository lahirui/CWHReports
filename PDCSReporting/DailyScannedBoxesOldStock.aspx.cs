using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class DailyScannedBoxesOldStock : System.Web.UI.Page
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

        protected void calToDate_SelectionChanged(object sender, EventArgs e)
        {
            txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
            calToDate.Visible = false;
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string dateFrom, dateTo = null;
            dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/DailyScannedBoxesDS.rdlc");//DailyScannedBoxesOldStockDS
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/DailyScannedBoxesOldStockDS.rdlc");
            ReportDataSource DailyScannedBoxesDS = new ReportDataSource("DailyScannedBoxesDS", dba.getDetailForDailyScannedBoxesOldStock(dateFrom, dateTo));
            ReportDataSource TotalPiecesAndBoxesDS = new ReportDataSource("TotalPiecesAndBoxesDS", dba.getTotalPiecesAndTotalBoxesScannedOldStock(dateFrom, dateTo));
            //ReportDataSource CMDS = new ReportDataSource("ClockedMinsDs", dba.getCMByTeams(date));


            
            //DailyScannedBoxesDS output = dba.getDetailForDailyScannedBoxesOldStock(dateFrom, dateTo);
            //ReportDataSource ds = new ReportDataSource("DailyScannedBoxesDS", output.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(DailyScannedBoxesDS);
            ReportViewer1.LocalReport.DataSources.Add(TotalPiecesAndBoxesDS);

            ReportParameter fromDate = new ReportParameter("FromDate", dateFrom);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fromDate });
            ReportParameter toDate = new ReportParameter("ToDate", dateTo);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { toDate });
        }
    }
}