﻿<%@ Page Language="C#"  MasterPageFile="~/MasterPages/WarehouseMaster.Master" AutoEventWireup="true" CodeBehind="LocationDisplay.aspx.cs" Inherits="PDCSReporting.LocationDisplay" %>

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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta http-equiv="refresh" content="1800";/>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
        <div class="page-header" style="margin-top: 80px">
            <%-- <h1 style="font-family: Georgia; color: #da532c;">Location</h1>--%>
        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Label ID="Label1" runat="server" style="font-size:xx-large; color:red; font-weight:bold">Available Precentage: </asp:Label>
                <asp:Label ID="lblPrecentage" runat="server" style="font-size:xx-large; color:red; font-weight:bold"></asp:Label>
            </div>
            <div class="col-md-4">
                 <asp:Label ID="Label2" runat="server" style="font-size:xx-large; color:red; font-weight:bold">Available Locations : </asp:Label>
                 <asp:Label ID="lblRemaining" runat="server" style="font-size:xx-large; color:red; font-weight:bold"></asp:Label>
            </div>
             <div class="col-md-4">
                 <asp:Label ID="Label3" runat="server" style="font-size:xx-large; color:red; font-weight:bold">Used Locations : </asp:Label>
                 <asp:Label ID="lblUsed" runat="server" style="font-size:xx-large; color:red; font-weight:bold"></asp:Label>
            </div>


        </div>
        <div class="row" style="margin-top:10px; overflow-x:scroll; overflow-y:scroll">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
       <%-- <div class="row" style="overflow:scroll">
             <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        </div>--%>
       
    </div>
</asp:Content>


