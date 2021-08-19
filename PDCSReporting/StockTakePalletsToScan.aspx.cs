using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class StockTakePalletsToScan : System.Web.UI.Page
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
                txtToDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);


                //$("#yourDropDownListId option[value='0']").hide();
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

        protected void FromDateLinkButton_Click(object sender, EventArgs e)
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

        protected void ToDateLinkButton_Click(object sender, EventArgs e)
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

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string DateFrom = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
            string DateTo = calToDate.SelectedDate.ToString("dd/MMM/yyyy");

            int palletCountInHeader = dba.getPalletCountIncartonHeader();
            int palletCountInPostCount = dba.getpalletCountInPostCount(DateFrom, DateTo);
            int palletCount = palletCountInHeader - palletCountInPostCount;

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PalletsToCountReportDesign.rdlc");

            ReportDataSource PalletsToCountDS = new ReportDataSource("PalletsToCountDS", dba.getStockTakeRemainingPalletsToScan(DateFrom, DateTo));

            ReportViewer1.LocalReport.DataSources.Clear();

            ReportViewer1.LocalReport.DataSources.Add(PalletsToCountDS);

            //Passing Text Box values (Date and Team) to report 
            ReportParameter fromDatep = new ReportParameter("FromDate", DateFrom);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fromDatep });

            ReportParameter toDatep = new ReportParameter("ToDate", DateTo);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { toDatep });

            ReportParameter palletCountp = new ReportParameter("PalletCount", palletCount.ToString());
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { palletCountp });
        }
    }
}