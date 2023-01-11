using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class chi_tiet_ky_gui : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] != null)
                {
                    loaddata();
                }
                else
                {
                    Response.Redirect("/trang-chu");
                }
            }            
        }
        public void loaddata()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                var ID = Request.QueryString["ID"].ToInt(0);
                if (ID > 0)
                {
                    var t = TransportationOrderNewController.GetByID(ID);
                    if (t != null)
                    {
                        ltrMainOrderID.Text += "Chi tiết đơn hàng ký gửi #" + t.ID + "";

                        #region Tổng quan
                        ltrOverView.Text += "<div class=\"col s12 m6\">";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Trạng thái đơn hàng: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\">" + PJUtils.GeneralTransportationOrderNewStatus(Convert.ToInt32(t.Status)) + "</div>";
                        ltrOverView.Text += "</div>";

                       
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Tên hàng hóa: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + t.ProductName + "</span></div>";
                        ltrOverView.Text += "</div>";

                       
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Giá trị đơn hàng: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(t.PriceVND)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";

                     
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Cân nặng: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + t.Weight + " Kg</span></div>";
                        ltrOverView.Text += "</div>";

                       
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Tiền bảo hiểm: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(t.InsurrancePrice)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";

                       
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Cước vật tư: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(t.SensorFeeeVND)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";

                      
                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phụ phí đặc biệt: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + string.Format("{0:N0}", Convert.ToDouble(t.AdditionFeeVND)) + " VNĐ</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "</div>";


                        ltrOverView.Text += "<div class=\"col s12 m6\">";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Kho TQ: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + WarehouseFromController.GetByID(Convert.ToInt32(t.WareHouseFromID)).WareHouseName + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Kho nhận: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + WarehouseController.GetByID(Convert.ToInt32(t.WareHouseID)).WareHouseName + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Phương thức vận chuyển: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + ShippingTypeToWareHouseController.GetByID(Convert.ToInt32(t.ShippingTypeID)).ShippingTypeName + "</span></div>";
                        ltrOverView.Text += "</div>";


                        string dateexre = "";
                        string dateout = "";
                        if (t.DateExportRequest != null)
                            dateexre = string.Format("{0:dd/MM/yyyy hh:mm}", t.DateExportRequest);
                        if (t.DateExport != null)
                            dateout = string.Format("{0:dd/MM/yyyy hh:mm}", t.DateExport);

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Ngày yêu cầu xuất kho: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + dateexre + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Ngày xuất kho: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + dateout + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Bảo hiểm: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\">";
                        if (t.IsInsurrance == true)
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons green-text left\">check_circle</i></span></div>";
                        else
                            ltrOverView.Text += "<span class=\"bold\"><i class=\"material-icons red-text left\">clear</i></span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "<div class=\"order-row\">";
                        ltrOverView.Text += "<div class=\"left-fixed\"><span class=\"lb\">Ghi chú đơn hàng: </span></div>";
                        ltrOverView.Text += "<div class=\"right-content\"><span class=\"bold\">" + t.Note + "</span></div>";
                        ltrOverView.Text += "</div>";

                        ltrOverView.Text += "</div>";
                        #endregion

                        #region lấy tất cả kiện
                        var smallpackages = SmallPackageController.GetByTransportationOrderID(t.ID);
                        if (smallpackages.Count > 0)
                        {
                            foreach (var s in smallpackages)
                            {
                                ltrSmallPackages.Text += "<tr class=\"slide-up\">";
                                ltrSmallPackages.Text += "<td>" + s.OrderTransactionCode + "</td>";
                                ltrSmallPackages.Text += "<td>" + Math.Round(Convert.ToDouble(s.Weight), 1) + "</td>";
                                ltrSmallPackages.Text += "<td>" + s.Length + "</td>";
                                ltrSmallPackages.Text += "<td>" + s.Width + "</td>";
                                ltrSmallPackages.Text += "<td>" + s.Height + "</td>";
                                ltrSmallPackages.Text += "<td>" + PJUtils.IntToStringStatusSmallPackageWithBGNew(Convert.ToInt32(s.Status)) + "</td>";
                                ltrSmallPackages.Text += "</tr>";
                            }
                        }
                        #endregion
                    }
                }    
            }    
        }
    }
}