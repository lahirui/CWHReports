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
    public partial class SingleScannedRFIDs : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                DataSet dsLocations = new DataSet();
                dsLocations = com.ReturnDataSet("SELECT Id, Code FROM     Styles WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsLocations.Tables[0].Rows.Count > 0)
                {
                    ddlFromStyle.DataSource = dsLocations.Tables[0];
                    ddlFromStyle.DataTextField = "Code";
                    ddlFromStyle.DataValueField = "Id";
                    ddlFromStyle.DataBind();

                    ddlToStyle.DataSource = dsLocations.Tables[0];
                    ddlToStyle.DataTextField = "Code";
                    ddlToStyle.DataValueField = "Id";
                    ddlToStyle.DataBind();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
            }
        }

        protected void ddlToStyle_DataBound(object sender, EventArgs e)
        {
            ddlToStyle.SelectedIndex = ddlFromStyle.Items.Count - 1;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddlFromStyle.SelectedIndex >= 0)
            {
                string fromstyle = ddlFromStyle.SelectedItem.Text;
                string tostyle = ddlToStyle.SelectedItem.Text;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/SingleScannedRFIDsDS.rdlc");
                SingleScannedRFIDsDS data = dba.getSingleScannedRFIDDetails(fromstyle, tostyle);
                ReportDataSource dts = new ReportDataSource("SingleScannedRFIDsDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);

                ReportParameter fLocation = new ReportParameter("FromStyle", fromstyle);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fLocation });

                ReportParameter tLocation = new ReportParameter("ToStyle", tostyle);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tLocation });
            }
        }

        protected void ddlFromStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToStyle.SelectedIndex = ddlFromStyle.SelectedIndex;
        }
    }
}