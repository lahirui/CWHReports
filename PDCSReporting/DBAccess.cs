﻿using System;
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
            //SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProdOrders.Code AS MPO,  dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode, " +
            //                                                   "SUM(dbo.FGWips.Quantity) AS Quantity " +
            //                                 "FROM     dbo.Products INNER JOIN " +
            //                                                   "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
            //                                                   "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                   "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                   "dbo.FGWips ON dbo.Products.Id = dbo.FGWips.ProductId INNER JOIN " +
            //                                                   "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
            //                                                   "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                   "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId " +
            //                                 "WHERE(CAST(dbo.FGWips.EffectiveDate AS DATE) >= '" + FromDate + "') AND(CAST(dbo.FGWips.EffectiveDate AS DATE) <= '" + ToDate + "') AND(dbo.FGWips.TransactionType = 1) AND(dbo.FGWips.WIPArea = 2) " +
            //                                 "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode " +
            //                                 "ORDER BY Style, Colour, Size, MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode");

            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode, " +
                                               "SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                               "FROM dbo.BoxCPOAllocationDetails INNER JOIN " +
                                               "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                               "dbo.ProdOrders INNER JOIN " +
                                               "dbo.Products INNER JOIN " +
                                               "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                               "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                               "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                               "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId ON dbo.ProdOrders.Id = dbo.CartonDetails.ProdOrderId ON dbo.Boxes.Id = dbo.CartonDetails.BoxId " +
                                               "WHERE(CAST(dbo.CartonDetails.Date AS DATE) >= '" + FromDate + "') AND(CAST(dbo.CartonDetails.Date AS DATE) <= '" + ToDate + "') AND(dbo.CartonDetails.TransactionType = 2) " +
                                               "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.BoxCPOAllocationDetails.SO, dbo.Boxes.BoxCode " +
                                               "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
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
            //newCmd.CommandText = "SELECT DISTINCT dbo.Boxes.BoxCode, dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.FGWips.Quantity) AS Quantity " +
            //                     "FROM dbo.Products INNER JOIN " +
            //                     "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                     "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                     "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
            //                     "dbo.BoxCPOAllocationDetails INNER JOIN " +
            //                     "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                     "dbo.FGWips ON dbo.Boxes.Id = dbo.FGWips.BoxId ON dbo.Products.Id = dbo.FGWips.ProductId " +
            //                     "WHERE(dbo.FGWips.WIPArea = 1) AND(dbo.FGWips.Quantity > 0) AND(dbo.BoxCPOAllocationDetails.CPO = '" + CPO + "') " +
            //                     "GROUP BY dbo.Boxes.BoxCode, dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code " +
            //                     "ORDER BY dbo.Sizes.Code";

            newCmd.CommandText = "SELECT dbo.Boxes.BoxCode,(CAST(dbo.Boxes.CreatedDate AS DATE)) AS BoxCreatedDate, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.CartonDetails.Quantity) AS Quantity, dbo.CartonHeaders.IsDeleted AS Status " +
                                    "FROM dbo.Products INNER JOIN "+
                                    "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN "+
                                    "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN "+
                                    "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN "+
                                    "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN "+
                                    "dbo.CartonHeaders ON dbo.CartonDetails.BoxId = dbo.CartonHeaders.BoxId INNER JOIN "+
                                    "dbo.BoxCPOAllocationDetails INNER JOIN "+
                                    "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id "+
                                    "WHERE(dbo.BoxCPOAllocationDetails.CPO = '"+ CPO +"') "+
                                    "GROUP BY dbo.Boxes.BoxCode,(CAST(dbo.Boxes.CreatedDate AS DATE)), dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code,dbo.CartonHeaders.IsDeleted " +
                                    "ORDER BY dbo.Sizes.Code,(CAST(dbo.Boxes.CreatedDate AS DATE))";

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

            //newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.FGWips.Quantity) AS Quantity, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes " +
            //                      "FROM dbo.Products INNER JOIN " +
            //                      "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                      "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                      "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
            //                      "dbo.BoxCPOAllocationDetails INNER JOIN " +
            //                      "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                      "dbo.FGWips ON dbo.Boxes.Id = dbo.FGWips.BoxId ON dbo.Products.Id = dbo.FGWips.ProductId " +
            //                      "WHERE(dbo.FGWips.WIPArea = 1) AND(dbo.FGWips.Quantity > 0) AND(dbo.BoxCPOAllocationDetails.CPO = '" + CPO + "') " +
            //                      "GROUP BY dbo.Styles.Code, dbo.Colours.Code, dbo.Sizes.Code " +
            //                      "ORDER BY Size";

            newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                  "FROM dbo.Products INNER JOIN " +
                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                  "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                  "dbo.CartonHeaders ON dbo.CartonDetails.BoxId = dbo.CartonHeaders.BoxId INNER JOIN " +
                                  "dbo.BoxCPOAllocationDetails INNER JOIN " +
                                  "dbo.Boxes ON dbo.BoxCPOAllocationDetails.BoxId = dbo.Boxes.Id ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id " +
                                  "WHERE (dbo.BoxCPOAllocationDetails.CPO = '" + CPO + "') " +
                                  "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code " +
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

            //SqlCommand cmd = new SqlCommand("SELECT  dbo.AODs.AODNumber AS AOD, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode AS BarCode, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.CartonDetails.Quantity " +
            //                                "FROM     dbo.AODs INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
            //                                                  "dbo.Styles INNER JOIN " +
            //                                                  "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id " +
            //                                "WHERE(dbo.AODs.id = " + AODId + ") AND(dbo.CartonHeaders.WIPArea IN(1, 2)) " +
            //                                "ORDER BY AOD, dbo.CartonHeaders.WIPArea, BarCode, Style, Colour, Size");

            SqlCommand cmd = new SqlCommand("SELECT dbo.AODs.AODNumber AS AOD, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode AS BarCode, dbo.Styles.Code AS Style, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.CartonDetails.Quantity " +
                                            "FROM     dbo.AODs INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                                              "dbo.Styles INNER JOIN " +
                                                              "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id " +
                                            "WHERE (dbo.CartonHeaders.IsDeleted = 0) AND (dbo.AODs.id = " + AODId + ")AND(dbo.CartonHeaders.WIPArea IN(1, 2)) " +
                                            "ORDER BY AOD, dbo.CartonHeaders.WIPArea, BarCode, Style,MPO, CPO, Colour, Size");

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

        public StockSummaryReportDS getStockSummaryDetails(string fromStyle, string toStyle, string fromCPO, string toCPO)
        {

            /*SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, " +
                                            " dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, COUNT(dbo.Boxes.BoxCode) AS BoxCount, " +
                                            " SUM(dbo.CartonDetails.Quantity) AS Quantity FROM dbo.CartonDetails INNER JOIN " +
                                            " dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
                                            " dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                            " dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                            " dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                            " dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                            " dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                            " dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                            " dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            " WHERE(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') " +
                                            " AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            " AND(dbo.CartonHeaders.WIPArea = 2) AND(dbo.CartonHeaders.IsDeleted = 0) " +
                                            " GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.BoxCPOAllocationDetails.CPO, " +
                                            " dbo.Sizes.Code ORDER BY Style, Colour, dbo.BoxCPOAllocationDetails.CPO, Size");*/


            SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, " +
                                            " dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, COUNT(dbo.Boxes.BoxCode) AS BoxCount, " +
                                            " SUM(dbo.CartonDetails.Quantity) AS Quantity FROM dbo.CartonDetails INNER JOIN " +
                                            " dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
                                            " dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                            " dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                            " dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                            " dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                            " dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                            " dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                            " dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            " WHERE(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') " +
                                            " AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            " AND(dbo.CartonHeaders.WIPArea = 2) AND(dbo.CartonHeaders.IsDeleted = 0) AND " +
                                            "  dbo.Pallets.Code !='StockWriteOff' " +
                                            " GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.BoxCPOAllocationDetails.CPO, " +
                                            " dbo.Sizes.Code ORDER BY Style, Colour, dbo.BoxCPOAllocationDetails.CPO, Size");


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

        public StockDetailedReportDS getStockDetailedReport(string fromStyle, string toStyle, string fromLocation, string toLocation,string fromPallet, string toPallet, string fromCPO, string toCPO)
        {
            SqlCommand cmd = new SqlCommand("SELECT  dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
                                            " dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code AS Pallet, dbo.Locations.Code AS Rack, " +
                                            " dbo.CartonHeaders.WIPArea,dbo.Boxes.BoxCode AS BarCode, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            " FROM     dbo.CartonDetails INNER JOIN dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN dbo.Products ON " +
                                            " dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN dbo.Styles ON " +
                                            " dbo.Products.StyleId = dbo.Styles.Id INNER JOIN  dbo.Colors ON " +
                                            " dbo.Products.ColorId = dbo.Colors.Id INNER JOIN dbo.Sizes ON " +
                                            " dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN  dbo.BoxCPOAllocationDetails ON " +
                                            " dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN dbo.Pallets ON " +
                                            " dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN dbo.Locations ON " +
                                            " dbo.Pallets.LocationId = dbo.Locations.Id WHERE (dbo.Styles.Code BETWEEN '" + fromStyle + "' AND " +
                                            " '" + toStyle + "') AND(dbo.CartonHeaders.WIPArea = 2) AND(dbo.CartonHeaders.IsDeleted = 0) AND " +
                                            " (dbo.Pallets.Code BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND " +
                                            " (dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') AND " +
                                            " (dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            " GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.BoxCPOAllocationDetails.CPO, " +
                                            "  dbo.Pallets.Code, dbo.Locations.Code, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode " +
                                            " ORDER BY Style, Colour, Size, dbo.BoxCPOAllocationDetails.CPO, Pallet, Rack, dbo.CartonHeaders.WIPArea, BarCode");



            //SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code AS Pallet, dbo.Locations.Code AS Rack, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode AS BarCode, " +
            //                                                  "SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.CartonDetails INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
            //                                                  "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
            //                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            //                                                  "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            //                                "WHERE (dbo.CartonHeaders.IsDeleted = 0) AND " +
            //                                "(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') AND " +
            //                                "(dbo.Pallets.Code BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND " +
            //                                "(dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') " +
            //                                "AND (dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "AND ( dbo.CartonHeaders.WIPArea=2) " +
            //                                "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code, dbo.Locations.Code, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode " +
            //                                "ORDER BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code, dbo.Locations.Code, dbo.CartonHeaders.WIPArea, dbo.Boxes.BoxCode");

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

        public GoodsReceivedDetailesDS getGoodsReceivedDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {
            
            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD,dbo.Boxes.BoxCode AS CartonNumber, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, " +
            //                                                  "dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.Colors INNER JOIN " +
            //                                                  "dbo.Sizes INNER JOIN " +
            //                                                  "dbo.Styles INNER JOIN " +
            //                                                  "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId ON dbo.Sizes.Id = dbo.Products.SizeId INNER JOIN " +
            //                                                  "dbo.AODs INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId ON dbo.Colors.Id = dbo.Products.ColorId INNER JOIN " +
            //                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id " +
            //                                "WHERE(dbo.AODs.DstinationWarehouse = N'" + factoryName + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.Boxes.BoxCode, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, " +
            //                                                  "dbo.Colors.Code, dbo.Sizes.Code " +
            //                                "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.Boxes.BoxCode, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, " +
            //                                                  "dbo.Colors.Code, dbo.Sizes.Code");

            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD,dbo.Boxes.BoxCode AS CartonNumber, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, " +
                                            "dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM  dbo.Colors INNER JOIN dbo.Sizes INNER JOIN dbo.Styles INNER JOIN " +
                                            "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId ON dbo.Sizes.Id = dbo.Products.SizeId INNER JOIN " +
                                            "dbo.AODs INNER JOIN " +
                                            "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                            "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                            "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId ON dbo.Colors.Id = dbo.Products.ColorId INNER JOIN " +
                                            "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id " +
                                            "WHERE(dbo.AODs.DstinationWarehouse = N'" + factoryName + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
                                            "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.Boxes.BoxCode, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, " +
                                                              "dbo.Colors.Code, dbo.Sizes.Code " +
                                            "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.Boxes.BoxCode, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, " +
                                                              "dbo.Colors.Code, dbo.Sizes.Code");


            
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
        public DataTable getGoodsReceivedStatus(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)     // Chaminda
        {
            //SqlCommand cmd = new SqlCommand("SELECT Date, LorryNumber, ToFactory AS SourceFactory, AOD, CartonNumber, MPO, CPO, Style, Colour, Size, Quantity, BoxId, WIPArea, CartonWIPId " +
            //                                "FROM GoodsReceivedStatus " +
            //                                "WHERE(CartonWIPId IN ((SELECT MAX(CartonWIPId) AS ID FROM GoodsReceivedStatus AS GoodsReceivedStatus_1 GROUP BY BoxId))) " +
            //                                "AND(ToFactory = '" + factoryName + "') " +
            //                                "AND(CAST(Date AS DATE) >= '" + fromDate + "') " +
            //                                "AND(CAST(Date AS DATE) <= '" + toDate + "') " +
            //                                "AND(AOD BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(FromFactory BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
            //                                "AND(CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "')");


            //cmd.CommandTimeout = 0;
            //using (SqlDataAdapter sda = new SqlDataAdapter())
            //{
            //    cmd.Connection = conn;
            //    conn.Open();
            //    sda.SelectCommand = cmd;
            //    using (GoodsReceivedStatusDS FHC = new GoodsReceivedStatusDS())
            //    {
            //        sda.Fill(FHC, "GoodsReceivedStatusDS");
            //        conn.Close();
            //        return FHC;
            //    }
            //}

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            DataTable dt = new DataTable();
            using (var cmd = new SqlCommand("GRN_Status", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);
                cmd.Parameters.AddWithValue("@FromAOD", fromAOD);
                cmd.Parameters.AddWithValue("@ToAOD", toAOD);
                cmd.Parameters.AddWithValue("@FromFactory", fromFactory);
                cmd.Parameters.AddWithValue("@ToFactory", toFactory);
                cmd.Parameters.AddWithValue("@FromCPO", fromCPO);
                cmd.Parameters.AddWithValue("@ToCPO", toCPO);
                cmd.Parameters.AddWithValue("@DestinationFactory", factoryName);
                cmd.CommandTimeout = 0;
                da.Fill(dt);
            }

            //using (GoodsReceivedStatusDS FHC = new GoodsReceivedStatusDS())
            //{
            //    da.Fill(FHC);
            //    conn.Close();
            //    return FHC;
            //}
            return dt;
        }
        public DataTable getGoodsReceivedStatus_NotReceived(string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)     // Chaminda
        {

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            DataTable dt = new DataTable();
            using (var cmd = new SqlCommand("GRN_Status_NotRecieved", conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", fromDate);
                //cmd.Parameters.AddWithValue("@ToDate", toDate);
                cmd.Parameters.AddWithValue("@FromAOD", fromAOD);
                cmd.Parameters.AddWithValue("@ToAOD", toAOD);
                cmd.Parameters.AddWithValue("@FromFactory", fromFactory);
                cmd.Parameters.AddWithValue("@ToFactory", toFactory);
                cmd.Parameters.AddWithValue("@FromCPO", fromCPO);
                cmd.Parameters.AddWithValue("@ToCPO", toCPO);
                cmd.Parameters.AddWithValue("@DestinationFactory", factoryName);
                cmd.CommandTimeout = 0;
                da.Fill(dt);
            }

            return dt;
        }
        public GoodsReceivedSummaryDS getGoodsReceivedSummary(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {
            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date,  dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO,COUNT(DISTINCT dbo.Boxes.BoxCode) AS NumberOfCartons,  SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.Colors INNER JOIN " +
            //                                                  "dbo.Sizes INNER JOIN " +
            //                                                  "dbo.Styles INNER JOIN " +
            //                                                  "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId ON dbo.Sizes.Id = dbo.Products.SizeId INNER JOIN " +
            //                                                  "dbo.AODs INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId ON dbo.Colors.Id = dbo.Products.ColorId INNER JOIN " +
            //                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id " +
            //                                "WHERE(dbo.AODs.DstinationWarehouse = N'" + factoryName + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
            //                                "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO");

            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date,  dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO,COUNT(DISTINCT dbo.Boxes.BoxCode) AS NumberOfCartons,  SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.Colors INNER JOIN dbo.Sizes INNER JOIN dbo.Styles INNER JOIN " +
                                           "dbo.Products ON dbo.Styles.Id = dbo.Products.StyleId ON dbo.Sizes.Id = dbo.Products.SizeId INNER JOIN " +
                                           "dbo.AODs INNER JOIN " +
                                           "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                           "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                           "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                           "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId ON dbo.Colors.Id = dbo.Products.ColorId INNER JOIN " +
                                           "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id " +
                                            "WHERE(dbo.AODs.DstinationWarehouse = N'" + factoryName + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
                                            "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
                                            "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO");



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
       
        public ShipmentDetailsDS getShipmentDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {

            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code AS MPO, " +
            //                                                  "dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.AODs INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
            //                                                  "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
            //                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
            //                                "WHERE(dbo.AODs.DstinationWarehouse = 'SHIPMENT') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse = '" + factoryName + "') " +
            //                                "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
            //                                                  "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
            //                                "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
            //                                                  "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");


            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code AS MPO, " +
                                            "dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.AODs INNER JOIN  dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                            "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                            "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                            "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                            "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                            "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                            "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                            "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
                                            "WHERE(dbo.AODs.DstinationWarehouse = 'SHIPMENT') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse = '" + factoryName + "') " +
                                            "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
                                            "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                            "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
                                            "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");





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

        public ShipmentSummaryDS getShipmentSummary(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {
            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date,  dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, COUNT(DISTINCT dbo.Boxes.BoxCode) AS NumberOfCartons, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.AODs INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
            //                                                  "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
            //                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
            //                                "WHERE(dbo.AODs.DstinationWarehouse = N'SHIPMENT') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
            //                                "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse = N'" + factoryName + "') " +
            //                                "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
            //                                "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO");

            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date,  dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, COUNT(DISTINCT dbo.Boxes.BoxCode) AS NumberOfCartons, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                           "FROM     dbo.AODs INNER JOIN " +
                                           "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                           "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                           "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                           "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                           "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                           "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                           "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                           "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                           "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
                                           "WHERE(dbo.AODs.DstinationWarehouse = N'SHIPMENT') " +
                                           "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                           "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                           "AND(dbo.AODs.SourceWarehouse = N'" + factoryName + "') " +
                                           "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
                                           "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO");



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

        public StockTakePalletDetailsDS getStockTakePalletDetails(string fromDate, string toDate, string FromPallet, string ToPallet)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT 'Saved'AS Status, (CAST(dbo.FGStockTakePostCountCartons.EffectiveDate AS DATE)) AS Date, dbo.Pallets.Code AS Pallet, dbo.Boxes.BoxCode AS BoxBarCode " +
                                            "FROM     dbo.FGStockTakePostCountCartons INNER JOIN " +
                                                              "dbo.Boxes ON dbo.FGStockTakePostCountCartons.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.Pallets ON dbo.FGStockTakePostCountCartons.PalletId = dbo.Pallets.Id " +
                                            "WHERE(dbo.FGStockTakePostCountCartons.IsDeleted = 0) AND(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') AND(CAST(dbo.FGStockTakePostCountCartons.EffectiveDate AS DATE) >= '" + fromDate + "') AND " +
                                                              "(CAST(dbo.FGStockTakePostCountCartons.EffectiveDate AS DATE) <= '" + toDate + "') " +
                                            "UNION " +

                                            "SELECT 'Un Saved'AS Status, (CAST(dbo.UnsavedBoxes.SavedDate AS DATE)) AS DATE, dbo.Pallets.Code AS Pallet, dbo.UnsavedBoxes.BoxCode AS BoxBarCode " +
                                            "FROM     dbo.UnsavedBoxes INNER JOIN " +
                                                              "dbo.Pallets ON dbo.UnsavedBoxes.PalletId = dbo.Pallets.Id " +
                                            "WHERE(CAST(dbo.UnsavedBoxes.SavedDate AS DATE) >= '" + fromDate + "') AND(CAST(dbo.UnsavedBoxes.SavedDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') " +

                                            "ORDER BY Status, Date, Pallet, BoxBarCode");


            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (StockTakePalletDetailsDS Customer = new StockTakePalletDetailsDS())
                {
                    sda.Fill(Customer, "StockTakePalletDetailsDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public CartonWiseStockReportDS getCartonWiseStockReportDetails(string fromDate, string toDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, dbo.Boxes.BoxCode, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.BoxCPOAllocationDetails.CPO, " +
                  "dbo.ProdOrders.Code AS MPO, dbo.CartonDetails.Quantity " +
"FROM     dbo.Colors INNER JOIN " +
                  "dbo.CartonDetails INNER JOIN " +
                  "dbo.Boxes INNER JOIN " +
                  "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                  "dbo.Products INNER JOIN " +
                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id ON dbo.CartonDetails.ProductId = dbo.Products.Id ON dbo.Colors.Id = dbo.Products.ColorId INNER JOIN " +
                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                  "dbo.FGStockTakePostCountCartons ON dbo.Boxes.Id = dbo.FGStockTakePostCountCartons.BoxId INNER JOIN " +
                  "dbo.Locations INNER JOIN " +
                  "dbo.Pallets ON dbo.Locations.Id = dbo.Pallets.LocationId ON dbo.FGStockTakePostCountCartons.PalletId = dbo.Pallets.Id " +
"WHERE (dbo.CartonHeaders.IsDeleted = 0) AND (CAST(dbo.FGStockTakePostCountCartons.EffectiveDate AS date) >= CAST('" + fromDate + "' AS date)) AND(CAST(dbo.FGStockTakePostCountCartons.EffectiveDate AS date) <= CAST('" + toDate + "' AS date)) " +
"ORDER BY Location, Pallet, dbo.Boxes.BoxCode");


            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (CartonWiseStockReportDS Customer = new CartonWiseStockReportDS())
                {
                    sda.Fill(Customer, "CartonWiseStockReportDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public FactoryTransfersDS getFactoryTransferDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO)
        {
            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
            //                                                  "dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                                "FROM     dbo.Products INNER JOIN " +
            //                                                  "dbo.CartonWips INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
            //                                                  "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
            //                                "WHERE(dbo.CartonWips.TransactionType IN(8, 10)) AND(dbo.CartonWips.WIPArea = 3) AND(dbo.CartonWips.Quantity < 0) " +
            //                                "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + fromCPO + "') " +
            //                                "GROUP BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
            //                                "ORDER BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");

            //SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, UPPER(dbo.AODs.LorryNumber) AS LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
            //                                                  "dbo.Boxes.BoxCode AS CartonNumber, dbo.CartonDetails.Quantity AS Quantity " +
            //                                "FROM     dbo.Products INNER JOIN " +
            //                                                  "dbo.CartonWips INNER JOIN " +
            //                                                  "dbo.AODBoxDetails ON dbo.CartonWips.BoxId = dbo.AODBoxDetails.BoxId INNER JOIN " +
            //                                                  "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
            //                                                  "dbo.BoxCPOAllocationDetails ON dbo.CartonWips.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
            //                                                  "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
            //                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
            //                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                                                  "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id " +
            //                                "WHERE(dbo.CartonWips.TransactionType =10) AND(dbo.CartonWips.WIPArea = 3) AND(dbo.CartonWips.Quantity < 0) " +
            //                                "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') " +
            //                                "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
            //                                "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
            //                                "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
            //                                "AND (dbo.AODs.LorryNumber IS NOT NULL) "+
            //                                "GROUP BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode,dbo.CartonDetails.Quantity " +
            //                                "ORDER BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode,dbo.CartonDetails.Quantity");

            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.CartonWips.EffectiveDate AS DATE)) AS Date, UPPER(dbo.AODs.LorryNumber)AS LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, " +
                                                              "(dbo.Boxes.BoxCode) AS CartonNumber, dbo.CartonDetails.Quantity AS Quantity " +
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
                                            "WHERE " +
                                            "(dbo.CartonWips.TransactionType = 8) " +
                                            "AND(dbo.CartonWips.WIPArea = 3) " +
                                            "AND(dbo.CartonWips.Quantity < 0) " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.CartonWips.EffectiveDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.SourceWarehouse BETWEEN '" + fromFactory + "' AND '" + toFactory + "') " +
                                            "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
                                            "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            //"AND(dbo.AODs.LorryNumber IS NOT NULL) " +
                                            //"AND (dbo.AODs.LorryNumber <> N'00') " +
                                            "GROUP BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode, dbo.CartonDetails.Quantity " +
                                            "ORDER BY(CAST(dbo.CartonWips.EffectiveDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode, dbo.CartonDetails.Quantity");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (FactoryTransfersDS FHC = new FactoryTransfersDS())
                {
                    sda.Fill(FHC, "FactoryTransfersDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public DataTable getAODDetailsforPrintN(int AODId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            //newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                     "FROM dbo.AODBoxDetails INNER JOIN " +
            //                     "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                     "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
            //                     "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                     "dbo.Products INNER JOIN " +
            //                     "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                     "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                     "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id ON dbo.CartonDetails.ProductId = dbo.Products.Id " +
            //                     "WHERE (dbo.AODBoxDetails.AODId = " + AODId + ") AND (dbo.CartonDetails.TransactionType =2) AND (dbo.CartonDetails.Quantity > 0) " +
            //                     "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code " +
            //                     "ORDER BY Colour, Size";

            newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes, " +
                                                 "SUM(dbo.CartonDetails.Quantity)AS Quantity , dbo.AODs.TransferredDate " +
                                "FROM     dbo.AODBoxDetails INNER JOIN " +
                                                 "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                 "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                 "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                 "dbo.Products INNER JOIN " +
                                                 "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                 "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                 "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                 "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                 "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId " +
                                //"WHERE(dbo.AODBoxDetails.AODId = " + AODId + ") AND(dbo.CartonDetails.TransactionType <> 8 OR " +
                                //                 "dbo.CartonDetails.TransactionType <> 10) AND(dbo.CartonDetails.Quantity > 0) " +
                                "WHERE (dbo.AODBoxDetails.AODId = " + AODId + ") " +
                                "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, " +
                                " dbo.AODs.TransferredDate ORDER BY Colour, Size";

            //newCmd.CommandText = "SELECT DISTINCT dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, COUNT(DISTINCT dbo.Boxes.Id) AS NoOfBoxes, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
            //                     "FROM dbo.AODBoxDetails INNER JOIN " +
            //                     "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                     "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
            //                     "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                     "dbo.Products INNER JOIN " +
            //                     "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
            //                     "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
            //                     "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id ON dbo.CartonDetails.ProductId = dbo.Products.Id " +
            //                     "WHERE (dbo.AODBoxDetails.AODId = " + AODId + ") " +
            //                     "GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code " +
            //                     "ORDER BY Colour, Size";



            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public string getLorryNumber(int AODId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT LorryNumber FROM dbo.AODs WHERE (Id = " + AODId + ")", conn);

            string AODNumber = com.ExecuteScalar().ToString();
            conn.Close();
            return AODNumber;
        }

        public string getAXTrNumber(int Aodid)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT AxJournalNo FROM AxAoDTrnDetails WHERE (AODId =  '" + Aodid + "')  group by AxJournalNo", conn);
            string AxTrn = Convert.ToString(com.ExecuteScalar());
            conn.Close();
            return AxTrn;
        }

        public int getAODBoxCount(int Aodid)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT Count(*) FROM  AODBoxDetails WHERE(AODId =  '" + Aodid + "')", conn);
            int BCount = Convert.ToInt16(com.ExecuteScalar());
            conn.Close();
            return BCount;
        }








        public string getDestination(int AODId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT DstinationWarehouse FROM dbo.AODs WHERE (Id = " + AODId + ")", conn);

            string destination = com.ExecuteScalar().ToString();
            conn.Close();
            return destination;
        }

        public AODCheckListDS getAODCheckListDetails(string AodNumber)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT dbo.AODs.AODNumber AS AOD, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode, " +
                                                  "SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                "FROM dbo.AODBoxDetails INNER JOIN " +
                                                  "dbo.AODs ON dbo.AODBoxDetails.AODId = dbo.AODs.Id INNER JOIN " +
                                                  "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                  "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                  "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                  "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId " +
                                "WHERE(dbo.AODs.AODNumber = '" + AodNumber + "') " +
                                "GROUP BY dbo.AODs.AODNumber, dbo.Styles.Code, dbo.Colors.Code,  dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                "ORDER BY AOD, Style, Colour, MPO, dbo.BoxCPOAllocationDetails.CPO, Size, dbo.Boxes.BoxCode";




            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (AODCheckListDS Customer = new AODCheckListDS())
                {
                    sda.Fill(Customer, "AODCheckListDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public WIPAtDefaultBoxDS getDefaultBoxDetails(int BoxId, string FromStyle, string ToStyle)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT dbo.Styles.Code AS Style,dbo.BoxCPOAllocationDetails.CPO,dbo.ProdOrders.Code AS MPO, dbo.Colors.Code AS Colour,   dbo.Sizes.Code AS Size, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                "FROM dbo.CartonDetails INNER JOIN " +
                                                  "dbo.CartonWips ON dbo.CartonDetails.BoxId = dbo.CartonWips.BoxId INNER JOIN " +
                                                  "dbo.BoxCPOAllocationDetails ON dbo.CartonDetails.BoxId = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                  "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                  "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                  "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                  "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id " +
                                "WHERE(dbo.CartonDetails.BoxId = " + BoxId + ") AND(dbo.CartonWips.WIPArea = 1) AND (dbo.Styles.Code BETWEEN '" + FromStyle + "' AND '" + ToStyle + "')" +
                                "GROUP BY dbo.Styles.Code, dbo.BoxCPOAllocationDetails.CPO,dbo.ProdOrders.Code, dbo.Colors.Code, dbo.Sizes.Code " +
                                "ORDER BY dbo.Styles.Code, dbo.BoxCPOAllocationDetails.CPO,dbo.ProdOrders.Code, dbo.Colors.Code, dbo.Sizes.Code";

            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (WIPAtDefaultBoxDS Customer = new WIPAtDefaultBoxDS())
                {
                    sda.Fill(Customer, "WIPAtDefaultBoxDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public StockPositionReportDS getStockPositionReport(string fromStyle, string toStyle, string date)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("GetStockPositionReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@FromStyle", fromStyle));
            cmd.Parameters.Add(new SqlParameter("@ToStyle", toStyle));
            cmd.Parameters.Add(new SqlParameter("@ToDate", date));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (StockPositionReportDS FHC = new StockPositionReportDS())
            {
                da.Fill(FHC, "StockPositionReportDS");
                conn.Close();
                return FHC;
            }

            //SqlCommand cmd = new SqlCommand("SELECT dbo.Styles.Code AS Style, RIGHT(dbo.Styles.Code,4) AS Season, " +
            //                               " dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, SUBSTRING(dbo.Boxes.BoxCode, " +
            //                               " 1,case when  CHARINDEX('-', dbo.Boxes.BoxCode) = 0 then LEN(dbo.Boxes.BoxCode) else " +
            //                               " CHARINDEX('-', dbo.Boxes.BoxCode) - 1 end) AS Factory," +
            //                               " dbo.Boxes.BoxCode, SUM(dbo.CartonDetails.Quantity) as Quantity " +
            //                               " FROM  dbo.Boxes INNER JOIN dbo.Styles INNER JOIN " +
            //                               " dbo.Products INNER JOIN dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId ON " +
            //                               " dbo.Styles.Id = dbo.Products.StyleId INNER JOIN  dbo.Colors ON " +
            //                               " dbo.Products.ColorId = dbo.Colors.Id INNER JOIN  dbo.Sizes ON " +
            //                               " dbo.Products.SizeId = dbo.Sizes.Id ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                               " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN " +
            //                               " dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id " +
            //                               " WHERE(dbo.CartonHeaders.IsDeleted = 0) AND dbo.Boxes.Id IN(SELECT BoxId FROM   dbo.CartonWips " +
            //                               " WHERE(CAST(EffectiveDate AS Date) <= CAST('" + date + "' AS Date)) AND " +
            //                               " (WIPArea = 2) GROUP BY BoxId HAVING(SUM(Quantity) > 0)) AND " +
            //                               " dbo.Pallets.code != 'StockWriteOff' " +
            //                               " GROUP BY dbo.Styles.Code, RIGHT(dbo.Styles.Code, 4), dbo.Colors.Code, " +
            //                               " dbo.Sizes.Code, SUBSTRING(dbo.Boxes.BoxCode, 1, case when  CHARINDEX('-', dbo.Boxes.BoxCode) = 0 then " +
            //                               " LEN(dbo.Boxes.BoxCode) else CHARINDEX('-', dbo.Boxes.BoxCode) - 1 end), dbo.Boxes.BoxCode " +
            //                               " HAVING SUM(dbo.CartonDetails.Quantity) > 0 AND(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND " +
            //                               " '" + toStyle + "') ORDER BY dbo.Styles.Code, RIGHT(dbo.Styles.Code, 4), dbo.Colors.Code, " +
            //                               " dbo.Sizes.Code, Factory, dbo.Boxes.BoxCode");

            //SqlCommand cmd = new SqlCommand("SELECT  dbo.Styles.Code AS Style, RIGHT(dbo.Styles.Code, 4) AS Season, " +
            //                                " dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size,dbo.CartonHeaders.ProducedFactory AS Factory, " +
            //                                " dbo.Boxes.BoxCode, SUM(dbo.CartonDetails.Quantity) AS Quantity, dbo.CartonWips.CPO " +
            //                                " FROM   dbo.Boxes INNER JOIN dbo.Styles INNER JOIN dbo.Products INNER JOIN " +
            //                                " dbo.CartonDetails ON dbo.Products.Id = dbo.CartonDetails.ProductId ON " +
            //                                " dbo.Styles.Id = dbo.Products.StyleId INNER JOIN dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
            //                                " dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                                " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            //                                " dbo.CartonWips ON dbo.Boxes.Id = dbo.CartonWips.BoxId WHERE(dbo.CartonHeaders.IsDeleted = 0) " +
            //                                " AND(dbo.Pallets.Code <> 'StockWriteOff') AND(dbo.Boxes.Id IN (SELECT BoxId FROM  dbo.CartonWips AS CartonWips_1 " +
            //                                " WHERE(CAST(EffectiveDate AS Date) <= CAST('" + date + "' AS Date)) AND(WIPArea = 2) " +
            //                                " GROUP BY BoxId HAVING(SUM(Quantity) > 0))) GROUP BY dbo.Styles.Code, RIGHT(dbo.Styles.Code, 4), " +
            //                                " dbo.Colors.Code, dbo.Sizes.Code, dbo.CartonHeaders.ProducedFactory,dbo.Boxes.BoxCode, dbo.CartonWips.CPO " +
            //                                " HAVING(SUM(dbo.CartonDetails.Quantity) > 0) AND(dbo.Styles.Code BETWEEN '" + fromStyle + "' AND '" + toStyle + "') " +
            //                                " ORDER BY Style, Season, Colour, Size, Factory, dbo.Boxes.BoxCode");



            //cmd.CommandTimeout = 0;
            //using (SqlDataAdapter sda = new SqlDataAdapter())
            //{
            //    cmd.Connection = conn;
            //    //conn.Open();
            //    sda.SelectCommand = cmd;
            //    using (StockPositionReportDS FHC = new StockPositionReportDS())
            //    {
            //        sda.Fill(FHC, "StockPositionReportDS");
            //        conn.Close();
            //        return FHC;
            //    }
            //}
        }

        public DataTable getConsolidatedStockReport()
        {


            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("ConsolidatedReport", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add(new SqlParameter("@FoDate", FoDate));
            //cmd.Parameters.Add(new SqlParameter("@ToDate", ToDate));

            //int rowcount = Convert.ToInt32(cmd.ExecuteNonQuery());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getDPAgingReport()
        {


            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("DefaultPalletAging", conn);
            cmd.CommandType = CommandType.StoredProcedure;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }


        public DataTable getRightoffPalletDetails(string FoDate, string ToDate)
        {


            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("RightOffPalletAging", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FromDate", FoDate));
            cmd.Parameters.Add(new SqlParameter("@ToDate", ToDate));


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }


        public DataTable getCodeChangeReDetails(string FoDate, string ToDate)
        {


            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            //SqlCommand cmd = new SqlCommand("Select * from CodeChangeView where " +
            //                                " TranDate  >= '" + FoDate + "'  and " +
            //                                " TranDate <= '" + ToDate + "'", conn);

            SqlCommand cmd = new SqlCommand("Select * from CodeChangeView", conn);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }




        public FactoryTransferDetailsDS getFactoryTransferDetails(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {
            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date, dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse AS SourceFactory,dbo.AODs.DstinationWarehouse AS DestinationFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code AS MPO, " +
                                                              "dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Boxes.BoxCode AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.AODs INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                                              "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
                                            "WHERE(dbo.AODs.DstinationWarehouse <> 'SHIPMENT') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
                                            "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            "AND(dbo.AODs.SourceWarehouse = '" + factoryName + "') " +
                                            "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.DstinationWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
                                                              "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode " +
                                            "HAVING SUM(dbo.CartonDetails.Quantity)<>0 " +
                                            "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.LorryNumber, dbo.AODs.SourceWarehouse, dbo.AODs.DstinationWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO, dbo.ProdOrders.Code, " +
                                                              "dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode");


            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (FactoryTransferDetailsDS FHC = new FactoryTransferDetailsDS())
                {
                    sda.Fill(FHC, "FactoryTransferDetailsDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public FactoryTransferSummaryDS getFactoryTransferSummary(string fromDate, string toDate, string fromFactory, string toFactory, string fromAOD, string toAOD, string fromCPO, string toCPO, string factoryName)
        {
            SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.AODs.TransferredDate AS DATE)) AS Date,  dbo.AODs.SourceWarehouse AS SourceFactory,dbo.AODs.DstinationWarehouse AS DestinationFactory, dbo.AODs.AODNumber AS AOD, dbo.BoxCPOAllocationDetails.CPO, COUNT(DISTINCT dbo.Boxes.BoxCode) AS CartonNumber, SUM(dbo.CartonDetails.Quantity) AS Quantity " +
                                            "FROM     dbo.AODs INNER JOIN " +
                                                              "dbo.AODBoxDetails ON dbo.AODs.Id = dbo.AODBoxDetails.AODId INNER JOIN " +
                                                              "dbo.Boxes ON dbo.AODBoxDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id " +
                                            "WHERE(dbo.AODs.DstinationWarehouse <> 'SHIPMENT') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) >= '" + fromDate + "') " +
                                            "AND(CAST(dbo.AODs.TransferredDate AS DATE) <= '" + toDate + "') " +
                                            "AND(dbo.AODs.AODNumber BETWEEN '" + fromAOD + "' AND '" + toAOD + "') " +
                                            "AND(dbo.BoxCPOAllocationDetails.CPO BETWEEN '" + fromCPO + "' AND '" + toCPO + "') " +
                                            "AND(dbo.AODs.SourceWarehouse = '" + factoryName + "') " +
                                            "GROUP BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.DstinationWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO " +
                                            "HAVING SUM(dbo.CartonDetails.Quantity)<>0" +
                                            "ORDER BY(CAST(dbo.AODs.TransferredDate AS DATE)), dbo.AODs.SourceWarehouse, dbo.AODs.DstinationWarehouse, dbo.AODs.AODNumber, dbo.BoxCPOAllocationDetails.CPO");

            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (FactoryTransferSummaryDS FHC = new FactoryTransferSummaryDS())
                {
                    sda.Fill(FHC, "FactoryTransferSummaryDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public PIPreAndPostCountDS getPiPreAndPostCounts(int RefId, string FromPallet, string ToPallet, string FromLocation, string ToLocation)
        {
            SqlCommand cmd = new SqlCommand("SELECT dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, COUNT(DISTINCT dbo.Boxes.BoxCode) AS 'NoOfCartons', SUM(dbo.CartonDetails.Quantity) AS Quantity, 'PRE' AS Category " +
                                             "FROM     dbo.FGStockTakePreCountCartons INNER JOIN " +
                                                               "dbo.Boxes ON dbo.FGStockTakePreCountCartons.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                               "dbo.Pallets ON dbo.FGStockTakePreCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                               "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                               "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                             "WHERE(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') AND(dbo.Locations.Code BETWEEN '" + FromLocation + "' AND '" + ToLocation + "') AND(dbo.FGStockTakePreCountCartons.PIReferenceId = " + RefId + ") " +
                                             "GROUP BY dbo.Locations.Code, dbo.Pallets.Code " +
                                             "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
                                             "UNION " +
                                             "SELECT  dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, COUNT(DISTINCT dbo.Boxes.BoxCode) AS 'NoOfCartons', SUM(dbo.CartonDetails.Quantity) AS Quantity, 'POST' AS Category " +
                                             "FROM     dbo.Boxes INNER JOIN " +
                                                               "dbo.FGStockTakePostCountCartons ON dbo.Boxes.Id = dbo.FGStockTakePostCountCartons.BoxId INNER JOIN " +
                                                               "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
                                                               "dbo.Pallets ON dbo.FGStockTakePostCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                               "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                             "WHERE(dbo.FGStockTakePostCountCartons.PIReferenceId = " + RefId + ") AND(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') AND(dbo.Locations.Code BETWEEN '" + FromLocation + "' AND '" + ToLocation + "') " +
                                             "GROUP BY dbo.Locations.Code, dbo.Pallets.Code " +
                                             "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
                                             "ORDER BY Category DESC, dbo.Locations.Code, dbo.Pallets.Code");
            //SqlCommand cmd = new SqlCommand("SELECT dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, COUNT(DISTINCT dbo.Boxes.BoxCode) AS 'NoOfCartons', SUM(dbo.CartonDetails.Quantity) AS Quantity, 'PRE' AS Category " +
            //                                "FROM dbo.FGStockTakePreCountCartons INNER JOIN " +
            //                                "dbo.Boxes ON dbo.FGStockTakePreCountCartons.BoxId = dbo.Boxes.Id INNER JOIN " +
            //                                "dbo.Pallets ON dbo.FGStockTakePreCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
            //                                "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                                "dbo.PIPallets ON dbo.Pallets.Id = dbo.PIPallets.PalletId INNER JOIN " +
            //                                "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            //                                "WHERE(dbo.FGStockTakePreCountCartons.PIReferenceId = " + RefId + ") AND(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') AND(dbo.Locations.Code BETWEEN '" + FromLocation + "' AND '" + ToLocation + "') " +
            //                                "GROUP BY dbo.Locations.Code, dbo.Pallets.Code " +
            //                                "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
            //                                "UNION " +
            //                                "SELECT dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet, COUNT(DISTINCT dbo.Boxes.BoxCode) AS 'NoOfCartons', SUM(dbo.CartonDetails.Quantity) AS Quantity, 'POST' AS Category " +
            //                                "FROM dbo.Boxes INNER JOIN " +
            //                                "dbo.FGStockTakePostCountCartons ON dbo.Boxes.Id = dbo.FGStockTakePostCountCartons.BoxId INNER JOIN " +
            //                                "dbo.Pallets ON dbo.FGStockTakePostCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
            //                                "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
            //                                "dbo.PIPallets ON dbo.Pallets.Id = dbo.PIPallets.PalletId INNER JOIN " +
            //                                "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            //                                "WHERE(dbo.FGStockTakePostCountCartons.PIReferenceId = " + RefId + ") AND(dbo.Pallets.Code BETWEEN '" + FromPallet + "' AND '" + ToPallet + "') AND(dbo.Locations.Code BETWEEN '" + FromLocation + "' AND '" + ToLocation + "') " +
            //                                "GROUP BY dbo.Locations.Code, dbo.Pallets.Code " +
            //                                "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
            //                                "ORDER BY Category DESC, dbo.Locations.Code, dbo.Pallets.Code");

            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (PIPreAndPostCountDS FHC = new PIPreAndPostCountDS())
                {
                    sda.Fill(FHC, "PIPreAndPostCountDS");
                    conn.Close();
                    return FHC;
                }
            }

        }

        public LocationDisplayDS getLocationDisplayDetails()
        {
           
            SqlCommand cmd = new SqlCommand("(SELECT " +
                                            "ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
                                            "(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'A' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
                                                "ISNULL(dbo.Locations.Code, 0) AS Location " +
                                                        "FROM dbo.CartonHeaders INNER JOIN " +
                                                        "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                        "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
                                                        "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                                        "GROUP BY dbo.Locations.Code " +
                                                        "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'A' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
                                            "UNION " +
                                            "(SELECT " +
                                            "ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
                                            "(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'B' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
                                                "ISNULL(dbo.Locations.Code, 0) AS Location " +
                                                        "FROM dbo.CartonHeaders INNER JOIN " +
                                                        "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                        "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
                                                        "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                                        "GROUP BY dbo.Locations.Code " +
                                                        "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'B' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
                                            "UNION " +
                                            "(SELECT " +
                                            "ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
                                            "(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'C' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
                                                "ISNULL(dbo.Locations.Code, 0) AS Location " +
                                                        "FROM dbo.CartonHeaders INNER JOIN " +
                                                        "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                                        "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
                                                        "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                                        "GROUP BY dbo.Locations.Code " +
                                                        "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'C' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
                                                        "UNION "+
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'D' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
"ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM     dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'D' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'E' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'E' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'F' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'F' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'G' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'G' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'H' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'H' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'I' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'I' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'J' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'J' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'K' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'K' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'L' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'L' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'M' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'M' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'N' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'N' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'O' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'O' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'P' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'P' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Q' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Q' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'R' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'R' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'S' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'S' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'T' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'T' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'U' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'U' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'V' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'V' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'W' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'W' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'X' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'X' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Y' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Y' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
"UNION " +
"(SELECT " +
"ROW_NUMBER() OVER(ORDER BY ISNULL(LEFT(dbo.Locations.Code, 1), 0), ISNULL(dbo.Locations.Code, 0)) AS Sequence, " +
"(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Z' THEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) ELSE 'NA' END) AS Prefix, " +
    "ISNULL(dbo.Locations.Code, 0) AS Location " +
            "FROM dbo.CartonHeaders INNER JOIN " +
            "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
            "dbo.Boxes ON dbo.CartonHeaders.BoxId = dbo.Boxes.Id FULL OUTER JOIN " +
            "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
            "GROUP BY dbo.Locations.Code " +
            "HAVING COUNT(dbo.Boxes.BoxCode) = 0 AND(CASE WHEN ISNULL(LEFT(dbo.Locations.Code, 1), 0) = 'Z' THEN ISNULL(dbo.Locations.Code, 0) ELSE 'NA' END) <> 'NA') " +
            "ORDER BY Prefix,Sequence, Location");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (LocationDisplayDS FHC = new LocationDisplayDS())
                {
                    sda.Fill(FHC, "LocationDisplayDS");
                    conn.Close();
                    return FHC;
                }
            }

        }

        //public PIPreAndPostDetailsDS getPIPreAndPostDetails(string fromDate, string toDate, string fromPI, string toPI, string fromPallet, string toPallet, string fromLocation, string toLocation)
        //{
        //    SqlCommand cmd = new SqlCommand("SELECT (CAST(dbo.PIs.CreatedDate AS DATE)) AS PIDate, dbo.PIs.PIReference AS Reference, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet,dbo.BoxCPOAllocationDetails.CPO AS CPO , dbo.Boxes.BoxCode, SUM(dbo.CartonDetails.Quantity) AS Quantity, 'PRE' AS Category " +
        //                                    "FROM     dbo.FGStockTakePreCountCartons INNER JOIN " +
        //                                                      "dbo.Boxes ON dbo.FGStockTakePreCountCartons.BoxId = dbo.Boxes.Id INNER JOIN " +
        //                                                      "dbo.Pallets ON dbo.FGStockTakePreCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
        //                                                      "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId INNER JOIN " +
        //                                                      "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id INNER JOIN " +
        //                                                      "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
        //                                                      "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
        //                                                      "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
        //                                                      "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
        //                                                      "dbo.PIs ON dbo.FGStockTakePreCountCartons.PIReferenceId = dbo.PIs.Id " +
        //                                                      "INNER JOIN dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id=BoxCPOAllocationDetails.BoxId " +
        //                                    "WHERE(dbo.Pallets.Code BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND(dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') AND(dbo.FGStockTakePreCountCartons.IsDeleted = 0) " +
        //                                    "AND(CAST(dbo.PIs.CreatedDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.PIs.CreatedDate AS DATE) <= '" + toDate + "') AND(dbo.PIs.PIReference BETWEEN '" + fromPI + "' AND '" + toPI + "') " +
        //                                    "GROUP BY dbo.Locations.Code, dbo.Pallets.Code, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode, dbo.PIs.PIReference, dbo.PIs.CreatedDate,dbo.BoxCPOAllocationDetails.CPO " +
        //                                    "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
        //                                    "UNION " +
        //                                    "SELECT(CAST(dbo.PIs.CreatedDate AS DATE)) AS PIDate, dbo.PIs.PIReference AS Reference, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.Locations.Code AS Location, dbo.Pallets.Code AS Pallet,dbo.BoxCPOAllocationDetails.CPO AS CPO , dbo.Boxes.BoxCode, " +
        //                                                      "SUM(dbo.CartonDetails.Quantity) AS Quantity, 'POST' AS Category " +
        //                                    "FROM     dbo.Products INNER JOIN " +
        //                                                      "dbo.Boxes INNER JOIN " +
        //                                                      "dbo.CartonDetails ON dbo.Boxes.Id = dbo.CartonDetails.BoxId ON dbo.Products.Id = dbo.CartonDetails.ProductId INNER JOIN " +
        //                                                      "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
        //                                                      "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
        //                                                      "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
        //                                                      "dbo.FGStockTakePostCountCartons ON dbo.Boxes.Id = dbo.FGStockTakePostCountCartons.BoxId INNER JOIN " +
        //                                                      "dbo.Locations INNER JOIN " +
        //                                                      "dbo.Pallets ON dbo.Locations.Id = dbo.Pallets.LocationId ON dbo.FGStockTakePostCountCartons.PalletId = dbo.Pallets.Id INNER JOIN " +
        //                                                      "dbo.PIs ON dbo.FGStockTakePostCountCartons.PIReferenceId = dbo.PIs.Id " +
        //                                                      "INNER JOIN dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id=BoxCPOAllocationDetails.BoxId " +
        //                                    "WHERE(dbo.Pallets.Code BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND(dbo.Locations.Code BETWEEN '" + fromLocation + "' AND '" + toLocation + "') AND(dbo.FGStockTakePostCountCartons.IsDeleted = 0) " +
        //                                    "AND(CAST(dbo.PIs.CreatedDate AS DATE) >= '" + fromDate + "')AND(CAST(dbo.PIs.CreatedDate AS DATE) <= '" + toDate + "') AND(dbo.PIs.PIReference BETWEEN '" + fromPI + "' AND '" + toPI + "') " +
        //                                    "GROUP BY dbo.Locations.Code, dbo.Pallets.Code, dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, dbo.Boxes.BoxCode, dbo.PIs.PIReference, dbo.PIs.CreatedDate,dbo.BoxCPOAllocationDetails.CPO " +
        //                                    "HAVING(SUM(dbo.CartonDetails.Quantity) > 0) " +
        //                                    "ORDER BY Category DESC, PIDate, Reference, Style, Colour, Size, Location, Pallet,CPO, BoxCode");

        //    cmd.CommandTimeout = 0;
        //    using (SqlDataAdapter sda = new SqlDataAdapter())
        //    {
        //        cmd.Connection = conn;
        //        conn.Open();
        //        sda.SelectCommand = cmd;
        //        using (PIPreAndPostDetailsDS FHC = new PIPreAndPostDetailsDS())
        //        {
        //            sda.Fill(FHC, "PIPreAndPostDetailsDS");
        //            conn.Close();
        //            return FHC;
        //        }
        //    }
        //}

        public PIPreAndPostDetailsDSNew getPIPreAndPostDetails(string fromDate, string toDate, string fromPI, string toPI, string fromPallet, string toPallet, string fromLocation, string toLocation)
        {
            SqlCommand cmd = new SqlCommand("SELECT CONVERT(VARCHAR(10), CreatedDate, 105) AS PIDate,PIReference AS Reference, Style,Color AS Colour, Size,Location, Pallet, CPO,BoxCode,SUM(PRE) AS PRE,SUM(POST) AS POST " +
                                            "FROM PIPrePostCountDetailReport " +
                                            "WHERE(Pallet BETWEEN '" + fromPallet + "' AND '" + toPallet + "') AND(Location BETWEEN '" + fromLocation + "' AND '" + toLocation + "') " +
                                            "AND(CAST(CreatedDate AS DATE) >= '" + fromDate + "')AND(CAST(CreatedDate AS DATE) <= '" + toDate + "') AND(PIReference BETWEEN '" + fromPI + "' AND '" + toPI + "') " +
                                            "GROUP BY CreatedDate, PIReference, Style, Color, Size, Location, Pallet, CPO, BoxCode " +
                                            "ORDER BY CreatedDate, PIReference, Style, Color, Size, Location, Pallet, CPO, BoxCode, PRE, POST");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (PIPreAndPostDetailsDSNew FHC = new PIPreAndPostDetailsDSNew())
                {
                    sda.Fill(FHC, "PIPreAndPostDetailsDSNew");
                    conn.Close();
                    return FHC;
                }
            }
        }
        public BoxMovementReportDS getBoxMovementDetails(int BoxId)
        {

            SqlCommand cmd = new SqlCommand("SELECT dbo.Boxes.BoxCode AS Box, dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, dbo.Sizes.Code AS Size, dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, " +
                                                              "(ISNULL(dbo.Pallets.Code, 'N/A')) AS Pallet, (ISNULL(dbo.Locations.Code, 'N/A')) AS Location, dbo.CartonWips.EffectiveDate, dbo.CartonWips.TransactionType, dbo.CartonWips.WIPArea, dbo.CartonWips.Quantity * dbo.CartonDetails.Quantity AS Quantity, " +
                                                              "(CASE WHEN dbo.CartonWips.WIPArea = 1 THEN((dbo.CartonWips.Quantity) * (dbo.CartonDetails.Quantity)) ELSE 0 END) AS 'INQ',  " +
                                                              "(CASE WHEN dbo.CartonWips.WIPArea = 2 THEN((dbo.CartonWips.Quantity) * (dbo.CartonDetails.Quantity)) ELSE 0 END) AS 'WIP',  " +
                                                              "(CASE WHEN dbo.CartonWips.WIPArea = 3 THEN((dbo.CartonWips.Quantity) * (dbo.CartonDetails.Quantity)) ELSE 0 END) AS 'OUTQ' " +
                                            "FROM dbo.CartonWips INNER JOIN " +
                                                              "dbo.Boxes ON dbo.CartonWips.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.CartonDetails ON dbo.CartonWips.BoxId = dbo.CartonDetails.BoxId INNER JOIN " +
                                                              "dbo.Products ON dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.CartonDetails.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId FULL OUTER JOIN " +
                                                              "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id FULL OUTER JOIN " +
                                                              "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id FULL OUTER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId " +
                                            "WHERE(dbo.CartonWips.BoxId = " + BoxId + ") " +
                                            "ORDER BY dbo.CartonWips.EffectiveDate, dbo.CartonWips.WIPArea,Pallet, Location");
            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                conn.Open();
                sda.SelectCommand = cmd;
                using (BoxMovementReportDS FHC = new BoxMovementReportDS())
                {
                    sda.Fill(FHC, "BoxMovementReportDS");
                    conn.Close();
                    return FHC;
                }
            }

        }

        public DataTable getStockTakeRemainingPalletsToScan(string fromDate, string toDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT DISTINCT dbo.CartonHeaders.PalletId, dbo.Pallets.Code, dbo.Locations.Code AS Location " +
                                  "FROM dbo.CartonHeaders INNER JOIN " +
                                  "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                  "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                  "WHERE(dbo.CartonHeaders.PalletId NOT IN(SELECT DISTINCT(PalletId) " +
                                                                            "FROM dbo.FGStockTakePostCountCartons " +
                                                                            "WHERE(CAST(EffectiveDate AS DATE) <= CAST('" + fromDate + "' AS DATE)) AND " +
                                                                            "(CAST(EffectiveDate AS DATE) >= CAST('" + toDate + "' AS DATE)))) " +
                                  "ORDER BY dbo.Pallets.Code";

            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public int getPalletCountIncartonHeader()
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT COUNT(DISTINCT dbo.CartonHeaders.PalletId) " +
                                  "FROM dbo.CartonHeaders INNER JOIN " +
                                  "dbo.Pallets ON dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN " +
                                  "dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id ", conn);

            int palletCount = Convert.ToInt32(com.ExecuteScalar());
            conn.Close();
            return palletCount;
        }

        public int getpalletCountInPostCount(string fromDate, string toDate)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand com = new SqlCommand("SELECT COUNT(DISTINCT PalletId) " +
                                            "FROM dbo.FGStockTakePostCountCartons " +
                                            "WHERE(CAST(EffectiveDate AS DATE) <= CAST('" + fromDate + "' AS DATE)) AND " +
                                            "(CAST(EffectiveDate AS DATE) >= CAST('" + toDate + "' AS DATE)) ", conn);

            int palletCount = Convert.ToInt32(com.ExecuteScalar());
            conn.Close();
            return palletCount;
        }






        public StockTakePalletDetailsDS getPIStyleColorCPODetails(string PIRef)              // Chaminda
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT PIs.PIReference,PIs.CreatedDate, Styles.Code AS Style, Colors.Code AS Color, CPOs.CPONumber AS CPO, Locations.Code AS Location, Pallets.Code AS Pallet, Boxes.BoxCode " +
                                            "FROM Boxes INNER JOIN BoxCPOAllocationDetails INNER JOIN CPOs ON BoxCPOAllocationDetails.CPO = CPOs.CPONumber ON Boxes.Id = BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                            "Pallets INNER JOIN CartonHeaders ON Pallets.Id = CartonHeaders.PalletId INNER JOIN Locations ON Pallets.LocationId = Locations.Id ON Boxes.Id = CartonHeaders.BoxId " +
                                            "RIGHT OUTER JOIN PIs INNER JOIN PIStyles ON PIs.Id = PIStyles.PIsId INNER JOIN Styles ON PIStyles.StyleId = Styles.Id INNER JOIN Colors ON PIStyles.ColorId = Colors.Id ON CPOs.CPONumber = PIStyles.CPO " +
                                            "WHERE(PIs.PIReference = '" + PIRef + "') " +
                                            "ORDER BY CPO, Location, Pallet, Boxes.BoxCode");


            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;

                sda.SelectCommand = cmd;
                using (StockTakePalletDetailsDS Customer = new StockTakePalletDetailsDS())
                {
                    sda.Fill(Customer, "StockTakePalletDetailsDS");
                    conn.Close();
                    return Customer;
                }
            }
        }

        public StockReportDS getStockReport()
        {
            SqlCommand cmd = new SqlCommand("SELECT  dbo.Styles.Code AS Style, dbo.Colors.Code AS Colour, " +
                                            " dbo.Sizes.Code AS Size, ISNULL(dbo.BoxCPOAllocationDetails.CPO, 'N/A') As CPO, " +
                                            " dbo.Pallets.Code AS Pallet, dbo.Locations.Code AS Rack, " +
                                            " ISNULL(dbo.AODs.SourceWarehouse, 'N/A') AS SourceWarehouse,ISNULL(dbo.AODs.AODNumber, 'N/A') AS ReceivedinAOD, " +
                                            " dbo.Boxes.BoxCode AS BarCode, SUM(dbo.CartonDetails.Quantity) AS Quantity  FROM  " +
                                            " dbo.CartonDetails INNER JOIN dbo.Boxes ON dbo.CartonDetails.BoxId = dbo.Boxes.Id INNER JOIN " +
                                            " dbo.CartonHeaders ON dbo.Boxes.Id = dbo.CartonHeaders.BoxId INNER JOIN dbo.Products ON " +
                                            " dbo.CartonDetails.ProductId = dbo.Products.Id INNER JOIN dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                            " dbo.Colors ON dbo.Products.ColorId = dbo.Colors.Id INNER JOIN dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                            " dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN dbo.Pallets ON " +
                                            " dbo.CartonHeaders.PalletId = dbo.Pallets.Id INNER JOIN dbo.Locations ON dbo.Pallets.LocationId = dbo.Locations.Id " +
                                            " FULL OUTER JOIN dbo.AODBoxDetails ON dbo.Boxes.Id = dbo.AODBoxDetails.BoxId FULL OUTER JOIN dbo.AODs ON " +
                                            " dbo.AODBoxDetails.AODId = dbo.AODs.Id WHERE(dbo.CartonHeaders.WIPArea = 2) AND (dbo.CartonHeaders.PalletId Not IN (3502,1) )  " +
                                            " AND(dbo.CartonHeaders.IsDeleted = 0) GROUP BY dbo.Styles.Code, dbo.Colors.Code, dbo.Sizes.Code, " +
                                            " dbo.BoxCPOAllocationDetails.CPO, dbo.Pallets.Code, dbo.Locations.Code, " +
                                            " dbo.Boxes.BoxCode, dbo.AODs.SourceWarehouse, dbo.AODs.AODNumber ORDER BY Style, Colour, Size, " +
                                            " dbo.BoxCPOAllocationDetails.CPO, Pallet, Rack, dbo.AODs.SourceWarehouse, " +
                                            " dbo.AODs.AODNumber, BarCode ");



            cmd.CommandTimeout = 0;
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                //conn.Open();
                sda.SelectCommand = cmd;
                using (StockReportDS FHC = new StockReportDS())
                {
                    sda.Fill(FHC, "StockReportDS");
                    conn.Close();
                    return FHC;
                }
            }
        }

        public DataTable getPIStatus(string RefId)  //Krishantha
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT  dbo.PIs.PIReference, Locations_1.Code AS [PILocation], " +
                                 "dbo.PIs.CreatedDate, dbo.Pallets.Code, Case  " +
                                 "When dbo.PIPallets.IsConfirmed = 1 THEN 'Confirmed' " +
                                 "When dbo.PIPallets.IsConfirmed = 0 THEN 'Open' " +
                                 "End as IsConfirmed, dbo.PIPallets.ConfirmedDate, Case " +
                                 "When dbo.PIPallets.IsVerified = 1 THEN 'Verified' " +
                                 "When dbo.PIPallets.IsVerified = 0 THEN 'Open' End as IsVerified " +
                                 "FROM dbo.PIs INNER JOIN " +
                                 "dbo.PIPallets ON dbo.PIs.Id = dbo.PIPallets.PIReferenceId INNER JOIN " +
                                 "dbo.Pallets ON dbo.PIPallets.PalletId = dbo.Pallets.Id INNER JOIN " +
                                 "dbo.Locations AS Locations_1 ON dbo.PIPallets.LocationID = Locations_1.Id " +
                                 "WHERE(dbo.PIs.id = '" + RefId + "') order by dbo.PIs.CreatedDate, " +
                                 "dbo.Pallets.Code";

            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

        public DataTable getPIStatusSummary(string FoDate, string ToDate)  //Krishantha
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("GetCountSummary", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@FoDate", FoDate));
            cmd.Parameters.Add(new SqlParameter("@ToDate", ToDate));

            //int rowcount = Convert.ToInt32(cmd.ExecuteNonQuery());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;

        }


    }
}