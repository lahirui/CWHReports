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
    public partial class AODCheckList : System.Web.UI.Page
    {
        Common com = new Common();
        DBAccess db = new DBAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataSet dsAOD = new DataSet();
                dsAOD = com.ReturnDataSet("SELECT Id, AODNumber FROM AODs WHERE(IsDeleted = 0) ORDER BY AODNumber");
                if (dsAOD.Tables[0].Rows.Count > 0)
                {
                    ddlAodNumbers.DataSource = dsAOD.Tables[0];
                    ddlAodNumbers.DataValueField = "Id";
                    ddlAodNumbers.DataTextField = "AODNumber";
                    ddlAodNumbers.DataBind();

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string AODNo = ddlAodNumbers.SelectedItem.Text;
            int AODId = Convert.ToInt32(ddlAodNumbers.SelectedValue);

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/AODCheckList.rdlc");
            AODCheckListDS data = db.getAODCheckListDetails(AODNo);
            ReportDataSource dts = new ReportDataSource("AODCheckListDS", data.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dts);

            ReportParameter AOD = new ReportParameter("AODNumber", AODNo);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { AOD });
        }

        protected void ddlAodNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
        }
    }
}