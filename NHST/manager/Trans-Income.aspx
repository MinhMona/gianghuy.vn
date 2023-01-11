<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/manager/adminMasterNew.Master" CodeBehind="Trans-Income.aspx.cs" Inherits="NHST.manager.Trans_Income" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Thống kê hoa hồng ký gửi sale</h4>
                    <div class="right-action">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap" style="display: block">
                        <div class="row mt-2 pt-2">
                            <div class="input-field col s12 l3">
                                <asp:TextBox runat="server" ID="search_name" type="text" class="validate autocomplete"></asp:TextBox>
                                <label for="search_name">Username</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <asp:ListBox runat="server" ID="ddlstatus">
                                    <asp:ListItem Value="0" Selected="true" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Chưa thanh toán"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Đã thanh toán"></asp:ListItem>
                                </asp:ListBox>
                                <label for="status">Trạng thái thanh toán</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <asp:TextBox ID="rFD" runat="server" placeholder="" Type="text" class="datetimepicker from-date"></asp:TextBox>
                                <label>Từ ngày</label>
                            </div>
                            <div class="input-field col s12 l3">
                                <asp:TextBox runat="server" Type="text" placeholder="" ID="rTD" class="datetimepicker to-date"></asp:TextBox>
                                <label>Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>
                            <div class="col s12 right-align">
                                <asp:Button runat="server" class="btn" ID="search" OnClick="search_Click" Text="Lọc kết quả"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="list-commission col s12 section">
                <div class="list-table card-panel">
                    <div class="table-info row center-align-xs">
                        <div class="checkout col s12 m6">
                            <div class="row-p">
                                <div class="cot-p">
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng hoa hồng sale: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTotalIncome"></asp:Literal>
                                        </div>
                                    </div>        
                                </div>
                                <div class="cot-p">
                                     <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng cân nặng: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTotalWeight"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                                <div class="cot-p">
                                    <div class="cot-ngang">
                                        <div class="text-bold">
                                            Tổng tiền thanh toán: 
                                        </div>
                                        <div class="price">
                                            <asp:Literal runat="server" ID="ltrTotaPrice"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="responsive-tb mt-2">
                        <table class="table bordered highlight  ">
                            <thead>
                                <tr>
                                    <th style="min-width: 50px;">ID thống kê</th>
                                    <th style="min-width: 100px;">Nhân viên
                                                            </br>kinh doanh</th>
                                    <th style="min-width: 100px;">Username khách</th>
                                    <th style="min-width: 100px;">Tổng cân (kg)</th>
                                    <th style="min-width: 100px;">Tổng bảo hiểm (VNĐ)</th>
                                    <th style="min-width: 100px;">Tổng thanh toán (VNĐ)</th>
                                    <th style="min-width: 100px;">Hoa hồng (VNĐ)</th>
                                    <th style="min-width: 100px;">Thanh toán</th>
                                    <th style="min-width: 100px;">Ngày xuất kho</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Literal ID="ltr" runat="server" EnableViewState="false"></asp:Literal>
                            </tbody>
                        </table>
                    </div>
                    <div class="pagi-table float-right mt-2">
                        <%this.DisplayHtmlStringPaging1();%>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>

    </div>
    <style>
        .row-p {
            display: flex;
            flex-wrap: wrap;
            margin: 0 -15px;
            justify-content: space-around;
        }

        @media screen and (max-width: 768px ) {
            .row-p {
                justify-content: space-between;
            }
        }

        .col .m6 {
            width: 100% !important;
        }

        .cot-p {
            width: calc( 3 / 12 * 100%);
            padding: 0 15px;
            margin-bottom: 30px !important;
        }

        .cot-p {
            margin-bottom: 30px;
        }

        .list-commission .checkout p {
            margin-bottom: 5px;
            font-size: 20px;
        }

            .list-commission .checkout p span {
                font-weight: bold;
                font-size: 16px;
                margin-right: 30px !important;
            }

        .cot-ngang {
            display: flex;
            justify-content: space-between;
        }

            .cot-ngang .text-bold {
                font-weight: bold;
                font-size: 16px;
            }

            .cot-ngang .price {
                font-size: 18px;
                color: green;
                font-weight: bold;
            }
    </style>
</asp:Content>
