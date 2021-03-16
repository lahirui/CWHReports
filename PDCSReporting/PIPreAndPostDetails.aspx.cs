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
    public partial class PIPreAndPostDetails : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        string factoryName;
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

                DataSet dsRacks = new DataSet();
                dsRacks = com.ReturnDataSet("SELECT Id, Code FROM     Locations WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsRacks.Tables[0].Rows.Count > 0)
                {
                    ddlFromLocation.DataSource = dsRacks.Tables[0];
                    ddlFromLocation.DataTextField = "Code";
                    ddlFromLocation.DataValueField = "Id";
                    ddlFromLocation.DataBind();

                    ddlToLocation.DataSource = dsRacks.Tables[0];
                    ddlToLocation.DataTextField = "Code";
                    ddlToLocation.DataValueField = "Id";
                    ddlToLocation.DataBind();


                }

                DataSet dsPallets = new DataSet();
                dsPallets = com.ReturnDataSet("SELECT Id, Code FROM     Pallets WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsPallets.Tables[0].Rows.Count > 0)
                {
                    ddlFromPallet.DataSource = dsPallets.Tables[0];
                    ddlFromPallet.DataTextField = "Code";
                    ddlFromPallet.DataValueField = "Id";
                    ddlFromPallet.DataBind();

                    ddlToPallet.DataSource = dsPallets.Tables[0];
                    ddlToPallet.DataTextField = "Code";
                    ddlToPallet.DataValueField = "Id";
                    ddlToPallet.DataBind();


                }

                string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
                string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");
                DataSet dsPI = new DataSet();
                dsPI = com.ReturnDataSet("SELECT  Id, PIReference FROM     PIs WHERE(IsDeleted = 0) AND(CAST(CreatedDate AS DATE) >= '" + dateFrom + "') AND(CAST(CreatedDate AS DATE) <= '" + dateTo + "') ORDER BY PIReference");
                if (dsPI.Tables[0].Rows.Count > 0)
                {
                    ddlFromPI.DataSource = dsPI.Tables[0];
                    ddlFromPI.DataTextField = "PIReference";
                    ddlFromPI.DataValueField = "Id";
                    ddlFromPI.DataBind();

                    ddlToPI.DataSource = dsPI.Tables[0];
                    ddlToPI.DataTextField = "PIReference";
                    ddlToPI.DataValueField = "Id";
                    ddlToPI.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
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

            string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");
            DataSet dsPI = new DataSet();
            dsPI = com.ReturnDataSet("SELECT  Id, PIReference FROM     PIs WHERE(IsDeleted = 0) AND(CAST(CreatedDate AS DATE) >= '" + dateFrom + "') AND(CAST(CreatedDate AS DATE) <= '" + dateTo + "') ORDER BY PIReference");
            if (dsPI.Tables[0].Rows.Count > 0)
            {
                ddlFromPI.DataSource = dsPI.Tables[0];
                ddlFromPI.DataTextField = "PIReference";
                ddlFromPI.DataValueField = "Id";
                ddlFromPI.DataBind();

                ddlToPI.DataSource = dsPI.Tables[0];
                ddlToPI.DataTextField = "PIReference";
                ddlToPI.DataValueField = "Id";
                ddlToPI.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
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
            ddlFromPI.Items.Clear();
            ddlToPI.Items.Clear();
            string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");
            DataSet dsPI = new DataSet();
            dsPI = com.ReturnDataSet("SELECT  Id, PIReference FROM     PIs WHERE(IsDeleted = 0) AND(CAST(CreatedDate AS DATE) >= '" + dateFrom + "') AND(CAST(CreatedDate AS DATE) <= '" + dateTo + "') ORDER BY PIReference");
            if (dsPI.Tables[0].Rows.Count > 0)
            {
                ddlFromPI.DataSource = dsPI.Tables[0];
                ddlFromPI.DataTextField = "PIReference";
                ddlFromPI.DataValueField = "Id";
                ddlFromPI.DataBind();

                ddlToPI.DataSource = dsPI.Tables[0];
                ddlToPI.DataTextField = "PIReference";
                ddlToPI.DataValueField = "Id";
                ddlToPI.DataBind();
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
        }

        protected void ddlToPI_DataBound(object sender, EventArgs e)
        {
            ddlToPI.SelectedIndex = ddlFromPI.Items.Count - 1;
        }

        protected void ddlToPallet_DataBound(object sender, EventArgs e)
        {
            ddlToPallet.SelectedIndex = ddlFromPallet.Items.Count - 1;
        }

        protected void ddlToLocation_DataBound(object sender, EventArgs e)
        {
            ddlToLocation.SelectedIndex = ddlToLocation.Items.Count - 1;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            
            string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");
            string fromPI = ddlFromPI.SelectedItem.Text;
            string toPI = ddlToPI.SelectedItem.Text;
            string fromPallet = ddlFromPallet.SelectedItem.Text;
            string toPallet = ddlToPallet.SelectedItem.Text;
            string fromLocation = ddlFromLocation.SelectedItem.Text;
            string toLocation = ddlToLocation.SelectedItem.Text;

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PIPreAndPostDetails.rdlc");
            PIPreAndPostDetailsDS output = dba.getPIPreAndPostDetails(dateFrom, dateTo, fromPI,toPI,fromPallet,toPallet,fromLocation,toLocation);
            ReportDataSource ds = new ReportDataSource("PIPreAndPostDetailsDS", output.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(ds);

            ReportParameter fromDate = new ReportParameter("FromDate", dateFrom);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fromDate });

            ReportParameter toDate = new ReportParameter("ToDate", dateTo);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { toDate });

            ReportParameter fFac = new ReportParameter("FromPI", fromPI);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fFac });

            ReportParameter tFac = new ReportParameter("ToPI", toPI);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tFac });

            ReportParameter fAOD = new ReportParameter("FromLocation", fromLocation);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fAOD });

            ReportParameter tAOD = new ReportParameter("ToLocation", toLocation);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tAOD });

            ReportParameter fCPO = new ReportParameter("FromPallet", fromPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fCPO });

            ReportParameter tCPO = new ReportParameter("ToPallet", toPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tCPO });

        }
    }

}