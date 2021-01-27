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
    public partial class PIPreAndPostCount : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                

                DataSet dsLoc = new DataSet();
                dsLoc = com.ReturnDataSet("SELECT Id, Code FROM     Locations WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsLoc.Tables[0].Rows.Count > 0)
                {
                    ddlFromLocation.DataSource = dsLoc.Tables[0];
                    ddlFromLocation.DataValueField = "Id";
                    ddlFromLocation.DataTextField = "Code";
                    ddlFromLocation.DataBind();
                    ddlFromLocation.Items.Insert(0, "SELECT LOCATION");

                    ddlToLocation.DataSource = dsLoc.Tables[0];
                    ddlToLocation.DataValueField = "Id";
                    ddlToLocation.DataTextField = "Code";
                    ddlToLocation.DataBind();
                    ddlToLocation.Items.Insert(0, "SELECT LOCATION");
                }

                DataSet dsRef = new DataSet();
                dsRef = com.ReturnDataSet("SELECT Id, PIReference FROM PIs WHERE(IsDeleted = 0) ORDER BY PIReference");
                if (dsRef.Tables[0].Rows.Count > 0)
                {
                    ddlPIReference.DataSource = dsRef.Tables[0];
                    ddlPIReference.DataValueField = "Id";
                    ddlPIReference.DataTextField = "PIReference";
                    ddlPIReference.DataBind();
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void ddlToPallet_DataBound(object sender, EventArgs e)
        {
            ddlToPallet.SelectedIndex = ddlFromPallet.Items.Count - 1;
        }

        protected void ddlFromPallet_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
           // ddlToPallet.SelectedIndex = ddlFromPallet.SelectedIndex;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            
                int PIRefId = Convert.ToInt32(ddlPIReference.SelectedValue);
                string fPallet = ddlFromPallet.SelectedItem.Text;
                string tPallet = ddlToPallet.SelectedItem.Text;
                string PiRef = ddlPIReference.SelectedItem.Text;
                string fLocation = ddlFromLocation.SelectedItem.Text;
                string tLocation = ddlToLocation.SelectedItem.Text;



                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PIPreAndPostCount.rdlc");
                PIPreAndPostCountDS output = dba.getPiPreAndPostCounts(PIRefId, fPallet, tPallet, fLocation, tLocation);
                ReportDataSource ds = new ReportDataSource("PIPreAndPostCountDS", output.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(ds);

            ReportParameter Pref = new ReportParameter("PiReference", PiRef);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { Pref });
            ReportParameter from = new ReportParameter("FromPallet", fPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { from });
            ReportParameter to = new ReportParameter("ToPallet", tPallet);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { to });
            ReportParameter fL = new ReportParameter("FromLocation", fLocation);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { fL });
            ReportParameter tl = new ReportParameter("ToLocation", tLocation);
            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { tl });

        }

        protected void ddlToLocation_DataBound(object sender, EventArgs e)
        {
            ddlToLocation.SelectedIndex = ddlFromLocation.Items.Count - 1;
        }

        protected void ddlFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);

            ddlToLocation.SelectedIndex = ddlFromLocation.SelectedIndex;
            ddlFromPallet.Items.Clear();
            ddlToPallet.Items.Clear();
            DataSet dsPallet = new DataSet();
            dsPallet = com.ReturnDataSet("SELECT TOP (100) PERCENT dbo.Pallets.Id, dbo.Pallets.Code "+
                                            "FROM     dbo.Pallets INNER JOIN " +
                                                              "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            "WHERE(dbo.Pallets.IsDeleted = 0) AND(dbo.Locations.IsDeleted = 0) AND(dbo.Locations.Code BETWEEN '"+ddlFromLocation.SelectedItem.Text+ "' AND '" + ddlToLocation.SelectedItem.Text + "') " +
                                            "ORDER BY dbo.Pallets.Code");
            if (dsPallet.Tables[0].Rows.Count > 0)
            {
                ddlFromPallet.DataSource = dsPallet.Tables[0];
                ddlFromPallet.DataValueField = "Id";
                ddlFromPallet.DataTextField = "Code";
                ddlFromPallet.DataBind();

                ddlToPallet.DataSource = dsPallet.Tables[0];
                ddlToPallet.DataValueField = "Id";
                ddlToPallet.DataTextField = "Code";
                ddlToPallet.DataBind();
            }
        }

        protected void ddlToLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "LoadSelect2();", true);
            ddlFromPallet.Items.Clear();
            ddlToPallet.Items.Clear();
            DataSet dsPallet = new DataSet();
            dsPallet = com.ReturnDataSet("SELECT TOP (100) PERCENT dbo.Pallets.Id, dbo.Pallets.Code " +
                                            "FROM     dbo.Pallets INNER JOIN " +
                                                              "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            "WHERE(dbo.Pallets.IsDeleted = 0) AND(dbo.Locations.IsDeleted = 0) AND(dbo.Locations.Code BETWEEN '" + ddlFromLocation.SelectedItem.Text + "' AND '" + ddlToLocation.SelectedItem.Text + "') " +
                                            "ORDER BY dbo.Pallets.Code");
            if (dsPallet.Tables[0].Rows.Count > 0)
            {
                ddlFromPallet.DataSource = dsPallet.Tables[0];
                ddlFromPallet.DataValueField = "Id";
                ddlFromPallet.DataTextField = "Code";
                ddlFromPallet.DataBind();

                ddlToPallet.DataSource = dsPallet.Tables[0];
                ddlToPallet.DataValueField = "Id";
                ddlToPallet.DataTextField = "Code";
                ddlToPallet.DataBind();
            }
        }
    }
}