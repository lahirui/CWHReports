<%@ Page Title="CWH Reports" Language="C#" AutoEventWireup="true" CodeBehind="WarehouseStartPage.aspx.cs" Inherits="PDCSReporting.WarehouseStartPage" %>

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
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-home"></span> Central  Warehouse Reports</div>
                        <a href="DailyScannedBoxes.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Daily Scanned Boxes Details - With CPO</h6>
                        </a>
                        <a href="DailyScannedBoxesOldStock.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Daily Scanned Boxes Details - Free Stock</h6>
                        </a>
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
                         <a href="ShipmentSummary.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Shipment Summary</h6>
                        </a>
                         <a href="ShipmentDetails.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Shipment Details</h6>
                        </a>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 ">
                    <div class="list-group" style="border-color: #da532c; color: whitesmoke">
                        <div class="list-group-item h4" style="font-family: Georgia; font-weight: 500; color: whitesmoke; background-color: #da532c; border-width: 3px; border-color: #da532c;"><span class="fas fa-list"></span> Stock Reports</div>
                        <a href="StockSummaryReport.aspx" class="list-group-item" style="border-top-color: #da532c; border-top-width: 3px; border-bottom-width: 1px; border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Stock Summary Report</h6>
                        </a>

                         <a href="StockDetailedReport.aspx" class="list-group-item" style="border-top-width: 1px; border-bottom-color: #da532c; border-bottom-width: 3px;  border-right-color: #da532c; border-right-width: 3px; border-left-color: #da532c; border-left-width: 3px;">
                            <h6 class="list-group-item-heading" style="color: #da532c;">Stock Detailed Report</h6>
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
