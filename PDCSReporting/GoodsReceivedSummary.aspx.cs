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
    public partial class GoodsReceivedSummary : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        string factoryName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                calFromDate.Visible = false;
                calToDate.Visible = false;
                calFromDate.SelectedDate = DateTime.Today;
                calToDate.SelectedDate = DateTime.Today;
                txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
                txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
               
                DataSet dsFactory = new DataSet();
                dsFactory = com.ReturnDataSet("SELECT DISTINCT SourceWarehouse FROM     AODs ORDER BY SourceWarehouse");
                if (dsFactory.Tables[0].Rows.Count > 0)
                {
                    ddlFromFactory.DataSource = dsFactory.Tables[0];
                    ddlFromFactory.DataValueField = "SourceWarehouse";
                    ddlFromFactory.DataTextField = "SourceWarehouse";
                    ddlFromFactory.DataBind();

                    ddlToFactory.DataSource = dsFactory.Tables[0];
                    ddlToFactory.DataValueField = "SourceWarehouse";
                    ddlToFactory.DataTextField = "SourceWarehouse";
                    ddlToFactory.DataBind();

                }

                DataSet dsAOD = new DataSet();
                dsAOD = com.ReturnDataSet("SELECT DISTINCT AODNumber FROM     AODs ORDER BY AODNumber");
                if (dsAOD.Tables[0].Rows.Count > 0)
                {
                    ddlFromAOD.DataSource = dsAOD.Tables[0];
                    ddlFromAOD.DataValueField = "AODNumber";
                    ddlFromAOD.DataTextField = "AODNumber";
                    ddlFromAOD.DataBind();

                    ddlToAOD.DataSource = dsAOD.Tables[0];
                    ddlToAOD.DataValueField = "AODNumber";
                    ddlToAOD.DataTextField = "AODNumber";
                    ddlToAOD.DataBind();
                }

                DataSet dsCPO = new DataSet();
                dsCPO = com.ReturnDataSet("SELECT DISTINCT CPO FROM     BoxCPOAllocationDetails ORDER BY CPO");
                if (dsCPO.Tables[0].Rows.Count > 0)
                {
                    ddlFromCPO.DataSource = dsCPO.Tables[0];
                    ddlFromCPO.DataValueField = "CPO";
                    ddlFromCPO.DataTextField = "CPO";
                    ddlFromCPO.DataBind();

                    ddlToCPO.DataSource = dsCPO.Tables[0];
                    ddlToCPO.DataValueField = "CPO";
                    ddlToCPO.DataTextField = "CPO";
                    ddlToCPO.DataBind();

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void lbFromDate_Click(object sender, EventArgs e)
        {
            if (calFromDate.Visible)
            {
                calFromDate.Visible = false;
            }
            else
            {
                calFromDate.Visible = true;
            }
        }

        protected void calFromDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
            }
        }

        protected void calFromDate_SelectionChanged(object sender, EventArgs e)
        {
            txtFromDate.Value = calFromDate.SelectedDate.ToString("dd/MMM/yyyy");
            calFromDate.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2()", true);
        }

        protected void lbToDate_Click(object sender, EventArgs e)
        {
            if (calToDate.Visible)
            {
                calToDate.Visible = false;
            }
            else
            {
                calToDate.Visible = true;
            }
        }

        protected void calToDate_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date.CompareTo(DateTime.Today) > 0)
            {
                e.Day.IsSelectable = false;
            }
        }

        protected void calToDate_SelectionChanged(object sender, EventArgs e)
        {
            txtToDate.Value = calToDate.SelectedDate.ToString("dd/MMM/yyyy");
            calToDate.Visible = false;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2()", true);
        }

        protected void ddlToFactory_DataBound(object sender, EventArgs e)
        {
            ddlToFactory.SelectedIndex = ddlFromFactory.Items.Count - 1;
        }

        protected void ddlToAOD_DataBound(object sender, EventArgs e)
        {
            ddlToAOD.SelectedIndex = ddlFromAOD.Items.Count - 1;
        }

        protected void ddlToCPO_DataBound(object sender, EventArgs e)
        {
            ddlToCPO.SelectedIndex = ddlFromCPO.Items.Count - 1;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            
            string dateFrom = calFromDate.SelectedDate.ToString("dd-MMM-yyyy");
            string dateTo = calToDate.SelectedDate.ToString("dd-MMM-yyyy");
            string fromFactory = ddlFromFactory.SelectedItem.Text;
            string toFactory = ddlToFactory.SelectedItem.Text;
            string fromAOD = ddlFromAOD.SelectedItem.Text;
            string toAOD = ddlToAOD.SelectedItem.Text;
            string fromCPO = ddlFromCPO.SelectedItem.Text;
            string toCPO = ddlToCPO.SelectedItem.Text;
            DataSet dsFac = new DataSet();
            dsFac = com.ReturnDataSet("SELECT TOP (200) ParamValue FROM     Configurations WHERE(ParamName = N'FactoryName')");
            if (dsFac.Tables[0].Rows.Count > 0)
            {
                factoryName = dsFac.Tables[0].Rows[0].ItemArray.GetValue(0).ToString();
            }
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/GoodsReceivedSummary.rdlc");
            GoodsReceivedSummaryDS output = dba.getGoodsReceivedSummary(dateFrom, dateTo,fromFactory,toFactory, fromAOD, toAOD, fromCPO, toCPO, factoryName);
            ReportDataSource ds = new ReportDataSource("GoodsReceivedSummaryDS", output.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(ds);

            ReportParameter fromDate = new ReportParameter("FromDate", dateFrom);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fromDate });
            ReportParameter toDate = new ReportParameter("ToDate", dateTo);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { toDate });
            ReportParameter fFac = new ReportParameter("FromFactory", fromFactory);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fFac });
            ReportParameter tFac = new ReportParameter("ToFactory", toFactory);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tFac });
            ReportParameter fAOD = new ReportParameter("FromAOD", fromAOD);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fAOD });
            ReportParameter tAOD = new ReportParameter("ToAOD", toAOD);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tAOD });
            ReportParameter fCPO = new ReportParameter("FromCPO", fromCPO);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fCPO });
            ReportParameter tCPO = new ReportParameter("ToCPO", toCPO);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tCPO });

        }
    }

}