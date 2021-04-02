using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace PDCSReporting.MasterPages
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/StockReport.rdlc");
            StockReportDS data = dba.getStockReport();
            ReportDataSource dts = new ReportDataSource("DataSet1", data.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dts);
            ReportViewer1.LocalReport.Refresh();
            ReportViewer1.AsyncRendering = false;
            ReportViewer1.SizeToReportContent = true;


        }
    }
}