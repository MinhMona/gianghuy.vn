<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chi-tiet-ky-gui.aspx.cs" MasterPageFile="~/UserMasterNew.Master" Inherits="NHST.chi_tiet_ky_gui" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div id="main">
        <div class="row">
            <div class="content-wrapper-before blue-grey lighten-5"></div>
            <div class="col s12">
                <div class="container">
                    <div class="all">
                        <div class="mt-3 no-shadow">
                            <div class="col s12">
                                <div class="page-title mt-2 center-align">
                                    <h4>
                                        <asp:Literal runat="server" ID="ltrMainOrderID"></asp:Literal></h4>
                                </div>
                            </div>
                            <div class="order-detail-wrap col s12 section account-sticky mt-2">
                                <div class="row">
                                    <div class="col s12">
                                        <div class="summary-detail mb-5">
                                            <div class="card-panel z-depth-3">
                                                <div class="title-header bg-dark-gradient  mb-1">
                                                    <h6 class="white-text ">Tổng quan đơn hàng</h6>
                                                </div>
                                                <div class="row">
                                                    <asp:Literal runat="server" ID="ltrOverView"></asp:Literal>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-panel">
                                            <div id="mvc-list" class="section scrollspy mvc-list">
                                                <div class="title-header bg-dark-gradient">
                                                    <h6 class="white-text ">Danh sách mã vận đơn</h6>
                                                </div>
                                                <div class="responsive-tb">
                                                    <table class="table    highlight bordered  centered   ">
                                                        <thead>
                                                            <tr>
                                                                <th>Mã vận đơn</th>
                                                                <th>Cân nặng (kg)</th>
                                                                <th>Chiều dài (cm)</th>
                                                                <th>Chiều rộng (cm)</th>
                                                                <th>Chiều cao (cm)</th>
                                                                <th>Trạng thái</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody class="list-product">
                                                            <asp:Literal runat="server" ID="ltrSmallPackages"></asp:Literal>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <style>
        span.badge {
            float: unset !important;
            margin-left: unset !important;
        }
    </style>
</asp:Content>
