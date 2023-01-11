<%@ Page Title="" Language="C#" MasterPageFile="~/GiangHuyMaster.Master" AutoEventWireup="true" CodeBehind="chi-tiet-trang.aspx.cs" Inherits="NHST.chi_tiet_trang2" %>

<%@ Register Src="~/UC/uc_Sidebar.ascx" TagName="SideBar" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/App_Themes/GiangHuy/css/style-custom.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="main" style="padding-top: 8%;">
        <div class="breadcrumb">
            <div class="container">
                <ul>
                    <li><a href="/">Trang chủ</a></li>
                    <asp:Literal runat="server" ID="ltrBread"></asp:Literal>
                </ul>
            </div>
        </div>
        <div id="primary" class="page1">
            <div class="container" style="padding-top: 50px;">
                <div class="main-content policy clear">
                    <asp:Panel runat="server" ID="pnSideBar" Visible="false">
                        <div class="columns ">
                            <div class="column left">
                                <section class="sidebar">
                                    <ul class="sidebar-menu">
                                        <asp:Literal runat="server" ID="ltrSidebar"></asp:Literal>
                                    </ul>
                                </section>
                            </div>
                            <div class="column right">
                                <section class="right-content">
                                    <p>
                                        <asp:Label runat="server" ID="lblTitle"></asp:Label>
                                        <asp:Literal runat="server" ID="ltrDetail"></asp:Literal>
                                    </p>
                                </section>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnFull">
                        <section class="about-section sec">
                            <div class="container">
                                <asp:Label runat="server" ID="lblTitleFull"></asp:Label>
                                <asp:Literal runat="server" ID="ltrContent"></asp:Literal>
                            </div>
                        </section>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </main>
    <script>
        $(".sidebar .sidebar-menu > li.menu-item-has-children.current-menu-item > .sub-menu").slideDown();
        $(".sidebar .sidebar-menu > li.menu-item-has-children > a").click(function (e) {
            e.preventDefault();
            if ($(this).parent().hasClass("current-menu-item")) {
                $(this).parent().removeClass("current-menu-item");
                $(this).next().stop().slideUp();
            } else {
                $(".sidebar .sidebar-menu > li.current-menu-item > .sub-menu").stop().slideUp();
                $(".sidebar .sidebar-menu > li.menu-item-has-children.current-menu-item").removeClass("current-menu-item");
                $(this).parent().addClass("current-menu-item");
                $(this).next().stop().slideDown();
            }

        });
    </script>
</asp:Content>
