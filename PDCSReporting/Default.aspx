<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PDCSReporting.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title> PDCS Reporting </title>

        <link rel="stylesheet" type="text/css" href="Content/bootstrap.min.css" />
        <script src="Scripts/jquery-1.10.2.min.js"></script>
        <script src="Scripts/bootstrap.min.js"></script>

<%--    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>--%>


    <style>
        .dropdown-submenu {
            position: relative;
        }

            .dropdown-submenu .dropdown-menu {
                top: 0;
                left: 100%;
                margin-top: -1px;
            }
        /*body {
            background-image: url("Images/LoginBackground.jpg");
        }*/

        .btn-lg {
            padding: 15px 21px;
            font-size: 26px;
            line-height: 3.00;
            border-radius: 6px;
        }
        .btn-success {
            background-color: purple;
        }

            .btn-success:hover,
            .btn-success:focus,
            .btn-success:active,
            .btn-success.active,
            .open .dropdown-toggle.btn-success {
                color: #ffffff;
                background-color: rebeccapurple;
                border-color: rebeccapurple;
            }

        .vertical-center {
            min-height: 100%; /* Fallback for browsers do NOT support vh unit */
            min-height: 100vh; /* These two lines are counted as one :-)       */
            display: flex;
            align-items: center;
        }

        .parent {
            position:relative;
        }

        .child {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
        }

        .list-group-item {
            position: relative;
            display: block;
            padding: 10px 15px;
            margin-bottom: -1px;
            background-color: #ffffff;
            border: 1px solid purple;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('.dropdown-submenu a.test').on("click", function (e) {
                $(this).next('ul').toggle();
                e.stopPropagation();
                e.preventDefault();
            });
        });
    </script>
</head>


<body>
    <form id="form1" runat="server">
        
        <div class="container-fluid">
            <div class="row">
                <img src="Images/pdcs logo2.png"/>
            </div>

            <div class="row parent">
                <div class="col-sm-offset-2 col-md-offset-2 col-lg-offset-2 col-sm-4 col-md-4 col-lg-4">
                    <div class="dropdown">
                        <button class="btn btn-success btn-lg dropdown-toggle" style="font-family:Georgia;" type="button" data-toggle="dropdown">Reports<span class="caret"></span></button>
                        <ul class="dropdown-menu">
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="GeneralStartPage.aspx">General Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="EmployeeStartPage.aspx">Employee Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="ProductionStartPage.aspx">Production Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="QualityStartPage.aspx">Quality Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="DowntimeStartPage.aspx">Downtime Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="IEStartPage.aspx">IE Reports</a></li>
                            <li><a class="test" style="font-family: Georgia;" tabindex="-1" href="WarehousestartPage.aspx">Warehouse Reports</a></li>
<%--                            <li class="dropdown-submenu">
                                <a class="test" style="font-family: Georgia;" tabindex="-1" href="#">General Reports <span class="caret"></span></a>
                                <ul class="dropdown-menu">
                                    <li><a tabindex="-1" href="CustomerWiseLineDetailsReport.aspx">Customer Wise Line Details</a></li>
                                    <li><a tabindex="-1" href="FactoryEfficiencyReport.aspx">Style Efficiency</a></li>
                                    <li><a tabindex="-1" href="TeamDailySummary.aspx">Team Daily Summary</a></li>
                                    <li><a tabindex="-1" href="WIPReport.aspx">WIP Report</a></li>
                                    <li class="divider"></li> 
                                    <li><a tabindex="-1" href="MPOCompletion.aspx">MPO Completion Report</a></li>
                                    <li class="divider"></li>   
                                    <li><a tabindex="-1" href="StyleOperationsDetails.aspx">Style Operations Details</a></li>
                                </ul>
                            </li>--%>
<%--                            <li class="dropdown-submenu">
                                <a class="test" style="font-family:Georgia;" tabindex="-1" href="#">Operator Reports<span class="caret"></span></a>
                                <ul class="dropdown-menu">
                                    <li><a tabindex="-1" href="EmployeePerformance.aspx">Detailed Operator Performance</a></li>
                                    <li><a tabindex="-1" href="OperatorPerformanceSummary.aspx">Operator Performance Summary</a></li>
                                    <li><a tabindex="-1" href="OperatorAttendance.aspx">Operator Attendance</a></li>
                                </ul>
                            </li>--%>
