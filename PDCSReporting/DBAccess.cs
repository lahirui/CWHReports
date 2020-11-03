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
        String Server, Catalog;
        
        
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
       
        public DataTable getCutListForMPO(string fromStyle, string toStyle, string fromMPO, string toMpo)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT DISTINCT dbo.CutInformations.Id, dbo.CutInformations.CutNumber AS Cut " +
                                  "FROM dbo.Products INNER JOIN " +
                                  "dbo.ProductionOrders ON dbo.Products.Id = dbo.ProductionOrders.ProductId INNER JOIN " +
                                  "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                  "dbo.CutInformations ON dbo.ProductionOrders.CutInformationId = dbo.CutInformations.Id INNER JOIN " +
                                  "dbo.ProdOrders ON dbo.ProductionOrders.ProdOrderId = dbo.ProdOrders.Id " +
                                  "WHERE(dbo.Styles.Code >= '" + fromStyle + "') AND(dbo.Styles.IsDeleted = 0) AND(dbo.Styles.Code <= '" + toStyle + "') AND(dbo.ProdOrders.Code BETWEEN N'" + fromMPO + "' AND N'" + toMpo + "') " +
                                  "ORDER BY Cut, dbo.CutInformations.Id";

            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }

       
        public DataTable getTeamIncentive(int TeamId, int Month, int Year)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand TeamCmd = conn.CreateCommand();
            TeamCmd.Connection = conn;
            TeamCmd.CommandType = CommandType.Text;

            TeamCmd.CommandText = "SELECT dbo.Teams.Name AS Team, DATEPART(dd, dbo.RptTeamDailySummary.EffectiveDate) AS CalDate, ISNULL(dbo.RptTeamDailySummary.Clocked_Minutes, 0) AS WorkedMinutes, ISNULL(dbo.RptTeamDailySummary.Achieved_Minutes, 0) " +
                                            "AS AchievedMinutes, dbo.RptTeamDailySummary.AQLBatchesChecked, dbo.RptTeamDailySummary.AQLBatchesChecked - dbo.RptTeamDailySummary.AQLBatchesFailed AS AQLBatchesPassed, dbo.RptTeamDailySummary.PlannedEfficiency " +
                                            "FROM     dbo.RptTeamDailySummary " +
                                            "INNER JOIN dbo.Teams ON dbo.RptTeamDailySummary.TeamId = dbo.Teams.Id " +
                                            "WHERE(dbo.RptTeamDailySummary.TeamId = " + TeamId + ") " +
                                            "AND(DATEPART(mm, dbo.RptTeamDailySummary.EffectiveDate) = " + Month + ") " +
                                            "AND(DATEPART(YYYY, dbo.RptTeamDailySummary.EffectiveDate) = " + Year + ") " +
                                            "AND(dbo.RptTeamDailySummary.Clocked_Minutes > 0) " +
                                            "GROUP BY dbo.Teams.Name, dbo.RptTeamDailySummary.EffectiveDate, dbo.RptTeamDailySummary.Clocked_Minutes, dbo.RptTeamDailySummary.Achieved_Minutes, dbo.RptTeamDailySummary.AQLBatchesChecked, " +
                                            "dbo.RptTeamDailySummary.AQLBatchesChecked, dbo.RptTeamDailySummary.AQLBatchesFailed, dbo.RptTeamDailySummary.PlannedEfficiency " +
                                            "ORDER BY Team, CalDate";

            SqlDataAdapter da = new SqlDataAdapter(TeamCmd);
            DataTable dat = new DataTable();
            da.Fill(dat);
            conn.Close();
            return dat;
        }

        public DataTable getEmployeeIncentive(int TeamId, int Month, int Year)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }
            SqlCommand EmpCmd = conn.CreateCommand();
            EmpCmd.Connection = conn;
            EmpCmd.CommandType = CommandType.Text;
            EmpCmd.CommandText = "SELECT dbo.Teams.Name AS Team, dbo.Employees.EmployeeId AS Employee, DATEPART(dd, dbo.RptEmployeeIncentives.EffectiveDate) AS CalDate, dbo.RptEmployeeIncentives.Incentive_Efficiency AS IncentiveEfficiency, " +
                                            "dbo.RptEmployeeIncentives.Incentive_Plan AS IncentivePlan " +
                                            "FROM     dbo.RptEmployeeIncentives INNER JOIN " +
                                            "dbo.Employees ON dbo.RptEmployeeIncentives.EmployeeID = dbo.Employees.Id INNER JOIN " +
                                            "dbo.Teams ON dbo.RptEmployeeIncentives.TeamID = dbo.Teams.Id " +
                                            "WHERE(dbo.RptEmployeeIncentives.TeamID = " + TeamId + ") AND(DATEPART(mm, dbo.RptEmployeeIncentives.EffectiveDate) = " + Month + ") AND(DATEPART(YYYY, dbo.RptEmployeeIncentives.EffectiveDate) = " + Year + ") " +
                                            "ORDER BY Team, CalDate, Employee";
            SqlDataAdapter da = new SqlDataAdapter(EmpCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;

        }

        public DataTable getBuyerWiseAQLSummary(string fromTeam, string toTeam, string fromDate, string toDate, string fromCus, string toCus)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT dbo.RptProductAQLSummary.EffectiveDate AS Date, RptProductAQLSummary.TeamID AS TeamID, dbo.Customers.Name AS Customer, dbo.Teams.Name AS Team, dbo.Styles.Code AS Style, SUM(dbo.RptProductAQLSummary.DefectQuantity) AS DefectQuantity, " +
                  "SUM(CASE WHEN dbo.RptProductAQLSummary.Status = 'Passed' THEN dbo.AqlBatches.BatchSize ELSE 0 END) AS BatchSize, SUM(dbo.AqlBatches.SampleSize)AS SampleSize, SUM(dbo.RptProductAQLSummary.AQLBatchesChecked) " +
                  "AS NoOfAudits, SUM(CASE WHEN dbo.RptProductAQLSummary.Status = 'Passed' THEN dbo.RptProductAQLSummary.AQLBatchesChecked ELSE 0 END) AS PassedAudits, " +
                  "SUM(CASE WHEN dbo.RptProductAQLSummary.Status = 'Failed' THEN dbo.RptProductAQLSummary.AQLBatchesChecked ELSE 0 END) AS FailedAudits " +
                  "FROM     dbo.RptProductAQLSummary INNER JOIN " +
                  "dbo.Teams ON dbo.RptProductAQLSummary.TeamID = dbo.Teams.Id INNER JOIN " +
                  "dbo.Styles ON dbo.RptProductAQLSummary.StyleID = dbo.Styles.Id INNER JOIN " +
                  "dbo.AqlBatches ON dbo.RptProductAQLSummary.BatchID = dbo.AqlBatches.Id INNER JOIN " +
                  "dbo.Customers ON dbo.Styles.CustomerId = dbo.Customers.Id " +
                  "WHERE(dbo.Teams.Name BETWEEN '" + fromTeam + "' AND '" + toTeam + "') " +
                  "AND(CAST(dbo.RptProductAQLSummary.EffectiveDate AS DATE) >= '" + fromDate + "') " +
                  "AND(CAST(dbo.RptProductAQLSummary.EffectiveDate AS DATE) <= '" + toDate + "') " +
                  "AND (dbo.Customers.Name BETWEEN '" + fromCus + "' AND '" + toCus + "') " +
                  "GROUP BY dbo.RptProductAQLSummary.EffectiveDate,dbo.Customers.Name, RptProductAQLSummary.TeamID,  dbo.Teams.Name, dbo.Styles.Code " +
                  "ORDER BY Customer, Team, Style";
            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        public DataTable getBuyerWiseTop3DefectsOfTheFactory(string fromDate, string toDate, string fromTeam, string toTeam, string fromCus, string toCus)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand newCmd = conn.CreateCommand();
            newCmd.Connection = conn;
            newCmd.CommandType = CommandType.Text;

            newCmd.CommandText = "SELECT TOP (3) dbo.Customers.Name AS Customer, dbo.AqlDefects.Description AS DefectName, SUM(dbo.RptProductAQLDefects.Quantity) AS Quantity " +
                                  "FROM dbo.RptProductAQLDefects INNER JOIN " +
                                  "dbo.RptProductAQLSummary ON dbo.RptProductAQLDefects.RptProductAQLSummaryID = dbo.RptProductAQLSummary.ID INNER JOIN " +
                                  "dbo.Teams ON dbo.RptProductAQLSummary.TeamID = dbo.Teams.Id INNER JOIN " +
                                  "dbo.Styles ON dbo.RptProductAQLSummary.StyleID = dbo.Styles.Id INNER JOIN " +
                                  "dbo.AqlDefects ON dbo.RptProductAQLDefects.DefectID = dbo.AqlDefects.Id INNER JOIN " +
                                  "dbo.Customers ON dbo.Styles.CustomerId = dbo.Customers.Id " +
                                  "WHERE(dbo.Teams.Name BETWEEN '" + fromTeam + "' AND '" + toTeam + "') " +
                                  "AND(CAST(dbo.RptProductAQLSummary.EffectiveDate AS DATE) >= '" + fromDate + "') " +
                                  "AND(CAST(dbo.RptProductAQLSummary.EffectiveDate AS DATE) <= '" + toDate + "') " +
                                  "AND (dbo.Customers.Name BETWEEN '" + fromCus + "' AND '" + toCus + "') " +
                                  "GROUP BY dbo.AqlDefects.Description, dbo.Customers.Name " +
                                  "ORDER BY Quantity DESC";
            newCmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(newCmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }


        public PalletDetailsDS getPalletDetails(int PalletId)
        {
            if (conn.State.ToString() == "Closed")
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT  dbo.Boxes.BoxCode AS BoxBarCode, dbo.Styles.Code AS Style, dbo.Colours.Code AS Colour,dbo.ProdOrders.Code AS MPO, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code AS Size, (CAST(dbo.BoxCPOAllocationDetails.AllocatedDate AS DATE)) AS ScannedDate, " +
                                                              "SUM(dbo.FGWips.Quantity) AS Quantity " +
                                            "FROM     dbo.FGWips INNER JOIN " +
                                                              "dbo.Boxes ON dbo.FGWips.BoxId = dbo.Boxes.Id INNER JOIN " +
                                                              "dbo.Products ON dbo.FGWips.ProductId = dbo.Products.Id INNER JOIN " +
                                                              "dbo.Styles ON dbo.Products.StyleId = dbo.Styles.Id INNER JOIN " +
                                                              "dbo.Colours ON dbo.Products.ColorId = dbo.Colours.Id INNER JOIN " +
                                                              "dbo.Sizes ON dbo.Products.SizeId = dbo.Sizes.Id INNER JOIN " +
                                                              "dbo.BoxCPOAllocationDetails ON dbo.Boxes.Id = dbo.BoxCPOAllocationDetails.BoxId INNER JOIN " +
                                                              "dbo.ProdOrders ON dbo.FGWips.ProdOrderId = dbo.ProdOrders.Id INNER JOIN " +
                                                              "dbo.WarehouseWips ON dbo.Boxes.Id = dbo.WarehouseWips.BoxId " +
                                            "WHERE(dbo.FGWips.WIPArea = 2)  AND(dbo.WarehouseWips.WIPArea = 2) AND (dbo.WarehouseWips.PalletId = " + PalletId + ") " +
                                            "GROUP BY dbo.Boxes.BoxCode, dbo.Styles.Code, dbo.Colours.Code, dbo.ProdOrders.Code, dbo.BoxCPOAllocationDetails.CPO, dbo.Sizes.Code, (CAST(dbo.BoxCPOAllocationDetails.AllocatedDate AS DATE)) " +
                                            "HAVING SUM(dbo.FGWips.Quantity) <> 0 " +
                                            "ORDER BY BoxBarCode, Style, Colour, dbo.BoxCPOAllocationDetails.CPO, Size, (CAST(dbo.BoxCPOAllocationDetails.AllocatedDate AS DATE))");
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


    }
}