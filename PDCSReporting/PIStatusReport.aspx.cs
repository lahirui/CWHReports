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
    public partial class WebForm2 : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                DataSet dsCountRef = new DataSet();
                dsCountRef = com.ReturnDataSet("SELECT Id, PIReference FROM PIs WHERE(CAST(CreatedDate AS date) > '01/Apr/2021') ORDER BY PIReference ");
                if (dsCountRef.Tables[0].Rows.Count > 0)
                {
                    ddlPIReference.DataSource = dsCountRef.Tables[0];
                    ddlPIReference.DataTextField = "PIReference";
                    ddlPIReference.DataValueField = "Id";
                    ddlPIReference.DataBind();

               }

            }

            }

        protected void ddlPIReference_SelectedIndexChanged(object sender, EventArgs e)
        {





        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {

            string PIRefID = ddlPIReference.SelectedValue;
            //string SO = SODropDownList.SelectedItem.Text;

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PIStatus.rdlc");

            ReportDataSource PIStatusDs = new ReportDataSource("DataSet1", dba.getPIStatus(PIRefID));
            

            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(PIStatusDs);


            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PIStatus.rdlc");
            //PIStatusDs data = dba.getPIStatus(PIRefID);
            //ReportDataSource dts = new ReportDataSource("DataSet1", data.Tables[0]);
            //ReportViewer1.LocalReport.DataSources.Clear();
            //ReportViewer1.LocalReport.DataSources.Add(dts);
            //ReportViewer1.LocalReport.Refresh();
            //ReportViewer1.AsyncRendering = false;
            //ReportViewer1.SizeToReportContent = true;





        }
    }
}