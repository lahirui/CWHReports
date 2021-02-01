using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public partial class LocationDisplay : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/LocationDisplay.rdlc");
                LocationDisplayDS data = dba.getLocationDisplayDetails();
                ReportDataSource dts = new ReportDataSource("LocationDisplayDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);
            }
            
        }
    }
}