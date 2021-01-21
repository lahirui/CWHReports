using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class FactoryDAOD : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        string LorryNumber;
        string destination;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "AODsModal();", true);
            }
        }

        protected void GenerateReportButton_Click(object sender, EventArgs e)
        {
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/AODDetailsReportDesign.rdlc");

            ReportDataSource AODDetailsDS = new ReportDataSource("AODDetailsDS", dba.getAODDetailsforPrintN(Convert.ToInt32(AODNumberDropDownList.SelectedItem.Value)));
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(AODDetailsDS);

            LorryNumber = dba.getLorryNumber(Convert.ToInt32(AODNumberDropDownList.SelectedItem.Value));
            destination = dba.getDestination(Convert.ToInt32(AODNumberDropDownList.SelectedItem.Value));
            if (LorryNumber == "" || LorryNumber==null)
            {
                LorryNumber = "N/A";
            }
           

            if (destination == "")
            {
                destination = "N/A";
            }
            

            ReportParameter AODNop = new ReportParameter("AODNumber", AODNumberDropDownList.SelectedItem.Text);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { AODNop });

            ReportParameter LorryNumberp = new ReportParameter("LorryNumber", LorryNumber);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { LorryNumberp });

            ReportParameter Destinationp = new ReportParameter("Destination", destination);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { Destinationp });
        }

        protected void AODNumberDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
        }
    }
}