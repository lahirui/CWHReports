<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/WarehouseMaster.Master" AutoEventWireup="true" CodeBehind="StockReport.aspx.cs" Inherits="PDCSReporting.MasterPages.WebForm1" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/utils.js"></script>
    <script src="Scripts/select2.full.min.js"></script>
    <link href="Content/select2.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(document).ready(function () {
            LoadSelect2();
        });

        var prm;
        if (typeof (Sys) !== 'undefined') {
            prm = Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) {
                { { scopeName } } _init();
            });
        }
        
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="container-fluid">
        <div class="page-header" style="padding-top:80px;">
            <h1 style="font-family: Georgia; color: #da532c;">On Hand Stock Report</h1>
        </div>
        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                
                 <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-success btn-lg" Font-Names="Georgia" BackColor="#da532c" OnClick="btnGenerate_Click"/>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </div>




</asp:Content>
