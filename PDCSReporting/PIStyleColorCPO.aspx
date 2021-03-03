<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/WarehouseMaster.Master" AutoEventWireup="true" CodeBehind="PIStyleColorCPO.aspx.cs" Inherits="PDCSReporting.PIStyleColorCPO" %>

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
            $('#L-OUTandC-OUTModal').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="page-header" style="padding-top: 80px;">
            <h1 style="font-family: Georgia; color: #00539C;"> PI (Style-Color-CPO)</h1>
        </div>
        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                <button type="button" id="customiseStyleReportBtn" class="btn btn-success btn-lg" data-toggle="modal" data-target="#L-OUTandC-OUTModal">Setup Report</button>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <div class="col-sm-offset-1 col-md-offset-1 col-lg-offset-1 col-sm-10 col-md-10 col-lg-10">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>  
            </div>
        </div>
    </div>

   
    <div id="L-OUTandC-OUTModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #00539C;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title" style="font-family: Georgia; color: white; font-weight: 500">PI (Style-Color-CPO)</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                     <div class="row">
                                        <label class="control-label col-sm-4 col-md-4 col-lg-4" for="team" style="font-family: Georgia">PI Reference:</label>
                                        <div class="col-sm-6 col-md-6 col-lg-6">
                                            <asp:SqlDataSource runat="server" ID="PIRefDataSource" ConnectionString='<%$ ConnectionStrings:ConString %>' SelectCommand="SELECT [PIs].[PIReference], [PIs].[Id] FROM [PIs] INNER JOIN [StockCountTypes] ON [PIs].[CountTypeId] = [StockCountTypes].[Id] WHERE([PIs].[IsDeleted] = 0) AND ([StockCountTypes].[CountType] = 'PI') AND ([PIs].[Status] = 1) AND ([PIs].[Pallet_Style] = 'S') AND ([PIs].[Id] IN (SELECT DISTINCT [PIsId] FROM [PIStyles])) ORDER BY [PIs].[PIReference] ASC"></asp:SqlDataSource>
                                            <asp:DropDownList ID="PIRefDropDownList" CssClass="form-control" Font-Names="Georgia" runat="server" DataSourceID="PIRefDataSource" DataTextField="PIReference" DataValueField="Id"></asp:DropDownList>

                                            <%--<asp:DropDownList ID="ddlFromPallet" CssClass="form-control" Font-Names="Georgia" AutoPostBack="true" runat="server"></asp:DropDownList>--%>

                                        </div>
                                        <%--<label class="control-label col-sm-2 col-md-2 col-lg-2" for="team" style="font-family: Georgia">To:</label>
                                        <div class="col-sm-3 col-md-3 col-lg-3">
                                            <asp:DropDownList ID="ddlToPallet" CssClass="form-control" Font-Names="Georgia" AutoPostBack="true" runat="server" OnDataBound="ddlToPallet_DataBound"></asp:DropDownList>
                                        </div>--%>
                                    </div>
                                    <%--<div class="row">
                                        <label class="control-label col-sm-2 col-md-2 col-lg-2" for="date" style="font-family: Georgia">Date From:</label>
                                        <div class="form-inline col-sm-4 col-md-4 col-lg-4">
                                            <input type="text" class="form-control" style="width: 120px; height: 40px;" runat="server" id="txtFromDate" disabled="disabled" /><asp:LinkButton ID="lbDate" runat="server" OnClick="lbDate_Click"> <span class="glyphicon glyphicon-calendar" style="color:#00539C; font-size:x-large;"></span></asp:LinkButton>
                                            <asp:Calendar ID="calFromDate" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Georgia" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px" runat="server" OnDayRender="calFromDate_DayRender" OnSelectionChanged="calFromDate_SelectionChanged">
                                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                <SelectedDayStyle BackColor="#00539C" ForeColor="White" />
                                                <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#00539C" />
                                                <TodayDayStyle BackColor="Gray" />
                                            </asp:Calendar>
                                        </div>
                                        <label class="control-label col-sm-1 col-md-1 col-lg-1" for="date" style="font-family: Georgia">To:</label>
                                        <div class="form-inline col-sm-4 col-md-4 col-lg-4">
                                            <input type="text" class="form-control" style="width: 120px; height: 40px;" runat="server" id="txtTodate" disabled="disabled" /><asp:LinkButton ID="lbToDate" runat="server" OnClick="lbToDate_Click"> <span class="glyphicon glyphicon-calendar" style="color:#00539C; font-size:x-large;"></span></asp:LinkButton>
                                            <asp:Calendar ID="calTodate" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Georgia" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="250px" runat="server" OnDayRender="calTodate_DayRender" OnSelectionChanged="calTodate_SelectionChanged">
                                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                                                <OtherMonthDayStyle ForeColor="#999999" />
                                                <SelectedDayStyle BackColor="#00539C" ForeColor="White" />
                                                <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#00539C" />
                                                <TodayDayStyle BackColor="Gray" />
                                            </asp:Calendar>
                                        </div>
                                    </div>--%>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                      <asp:Button ID="GenerateReportButton" CssClass="btn btn-success btn-lg" Font-Names="Georgia" runat="server" Text="Generate Report" OnClick="GenerateReportButton_Click"/>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

