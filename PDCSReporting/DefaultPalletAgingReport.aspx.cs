
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace PDCSReporting
{
    public partial class DefaultPalletAgingReport : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {


            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/DefaultPalletAgingReport.rdlc");

            ReportDataSource DefaultPalletAgingReportDS = new ReportDataSource("DefaultPalletAgingReportDS", dba.getDPAgingReport());


            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(DefaultPalletAgingReportDS);


        }
    }
}

