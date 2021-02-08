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
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/ReportDesigns/LocationDisplay.rdlc");
                LocationDisplayDS data = dba.getLocationDisplayDetails();
                ReportDataSource dts = new ReportDataSource("LocationDisplayDS", data.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dts);

                //DataSet dsLocations = new DataSet();
                //dsLocations = com.ReturnDataSet("SELECT LEFT(dbo.Locations.Code, 1) AS Prefix,RIGHT(dbo.Locations.Code, 1) AS RowNum, LEFT(RIGHT(dbo.Locations.Code, 4), 2) AS ColNum, ISNULL(dbo.Locations.Code, 0) AS Location, COUNT(dbo.Boxes.BoxCode) AS Boxes "+
                //                                "FROM     dbo.CartonHeaders INNER JOIN " +
                //                                                  "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                //                                                  "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
                //                                                  "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                //                                "GROUP BY dbo.Locations.Code " +
                //                                "HAVING COUNT(dbo.Boxes.BoxCode) = 0 " +
                //                                "ORDER BY Prefix, RowNum, ColNum, Location");

                //if (dsLocations.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i <= dsLocations.Tables[0].Rows.Count; i++)
                //    {
                //        string RowLetter = dsLocations.Tables[0].Rows[i].ItemArray.GetValue(0).ToString();
                //        string Prefix = "";
                //        while (Prefix!=RowLetter)
                //        {
                //            GridView1.DataSource = dsLocations.Tables[0].Rows[i].ItemArray.GetValue(3).ToString();
                //            GridView1.DataBind();
                //        }


                //    }
                //}
            }
            
        }
    }
}