﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/WarehouseMaster.Master" AutoEventWireup="true" CodeBehind="WIPAtDefaultBox.aspx.cs" Inherits="PDCSReporting.WIPAtDefaultBox" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function openModal() {
            $('#DataFilterModal').modal('show');
        }
    </script>

  

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
        function LoadSelect2() {
           $("#<%=ddlFromStyle.ClientID%>").select2({
                placeholder: "Select Style",
                allowClear: true
           });

             $("#<%=ddlToStyle.ClientID%>").select2({
                placeholder: "Select Style",
                allowClear: true
             });

        }
       <%-- $(document).ready(function () {
            $("#<%=ddlAodNumbers.ClientID%>").select2({
                placeholder: "Select CPO",
                allowClear: true
            });
        });--%>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-fluid">
        <div class="page-header" style="padding-top:80px;">
            <h1 style="font-family: Georgia; color: #da532c;">WIP at Default Box</h1>
        </div>
        <div class="row">
            <div class="col-sm-2 col-md-2 col-lg-2">
                <button type="button" id="customiseStyleReportBtn" class="btn btn-success btn-lg" data-toggle="modal" data-target="#DataFilterModal" style="background-color:#da532c;" onclick="LoadSelect2()">Setup Report</button>
            </div>
        </div>
        <div class="row" style="padding-top: 20px; padding-bottom: 10px;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" SizeToReportContent="true"></rsweb:ReportViewer>
        </div>
    </div>

    <%-- Modal --%>
    <div id="DataFilterModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #da532c;">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title" style="font-family: Georgia; color: white; font-weight: 500">WIP at Default Box</h3>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal" role="form">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label class="control-label col-md-4" for="cpo" style="font-family: Georgia">From Style:</label>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="ddlFromStyle" Style="width: 300px" runat="server" AutoPostBack="true" AppendDataBoundItems="true" CssClass="form-control" OnSelectedIndexChanged="ddlFromStyle_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-4" for="so" style="font-family: Georgia">To Style:</label>
                                            <div class="col-md-5">
                                                <asp:DropDownList ID="ddlToStyle" Style="width: 300px" runat="server" CssClass="form-control" OnDataBound="ddlToStyle_DataBound"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-success btn-lg" Font-Names="Georgia" BackColor="#da532c" OnClick="btnGenerate_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

