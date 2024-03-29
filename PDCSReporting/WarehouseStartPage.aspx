﻿<%@ Page Title="CWH Reports" Language="C#" AutoEventWireup="true" CodeBehind="WarehouseStartPage.aspx.cs" Inherits="PDCSReporting.WarehouseStartPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="icon" type="image/ico" href="Images/Report.ico" sizes="16x16" />
    <title>PDCS - Warehouse Reports</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <%-- Link to Bootstrap 3--%>
    <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/utils.js"></script>

    <%-- Link to W3.css --%>
    <link rel="stylesheet" href="Content/W3.css" />
    <link rel="stylesheet" href="Content/W3-2019-Fashion.css" />

    <%-- Link to Font Awesome 5.5.0--%>
    <link rel="stylesheet" href="fontawesome/css/all.css" />

    <style>
        a:link {
            text-decoration: none;
        }

        a:visited {
            text-decoration: none;
        }

        a:hover {
            text-decoration: none;
        }

        a:active {
            text-decoration: none;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <div class="row w3-xxxlarge" style="font-family: Georgia; font-weight: 600; color: #da532c;">
                <pre style="font-family: Georgia; color: #da532c;"> <a href="#"> Central Warehouse Reports</a> </pre>
            </div>
            <div class="row col-sm-offset-1 col-md-offset-1 col-lg-offset-1">
                <%-- Status Reports --%>
                <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-home"></span> General Reports</div>
                        <a href="DailyScannedBoxes.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Daily Scanned Boxes Details</h6>
                        </a>
                      <%--  <a href="DailyScannedBoxesOldStock.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Daily Scanned Boxes Details - Free Stock</h6>
                        </a>--%>
                         <a href="ScannedReportCPOWise.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Scanned Details - CPO Wise</h6>
                        </a>
                         <%--<a href="PalletDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Pallet Details</h6>
                        </a>
                        <a href="LocationDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Location Details</h6>
                        </a>--%>
                         <a href="AODUnavailableQuantities.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">AOD Unavailable Quantities</h6>
                        </a>
                         <a href="GoodsReceivedSummary.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Goods Received Summary</h6>
                        </a>
                         <a href="GoodsReceivedDetailes.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Goods Received Details</h6>
                        </a>
                        <a href="GoodsReceivedDetailesStatus.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Goods Received Status - Received</h6>
                        </a>
                        <a href="GoodsReceivedDetailesStatus_NotReceived.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Goods Received Status - Not Received</h6>
                        </a>
                         <a href="ShipmentSummary.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Shipment Summary</h6>
                        </a>
                         <a href="ShipmentDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Shipment Details</h6>
                        </a>
                        <a href="FactoryTransfers.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Factory Transfer Details</h6>
                        </a>
                         <a href="AODCheckList.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">AOD Check List</h6>
                        </a>
                        <a href="WIPAtDefaultBox.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">WIP at Default Box</h6>
                        </a>
                        <a href="FactoryTransferSummary.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Factory Transfer Summary</h6>
                        </a>
                        <a href="FactoryTransferDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Factory Transfer Details</h6>
                        </a>
                         <a href="BoxMovementReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 1px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Box Movement Report</h6>
                        </a>
                         <a href="SingleScannedRFIDs.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Scanned RFIDs Details - (Single Scan)</h6>
                        </a>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-list"></span> Stock Take Reports</div>

                         <a href="StockTakePalletDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Pallet Details For Stock Take</h6>
                        </a>
                         <a href="CartonWiseStockReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Carton Wise Stock Report</h6>
                        </a>
                    </div>

                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-briefcase"></span>  PI Count Reports</div>
                         <a href="PIStyleColorCPO.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">PI (Style-Color-CPO) Report </h6>
                        </a>

                        <a href="PIPreAndPostCount.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">PI Pre & Post Counts Summary Report</h6>
                        </a>
                        <a href="PIPreAndPostDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">PI Pre & Post Counts Detailed Report</h6>
                        </a>
                        <a href="PIStatusReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">PI Status Report</h6>
                        </a>
                         <a href="PIStatusSummary.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">PI Status Summary</h6>
                        </a>
                        <a href="StockTakePalletsToScan.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Pallets Remaining to Scan</h6>
                        </a>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-archive"></span> On Hand Stock Reports</div>
                        <a href="StockSummaryReport.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">On Hand Stock Summary Report</h6>
                        </a>

                         <a href="StockDetailedReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">On Hand Stock Detailed Report</h6>
                        </a>

                        <a href="StockReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">On Hand Stock Report</h6>
                        </a>

                        <a href="ConsolidatedStockReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Consolidated Stock Report</h6>
                        </a>
                        <a href="DefaultPalletAgingReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">DP Aging Report</h6>
                        </a>

                        <a href="RightoffpalletDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">RightOff Box Detail Report</h6>
                        </a>

                        <a href="StockPositionReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Stock Position Report</h6>
                        </a>
                          

                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-folder"></span>  AODs</div>
                        <a href="FactoryDAOD.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 3px; border-bottom-color:#da532c; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Dispatched AODs</h6>
                        </a>
                        
                        <a href="CodeChange.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 3px; border-bottom-color:#da532c; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Code Change Tracker</h6>
                        </a>

                    </div>
                </div>

                 <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-image"></span>  Display</div>
                        <a href="LocationDisplay.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 3px; border-bottom-color:#da532c; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Location Display</h6>
                        </a>
                    </div>
                </div>         
            </div>
        </div>
        <footer class="navbar navbar-fixed-bottom" style="background-color: whitesmoke;">
            <div class="container-fluid">
                <h6 class="text-center" style="font-family: Georgia; color: #da532c; font-weight: 600; padding-top: 10px; padding-bottom: 10px;">
                    <asp:Label ID="YearLabel2" runat="server" Text=""></asp:Label>
                </h6>
            </div>
        </footer>
    </form>
</body>
</html>
