<%@ Page Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="tao-ma-van-don-ky-gui-app.aspx.cs" Inherits="NHST.tao_ma_van_don_ky_gui_app" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <main id="main-wrap">
        <div class="page-wrap">
            <asp:Panel ID="pnMobile" runat="server" Visible="false">
                <div class="page-body">
                    <div class="all">
                        <div class="white-nooffset-cont account-page">

                            <div class="form-wrap350">
                                <h1 class="page-title">Tạo đơn hàng vận chuyển hộ</h1>
                                <div class="frow">
                                    <div class="lb">Tên đăng nhập</div>
                                    <asp:Label runat="server" ID="lbUsername" Style="font-weight: 500;"></asp:Label>
                                </div>
                                <div class="frow">
                                    <p class="lb">Chọn kho Trung Quốc</p>
                                    <div class="ip-with-sufix">
                                        <asp:DropDownList ID="ddlWarehouseFrom" runat="server" CssClass="fcontrol"
                                            DataValueField="ID" DataTextField="WareHouseName">
                                        </asp:DropDownList>
                                        <span class="sufix"><i class="fa fa-caret-down hl-txt"></i></span>
                                    </div>
                                </div>
                                <div class="frow">
                                    <p class="lb">Chọn kho đích</p>
                                    <div class="ip-with-sufix">
                                        <asp:DropDownList ID="ddlReceivePlace" runat="server" CssClass="fcontrol"
                                            DataValueField="ID" DataTextField="WareHouseName">
                                        </asp:DropDownList>
                                        <span class="sufix"><i class="fa fa-caret-down hl-txt"></i></span>
                                    </div>
                                </div>
                                <div class="frow">
                                    <p class="lb">Chọn phương thức vận chuyển</p>
                                    <div class="ip-with-sufix">
                                        <asp:DropDownList ID="ddlShippingType" runat="server" CssClass="fcontrol"
                                            DataValueField="ID" DataTextField="ShippingTypeName">
                                        </asp:DropDownList>
                                        <span class="sufix"><i class="fa fa-caret-down hl-txt"></i></span>
                                    </div>
                                </div>
                                <div class="frow">
                                    <p class="lb">Danh sách kiện</p>
                                    <table class="tb-kienhang tr-removeable">
                                        <thead>
                                            <tr>
                                                <th colspan="2" style="width: 261px; text-align: center; font-size: 14px">TẠO ĐƠN HÀNG KÝ GỬI</th>
                                            </tr>
                                        </thead>
                                        <tbody class="product-list">
                                            <tr class="product-item">
                                                <td>Mã vận đơn</td>
                                                <td>
                                                    <asp:TextBox runat="server" placeholder="Mã vận đơn" ID="txtBarcode" type="text" class="pack-name gray-bg fcontrol product-mvd"></asp:TextBox></td>
                                            </tr>
                                            <tr class="product-item">
                                                <td>Tên hàng
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" placeholder="Tên hàng" ID="txtProductName" type="text" class="pack-name gray-bg fcontrol product-name"></asp:TextBox></td>
                                            </tr>
                                            <tr class="product-item">
                                                <td>Giá trị (VNĐ)</td>
                                                <td>
                                                    <asp:TextBox runat="server" value="0" ID="txtProductPrice" type="number" class="pack-name gray-bg fcontrol product-price"></asp:TextBox></td>
                                            </tr>
                                            <tr class="product-item">
                                                <td>Bảo hiểm
                                                </td>
                                                <td>
                                                    <label>
                                                        <input type="checkbox" class="checbox-item pack-dv5" id="packdv5" data-id="1">
                                                        <span class="checkmark"></span>
                                                    </label>
                                                    <asp:HiddenField ID="chkIsDV5" runat="server" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="frow">
                                    <p class="lb">Ghi chú</p>
                                    <asp:TextBox ID="txtNote" runat="server" CssClass="fcontrol gray-bg" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <a href="#" onclick="CreateOrder()" class="btn primary-btn fw-btn">Tạo đơn hàng</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="page-bottom-toolbar">
                </div>
            </asp:Panel>
            <asp:Panel ID="pnShowNoti" runat="server" Visible="false">
                <div class="page-body">
                    <div class="heading-search logout">
                        <h4 class="page-title">Bạn vui lòng đăng xuất và đăng nhập lại!</h4>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </main>
    <asp:HiddenField ID="hdfProductList" runat="server" />
    <asp:HiddenField ID="hdfListCheckBox" runat="server" />
    <asp:Button ID="btncreateuser" runat="server" Text="Cập nhật" CssClass="btn btn-success btn-block pill-btn primary-btn main-btn hover" OnClick="btncreateuser_Click" Style="display: none" />
    <style>
        .pane-primary .heading {
            background-color: #366136 !important;
        }

        .btn.payment-btn {
            background-color: #3f8042;
            color: white;
            padding: 10px;
            height: 40px;
            width: 49%;
        }

        .btn.cancel-btn {
            background-color: #f84a13;
            color: white;
            padding: 10px;
            height: 40px;
            width: 49%;
        }

        .pagenavi {
            float: right;
            margin-top: 20px;
        }

            .pagenavi a,
            .pagenavi span {
                width: 30px;
                height: 35px;
                line-height: 40px;
                text-align: center;
                color: #959595;
                font-weight: bold;
                background: #f8f8f8;
                display: inline-block;
                font-weight: bold;
                margin-right: 1px;
            }

                .pagenavi .current,
                .pagenavi a:hover {
                    background: #ea1f28;
                    color: #fff;
                }

        .pagenavi {
            float: right;
            margin-top: 20px;
        }

            .pagenavi a,
            .pagenavi span {
                width: 30px;
                height: 35px;
                line-height: 40px;
                text-align: center;
                color: #959595;
                font-weight: bold;
                background: #f8f8f8;
                display: inline-block;
                font-weight: bold;
                margin-right: 1px;
            }

                .pagenavi .current,
                .pagenavi a:hover {
                    background: #ea1f28;
                    color: #fff;
                }

        .filters {
            background: #ebebeb;
            border: 1px solid #e1e1e1;
            font-weight: bold;
            padding: 20px;
            margin-bottom: 20px;
        }

            .filters ul li {
                display: inline-block;
                text-align: center;
                padding-right: 2px;
            }

            .filters ul li {
                padding-right: 4px;
            }

        select.form-control {
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            -ms-appearance: none;
            -o-appearance: none;
            background: #fff url(/App_Themes/NHST/images/icon-select.png) no-repeat right 15px center;
            padding-right: 25px;
            padding-left: 15px;
            line-height: 40px;
        }

        .tb-kienhang .cn-ip {
            width: 100%;
        }

        .product-item td {
            position: relative;
        }

            .product-item td .checbox-item {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
            }

        .checkmark {
            position: absolute;
            top: 7px;
            right: 10px;
            height: 25px;
            width: 25px;
            border: 1px solid #80808024;
            background: #fff;
        }

        .product-item td .checbox-item:checked ~ .checkmark {
            background-color: #ffffff;
            border: 1px solid #80808024;
        }

        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        .product-item td .checbox-item:checked ~ .checkmark:after {
            display: block;
        }

        .checkmark:after {
            left: 9px;
            top: 6px;
            width: 7px;
            height: 10px;
            border: solid #f37421;
            border-width: 0 3px 3px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }

        .product-mvd.border-select {
            border: 1px solid #f37421;
        }
    </style>

    <script type="text/javascript">
        //function addProduct() {
        //    var html = "";
        //    html += "<tr class=\"product-item\">";
        //    html += " <td><input type=\"text\" class=\"gray-bg fcontrol mvd-ip product-link\" placeholder=\"Nhập mã vận đơn\"></td>";
        //    html += " <td><input type=\"text\" class=\"gray bg fcontrol cn-ip product-note\" placeholder=\"Ghi chú\"/></td>";
        //    html += "<td><a href=\"javascript:;\" onclick=\"deleteProduct($(this))\" class=\"rm-action\"><img src=\"/App_Themes/App/images/icon-remove-red.png\" alt=\"\"></a></td>";
        //    html += "</tr>";
        //    $(".product-list").append(html);
        //}
        //function deleteProduct(obj) {
        //    var c = confirm('Bạn muốn xóa kiện này?');
        //    if (c == true) {
        //        obj.parent().parent().remove();
        //    }
        //}
        function CreateOrder() {
            var html = "";
            var check = false;
            let productmvd = $(".product-mvd").val();
            validateText($(".product-mvd"));
            console.log(productmvd)
            if (isBlank(productmvd)) {
                check = true;
            }
            if (check == true) {
                alert('Vui lòng nhập mã vận đơn!');
                return false;
            }
            else {
                $(".product-list").each(function () {
                    var item = $(this);
                    var productmvd = item.find(".product-mvd").val();
                    var productname = item.find(".product-name").val();
                    var productprice = item.find(".product-price").val();
                    var productdv5 = item.find(".pack-dv5").val();

                    html += productmvd + "]" + productname + "]" + productprice + "]" + productdv5 + "|";
                });
                $.ajax({
                    type: "POST",
                    url: "/tao-ma-van-don-ky-gui-app.aspx/checkbefore",
                    data: "{listStr:'" + html + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var ret = msg.d;
                        console.log(ret);
                        var chuoi = "";
                        $('.checbox-item').each(function () {
                            var id = $(this).attr('data-id');

                            if ($(this).prop("checked") == true) {
                                chuoi += id + "," + "1|";
                            }
                            else {
                                chuoi += id + "," + "0|";
                            }
                        });
                        if (ret == "ok") {
                            $("#<%=hdfListCheckBox.ClientID%>").val(chuoi);
                            $("#<%=hdfProductList.ClientID%>").val(html);
                            $("#<%=btncreateuser.ClientID%>").click();
                        }
                        else {
                            alert('Mã vận đơn bị trùng: ' + ret + '. Vui lòng thay đổi mã.');
                            return false;
                        }
                    },
                    error: function (xmlhttprequest, textstatus, errorthrow) {
                    }
                });
            }
        }
        function validateText(obj) {
            var value = obj.val();
            if (isBlank(value)) {
                obj.addClass("border-select");
            }
            else {
                obj.removeClass("border-select");
            }
        }
        function validateNumber(obj) {
            var value = parseFloat(obj.val());
            if (value <= 0)
                obj.addClass("border-select");
            else
                obj.removeClass("border-select");
        }
        function isBlank(str) {
            return (!str || /^\s*$/.test(str));
        }
        $(document).ready(function () {
            var cb1 = $("#<%= chkIsDV5.ClientID%>").val();

            $('#packdv5').prop("checked", (/true/i).test(cb1.toLowerCase()))
        });
    </script>
</asp:Content>
