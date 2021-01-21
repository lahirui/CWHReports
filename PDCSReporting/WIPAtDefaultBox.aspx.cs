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
    public partial class WIPAtDefaultBox : System.Web.UI.Page
    {
        Common com = new Common();
        DBAccess db = new DBAccess();
        int Boxid;
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


                
            }
        }

        protected void ddlFromStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlToStyle.SelectedIndex = ddlFromStyle.SelectedIndex;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
        }

        protected void ddlToStyle_DataBound(object sender, EventArgs e)
        {
            ddlToStyle.SelectedIndex = ddlFromStyle.Items.Count - 1;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string FromStyle = ddlFromStyle.SelectedItem.Text;
            string ToStyle = ddlToStyle.SelectedItem.Text;
            DataSet dsBox = new DataSet();
            dsBox = com.ReturnDataSet("SELECT ParamValue FROM Configurations WHERE(ParamName = N'DefaultBoxId')");
            if (dsBox.Tables[0].Rows.Count > 0)
            {
                Boxid = Convert.ToInt32(dsBox.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());
            }

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/WIPAtDefaultBox.rdlc");
            WIPAtDefaultBoxDS data = db.getDefaultBoxDetails(Boxid, FromStyle, ToStyle);
            ReportDataSource dts = new ReportDataSource("WIPAtDefaultBoxDS", data.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dts);

            ReportParameter fLocation = new ReportParameter("FromStyle", FromStyle);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fLocation });

            ReportParameter tLocation = new ReportParameter("ToStyle", ToStyle);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tLocation });

        }
    }
}