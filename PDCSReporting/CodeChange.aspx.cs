using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


namespace PDCSReporting
{
    public partial class CodeChange : System.Web.UI.Page
    {

        DBAccess dba = new DBAccess();
        Common com = new Common();
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
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);




        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/CodeChangeReport.rdlc");

            ReportDataSource CodeChangeDS = new ReportDataSource("CodeChangeDS", dba.getCodeChangeReDetails(dateFrom, dateTo));


            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(CodeChangeDS);




        }

        protected void calToDate_SelectionChanged(object sender, EventArgs e)
        {
            txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
            calToDate.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2()", true);


        }

        protected void calFromDate_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
            calFromDate.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2()", true);

        }

        protected void calToDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
            }


        }

        protected void calFromDate_DayRender(object sender, DayRenderEventArgs e)
        {

            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
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
    }
}