<%--                            <li class="dropdown-submenu">
                                <a class="test" style="font-family:Georgia;" tabindex="-1" href="#">Production Reports <span class="caret"></span></a>
                                <ul class="dropdown-menu">
                                    <li><a tabindex="-1" href="HourlyProductionReport.aspx">Hourly Production Report</a></li>
                                    <li class="divider"></li> 
                                    <li><a tabindex="-1" href="BundleTracker.aspx">Bundle Movement</a></li>
                                    <li><a tabindex="-1" href="BundleDetails.aspx">Bundle Details</a></li>
                                    <li><a tabindex="-1" href="BundleSummary.aspx">Bundle Movement Summary</a></li>
                                    <li class="divider"></li>
                                    <li><a tabindex="-1" href="BundleAllocationReport.aspx">Bundle Allocation</a></li>
                                    <li><a tabindex="-1" href="COUTandLOUTDetails.aspx">L-OUT and C-OUT Details</a></li>
                                    <li><a tabindex="-1" href="LOUTDetails.aspx">L-OUT Details</a></li>
                                    <li class="divider"></li>
                                    <li><a tabindex="-1" href="PrintTicket.aspx">Print Tickets</a></li>
                                    <li><a tabindex="-1" href="PrintTicketWithQty.aspx">Print Tickets-With Quantity</a></li>
                                    <li><a tabindex="-1" href="PrintLineInput.aspx">Print Line Input</a></li>
                                    <li class="divider"></li>                         
                                    <li><a tabindex="-1" href="SecondsDetails.aspx">Seconds Details</a></li>
                                    
                                </ul>
                            </li>--%>
<%--                            <li class="dropdown-submenu">
                                <a class="test" style="font-family: Georgia;" tabindex="-1" href="#">Quality Reports <span class="caret"></span></a>
                                <ul class="dropdown-menu">
                                    <li><a tabindex="-1" href="IQSummary.aspx">Style Wise Inline Quality Summary</a></li>
                                    <li><a tabindex="-1" href="ILQControlPoints.aspx">ILQ Summary - Control Point Wise</a></li>
                                    <li><a tabindex="-1" href="ILQuality.aspx">Team Wise ILQ Defects Details</a></li>
                                    <li><a tabindex="-1" href="ILQDefectsByOperator.aspx">Team Wise ILQ Defects Details - By Operator</a></li>
                                    <li><a tabindex="-1" href="IQStyleAnalysis.aspx">Inline Quality Style Analysis</a></li>
                                    <li><a tabindex="-1" href="ILQHourlyDefects.aspx">Hourly ILQ Details</a></li>
                                    <li><a tabindex="-1" href="InLineMeasurementDetails.aspx">ILQ Measurement Details</a></li>
                                    <li class="divider"></li>
                                    <li><a tabindex="-1" href="AQLSummary.aspx">AQL Summary</a></li>
                                    <li><a tabindex="-1" href="AQLTeamDailyDetails.aspx">AQL Daily Details</a></li>
                                    <li><a tabindex="-1" href="AQLBatchWiseMeasurements.aspx">AQL Measurement Details</a></li>
                                </ul>
                            </li>--%>
<%--                            <li class="dropdown-submenu">
                                <a class="test" style="font-family: Georgia;" tabindex="-1" href="#">Downtime Reports <span class="caret"></span></a>
                                <ul class="dropdown-menu">
                                    <li><a tabindex="-1" href="DowntimeDetails.aspx">Downtime Details</a></li>
                                    <li><a tabindex="-1" href="DowntimeAnalysis.aspx">Downtime Analysis</a></li>
                                </ul>
                            </li>--%>
                        </ul>
                    </div>
                </div>
                <div class="col-sm-offset-1 col-md-offset-1 col-lg-offset-1 col-sm-4 col-md-4 col-lg-4">
                    <div class="dropdown">
                        <button class="btn btn-success btn-lg dropdown-toggle" style="font-family: Georgia;" type="button" data-toggle="dropdown">Charts <span class="caret"></span></button>
                        <ul class="dropdown-menu">
                            <li><a href="LineSummaryDashboardNew.aspx">Line Dashboard</a></li>
                            <li><a href="DowntimeDashboard.aspx">Downtime Dashboard</a></li>
                            <%--<li><a href="LineSummaryDashboardNew2.aspx">Line Dashboard 2</a></li>--%>
                            <li><a href="LineWIPDashboard.aspx">Line WIP Details Dashboard</a></li>
                            <li><a href="LineAlertsDashboard.aspx">Line Alerts Dashboard</a></li>
                             <li><a href="WIPChart.aspx">WIP Kanban Dashboard</a></li>
                        </ul>
                    </div>
                </div>
            </div>

