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
    public partial class ScannedReportCPOWise : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        DataTable details;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CPODropDownList.Items.Insert(0, new ListItem("-- Select a CPO --", "0"));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void CPODropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* if (CPODropDownList.SelectedIndex != 0)
            {
                SODropDownList.Items.Clear();

                details = dba.getSOByCPOId(CPODropDownList.SelectedItem.Text);
                SODropDownList.DataSource = details;
                SODropDownList.DataTextField = "SO";
                SODropDownList.DataValueField = "SO";
                SODropDownList.DataBind();
            }
            else
            {
                SODropDownList.Items.Clear();
            }*/
        }

        protected void GenerateReportButton_Click(object sender, EventArgs e)
        {
            string CPOName = CPODropDownList.SelectedItem.Text;
            //string SO = SODropDownList.SelectedItem.Text;

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/ScannedDetailsCPOWiseReportDesign.rdlc");

            ReportDataSource WarehouseInDetailsDs = new ReportDataSource("ScannedDetailsCPOWiseDS", dba.getScannedBoxDetailsCPOWise(CPOName));
            ReportDataSource WarehouseInSummaryDs = new ReportDataSource("ScannedDetailsCPOWiseSummaryDS", dba.getSummarizedScannedDetailsByCPO(CPOName));

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(WarehouseInDetailsDs);
            ReportViewer1.LocalReport.DataSources.Add(WarehouseInSummaryDs);

            //Passing Text Box values (Date and Team) to report 
            ReportParameter CPOp = new ReportParameter("CPO", CPOName);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { CPOp });
        }
    }
}