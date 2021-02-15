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
    public partial class BoxMovementReport : System.Web.UI.Page
    {
        Common com = new Common();
        DBAccess db = new DBAccess();
        string SourceFactoryName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                DataSet dsAOD = new DataSet();
                dsAOD = com.ReturnDataSet("SELECT Id, BoxCode FROM Boxes ORDER BY BoxCode");
                if (dsAOD.Tables[0].Rows.Count > 0)
                {
                    ddlBoxCode.DataSource = dsAOD.Tables[0];
                    ddlBoxCode.DataValueField = "Id";
                    ddlBoxCode.DataTextField = "BoxCode";
                    ddlBoxCode.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string BoxCode = ddlBoxCode.SelectedItem.Text;
            int BoxCodeId = Convert.ToInt32(ddlBoxCode.SelectedValue);
           
            string SourceFactory = "N/A";
            string AODNumber = "N/A";
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/BoxMovementReport.rdlc");
            BoxMovementReportDS data = db.getBoxMovementDetails(BoxCodeId);
            ReportDataSource dts = new ReportDataSource("BoxMovementReportDS", data.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(dts);

            DataSet dsFac = new DataSet();
            dsFac = com.ReturnDataSet("SELECT TOP (200) ParamValue FROM     Configurations WHERE(ParamName = N'FactoryName')");
            if (dsFac.Tables[0].Rows.Count > 0)
            {
                SourceFactoryName = dsFac.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
            }
            DataSet dsDetails = new DataSet();
            dsDetails = com.ReturnDataSet("SELECT dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber " +
                                        "FROM     dbo.AODBoxDetails INNER JOIN " +
                                                          "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id " +
                                        "WHERE(dbo.AODBoxDetails.BoxId = "+BoxCodeId+") AND(dbo.AODs.SourceWarehouse <> '"+SourceFactoryName+"') "+
                                        "ORDER BY dbo.AODs.SourceWarehouse ");
            if (dsDetails.Tables[0].Rows.Count > 0)
            {
                SourceFactory = dsDetails.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
                AODNumber= dsDetails.Tables[0].Rows[0].ItemArray.GetValue(1).ToString();
            }

            ReportParameter Box = new ReportParameter("BoxCode", BoxCode);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { Box });

            ReportParameter Fac = new ReportParameter("Factory", SourceFactory);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { Fac });

            ReportParameter Aod = new ReportParameter("AOD", AODNumber);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { Aod });
        }
    }
}