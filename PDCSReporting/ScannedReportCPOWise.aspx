<%@ Page Title="CWH Reports" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/WarehouseMaster.Master" CodeBehind="ScannedReportCPOWise.aspx.cs" Inherits="PDCSReporting.ScannedReportCPOWise" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#WarehouseInDetailsModal').modal('show');
        }
    </script>

  

     <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/utils.js"></script>
    <script src="Scripts/select2.full.min.js"></script>
    <link href="Content/select2.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=CPODropDownList.ClientID%>").select2({
                placeholder: "Select CPO",
                allowClear: true
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
        <div class="page-header" style="padding-top:80px;">
            <h1 style="font-family: Georgia; color: #da532c;">Scanned Report - CPO Wise</h1>
        </div>
        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                <button type="button" id="customiseStyleReportBtn" class="btn btn-success btn-lg" data-toggle="modal" data-target="#WarehouseInDetailsModal" style="background-color:#da532c;">Setup Report</button>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </div>

    <%-- Modal --%>
    <div id="WarehouseInDetailsModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #da532c;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title" style="font-family: Georgia; color: white; font-weight: 500">Scanned Report - CPO Wise</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label class="control-label col-sm-1 col-md-1 col-lg-1" for="cpo" style="font-family: Georgia">CPO:</label>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <asp:DropDownList ID="CPODropDownList" runat="server" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" OnSelectedIndexChanged="CPODropDownList_SelectedIndexChanged" DataSourceID="CPODetailsDataSource" DataTextField="CPO" DataValueField="CPO"></asp:DropDownList>
                                        <asp:SqlDataSource runat="server" ID="CPODetailsDataSource" ConnectionString='<%$ ConnectionStrings:ConString %>' SelectCommand="SELECT DISTINCT [CPO] FROM [BoxCPOAllocationDetails] ORDER BY [CPO]"></asp:SqlDataSource>
                                    </div>
                                </div>
<%--                                <div class="form-group">
                                    <label class="control-label col-sm-1 col-md-1 col-lg-1" for="so" style="font-family: Georgia">SO:</label>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <asp:DropDownList ID="SODropDownList" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>--%>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="GenerateReportButton" runat="server" Text="Generate Report" CssClass="btn btn-success btn-lg" Font-Names="Georgia" BackColor="#da532c" OnClick="GenerateReportButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
