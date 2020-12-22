using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class CartonWiseStockReport : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                calFromDate.Visible = false;
                calFromDate.SelectedDate = DateTime.Today;
                txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");

                calTodate.Visible = false;
                calTodate.SelectedDate = DateTime.Today;
                txtTodate.Value = calTodate.SelectedDate.ToString("dd/MMM/yyyy");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMyModal();", true);
            }
        }

        protected void GenerateReportButton_Click(object sender, EventArgs e)
        {
            string FromDate = calFromDate.SelectedDate.ToString("yyyy-MM-dd");
            string ToDate = calTodate.SelectedDate.ToString("yyyy-MM-dd");

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/CartonWiseStockReportDesign.rdlc");
            
            CartonWiseStockReportDS Customers = dba.getCartonWiseStockReportDetails(FromDate, ToDate);
            ReportDataSource datasource = new ReportDataSource("CartonWiseStockReportDS", Customers.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportParameter fDate = new ReportParameter("FromDate", FromDate);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fDate });

            ReportParameter tDate = new ReportParameter("ToDate", ToDate);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tDate });
        }

        protected void lbDate_Click(object sender, EventArgs e)
        {
            if (calFromDate.Visible)
                calFromDate.Visible = false;
            else
                calFromDate.Visible = true;
        }

        protected void calFromDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
                e.Day.IsSelectable = false;
        }

        protected void calFromDate_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
            calFromDate.Visible = false;
        }

        protected void lbToDate_Click(object sender, EventArgs e)
        {
            if (calTodate.Visible)
                calTodate.Visible = false;
            else
                calTodate.Visible = true;
        }

        protected void calTodate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
                e.Day.IsSelectable = false;
        }

        protected void calTodate_SelectionChanged(object sender, EventArgs e)
        {
            txtTodate.Value = calTodate.SelectedDate.ToString("dd/MMM/yyyy");
            calTodate.Visible = false;
        }
    }
}