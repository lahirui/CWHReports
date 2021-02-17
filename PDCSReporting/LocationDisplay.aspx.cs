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
    public partial class LocationDisplay : System.Web.UI.Page
    {
        DBAccess dba = new DBAccess();
        Common com = new Common();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                int totalLocations, usedLocations, remainingLocations;

                //Get Total Location Count
                DataSet dsTotLoc = new DataSet();
                dsTotLoc = com.ReturnDataSet("SELECT COUNT( Code) AS TotalLocations FROM Locations WHERE(IsDeleted = 0)");
                totalLocations = Convert.ToInt32(dsTotLoc.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());

                //Get Remaining Location Count
                DataSet dsLocCount = new DataSet();
                dsLocCount = com.ReturnDataSet("SELECT COUNT(*) AS LocationCount FROM (SELECT ISNULL(dbo.Locations.Code, 0) AS Location " +
                                            "FROM     dbo.CartonHeaders INNER JOIN " +
                                            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
                                            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            "GROUP BY dbo.Locations.Code " +
                                            "HAVING COUNT(dbo.Boxes.BoxCode) = 0) AS LocationCount");
                remainingLocations = Convert.ToInt32(dsLocCount.Tables[0].Rows[0].ItemArray.GetValue(0).ToString());

                double precentage = Math.Round((double)remainingLocations / (double)totalLocations * 100, 2);
                lblPrecentage.Text = Convert.ToString(precentage) + " %";

                lblRemaining.Text = Convert.ToString(remainingLocations) + "/" + Convert.ToString(totalLocations);

                usedLocations = (totalLocations - remainingLocations);
                lblUsed.Text = Convert.ToString(usedLocations) + "/" + Convert.ToString(totalLocations);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/LocationDisplay.rdlc");
                LocationDisplayDS data = dba.getLocationDisplayDetails();
                ReportDataSource dts = new ReportDataSource("LocationDisplayDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);
            }
            
        }
    }
}