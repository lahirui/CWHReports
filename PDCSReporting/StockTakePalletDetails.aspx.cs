using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class StockTakePalletDetails : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Disabled = true;
                txtTodate.Disabled = true;
                
                calFromDate.Visible = false;
                calFromDate.SelectedDate = DateTime.Today;
                txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");

                calTodate.Visible = false;
                calTodate.SelectedDate = DateTime.Today;
                txtTodate.Value = calTodate.SelectedDate.ToString("dd/MMM/yyyy");


                DataSet dsPallets = new DataSet();
                dsPallets = com.ReturnDataSet("SELECT  Id, Code FROM     Pallets WHERE(IsDeleted = 0) ORDER BY Code");
                ddlFromPallet.DataSource = dsPallets.Tables[0];
                ddlFromPallet.DataTextField = "Code";
                ddlFromPallet.DataValueField = "Id";
                ddlFromPallet.DataBind();
                // ddlFromPallet.Items.Insert(0, "-- SELECT AREA --");

                ddlToPallet.DataSource = dsPallets.Tables[0];
                ddlToPallet.DataTextField = "Code";
                ddlToPallet.DataValueField = "Id";
                ddlToPallet.DataBind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openMyModal();", true);

            }
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

        protected void GenerateReportButton_Click(object sender, EventArgs e)
        {
            string FromDate = calFromDate.SelectedDate.ToString("yyyy-MM-dd");
            string ToDate = calTodate.SelectedDate.ToString("yyyy-MM-dd");
            string FromPallet = ddlFromPallet.SelectedItem.Text;
            string ToPallet = ddlToPallet.SelectedItem.Text;

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/StockTakePalletDetails.rdlc");
            StockTakePalletDetailsDS Customers = dba.getStockTakePalletDetails(FromDate, ToDate,FromPallet, ToPallet);
            ReportDataSource datasource = new ReportDataSource("StockTakePalletDetails", Customers.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);

            ReportParameter fDate = new ReportParameter("FromDate", FromDate);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fDate });

            ReportParameter tDate = new ReportParameter("ToDate", ToDate);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tDate });

            ReportParameter fPallet = new ReportParameter("FromPallet", FromPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fPallet });

            ReportParameter tPallet = new ReportParameter("ToPallet", ToPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tPallet });
        }

        protected void ddlToPallet_DataBound(object sender, EventArgs e)
        {
            ddlToPallet.SelectedIndex = ddlFromPallet.Items.Count - 1;
        }
    }
}