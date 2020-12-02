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
    public partial class StockDetailedReport : System.Web.UI.Page
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
                DataSet dsRacks = new DataSet();
                dsRacks = com.ReturnDataSet("SELECT Id, Code FROM     Locations WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsRacks.Tables[0].Rows.Count > 0)
                {
                    ddlFromRack.DataSource = dsRacks.Tables[0];
                    ddlFromRack.DataTextField = "Code";
                    ddlFromRack.DataValueField = "Id";
                    ddlFromRack.DataBind();

                    ddlToRack.DataSource = dsRacks.Tables[0];
                    ddlToRack.DataTextField = "Code";
                    ddlToRack.DataValueField = "Id";
                    ddlToRack.DataBind();


                }

                DataSet dsPallets = new DataSet();
                dsPallets = com.ReturnDataSet("SELECT Id, Code FROM     Pallets WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsPallets.Tables[0].Rows.Count > 0)
                {
                    ddlFromPallet.DataSource = dsPallets.Tables[0];
                    ddlFromPallet.DataTextField = "Code";
                    ddlFromPallet.DataValueField = "Id";
                    ddlFromPallet.DataBind();

                    ddlToPallet.DataSource = dsPallets.Tables[0];
                    ddlToPallet.DataTextField = "Code";
                    ddlToPallet.DataValueField = "Id";
                    ddlToPallet.DataBind();


                }

                DataSet dsCPO = new DataSet();
                dsCPO = com.ReturnDataSet("SELECT DISTINCT CPO FROM dbo.BoxCPOAllocationDetails ORDER BY CPO");
                if (dsCPO.Tables[0].Rows.Count > 0)
                {
                    ddlFromCPO.DataSource = dsCPO.Tables[0];
                    ddlFromCPO.DataTextField = "CPO";
                    ddlFromCPO.DataValueField = "CPO";
                    ddlFromCPO.DataBind();

                    ddlToCPO.DataSource = dsCPO.Tables[0];
                    ddlToCPO.DataTextField = "CPO";
                    ddlToCPO.DataValueField = "CPO";
                    ddlToCPO.DataBind();
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void ddlToStyle_DataBound(object sender, EventArgs e)
        {
            ddlToStyle.SelectedIndex = ddlFromStyle.Items.Count - 1;
        }

        protected void ddlToRack_DataBound(object sender, EventArgs e)
        {
            ddlToRack.SelectedIndex = ddlFromRack.Items.Count - 1;
        }

        protected void ddlToPallet_DataBound(object sender, EventArgs e)
        {
            ddlToPallet.SelectedIndex = ddlFromPallet.Items.Count - 1;
        }

        protected void ddlToCPO_DataBound(object sender, EventArgs e)
        {
            ddlToCPO.SelectedIndex = ddlFromCPO.Items.Count - 1;
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddlFromStyle.SelectedIndex >= 0)
            {
                string fromstyle = ddlFromStyle.SelectedItem.Text;
                string tostyle = ddlToStyle.SelectedItem.Text;
                string fromRack = ddlFromRack.SelectedItem.Text;
                string toRack =ddlToRack.SelectedItem.Text;
                string fromPallet = ddlFromPallet.SelectedItem.Text;
                string toPallet = ddlToPallet.SelectedItem.Text;
                string fromCPO = ddlFromCPO.SelectedItem.Text;
                string toCPO = ddlToCPO.SelectedItem.Text;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/StockDetailedReport.rdlc");
                StockDetailedReportDS data = dba.getStockDetailedReport(fromstyle, tostyle, fromRack, toRack, fromPallet, toPallet, fromCPO, toCPO);
                ReportDataSource dts = new ReportDataSource("StockDetailedReportDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);

                ReportParameter fStyle = new ReportParameter("FromStyle", fromstyle);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fStyle });

                ReportParameter tStyle = new ReportParameter("ToStyle", tostyle);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tStyle });

                ReportParameter fRack = new ReportParameter("FromRack", fromRack);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fRack });
                ReportParameter tRack = new ReportParameter("ToRack", toRack);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tRack });

                ReportParameter fPallet = new ReportParameter("FromPallet", fromPallet);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fPallet });
                ReportParameter tPallet = new ReportParameter("ToPallet", toPallet);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tPallet });

                ReportParameter fCPO = new ReportParameter("FromCPO", fromCPO);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fCPO });
                ReportParameter tCPO = new ReportParameter("ToCPO", toCPO);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tCPO });

            }
        }

       
    }
}