<%--            <div class="row" style="padding-bottom:10px;">
                <div class="col-sm-offset-3 col-md-offset-3 col-lg-offset-3 col-sm-2 col-md-2 col-lg-2">
                    <asp:LinkButton ID="TicketLinkButton" class="btn btn-success btn-lg" Font-Names="Georgia" runat="server" OnClick="TicketLinkButton_Click"><span class="glyphicon glyphicon-print"></span> Print Tickets</asp:LinkButton>
                </div>
                <div class="col-sm-offset-1 col-md-offset-1 col-lg-offset-1 col-sm-2 col-md-2 col-lg-2">
                    <asp:LinkButton ID="BundleDetailsLinkButton" class="btn btn-success btn-lg" Font-Names="Georgia" runat="server" OnClick="BundleDetailsLinkButton_Click"><span class="glyphicon glyphicon-list-alt"></span> Bundle Details</asp:LinkButton>
                </div>
            </div>

            <div class="row parent">
                <div class="col-sm-offset-3 col-md-offset-3 col-lg-offset-3 col-sm-2 col-md-2 col-lg-2">
                    <asp:LinkButton ID="ReportsLinkButton" class="btn btn-success btn-lg" Font-Names="Georgia" runat="server" OnClick="ReportsLinkButton_Click"><span class="glyphicon glyphicon-list-alt"></span> Reports <span class="glyphicon glyphicon-chevron-right"></span></asp:LinkButton>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3" id="reportDiv" runat="server" visible="false">
                    <div class="list-group">
                        <a href="CustomerWiseLineDetailsReport.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Customer Wise Analysis of Lines</a>
                        <a href="FactoryEfficiencyReport.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Style Wise Efficiency Analysis</a>
                        <a href="TeamDailySummary.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Team Daily Summary</a>
                        <a href="EmployeePerformance.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Detailed Operator Performance</a>
                        <a href="OperatorPerformanceSummary.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Operator Performance Summary</a>
                        <a href="BundleTracker.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Bundle Movement</a>
                    </div>
                </div>
                <div class="col-sm-offset-1 col-md-offset-1 col-lg-offset-1 col-sm-2 col-md-2 col-lg-2">
                    <asp:LinkButton ID="chartsLinkButton" class="btn btn-success btn-lg" Font-Names="Georgia" runat="server" OnClick="chartsLinkButton_Click"><span class="glyphicon glyphicon-dashboard"></span> Charts <span class="glyphicon glyphicon-chevron-right"></span></asp:LinkButton>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3" id="chartDiv" runat="server" visible="false">
                    <div class="list-group">
                        <a href="LineSummaryDashboardNew.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Summary of Lines</a>
                        <a href="LineWIPDashboard.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Line WIP Details</a>
                        <a href="LineAlertsDashboard.aspx" class="list-group-item" style="font-family: Georgia; font-size: larger; font-weight: 400">Line Alerts</a>
                    </div>
                </div>
            </div>--%>
        </div>
        <footer class="navbar navbar-fixed-bottom" style="background-color: #faf0fc;">
            <div class="container-fluid">
                <h5 class="text-center" style="font-family: Georgia; color: purple; font-weight: 600; padding-top: 10px; padding-bottom: 10px;">
                    <asp:Label ID="YearLabel" runat="server" Text=""></asp:Label></h5>
            </div>
        </footer>
    </form>
</body>
</html>
