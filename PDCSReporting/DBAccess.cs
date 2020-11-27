using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PDCSReporting
{
    public class DBAccess
    {

        SqlConnectionStringBuilder SSB = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["ConString"].ConnectionString);
       // String Server, Catalog;
        
        
        SqlConnection conn;
        public DBAccess()
        {
            conn = DBConnect.GetConnection();
        }

        //Methods to Reports
     
        public DailyScannedBoxesDS getDetailForDailyScannedBoxes(string FromDate, string ToDate)
        {
            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProdOrders.Code AS MPO,  dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode, " +
                                                               "SUM(dbo.FGWips.Quantity) AS Quantity " +
                                             "FROM     dbo.Products INNER JOIN " +
                                                               "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                                               "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                               "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                               "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN " +
                                                               "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                               "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                               "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId " +
                                             "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '" + FromDate + "') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '" + ToDate + "') AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) " +
                                             "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode " +
                                             "ORDER BY Style, Colour, Size, MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (DailyScannedBoxesDS FHC = new DailyScannedBoxesDS())
                {
                    sda.Fill(FHC, "DailyScannedBoxesDS");
                    conn.Close();
                    return FHC;
                }
            }
        }


        public DataTable getDetailForDailyScannedBoxesOldStock(string FromDate, string ToDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;
            newCmd.CommandText = "SELECT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode, SUM(dbo.FGWips.Quantity) AS Quantity "+
                                          "FROM dbo.Products INNER JOIN "+
                                          "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN "+
                                          "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN "+
                                          "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN "+
                                          "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN "+
                                          "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id "+
                                          "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '"+ FromDate +"') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '"+ ToDate +"') "+
                                          "AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) AND(dbo.Boxes.Id NOT IN(SELECT dbo.Boxes.Id "+
                                                                    "FROM dbo.Products INNER JOIN "+
                                                                    "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN "+
                                                                    "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN "+
                                                                    "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN "+
                                                                    "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN "+
                                                                    "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN "+
                                                                    "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN "+
                                                                    "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId "+
                                                                    "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '"+ FromDate +"') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '"+ ToDate+ "') AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) "+
                                                                    "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.Id)) "+
                                          "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode, dbo.Boxes.Id "+
                                          "ORDER BY Style, Colour, Size, dbo.Boxes.BoxCode";


            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getDetailForDailyScannedBoxesOldStock2(string FromDate, string ToDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;
            newCmd.CommandText = "SELECT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode, SUM(dbo.FGWips.Quantity) AS Quantity " +
                                          "FROM dbo.Products INNER JOIN " +
                                          "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                          "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                          "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                          "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN " +
                                          "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id " +
                                          "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '" + FromDate + "') AND (CAST(dbo.FGWips.EffectiveDate AS DATE) <= '" + ToDate + "') AND (dbo.FGWips.TransactionType = 1) AND (dbo.FGWips.WIPArea = 2) " +
                                          "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                          "ORDER BY Style, Colour, Size, dbo.Boxes.BoxCode";


            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getTotalPiecesAndTotalBoxesScannedOldStock(string FromDate, string ToDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;
            newCmd.CommandText = "SELECT COUNT(dbo.Boxes.BoxCode) AS TotalBoxes, SUM(dbo.FGWips.Quantity) AS Quantity "+
                                          "FROM dbo.Products INNER JOIN "+
                                          "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN "+
                                          "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN "+
                                          "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN "+
                                          "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN "+
                                          "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id "+
                                          "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '"+ FromDate +"') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '"+ ToDate +"') "+
                                          "AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) AND(dbo.Boxes.Id NOT IN(SELECT dbo.Boxes.Id "+
                                                    "FROM dbo.Products INNER JOIN "+
                                                    "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN "+
                                                    "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN "+
                                                    "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN "+
                                                    "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN "+
                                                    "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN "+
                                                    "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN "+
                                                    "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId "+
                                                    "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '"+ FromDate +"') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '" + ToDate + "') AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) "+
                                                    "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.Id))";


            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getTotalPiecesAndTotalBoxesScannedOldStock2(string FromDate, string ToDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;
            newCmd.CommandText = "SELECT COUNT(dbo.Boxes.BoxCode) AS TotalBoxes, SUM(dbo.FGWips.Quantity) AS Quantity " +
                                          "FROM dbo.Products INNER JOIN " +
                                          "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                          "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                          "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                          "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN " +
                                          "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id " +
                                          "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '" + FromDate + "') AND (CAST(dbo.FGWips.EffectiveDate AS DATE) <= '" + ToDate + "') AND (dbo.FGWips.TransactionType = 1) AND (dbo.FGWips.WIPArea = 2) ";


            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public BSScannedBoxDetailsDS getDetailForBSScannedBoxDetails(string FromDate, string ToDate)
        {
            SqlCommand cmd = new SqlCommand("SELECT Size_and_CC_Code AS StyleAndSize, Description AS ColourDescription, CPO, BoxNumber,EnteredBoxNumber, Quantity " +
                                            "FROM     BoxScanDetails " +
                                            "WHERE(CAST(EffectiveDate AS DATE) >= '" + FromDate + "') AND(CAST(EffectiveDate AS DATE) <= '" + ToDate + "') " +
                                            "ORDER BY StyleAndSize,ColourDescription, CPO,BoxNumber,EnteredBoxNumber");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (BSScannedBoxDetailsDS FHC = new BSScannedBoxDetailsDS())
                {
                    sda.Fill(FHC, "BSScannedBoxDetailsDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public DataTable getSOByCPOId(string CPO)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT DISTINCT SO FROM BoxCPOAllocationDetails WHERE CPO = '" + CPO + "'";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getScannedBoxDetailsCPOWise(string CPO)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;
            newCmd.CommandText = "SELECT DISTINCT dbo.Boxes.BoxCode, dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.FGWips.Quantity) AS Quantity " +
                                 "FROM dbo.Products INNER JOIN " +
                                 "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                 "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                 "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                 "dbo.BoxCPOAllocationDetails INNER JOIN " +
                                 "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                 "dbo.FGWips ON dbo.Boxes.Id = dbo.FGWips.BoxId ON dbo.Products.Id = dbo.FGWips.ProductId " +
                                 "WHERE(dbo.FGWips.WIPArea = 1) AND(dbo.FGWips.Quantity > 0) AND(dbo.BoxCPOAllocationDetails.CPO = '" + CPO + "') " +
                                 "GROUP BY dbo.Boxes.BoxCode, dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code " +
                                 "ORDER BY dbo.Sizes.Code";

            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getSummarizedScannedDetailsByCPO(string CPO)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.FGWips.Quantity) AS Quantity, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes " +
                                  "FROM dbo.Products INNER JOIN " +
                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                  "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                  "dbo.BoxCPOAllocationDetails INNER JOIN " +
                                  "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                  "dbo.FGWips ON dbo.Boxes.Id = dbo.FGWips.BoxId ON dbo.Products.Id = dbo.FGWips.ProductId " +
                                  "WHERE(dbo.FGWips.WIPArea = 1) AND(dbo.FGWips.Quantity > 0) AND(dbo.BoxCPOAllocationDetails.CPO = '" + CPO + "') " +
                                  "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code " +
                                  "ORDER BY Size";

            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        public DailyScannedRFIDTagsDS getDetailForDailyScannedRFIDTags(string FromDate, string ToDate)
        {
            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS PDCSStyle, dbo.RFIDAllocationDetails.DecathlonStyle AS CustomerStyle, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, COUNT(dbo.RFIDAllocationDetails.RFIDTag) AS RFIDCount " +
                                            "FROM     dbo.RFIDAllocationDetails INNER JOIN " +
                                                              "dbo.Styles ON dbo.RFIDAllocationDetails.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colours ON dbo.RFIDAllocationDetails.ColourId = dbo.Colours.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.RFIDAllocationDetails.SizeId = dbo.Sizes.Id " +
                                            "WHERE(dbo.RFIDAllocationDetails.IsDeleted = 0) AND(CAST(dbo.RFIDAllocationDetails.CreatedDate AS DATE) >= '" + FromDate + "') AND(CAST(dbo.RFIDAllocationDetails.CreatedDate AS DATE) <= '" + ToDate + "') " +
                                            "GROUP BY dbo.Styles.Code, dbo.RFIDAllocationDetails.DecathlonStyle, dbo.Colours.Code, dbo.Sizes.Code " +
                                            "ORDER BY PDCSStyle, CustomerStyle, Colour, Size");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (DailyScannedRFIDTagsDS FHC = new DailyScannedRFIDTagsDS())
                {
                    sda.Fill(FHC, "DailyScannedRFIDTagsDS");
                    conn.Close();
                    return FHC;
                }
            }
        }
       
        public PalletDetailsDS getPalletDetails(int PalletId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT  dbo.Boxes.BoxCode AS BoxBarCode, dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour,dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, (CAST( dbo.Boxes.CreatedDate AS DATE)) AS ScannedDate, " +
                                                              "SUM(dbo.FGWips.Quantity) AS Quantity " +
                                            "FROM     dbo.FGWips INNER JOIN " +
                                                              "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.Products ON dbo.FGWips.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id FULL OUTER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.WarehouseWips ON dbo.Boxes.Id = dbo.WarehouseWips.BoxId " +
                                            "WHERE(dbo.FGWips.WIPArea = 2)  AND(dbo.WarehouseWips.WIPArea = 2) AND (dbo.WarehouseWips.PalletId = " + PalletId + ") " +
                                            "GROUP BY dbo.Boxes.BoxCode, dbo.Styles.Code, dbo.Colours.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code, (CAST( dbo.Boxes.CreatedDate AS DATE)) " +
                                            "HAVING SUM(dbo.FGWips.Quantity) <> 0 " +
                                            "ORDER BY BoxBarCode, Style, Colour, dbo.BoxCPOAllocationDetails.CPO, Size, (CAST( dbo.Boxes.CreatedDate AS DATE))");
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (PalletDetailsDS Customer = new PalletDetailsDS())
                {
                    sda.Fill(Customer, "PalletDetailsDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public LocationDetailsDS getLocationDetails(string fromLocation, string toLocation)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, dbo.Boxes.BoxCode AS BoxBarCode, dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.ProdOrders.Code AS MPO, " +
                                                              "dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, (CAST(dbo.Boxes.CreatedDate AS DATE)) AS BoxCreatedDate, (CAST(dbo.WarehouseWips.EffectiveDate AS DATE)) AS PalletAllocatedDate, SUM(dbo.FGWips.Quantity) AS Quantity " +
                                            "FROM     dbo.FGWips INNER JOIN " +
                                                              "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.Products ON dbo.FGWips.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id FULL OUTER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.WarehouseWips ON dbo.Boxes.Id = dbo.WarehouseWips.BoxId FULL OUTER JOIN " +
                                                              "dbo.Locations ON dbo.WarehouseWips.LocationId = dbo.Locations.Id FULL OUTER JOIN " +
                                                              "dbo.Pallets ON dbo.WarehouseWips.PalletId = dbo.Pallets.Id " +
                                            "WHERE(dbo.FGWips.WIPArea = 2) AND(dbo.WarehouseWips.WIPArea = 2) AND(dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') " +
                                            "GROUP BY dbo.Locations.Code, dbo.Pallets.Code, dbo.Boxes.BoxCode, dbo.Styles.Code, dbo.Colours.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code, (CAST(dbo.Boxes.CreatedDate AS DATE)), (CAST(dbo.WarehouseWips.EffectiveDate AS DATE)) " +
                                            "HAVING(SUM(dbo.FGWips.Quantity) <> 0) " +
                                            "ORDER BY Location, Pallet, BoxBarCode, Style, Colour, MPO, dbo.BoxCPOAllocationDetails.CPO, Size, BoxCreatedDate, PalletAllocatedDate");
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (LocationDetailsDS Customer = new LocationDetailsDS())
                {
                    sda.Fill(Customer, "LocationDetailsDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public AODUnavailableQuantitiesDS getAODReceiveDetails(int AODId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT  dbo.AODs.AODNumber AS AOD, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode AS BarCode, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.CartonDetails.Quantity " +
                                            "FROM     dbo.AODs INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                                              "dbo.Styles INNER JOIN " +
                                                              "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id " +
                                            "WHERE(dbo.AODs.id = " + AODId + ") AND(dbo.CartonHeaders.WIPArea IN(1, 2)) " +
                                            "ORDER BY AOD, dbo.CartonHeaders.WIPArea, BarCode, Style, Colour, Size");
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (AODUnavailableQuantitiesDS Customer = new AODUnavailableQuantitiesDS())
                {
                    sda.Fill(Customer, "AODUnavailableQuantitiesDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public StockSummaryReportDS getStockSummaryDetails(string fromStyle, string toStyle)
        {



            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.CartonDetails INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
                                                              "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                              "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            "WHERE(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') " +
                                            "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code " +
                                            "ORDER BY dbo.Styles.Code, dbo.Colors.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (StockSummaryReportDS FHC = new StockSummaryReportDS())
                {
                    sda.Fill(FHC, "StockSummaryReportDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public StockDetailedReportDS getStockDetailedReport(string fromStyle, string toStyle, string fromLocation, string toLocation,string fromPallet, string toPallet)
        {



            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code AS Pallet, dbo.Locations.Code AS Rack, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode AS BarCode, " +
                                                              "SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.CartonDetails INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
                                                              "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                              "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            "WHERE " +
                                            "(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') AND " +
                                            "(dbo.Pallets.Code BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND " +
                                            "(dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') " +
                                            "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code, dbo.Locations.Code, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode " +
                                            "ORDER BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code, dbo.Locations.Code, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (StockDetailedReportDS FHC = new StockDetailedReportDS())
                {
                    sda.Fill(FHC, "StockDetailedReportDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public GoodsReceivedDetailesDS getGoodsReceivedDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO)
        {
            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
                                                              "dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.Products INNER JOIN " +
                                                              "dbo.CartonWips INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
                                                              "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
                                            "WHERE(dbo.CartonWips.TransactionType = 8) AND(dbo.CartonWips.WIPArea = 1) AND(dbo.CartonWips.Quantity > 0) " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
                                            "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
                                            "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            "GROUP BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                            "ORDER BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (GoodsReceivedDetailesDS FHC = new GoodsReceivedDetailesDS())
                {
                    sda.Fill(FHC, "GoodsReceivedDetailesDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public GoodsReceivedSummaryDS getGoodsReceivedSummary(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO)
        {
            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO,COUNT(*) AS NumberOfCartons, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.Products INNER JOIN " +
                                                              "dbo.CartonWips INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
                                                              "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
                                            "WHERE(dbo.CartonWips.TransactionType = 8) AND(dbo.CartonWips.WIPArea = 1) AND(dbo.CartonWips.Quantity > 0)  " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "') AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            "GROUP BY (CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
                                            "ORDER BY (CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, SourceFactory, AOD, dbo.BoxCPOAllocationDetails.CPO");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (GoodsReceivedSummaryDS FHC = new GoodsReceivedSummaryDS())
                {
                    sda.Fill(FHC, "GoodsReceivedSummaryDS");
                    conn.Close();
                    return FHC;
                }
            }
        }
       
        public ShipmentDetailsDS getShipmentDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO)
        {
            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
                                                              "dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.Products INNER JOIN " +
                                                              "dbo.CartonWips INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
                                                              "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
                                            "WHERE(dbo.CartonWips.TransactionType IN(8, 10)) AND(dbo.CartonWips.WIPArea = 3) AND(dbo.CartonWips.Quantity < 0) " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
                                            "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toFactory + "') " +
                                            "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + fromCPO + "') " +
                                            "GROUP BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                            "ORDER BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (ShipmentDetailsDS FHC = new ShipmentDetailsDS())
                {
                    sda.Fill(FHC, "ShipmentDetailsDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public ShipmentSummaryDS getShipmentSummary(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO)
        {



            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO,COUNT(*) AS NumberOfCartons, ABS(SUM(dbo.CartonDetails.Quantity)) AS Quantity " +
                                            "FROM     dbo.Products INNER JOIN " +
                                                              "dbo.CartonWips INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
                                                              "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
                                            "WHERE(dbo.CartonWips.TransactionType IN(8, 10)) AND(dbo.CartonWips.WIPArea = 3) AND(dbo.CartonWips.Quantity < 0)  " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "') AND(CAST(dbo.CartonWips.EffectiveDate AS DATE)<= '" + toDate + "') AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            "GROUP BY (CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
                                            "ORDER BY (CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, SourceFactory, AOD, dbo.BoxCPOAllocationDetails.CPO");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (ShipmentSummaryDS FHC = new ShipmentSummaryDS())
                {
                    sda.Fill(FHC, "ShipmentSummaryDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public SingleScannedRFIDsDS getSingleScannedRFIDDetails(string fromStyle, string toStyle)
        {



            SqlCommand cmd = new SqlCommand("SELECT dbo.ProductModelCodes.CustomerStyle, dbo.Styles.Code AS PDCSStyle, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProductModelCodes.ModelCode, dbo.ProductModelCodes.ItemCode, COUNT(*) AS ScannedRFIDCount " +
                                            "FROM   dbo.ProductModelCodes INNER JOIN " +
                                                   "dbo.Styles ON dbo.ProductModelCodes.StyleID = dbo.Styles.Id INNER JOIN " +
                                                   "dbo.Colors ON dbo.ProductModelCodes.ColourID = dbo.Colors.Id INNER JOIN " +
                                                   "dbo.Sizes ON dbo.ProductModelCodes.SizeID = dbo.Sizes.Id " +
                                            "WHERE(dbo.ProductModelCodes.IsDeleted = 0) AND(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') " +
                                            "GROUP BY dbo.ProductModelCodes.CustomerStyle, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.ProductModelCodes.ModelCode, dbo.ProductModelCodes.ItemCode " +
                                            "ORDER BY dbo.ProductModelCodes.CustomerStyle, PDCSStyle, Colour, Size, dbo.ProductModelCodes.ModelCode, dbo.ProductModelCodes.ItemCode");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (SingleScannedRFIDsDS FHC = new SingleScannedRFIDsDS())
                {
                    sda.Fill(FHC, "SingleScannedRFIDsDS");
                    conn.Close();
                    return FHC;
                }
            }
        }




    }
}