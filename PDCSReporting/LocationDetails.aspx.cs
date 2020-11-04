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
    public partial class LocationDetails : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                DataSet dsLocations = new DataSet();
                dsLocations = com.ReturnDataSet("SELECT Id, Code FROM     Locations WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsLocations.Tables[0].Rows.Count > 0)
                {
                    ddlFromLocation.DataSource = dsLocations.Tables[0];
                    ddlFromLocation.DataTextField = "Code";
                    ddlFromLocation.DataValueField = "Id";
                    ddlFromLocation.DataBind();

                    ddlToLocation.DataSource = dsLocations.Tables[0];
                    ddlToLocation.DataTextField = "Code";
                    ddlToLocation.DataValueField = "Id";
                    ddlToLocation.DataBind();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void ddlToLocation_DataBound(object sender, EventArgs e)
        {
            ddlToLocation.SelectedIndex = ddlFromLocation.Items.Count - 1;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddlFromLocation.SelectedIndex >= 0)
            {
                string fromLocation = ddlFromLocation.SelectedItem.Text;
                string toLocation = ddlToLocation.SelectedItem.Text;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/LocationDetails.rdlc");
                LocationDetailsDS data = dba.getLocationDetails(fromLocation,toLocation);
                ReportDataSource dts = new ReportDataSource("LocationDetailsDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);

                ReportParameter fLocation = new ReportParameter("FromLocation", fromLocation);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fLocation });

                ReportParameter tLocation = new ReportParameter("ToLocation", toLocation);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tLocation });
            }
        }
    }
}