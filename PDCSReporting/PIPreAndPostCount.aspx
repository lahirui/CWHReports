<%@ Page Language="C#" MasterPageFile="~/MasterPages/WarehouseMaster.Master" AutoEventWireup="true" CodeBehind="PIPreAndPostCount.aspx.cs" Inherits="PDCSReporting.PIPreAndPostCount" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/utils.js"></script>
    <script src="Scripts/select2.full.min.js"></script>
    <link href="Content/select2.min.css" rel="stylesheet" />

    <script type="text/javascript">
        function openModal() {
            $('#ReportDataFilterModal').modal('show');
        }
        $(document).ready(function () {
            LoadSelect2();
        });

        var prm;
        if (typeof (Sys) !== 'undefined') {
            prm = Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) {
                { { scopeName } } _init();
            });
        }
        function LoadSelect2() {
             $("#<%=ddlFromLocation.ClientID%>").select2({
                placeholder: "Select Location",
                allowClear: true
             });
             $("#<%=ddlToLocation.ClientID%>").select2({
                placeholder: "Select Location",
                allowClear: true
              });
              $("#<%=ddlPIReference.ClientID%>").select2({
                placeholder: "Select Reference",
                allowClear: true
              });
             $("#<%=ddlFromPallet.ClientID%>").select2({
                placeholder: "Select Pallet",
                allowClear: true
             });
              $("#<%=ddlToPallet.ClientID%>").select2({
                  placeholder: "Select Pallet",
                allowClear: true
              });
        }
       
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
        <div class="page-header" style="padding-top:80px;">
            <h1 style="font-family: Georgia; color: #da532c;">PI Pre Count & Post Count Summary</h1>
        </div>
        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                <button type="button" id="customiseStyleReportBtn" class="btn btn-success btn-lg" data-toggle="modal" data-target="#ReportDataFilterModal" style="background-color:#da532c;" onclick="LoadSelect2()">Setup Report</button>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </div>

    <%-- Modal --%>
    <div id="ReportDataFilterModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #da532c;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title" style="font-family: Georgia; color: white; font-weight: 500">PI Pre Count & Post Count Summary</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal" role="form">
                                

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label col-md-4" for="cpo" style="font-family: Georgia">PI Reference:</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlPIReference" style="width:300px" runat="server"  AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                       
                                    </div>

                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label col-md-4" for="cpo" style="font-family: Georgia">Location From:</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFromLocation" style="width:300px" runat="server" AutoPostBack="true"  AppendDataBoundItems="true" CssClass="form-control" OnSelectedIndexChanged="ddlFromLocation_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                       
                                    </div>
                                    <div class="col-md-6">
                                         <div class="form-group">
                                            <label class="control-label col-md-3" for="so" style="font-family: Georgia">To:</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlToLocation" style="width:300px" runat="server" AutoPostBack="true"   CssClass="form-control" OnDataBound="ddlToLocation_DataBound" OnSelectedIndexChanged="ddlToLocation_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label class="control-label col-md-4" for="cpo" style="font-family: Georgia">Pallet From:</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlFromPallet" style="width:300px" runat="server" AutoPostBack="true"  AppendDataBoundItems="true" CssClass="form-control" OnSelectedIndexChanged="ddlFromPallet_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                       
                                    </div>
                                    <div class="col-md-6">
                                         <div class="form-group">
                                            <label class="control-label col-md-3" for="so" style="font-family: Georgia">To:</label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlToPallet" style="width:300px" runat="server"  CssClass="form-control" OnDataBound="ddlToPallet_DataBound"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                               

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-success btn-lg" Font-Names="Georgia" BackColor="#da532c" OnClick="btnGenerate_Click"/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>





