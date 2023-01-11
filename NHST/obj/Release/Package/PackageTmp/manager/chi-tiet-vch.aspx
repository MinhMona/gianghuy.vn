<%@ Page Language="C#" MasterPageFile="~/manager/adminMasterNew.Master" AutoEventWireup="true" CodeBehind="chi-tiet-vch.aspx.cs" Inherits="NHST.manager.chi_tiet_vch" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="main" class="main-full">
        <div class="row">
            <div class="content-wrapper-before bg-dark-gradient"></div>
            <div class="page-title">
                <div class="card-panel">
                    <h4 class="title no-margin" style="display: inline-block;">Chi tiết vận chuyển</h4>
                    <div class="clearfix"></div>
                </div>
            </div>
            <div class="list-staff col s12 m6 section">
                <div class="rp-detail card-panel row">
                    <div class="col s12">
                        <div class="row pb-2 border-bottom-1 ">

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="rp_username" type="text" class="validate" Enabled="false"></asp:TextBox>
                                <label for="rp_username">Username</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtBarcode" Enabled="false" type="text" class="validate"></asp:TextBox>
                                <label for="rp_vnd">Mã vận đơn</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtName" Enabled="false" type="text" class="validate"></asp:TextBox>
                                <label for="rp_vnd">Tên hàng hóa</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtPriceVND" type="text" class="validate"></asp:TextBox>
                                <label for="rp_vnd">Giá trị đơn hàng</label>
                            </div>

                            <div class="input-field col s12 m6">
                                <label>
                                    <input type="checkbox" id="cbMaVanDon" class="chk-check-option" data-id="1" />
                                    <span id="lbCheckBox">Bảo hiểm đơn hàng</span></label>
                                <asp:HiddenField runat="server" ID="hdfCheckBox" />
                            </div>

                            <div class="input-field col s12 m6">
                                <asp:TextBox runat="server" ID="txtInsurrancePrice" placeholder="0" type="text" Enabled="false" value="0"></asp:TextBox>
                                <label>Việt Nam Đồng (VNĐ)</label>
                            </div>

                            <div class="input-field col s12">
                                <telerik:RadNumericTextBox runat="server" CssClass="form-control"
                                    Skin="MetroTouch" ID="rWeight" NumberFormat-DecimalDigits="2"
                                    Value="0" NumberFormat-GroupSizes="3" placerholder="" Width="100%" MinValue="0" Enabled="false">
                                </telerik:RadNumericTextBox>
                                <label for="rp_vnd" class="active">Cân nặng</label>
                            </div>

                            <div class="input-field col s12">
                                <telerik:RadNumericTextBox runat="server" CssClass="form-control" Skin="MetroTouch"
                                    ID="rSensorFeeCYN" NumberFormat-DecimalDigits="2" Value="0" oninput="CountFee('ndt','sensor',$(this))"
                                    NumberFormat-GroupSizes="3" Width="45%" MinValue="0">
                                </telerik:RadNumericTextBox>
                                <label for="rp_vnd" class="active">Cước vật tư</label>
                                <telerik:RadNumericTextBox runat="server" CssClass="form-control" Skin="MetroTouch"
                                    ID="rSensorFeeeVND" NumberFormat-DecimalDigits="0" Value="0" oninput="CountFee('vnd','sensor',$(this))"
                                    NumberFormat-GroupSizes="3" Width="45%" MinValue="0">
                                </telerik:RadNumericTextBox>
                            </div>

                            <div class="input-field col s12">
                                <telerik:RadNumericTextBox runat="server" CssClass="form-control" Skin="MetroTouch"
                                    ID="rAdditionFeeCYN" NumberFormat-DecimalDigits="2" Value="0" oninput="CountFee('ndt','feeadd',$(this))"
                                    NumberFormat-GroupSizes="3" Width="45%" MinValue="0">
                                </telerik:RadNumericTextBox>
                                <label for="rp_vnd" class="active">Phụ phí hàng đặc biệt</label>
                                <telerik:RadNumericTextBox runat="server" CssClass="form-control" Skin="MetroTouch"
                                    ID="rAdditionFeeVND" NumberFormat-DecimalDigits="0" Value="0" oninput="CountFee('vnd','feeadd',$(this))"
                                    NumberFormat-GroupSizes="3" Width="45%" MinValue="0">
                                </telerik:RadNumericTextBox>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtSummary"
                                    class="materialize-textarea"></asp:TextBox>
                                <label for="rp_textarea" class="active">Ghi chú khách</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtStaffNote"
                                    class="materialize-textarea"></asp:TextBox>
                                <label for="rp_textarea" class="active">Ghi chú của nhân viên</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlWarehouseFrom" runat="server" CssClass="form-control"
                                    DataValueField="ID" DataTextField="WareHouseName" onchange="returnWeightFee()">
                                </asp:DropDownList>
                            </div>

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlReceivePlace" runat="server" CssClass="form-control"
                                    DataValueField="ID" DataTextField="WareHouseName" onchange="returnWeightFee()">
                                </asp:DropDownList>
                            </div>

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlShippingTypeKyGui" runat="server" CssClass="form-control"
                                    DataValueField="ID" DataTextField="ShippingTypeName" onchange="returnWeightFee()">
                                </asp:DropDownList>
                            </div>

                            <div class="input-field col s12">
                                <asp:DropDownList runat="server" ID="ddlStatus">
                                    <asp:ListItem Value="1" Text="Đơn hàng mới"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Đã về kho TQ"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Đã về kho đích"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Đã yêu cầu"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Đã nhận hàng"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Đơn hàng hủy"></asp:ListItem>
                                </asp:DropDownList>
                                <label>Trạng thái</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:DropDownList ID="ddlShippingType" runat="server" Enabled="false"
                                    Width="40%" CssClass="form-control">
                                </asp:DropDownList>
                                <label>Hình thức vận chuyển</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtExportRequestNote"
                                    class="materialize-textarea" placeholder=""></asp:TextBox>
                                <label for="rp_textarea">Ghi chú hình thức vận chuyển</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtDateExportRequest" ReadOnly="true" placeholder=""></asp:TextBox>
                                <label for="rp_username">Ngày yêu cầu xuất kho</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtDateExport" ReadOnly="true" placeholder=""></asp:TextBox>
                                <label for="rp_username">Ngày xuất kho</label>
                            </div>

                            <div class="input-field col s12">
                                <asp:TextBox runat="server" ID="txtCancelReason"
                                    class="materialize-textarea" placeholder=""></asp:TextBox>
                                <label for="rp_textarea">Lý do hủy đơn</label>
                            </div>

                        </div>
                        <div class="row section mt-2">
                            <div class="col s12">
                                <a href="javascript:;" onclick="UpdateOrder()" class="btn">Cập nhật</a>
                                <a href="#" class="btn btnBackLink">Trở về</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:Button ID="btnSaveOrder" runat="server" UseSubmitBehavior="false" Text="Cập nhật" Style="display: none" CssClass="btn" OnClick="btnSave_Click" />
    <asp:Button runat="server" ID="btnBackLink" OnClick="btnBackLink_Click" UseSubmitBehavior="false" Style="display: none" />
    <asp:HiddenField ID="hdfListCheckBox" runat="server" />
    <asp:HiddenField ID="hdfCurrency" runat="server" />
    <script type="text/javascript">
        function UpdateOrder() {
            var chuoi = "";
            $('.chk-check-option').each(function () {
                var id = $(this).attr('data-id');

                if ($(this).prop("checked") == true) {
                    chuoi += id + "," + "1|";
                }
                else {
                    chuoi += id + "," + "0|";
                }
            });
            $("#<%=hdfListCheckBox.ClientID%>").val(chuoi);
            $("#<%=btnSaveOrder.ClientID%>").click();
        }
        $('.btnBackLink').click(function () {
            $('#<%=btnBackLink.ClientID%>').click();
        });
        function keypress(e) {
            var keypressed = null;
            if (window.event) {
                keypressed = window.event.keyCode; //IE
            }
            else {
                keypressed = e.which; //NON-IE, Standard
            }
            if (keypressed < 48 || keypressed > 57) {
                if (keypressed == 8 || keypressed == 127) {
                    return;
                }
                return false;
            }
        }
        $(document).ready(function () {
            var cb1 = $("#<%=hdfCheckBox.ClientID%>").val();
            $('#cbMaVanDon').prop("checked", (/true/i).test(cb1.toLowerCase()))
        });
    </script>
    <script type="text/javascript">
        var currency = parseFloat($("#<%= hdfCurrency.ClientID%>").val());
        function CountFee(currencyType, feild, obj) {
            var valu = parseFloat(obj.val());
            if (feild == "feeadd") {
                if (currencyType == 'vnd') {
                    var convertvalue = valu / currency;
                    $("#<%= rAdditionFeeCYN.ClientID%>").val(convertvalue);
                }
                else {
                    var convertvalue = valu * currency;
                    $("#<%= rAdditionFeeVND.ClientID%>").val(convertvalue);
                }
            }
            else {
                if (currencyType == 'vnd') {
                    var convertvalue = valu / currency;
                    $("#<%= rSensorFeeCYN.ClientID%>").val(convertvalue);
                }
                else {
                    var convertvalue = valu * currency;
                    $("#<%= rSensorFeeeVND.ClientID%>").val(convertvalue);
                }
            }
        }
        $(document).ready(function () {
            $('.select2').select2();
        });
    </script>   
</asp:Content>
