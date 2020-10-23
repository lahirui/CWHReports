<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/WarehouseMaster.Master" CodeBehind="BSScannedBoxDetails.aspx.cs" Inherits="PDCSReporting.BSScannedBoxDetails" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/utils.js"></script>

    <script>
        $(document).ready(function () {
            $('.dropdown-submenu a.test').on("click", function (e) {
                $(this).next('ul').toggle();
                e.stopPropagation();
                e.preventDefault();
            });
        });
    </script>

    <script type="text/javascript">
        function openMyModal() {
            $('#myTeamwiseModal').modal('show');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container-fluid">
          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="page-header" style="padding-top: 80px;">
            <h1 style="font-family: Georgia; color: #da532c">Box Stock Scanned Boxes Detailes</h1>
        </div>

        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                <button type="button" id="btnTeamWise" class="btn btn-success btn-lg" data-toggle="modal" data-target="#myTeamwiseModal">Setup Report</button>
            </div>
        </div>

        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <div class="col-sm-12 col-md-12 col-lg-12" style="width: 100%;">
                <rsweb:ReportViewer ID="rvTeamOutput" Width="100%" AsyncRendering="false" runat="server"></rsweb:ReportViewer>
            </div>
        </div>
    </div>
       <%-- Modal to Customize the team summary report starts here --%>
    <div id="myTeamwiseModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color:#da532c;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title" style="font-family: Georgia; color: white; font-weight: 500">Box Stock  Scanned Boxes Detailes</h3>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal" role="form">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 col-md-2 col-lg-2" for="Month" style="font-family: Georgia">Date:</label>
                                    <label class="control-label col-sm-1 col-md-1 col-lg-1" for="fromMonth" style="font-family: Georgia">From:</label>
                                    <div class="form-inline col-sm-4 col-md-4 col-lg-4">
                                        <input type="text" class="form-control" style="width: 120px; height: 40px;" runat="server" id="txtFromDate" disabled="disabled" /><asp:LinkButton ID="lbFromDate" runat="server" OnClick="lbFromDate_Click"> <span class="glyphicon glyphicon-calendar" style="color:#da532c; font-size:x-large;"></span></asp:LinkButton>
                                        <asp:Calendar ID="calFromDate" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Georgia" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px" OnDayRender="calFromDate_DayRender" OnSelectionChanged="calFromDate_SelectionChanged">
                                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                            <OtherMonthDayStyle ForeColor="#999999" />
                                            <SelectedDayStyle BackColor="#da532c" ForeColor="White" />
                                            <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#da532c" />
                                            <TodayDayStyle BackColor="Gray" />
                                        </asp:Calendar>
                                    </div>
                                    <label class="control-label col-sm-1 col-md-1 col-lg-1" for="fromMonth" style="font-family: Georgia">To:</label>
                                    <div class="form-inline col-sm-4 col-md-4 col-lg-4">
                                        <input type="text" class="form-control" style="width: 120px; height: 40px;" runat="server" id="txtToDate" disabled="disabled" /><asp:LinkButton ID="lbToDate" runat="server" OnClick="lbToDate_Click"> <span class="glyphicon glyphicon-calendar" style="color:#da532c; font-size:x-large;"></span></asp:LinkButton>
                                        <asp:Calendar ID="calToDate" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Georgia" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px" OnDayRender="calToDate_DayRender" OnSelectionChanged="calToDate_SelectionChanged">
                                            <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                            <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                            <OtherMonthDayStyle ForeColor="#999999" />
                                            <SelectedDayStyle BackColor="#da532c" ForeColor="White" />
                                            <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#da532c" />
                                            <TodayDayStyle BackColor="Gray" />
                                        </asp:Calendar>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:Button ID="btnGenerateReport" CssClass="btn btn-success btn-lg" Font-Names="Georgia" runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


