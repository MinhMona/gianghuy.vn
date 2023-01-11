<%@ Page Title="" Language="C#" MasterPageFile="~/GiangHuyMaster.Master" AutoEventWireup="true" CodeBehind="danh-muc-trang.aspx.cs" Inherits="NHST.danh_muc_trang1" %>

<%@ Register Src="~/UC/uc_Sidebar.ascx" TagName="SideBar" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App_Themes/GiangHuy/css/style-custom.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        @media screen and (max-width: 600px) {
            .pagi-table {
                float: none !important;
                text-align: center;
                margin-top: 15px !important
            }
        }

        .pagi-table .pagi-button {
            padding: 5px 10px;
            cursor: pointer;
            transition: all .2s ease;
            -webkit-transition: all .2s ease;
            -moz-transition: all .2s ease;
            border-radius: 3px
        }

            .pagi-table .pagi-button.current-active {
                background: #F64302;
                color: #fff
            }

            .pagi-table .pagi-button:hover {
                background: #F64302;
                color: #fff
            }
    </style>
    <main class="main" style="padding-top: 8%">
        <div class="breadcrumb wow fadeIn" data-wow-duration="1s" data-wow-delay="0s">
            <div class="container">
                <ul>
                    <li style="color: #fff"><a href="/Default.aspx">Trang chủ</a></li>
                    <li style="color: #fff">
                        <asp:Literal runat="server" ID="ltrbre"></asp:Literal></li>
                </ul>
            </div>
        </div>
        <div class="service-page-section sec">
            <div class="container">
                <div class="columns">
                    <div class="column left wow fadeInLeft" data-wow-duration="1s" data-wow-delay="0s">
                        <uc:SideBar ID="SideBar1" runat="server" />
                        <%-- <div class="sidebar">
                            <p class="sidebar-title">DANH MỤC</p>
                            <div class="sidebar-list-item">
                                <div class="sidebar-item">
                                    <asp:Literal runat="server" ID="ltrCategory"></asp:Literal>
                                    <ul class="sidebar-item-nav">
                                    </ul>
                                </div>
                            </div>
                        </div>--%>
                    </div>

                    <div class="column right wow fadeInRight" data-wow-duration="1s" data-wow-delay="0s">
                        <section class="price-section">
                            <asp:Literal runat="server" ID="ltrTitle"></asp:Literal>
                            <div class="">
                                <asp:Literal runat="server" ID="ltrDetail"></asp:Literal>
                            </div>
                            <div class="guide-list">
                                <div class="columns">
                                    <asp:Literal runat="server" ID="ltrList"></asp:Literal>
                                </div>
                            </div>
                            <div class="pagi-table float-right mt-2" style="padding-top: 20px; display: flex; justify-content: right">
                                <%this.DisplayHtmlStringPaging1();%>
                            </div>
                            <div class="clearfix"></div>
                        </section>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
