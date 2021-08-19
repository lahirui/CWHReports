using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace PDCSReporting.MasterPages
{
    public partial class WebForm4 : System.Web.UI.Page
    {

        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {


            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/ConsolidatedStockReportN.rdlc");
            //ConsolidatedStockReportDS data = dba.getConsolidatedStockReport();
            //ReportDataSource dts = new ReportDataSource("DataSet1", data.Tables[0]);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(dts);

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/ConsolidatedStReport.rdlc");

            ReportDataSource ConsolidatedStockReportDS = new ReportDataSource("DataSet1", dba.getConsolidatedStockReport());


            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(ConsolidatedStockReportDS);
        }
    }
}

