<%@ Page Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="danh-sach-vch.aspx.cs" Inherits="NHST.manager.danh_sach_vch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Import Namespace="NHST.Controllers" %>
<%@ Import Namespace="NHST.Models" %>
<%@ Import Namespace="NHST.Bussiness" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Danh sách vận chuyển hộ</h4>
                    <div class="right-action">
                        <a href="#" class="btn" id="filter-btn">Bộ lọc</a>
                    </div>
                    <div class="clearfix"></div>
                    <div class="filter-wrap" style="display: none">
                        <div class="row mt-2 pt-2">
                            <div class="input-field col s12 l4">
                                <asp:TextBox runat="server" type="text" ID="search_id" placeholder="" class=""></asp:TextBox>
                                <label>ID vận đơn</label>
                            </div>
                            <div class="input-field col s12 l4">
                                <asp:TextBox runat="server" type="text" ID="search_mvd" placeholder="" class=""></asp:TextBox>
                                <label>Mã vận đơn</label>
                            </div>

                            <div class="input-field col s12 l4">
                                <asp:TextBox runat="server" type="text" ID="search_name" placeholder="" class=""></asp:TextBox>
                                <label>Username</label>
                            </div>

                            <div class="input-field col s6 l3">
                                <asp:TextBox runat="server" ID="rFD" type="text" class="datetimepicker from-date" placeholder=""></asp:TextBox>
                                <label class="active">Từ ngày</label>
                            </div>
                            <div class="input-field col s6 l3">
                                <asp:TextBox runat="server" ID="rTD" type="text" class="datetimepicker to-date" placeholder=""></asp:TextBox>
                                <label class="active">Đến ngày</label>
                                <span class="helper-text" data-error="Vui lòng chọn ngày bắt đầu trước"></span>
                            </div>

                            <div class="input-field col s12 l6">
                                <asp:ListBox runat="server" ID="status">
                                    <asp:ListItem Value="-1" Selected="true" Text="Tất cả"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Đơn hàng mới"></asp:ListItem>                                   
                                    <asp:ListItem Value="3" Text="Đã về kho TQ"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Đã về kho đích"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Đã thanh toán"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Đã gửi yêu cầu"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Đơn hàng hủy"></asp:ListItem>
                                </asp:ListBox>
                                <label for="status">Trạng thái</label>
                            </div>

                            <div class="input-field col s6 left-align">
                                <asp:Button ID="search" runat="server" OnClick="btnSearch_Click" class="btn " Text="Lọc kết quả"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="list-staff col s12 section">
                <div class="list-table card-panel">
                    <div class="responsive-tb">
                        <table class="table  highlight bordered   mt-2">
                            <thead>
                                <tr>
                                    <th style="min-width: 100px;">ID</th>
                                    <th style="min-width: 100px;">Username khách</th>
                                    <th style="min-width: 200px;">Nhân viên kinh doanh</th>
                                    <th style="min-width: 100px;">Mã vận đơn</th>
                                    <th style="min-width: 100px;">Cân nặng</th>
                                    <th style="min-width: 100px;">Thông tin phí</th>                                                       
                                    <th style="min-width: 100px;">Ngày đơn</th>                                    
                                    <th style="min-width: 100px;">Trạng thái</th>
                                    <th style="min-width: 100px;">Thao tác</th>
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
        <div class="row">
            <div class="bg-overlay"></div>
            <!-- Edit mode -->
            <div class="detail-fixed  col s12 m12 l6 xl6 section">
                <div class="rp-detail card-panel row">
                    <div class="col s12">
                        <div class="page-title">
                            <h5>Mã vận đơn #1101</h5>
                            <a href="#!" class="close-editmode top-right valign-wrapper">
                                <i class="material-icons">close</i>Close</a>
                        </div>
                    </div>
                    <div class="col s12">
                        <div class="row">
                            <div class="input-field col s12 m12">
                                <input id="mvc_detail-mvc" type="text" class="" value="123412341234">
                                <label for="mvc_detail-mvc">Mã vận đơn</label>
                            </div>
                            <div class="input-field col s12 m6">
                                <select id="mvc_detail-bh">
                                    <option value="" disabled>Bao hàng</option>
                                    <option value="0">Chưa có</option>
                                    <option value="1" selected>Chị Vân Anh HN</option>
                                    <option value="2">Anh hùng SG</option>
                                </select>
                                <label for="mvc_detail-bh">Bao hàng</label>
                            </div>
                            <div class="input-field col s12 m6">
                                <input id="mvc_detail-lh" type="text" class="" value="Điện tử">
                                <label for="mvc_detail-lh">Loại hàng</label>
                            </div>
                            <div class="input-field col s12 m4">
                                <input id="mvc_detail-ps" type="text" class="" value="20,000">
                                <label for="mvc_detail-ps">Phí ship (tệ)</label>
                            </div>
                            <div class="input-field col s12 m4">
                                <input id="mvc_detail-tl" type="text" class="" value="214">
                                <label for="mvc_detail-tl">Trọng lượng (kg)</label>
                            </div>
                            <div class="input-field col s12 m4">
                                <input id="mvc_detail-khoi" type="text" class="" value="124">
                                <label for="mvc_detail-khoi">Khối (m<sup>3</sup>)</label>
                            </div>
                            <div class="col s12 m12">
                                <span class="black-text">Hình ảnh</span>
                                <div style="display: inline-block; margin-left: 15px;">
                                    <input class="upload-img" type="file" onchange="previewFiles(this)" multiple title="">
                                    <span href="#!" class="btn-upload">Upload</span>
                                </div>
                                <div class="preview-img">
                                </div>
                            </div>
                            <div class="input-field col s12 m12">
                                <input id="mvc_detail-note" type="text" class=""
                                    value="Hàng dễ hư xin đừng nhẹ tay...">
                                <label for="mvc_detail-note">Ghi chú</label>
                            </div>
                            <div class="input-field col s12 m12">
                                <select id="mvc_detail-status">
                                    <option value="" disabled>Trạng thái</option>
                                    <option value="0">Chưa nhận hàng tại TQ</option>
                                    <option value="1" selected>Đã về kho TQ</option>
                                    <option value="2">Đã về kho VN</option>
                                    <option value="2">Đã giao cho khách</option>
                                </select>
                                <label for="mvc_detail-status">Trạng thái</label>
                            </div>
                            <div class="input-field col s12 m12">
                                <div class="action-wrap">
                                    <button class="btn">Cập nhật</button>
                                    <button class="btn close-editmode">Trở về</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function ChooseSaler(OrderID, obj) {
            var SalerID = obj.val();
            $.ajax({
                type: "POST",
                url: "/manager/danh-sach-vch.aspx/UpdateStaff",
                data: "{OrderID:'" + OrderID + "',StaffID:'" + SalerID + "',Type:'1'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data != "null") {
                        if (data != "notpermission") {
                            //location.reload();
                        }
                        else {
                            alert('Bạn không có quyền');
                        }
                    }
                    else {
                        alert('Vui lòng đăng nhập lại.');
                    }
                },
                error: function (xmlhttprequest, textstatus, errorthrow) {
                    alert('lỗi checkend');
                }
            });
        }
        $(document).ready(function () {
            $('#txtSearchName').autocomplete({
                data: {
                    "Apple": null,
                    "Microsoft": null,
                    "Google": 'https://placehold.it/250x250',
                    "Asgard": null
                },
            });
        });
        function myFunction() {
            if (event.which == 13 || event.keyCode == 13) {
                $('#<%=search.ClientID%>').click();
            }
        }
        $('.search-action').click(function () {

            $('#<%=search.ClientID%>').click();
        })
    </script>
</asp:Content>
