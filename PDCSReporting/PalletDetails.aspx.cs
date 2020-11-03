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
    public partial class PalletDetails : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
               
                DataSet dsPallet = new DataSet();
                dsPallet = com.ReturnDataSet("SELECT Id, Code FROM     Pallets WHERE(IsDeleted = 0) ORDER BY Code");
                if (dsPallet.Tables[0].Rows.Count > 0)
                {
                    ddlPallets.DataSource = dsPallet.Tables[0];
                    ddlPallets.DataTextField = "Code";
                    ddlPallets.DataValueField = "Id";
                    ddlPallets.DataBind();
                    ddlPallets.Items.Insert(0, new ListItem("-- Select a Pallet --", "0"));

                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddlPallets.SelectedIndex > 0)
            {
                string PalletName = ddlPallets.SelectedItem.Text;
                int PalletId = Convert.ToInt32(ddlPallets.SelectedValue);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/PalletDetails.rdlc");
                PalletDetailsDS data = dba.getPalletDetails(PalletId);
                ReportDataSource dts = new ReportDataSource("PalletDetailsDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);

                //Passing Text Box values (Date and Team) to report 
                ReportParameter PName = new ReportParameter("PalletName", PalletName);
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { PName });
            }

        }
    }
}