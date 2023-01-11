using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class danh_sach_kien_yeu_cau_ky_gui : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                LoadData();
                LoadShippingTypeVN();
            }
        }

        public void LoadShippingTypeVN()
        {
            string html = "";
            var s = ShippingTypeVNController.GetAllWithIsHidden("", false);
            if (s.Count > 0)
            {
                foreach (var item in s)
                {
                    html += item.ID + ":" + item.ShippingTypeVNName + "|";
                }
            }
            hdfListShippingVN.Value = html;
        }

        public void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int UID = obj_user.ID;

                string code = Request.QueryString["code"];
                int type = Request.QueryString["type"].ToInt(3);
                int status = Request.QueryString["stt"].ToInt(-1);
                int phantrang = Request.QueryString["phantrang"].ToInt(-1);
                string fd = Request.QueryString["fd"];
                string td = Request.QueryString["td"];

                if (Request.QueryString["stt"] != null)
                    ddlStatus.SelectedValue = status.ToString();
                if (Request.QueryString["phantrang"] != null)
                    ddlPhanTrang.SelectedValue = phantrang.ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
                {
                    FD.Text = fd;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                    TD.Text = td;

                if (!string.IsNullOrEmpty(code))
                    txtOrderCode.Text = code;
                ddlType.SelectedValue = type.ToString();

                int page = 0;

                Int32 Page = GetIntFromQueryString("Page");
                if (Page > 0)
                {
                    page = Page - 1;
                }

                var os = TransportationOrderNewController.GetAllByUIDWithFilter_SqlHelperWithPage(UID, code, type, status, fd, td, page, phantrang);
                List<tbl_TransportationOrderNew> oAll = new List<tbl_TransportationOrderNew>();
                if (os.Count > 0)
                {
                    //var os0 = os.Where(o => o.Status == 0).ToList();
                    //var os1 = os.Where(o => o.Status == 1).ToList();
                    //var os2 = os.Where(o => o.Status == 2).ToList();
                    //var os3 = os.Where(o => o.Status == 3).ToList();
                    //var os4 = os.Where(o => o.Status == 4).ToList();
                    //var os5 = os.Where(o => o.Status == 5).ToList();
                    //var os6 = os.Where(o => o.Status == 6).ToList();
                    //if (os3.Count > 0)
                    //{
                    //    foreach (var o in os3)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os1.Count > 0)
                    //{
                    //    foreach (var o in os1)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os2.Count > 0)
                    //{
                    //    foreach (var o in os2)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os4.Count > 0)
                    //{
                    //    foreach (var o in os4)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os5.Count > 0)
                    //{
                    //    foreach (var o in os5)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os6.Count > 0)
                    //{
                    //    foreach (var o in os6)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}
                    //if (os0.Count > 0)
                    //{
                    //    foreach (var o in os0)
                    //    {
                    //        oAll.Add(o);
                    //    }
                    //}

                    int total = TransportationOrderNewController.GetTotalUser(UID, code, type, status, fd, td);

                    pagingall(os, total);
                }

            }
        }


        #region Paging
        public void pagingall(List<tbl_TransportationOrderNew> acs, int total)
        {
            string username_current = Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username_current);
            if (obj_user != null)
            {
                int phantrang = Request.QueryString["phantrang"].ToInt(-1);
                if (Request.QueryString["phantrang"] != null)
                    ddlPhanTrang.SelectedValue = phantrang.ToString();
                int PageSize = 0;
                if (phantrang == -1)
                {
                    PageSize = 20;

                }
                else if (phantrang == 1)
                {
                    PageSize = 30;

                }
                else if (phantrang == 2)
                {
                    PageSize = 40;
                }
                else
                {
                    PageSize = 50;
                }
                if (total > 0)
                {
                    int TotalItems = total;
                    if (TotalItems % PageSize == 0)
                        PageCount = TotalItems / PageSize;
                    else
                        PageCount = TotalItems / PageSize + 1;

                    Int32 Page = GetIntFromQueryString("Page");

                    if (Page == -1) Page = 1;
                    int FromRow = (Page - 1) * PageSize;
                    int ToRow = Page * PageSize - 1;
                    if (ToRow >= TotalItems)
                        ToRow = TotalItems - 1;
                    StringBuilder html = new StringBuilder();

                    for (int i = 0; i < acs.Count; i++)
                    {
                        var item = acs[i];

                        string Insurrance = "Không chọn";
                        if (item.IsInsurrance == true)
                        {
                            Insurrance = "Có";
                        }
                        string ngayycxk = "";
                        string ngayxk = "";
                        if (item.DateExportRequest != null)
                        {
                            ngayycxk = string.Format("{0:dd/MM/yyyy}", item.DateExportRequest);
                        }
                        if (item.DateExport != null)
                        {
                            ngayxk = string.Format("{0:dd/MM/yyyy}", item.DateExport);
                        }
                        string shippintType = "";
                        if (item.ShippingTypeVN != null)
                        {
                            var sht = ShippingTypeVNController.GetByID(item.ShippingTypeVN.ToString().ToInt(0));
                            if (sht != null)
                            {
                                shippintType = sht.ShippingTypeVNName;
                            }
                        }

                        ltr.Text += "<tr data-id=\"" + item.ID + "\">";
                        ltr.Text += "<td>";
                        if (item.Status == 4)
                        {
                            ltr.Text += "    <label><input type=\"checkbox\" onchange=\"selectdeposit()\" class=\"form-control chk-deposit\" data-id=\"" + item.ID + "\"><span></span></label>";
                        }
                        ltr.Text += "</td>";
                        ltr.Text += "<td>" + item.ID + "</td>";
                        ltr.Text += "<td>" + item.BarCode + "</td>";
                        ltr.Text += "<td>" + item.Weight + "</td>";

                        ltr.Text += "<td>";
                        ltr.Text += "<p class=\"s-txt red-text no-wrap\"><span class=\"total\">Bảo hiểm: </span><span>" + Insurrance + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Tiền bảo hiểm: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.InsurrancePrice)) + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Cước vật tư: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.SensorFeeeVND)) + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Phụ phí đặc biệt: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.AdditionFeeVND)) + "</span></p>";
                        ltr.Text += "</td>";
                        ltr.Text += "<td>";
                        ltr.Text += "<p class=\"s-txt red-text no-wrap\"><span class=\"total\">Tên hàng hóa: </span><span>" + item.ProductName + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Giá trị đơn hàng: </span><span>" + string.Format("{0:N0}", Convert.ToDouble(item.PriceVND)) + "</span></p>";
                        ltr.Text += "</td>";
                        ltr.Text += "<td>" + item.Note + "</td>";
                        ltr.Text += "<td>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Ngày tạo: </span><span>" + string.Format("{0:dd/MM/yyyy}", item.CreatedDate) + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Yêu cầu xuất kho: </span><span>" + ngayycxk + "</span></p>";
                        ltr.Text += "<p class=\"s-txt no-wrap\"><span class=\"total\">Ngày xuất kho: </span><span>" + ngayxk + "</span></p>";
                        ltr.Text += "</td>";
                        ltr.Text += "<td>" + shippintType + "</td>";
                        ltr.Text += "<td>" + PJUtils.GeneralTransportationOrderNewStatus(Convert.ToInt32(item.Status)) + "</td>";
                        ltr.Text += "<td>";
                        if (item.Status == 1)
                        {
                            ltr.Text += "<a href=\"javascript:;\" onclick=\"rejectOrder($(this))\" class=\"bg-black\" style=\"float:left;width:100%;margin-bottom:5px;\">Hủy đơn</a>";
                        }
                        ltr.Text += " <a href=\"/chi-tiet-ky-gui.aspx?id=" + item.ID + "\" data-position=\"top\" ><i class=\"material-icons\">remove_red_eye</i><span>Chi tiết</span></a>";
                        ltr.Text += "</td>";
                        ltr.Text += "</tr>";
                    }
                    // ltr.Text = html.ToString();
                }
            }
        }
        public static Int32 GetIntFromQueryString(String key)
        {
            Int32 returnValue = -1;
            String queryStringValue = HttpContext.Current.Request.QueryString[key];
            try
            {
                if (queryStringValue == null)
                    return returnValue;
                if (queryStringValue.IndexOf("#") > 0)
                    queryStringValue = queryStringValue.Substring(0, queryStringValue.IndexOf("#"));
                returnValue = Convert.ToInt32(queryStringValue);
            }
            catch
            { }
            return returnValue;
        }
        private int PageCount;
        protected void DisplayHtmlStringPaging1()
        {
            Int32 CurrentPage = Convert.ToInt32(Request.QueryString["Page"]);
            if (CurrentPage == -1) CurrentPage = 1;
            string[] strText = new string[4] { "Trang đầu", "Trang cuối", "Trang sau", "Trang trước" };
            if (PageCount > 1)
                Response.Write(GetHtmlPagingAdvanced(6, CurrentPage, PageCount, Context.Request.RawUrl, strText));
        }
        private static string GetPageUrl(int currentPage, string pageUrl)
        {
            pageUrl = Regex.Replace(pageUrl, "(\\?|\\&)*" + "Page=" + currentPage, "");
            if (pageUrl.IndexOf("?") > 0)
            {
                pageUrl += "&Page={0}";
            }
            else
            {
                pageUrl += "?Page={0}";
            }
            return pageUrl;
        }
        public static string GetHtmlPagingAdvanced(int pagesToOutput, int currentPage, int pageCount, string currentPageUrl, string[] strText)
        {
            //Nếu Số trang hiển thị là số lẻ thì tăng thêm 1 thành chẵn
            if (pagesToOutput % 2 != 0)
            {
                pagesToOutput++;
            }

            //Một nửa số trang để đầu ra, đây là số lượng hai bên.
            int pagesToOutputHalfed = pagesToOutput / 2;

            //Url của trang
            string pageUrl = GetPageUrl(currentPage, currentPageUrl);


            //Trang đầu tiên
            int startPageNumbersFrom = currentPage - pagesToOutputHalfed; ;

            //Trang cuối cùng
            int stopPageNumbersAt = currentPage + pagesToOutputHalfed; ;

            StringBuilder output = new StringBuilder();

            //Nối chuỗi phân trang
            //output.Append("<div class=\"paging\">");
            //output.Append("<ul class=\"paging_hand\">");

            //Link First(Trang đầu) và Previous(Trang trước)
            if (currentPage > 1)
            {
                //output.Append("<li class=\"UnselectedPrev \" ><a title=\"" + strText[0] + "\" href=\"" + string.Format(pageUrl, 1) + "\">|<</a></li>");
                //output.Append("<li class=\"UnselectedPrev\" ><a title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\"><i class=\"fa fa-angle-left\"></i></a></li>");
                output.Append("<a class=\"prev-page pagi-button\" title=\"" + strText[1] + "\" href=\"" + string.Format(pageUrl, currentPage - 1) + "\">Prev</a>");
                //output.Append("<span class=\"Unselect_prev\"><a href=\"" + string.Format(pageUrl, currentPage - 1) + "\"></a></span>");
            }

            /******************Xác định startPageNumbersFrom & stopPageNumbersAt**********************/
            if (startPageNumbersFrom < 1)
            {
                startPageNumbersFrom = 1;

                //As page numbers are starting at one, output an even number of pages.  
                stopPageNumbersAt = pagesToOutput;
            }

            if (stopPageNumbersAt > pageCount)
            {
                stopPageNumbersAt = pageCount;
            }

            if ((stopPageNumbersAt - startPageNumbersFrom) < pagesToOutput)
            {
                startPageNumbersFrom = stopPageNumbersAt - pagesToOutput;
                if (startPageNumbersFrom < 1)
                {
                    startPageNumbersFrom = 1;
                }
            }
            /******************End: Xác định startPageNumbersFrom & stopPageNumbersAt**********************/

            //Các dấu ... chỉ những trang phía trước  
            if (startPageNumbersFrom > 1)
            {
                output.Append("<a href=\"" + string.Format(GetPageUrl(currentPage - 1, pageUrl), startPageNumbersFrom - 1) + "\">&hellip;</a>");
            }

            //Duyệt vòng for hiển thị các trang
            for (int i = startPageNumbersFrom; i <= stopPageNumbersAt; i++)
            {
                if (currentPage == i)
                {
                    output.Append("<a class=\"pagi-button current-active\">" + i.ToString() + "</a>");
                }
                else
                {
                    output.Append("<a class=\"pagi-button\" href=\"" + string.Format(pageUrl, i) + "\">" + i.ToString() + "</a>");
                }
            }

            //Các dấu ... chỉ những trang tiếp theo  
            if (stopPageNumbersAt < pageCount)
            {
                output.Append("<a href=\"" + string.Format(pageUrl, stopPageNumbersAt + 1) + "\">&hellip;</a>");
            }

            //Link Next(Trang tiếp) và Last(Trang cuối)
            if (currentPage != pageCount)
            {
                //output.Append("<span class=\"Unselect_next\"><a href=\"" + string.Format(pageUrl, currentPage + 1) + "\"></a></span>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\"><i class=\"fa fa-angle-right\"></i></a></li>");
                output.Append("<a class=\"next-page pagi-button\" title=\"" + strText[2] + "\" href=\"" + string.Format(pageUrl, currentPage + 1) + "\">Next</a>");
                //output.Append("<li class=\"UnselectedNext\" ><a title=\"" + strText[3] + "\" href=\"" + string.Format(pageUrl, pageCount) + "\">>|</a></li>");
            }
            //output.Append("</ul>");
            //output.Append("</div>");
            return output.ToString();
        }
        #endregion


        protected void btnSear_Click(object sender, EventArgs e)
        {
            string ordercode = txtOrderCode.Text;
            string type = ddlType.SelectedValue;
            string status = ddlStatus.SelectedValue;
            string phantrang = ddlPhanTrang.SelectedValue;
            string fd = "";
            string td = "";
            if (!string.IsNullOrEmpty(FD.Text))
            {
                fd = FD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(TD.Text))
            {
                td = TD.Text.ToString();
            }


            if (!string.IsNullOrEmpty(fd) && !string.IsNullOrEmpty(td))
            {
                if (Convert.ToDateTime(fd) > Convert.ToDateTime(td))
                {
                    PJUtils.ShowMessageBoxSwAlert("Từ ngày phải trước đến ngày", "e", false, Page);
                }
                else
                {
                    Response.Redirect("/danh-sach-kien-yeu-cau-ky-gui?stt=" + status + "&fd=" + fd + "&phantrang=" + phantrang + "&td=" + td + "&code=" + ordercode + "&type=" + type + "");
                }
            }
            else
            {
                Response.Redirect("/danh-sach-kien-yeu-cau-ky-gui?stt=" + status + "&fd=" + fd + "&phantrang=" + phantrang + "&td=" + td + "&code=" + ordercode + "&type=" + type + "");
            }
        }

        #region webservice
        [WebMethod]
        public static string rejectOrder(int id, string cancelnote)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    int UID = acc.ID;
                    var t = TransportationOrderNewController.GetByIDAndUID(id, UID);
                    if (t != null)
                    {
                        TransportationOrderNewController.UpdateCancelOrder(id, 0, cancelnote, DateTime.Now, username);
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        [WebMethod]
        public static string exportAll()
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    int UID = acc.ID;
                    double wallet = Convert.ToDouble(acc.Wallet);

                    var ts = TransportationOrderNewController.GetByUIDAndStatus(UID, 4);
                    if (ts.Count > 0)
                    {
                        double feeOutStockCYN = 0;
                        double feeOutStockVND = 0;
                        double FeeInsurrance = 0;
                        double totalWeight = 0;
                        double currency = 0;
                        double TotalAdditionFeeCYN = 0;
                        double TotalAdditionFeeVND = 0;
                        double TotalSensoredFeeCYN = 0;
                        double TotalSensoredFeeVND = 0;
                        double totalWeightPriceVND = 0;
                        double totalWeightPriceCYN = 0;
                        double totalPriceVND = 0;
                        double totalPriceCYN = 0;
                        double totalCount = 0;
                        string listID = "";

                        var config = ConfigurationController.GetByTop1();
                        if (config != null)
                        {
                            currency = Convert.ToDouble(config.AgentCurrency);
                            feeOutStockCYN = Convert.ToDouble(config.PriceCheckOutWareDefault);
                            feeOutStockVND = feeOutStockCYN * currency;
                        }

                        int KhoChina = 0;
                        int KhoVN = 0;
                        int PTVC = 0;
                        List<WareHouse> lw = new List<WareHouse>();

                        foreach (var t in ts)
                        {                          
                            KhoChina = t.WareHouseFromID.Value;
                            KhoVN = t.WareHouseID.Value;
                            PTVC = t.ShippingTypeID.Value;

                            var small = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                            if (small != null)
                            {
                                if (small.Status == 3)
                                {
                                    var check = RequestOutStockController.GetBySmallpackageID(small.ID);
                                    if (check == null)
                                    {
                                        WareHouse w1 = new WareHouse();

                                        w1.WareHouseFromID = t.WareHouseFromID.Value;
                                        w1.WareHouseID = t.WareHouseID.Value;
                                        w1.ShippingTypeID = t.ShippingTypeID.Value;

                                        totalCount++;

                                        if (t.SmallPackageID != null)
                                        {
                                            if (t.SmallPackageID > 0)
                                            {
                                                List<Package> lp = new List<Package>();
                                                var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                if (package != null)
                                                {
                                                    double weight = 0;
                                                    if (package.Weight != null)
                                                    {
                                                        if (package.Weight > 0)
                                                        {
                                                            Package p = new Package();
                                                            weight = Math.Round(Convert.ToDouble(package.Weight), 2);
                                                            totalWeight += weight;
                                                            w1.TotalWeight = weight;
                                                            p.Weight = weight;
                                                            p.TransportationID = t.ID;
                                                            lp.Add(p);
                                                        }
                                                    }
                                                }
                                                w1.ListPackage = lp;
                                                lw.Add(w1);
                                            }
                                        }
                                    }
                                }
                            }                            

                            listID += t.ID + "|";

                            double addfeeVND = 0;
                            double addfeeCYN = 0;
                            double sensorVND = 0;
                            double sensorCYN = 0;
                            double TienBaoHiem = 0;

                            if (!string.IsNullOrEmpty(t.AdditionFeeVND))
                                if (t.AdditionFeeVND.ToFloat(0) > 0)
                                    addfeeVND = Convert.ToDouble(t.AdditionFeeVND);

                            if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
                                if (t.AdditionFeeCYN.ToFloat(0) > 0)
                                    addfeeCYN = Convert.ToDouble(t.AdditionFeeCYN);

                            if (!string.IsNullOrEmpty(t.SensorFeeeVND))
                                if (t.SensorFeeeVND.ToFloat(0) > 0)
                                    sensorVND = Convert.ToDouble(t.SensorFeeeVND);

                            if (!string.IsNullOrEmpty(t.SensorFeeCYN))
                                if (t.SensorFeeCYN.ToFloat(0) > 0)
                                    sensorCYN = Convert.ToDouble(t.SensorFeeCYN);

                            if (!string.IsNullOrEmpty(t.InsurrancePrice))
                                if (t.InsurrancePrice.ToFloat(0) > 0)
                                    TienBaoHiem = Convert.ToDouble(t.InsurrancePrice);

                            TotalAdditionFeeCYN += addfeeCYN;
                            TotalAdditionFeeVND += addfeeVND;

                            TotalSensoredFeeVND += sensorVND;
                            TotalSensoredFeeCYN += sensorCYN;

                            FeeInsurrance += TienBaoHiem;
                        }

                        bool CheckIsDifference = true;
                        if (lw.Count > 0)
                        {
                            foreach (var item in lw)
                            {
                                if (item.WareHouseID != KhoVN || item.WareHouseFromID != KhoChina || item.ShippingTypeID != PTVC)
                                {
                                    CheckIsDifference = false;
                                    break;
                                }
                            }
                        }

                        string ret = "";
                        if (CheckIsDifference == false)
                        {
                            ret = 3 + ":";
                        }
                        else
                        {
                            double TotalFeeVND = 0;
                            if (acc.FeeTQVNPerWeight.ToFloat(0) > 0)
                            {
                                TotalFeeVND = Convert.ToDouble(acc.FeeTQVNPerWeight) * totalWeight;
                                totalWeightPriceVND = TotalFeeVND;
                            }
                            else
                            {
                                if (lw.Count > 0)
                                {
                                    var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(KhoChina, KhoVN, PTVC, true);
                                    if (fee.Count > 0)
                                    {
                                        foreach (var f in fee)
                                        {
                                            if (totalWeight >= f.WeightFrom && totalWeight <= f.WeightTo)
                                            {
                                                TotalFeeVND = Convert.ToDouble(f.Price) * totalWeight;
                                                totalWeightPriceVND = TotalFeeVND;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            totalPriceVND = Math.Round(totalWeightPriceVND + feeOutStockVND + TotalSensoredFeeVND + TotalAdditionFeeVND + FeeInsurrance, 0);
                            totalPriceCYN = Math.Round(totalPriceVND / currency, 2);
                           
                            if (wallet >= totalPriceVND)
                            {
                                ret = 1 + ":" + wallet + ":" + totalCount + ":"
                                          + totalWeight + ":"
                                          + totalWeightPriceCYN + ":"
                                          + totalWeightPriceVND + ":"
                                          + feeOutStockCYN + ":" + feeOutStockVND + ":"
                                          + totalPriceCYN + ":"
                                          + totalPriceVND + ":"
                                          + listID + ":"
                                          + "0" + ":"
                                          + TotalAdditionFeeCYN + ":"
                                          + TotalAdditionFeeVND + ":" + TotalSensoredFeeCYN + ":"
                                          + TotalSensoredFeeVND + ":" + FeeInsurrance;

                            }
                            else
                            {
                                double leftmoney = totalPriceVND - wallet;
                                if (leftmoney > 0)
                                {
                                    ret = 0 + ":" + wallet + ":" + totalCount + ":"
                                             + totalWeight + ":"
                                             + totalWeightPriceCYN + ":"
                                             + totalWeightPriceVND + ":"
                                             + feeOutStockCYN + ":" + feeOutStockVND + ":"
                                             + totalPriceCYN + ":"
                                             + totalPriceVND + ":"
                                             + listID + ":"
                                             + leftmoney + ":"
                                             + TotalAdditionFeeCYN + ":"
                                             + TotalAdditionFeeVND + ":" + ":" + TotalSensoredFeeCYN + ":"
                                             + TotalSensoredFeeVND + ":" + FeeInsurrance;
                                }
                            }
                        }
                        return ret;
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        [WebMethod]
        public static string exportSelectedAll(string listID)
        {
            if (HttpContext.Current.Session["userLoginSystem"] != null)
            {
                string username = HttpContext.Current.Session["userLoginSystem"].ToString();
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    int UID = acc.ID;
                    double wallet = Convert.ToDouble(acc.Wallet);
                    double feeOutStockCYN = 0;
                    double feeOutStockVND = 0;
                    double FeeInsurrance = 0;
                    double totalWeight = 0;
                    double currency = 0;
                    double TotalAdditionFeeCYN = 0;
                    double TotalAdditionFeeVND = 0;
                    double TotalSensoredFeeCYN = 0;
                    double TotalSensoredFeeVND = 0;
                    double totalWeightPriceVND = 0;
                    double totalWeightPriceCYN = 0;
                    double totalPriceVND = 0;
                    double totalPriceCYN = 0;
                    double totalCount = 0;

                    var config = ConfigurationController.GetByTop1();
                    if (config != null)
                    {
                        currency = Convert.ToDouble(config.AgentCurrency);
                        feeOutStockCYN = Convert.ToDouble(config.PriceCheckOutWareDefault);
                        feeOutStockVND = feeOutStockCYN * currency;
                    }

                    int KhoChina = 0;
                    int KhoVN = 0;
                    int PTVC = 0;
                    List<WareHouse> lw = new List<WareHouse>();
                    string[] IDs = listID.Split('|');
                    if (IDs.Length - 1 > 0)
                    {
                        for (int i = 0; i < IDs.Length - 1; i++)
                        {
                            int ID = IDs[i].ToInt(0);
                            var t = TransportationOrderNewController.GetByID(ID);
                            if (t != null)
                            {                             
                                KhoChina = t.WareHouseFromID.Value;
                                KhoVN = t.WareHouseID.Value;
                                PTVC = t.ShippingTypeID.Value;

                                var small = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                if (small != null)
                                {
                                    if (small.Status == 3)
                                    {
                                        var check = RequestOutStockController.GetBySmallpackageID(small.ID);
                                        if (check == null)
                                        {
                                            WareHouse w1 = new WareHouse();

                                            w1.WareHouseFromID = t.WareHouseFromID.Value;
                                            w1.WareHouseID = t.WareHouseID.Value;
                                            w1.ShippingTypeID = t.ShippingTypeID.Value;

                                            totalCount++;

                                            if (t.SmallPackageID != null)
                                            {
                                                if (t.SmallPackageID > 0)
                                                {
                                                    List<Package> lp = new List<Package>();
                                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                    if (package != null)
                                                    {
                                                        double weight = 0;
                                                        if (package.Weight != null)
                                                        {
                                                            if (package.Weight > 0)
                                                            {
                                                                Package p = new Package();
                                                                weight = Math.Round(Convert.ToDouble(package.Weight), 2);
                                                                totalWeight += weight;
                                                                w1.TotalWeight = weight;
                                                                p.Weight = weight;
                                                                p.TransportationID = t.ID;
                                                                lp.Add(p);
                                                            }
                                                        }
                                                    }
                                                    w1.ListPackage = lp;
                                                    lw.Add(w1);
                                                }
                                            }
                                        }
                                    }
                                }

                                double addfeeVND = 0;
                                double addfeeCYN = 0;
                                double sensorVND = 0;
                                double sensorCYN = 0;
                                double TienBaoHiem = 0;

                                if (!string.IsNullOrEmpty(t.AdditionFeeVND))
                                    if (t.AdditionFeeVND.ToFloat(0) > 0)
                                        addfeeVND = Convert.ToDouble(t.AdditionFeeVND);

                                if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
                                    if (t.AdditionFeeCYN.ToFloat(0) > 0)
                                        addfeeCYN = Convert.ToDouble(t.AdditionFeeCYN);

                                if (!string.IsNullOrEmpty(t.SensorFeeeVND))
                                    if (t.SensorFeeeVND.ToFloat(0) > 0)
                                        sensorVND = Convert.ToDouble(t.SensorFeeeVND);

                                if (!string.IsNullOrEmpty(t.SensorFeeCYN))
                                    if (t.SensorFeeCYN.ToFloat(0) > 0)
                                        sensorCYN = Convert.ToDouble(t.SensorFeeCYN);

                                if (!string.IsNullOrEmpty(t.InsurrancePrice))
                                    if (t.InsurrancePrice.ToFloat(0) > 0)
                                        TienBaoHiem = Convert.ToDouble(t.InsurrancePrice);

                                TotalAdditionFeeCYN += addfeeCYN;
                                TotalAdditionFeeVND += addfeeVND;

                                TotalSensoredFeeVND += sensorVND;
                                TotalSensoredFeeCYN += sensorCYN;

                                FeeInsurrance += TienBaoHiem;
                            }
                        }
                    }

                    bool CheckIsDifference = true;
                    if (lw.Count > 0)
                    {
                        foreach (var item in lw)
                        {
                            if (item.WareHouseID != KhoVN || item.WareHouseFromID != KhoChina || item.ShippingTypeID != PTVC)
                            {
                                CheckIsDifference = false;
                                break;
                            }
                        }
                    }

                    string ret = "";
                    if (CheckIsDifference == false)
                    {
                        ret = 3 + ":";
                    }
                    else
                    {
                        double TotalFeeVND = 0;
                        if (acc.FeeTQVNPerWeight.ToFloat(0) > 0)
                        {
                            TotalFeeVND = Convert.ToDouble(acc.FeeTQVNPerWeight) * totalWeight;
                            totalWeightPriceVND = TotalFeeVND;
                        }
                        else
                        {
                            if (lw.Count > 0)
                            {
                                var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(KhoChina, KhoVN, PTVC, true);
                                if (fee.Count > 0)
                                {
                                    foreach (var f in fee)
                                    {
                                        if (totalWeight >= f.WeightFrom && totalWeight <= f.WeightTo)
                                        {
                                            TotalFeeVND = Convert.ToDouble(f.Price) * totalWeight;
                                            totalWeightPriceVND = TotalFeeVND;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        totalPriceVND = Math.Round(totalWeightPriceVND + feeOutStockVND + TotalSensoredFeeVND + TotalAdditionFeeVND + FeeInsurrance, 0);
                        totalPriceCYN = Math.Round(totalPriceVND / currency, 2);
                       
                        if (wallet >= totalPriceVND)
                        {
                            ret = 1 + ":" + wallet + ":" + totalCount + ":"
                                         + totalWeight + ":"
                                         + totalWeightPriceCYN + ":"
                                         + totalWeightPriceVND + ":"
                                         + feeOutStockCYN + ":" + feeOutStockVND + ":"
                                         + totalPriceCYN + ":"
                                         + totalPriceVND + ":"
                                         + listID + ":" + TotalSensoredFeeVND + ":" + TotalAdditionFeeVND + ":" + FeeInsurrance;
                        }
                        else
                        {
                            double leftmoney = totalPriceVND - wallet;
                            if (leftmoney > 0)
                            {
                                ret = 0 + ":" + wallet + ":" + totalCount + ":"
                                        + totalWeight + ":"
                                        + totalWeightPriceCYN + ":"
                                        + totalWeightPriceVND + ":"
                                        + feeOutStockCYN + ":" + feeOutStockVND + ":"
                                        + totalPriceCYN + ":"
                                        + totalPriceVND + ":"
                                        + listID + ":" + TotalSensoredFeeVND + ":" + TotalAdditionFeeVND + ":"
                                        + leftmoney + ":" + FeeInsurrance;
                            }
                        }
                    }    
                    return ret;
                }
                else
                {
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }
        #endregion

        protected void btnPayExport_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int UID = acc.ID;
                DateTime currentDate = DateTime.Now;
                double wallet = Convert.ToDouble(acc.Wallet);
                string strListID = hdfListID.Value;               
                if (!string.IsNullOrEmpty(strListID))
                {
                    string[] listID = strListID.Split('|');
                    if (listID.Length - 1 > 0)
                    {
                        double feeOutStockCYN = 0;
                        double feeOutStockVND = 0;      
                        double totalWeight = 0;
                        double currency = 0;
                        double FeeInsurrance = 0;
                        double TotalAdditionFeeCYN = 0;
                        double TotalAdditionFeeVND = 0;
                        double TotalSensoredFeeCYN = 0;
                        double TotalSensoredFeeVND = 0;
                        double totalWeightPriceVND = 0;   
                        double totalPriceVND = 0;
                        double totalPriceCYN = 0;

                        var config = ConfigurationController.GetByTop1();
                        if (config != null)
                        {
                            currency = Convert.ToDouble(config.AgentCurrency);
                            feeOutStockCYN = Convert.ToDouble(config.PriceCheckOutWareDefault);
                            feeOutStockVND = feeOutStockCYN * currency;
                        }

                        int KhoChina = 0;
                        int KhoVN = 0;
                        int PTVC = 0;
                        List<WareHouse> lw = new List<WareHouse>();
                        for (int i = 0; i < listID.Length - 1; i++)
                        {
                            int ID = listID[i].ToInt(0);
                            var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
                            if (t != null)
                            {
                                KhoChina = t.WareHouseFromID.Value;
                                KhoVN = t.WareHouseID.Value;
                                PTVC = t.ShippingTypeID.Value;

                                var small = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                if (small != null)
                                {
                                    if (small.Status == 3)
                                    {
                                        var check = RequestOutStockController.GetBySmallpackageID(small.ID);
                                        if (check == null)
                                        {
                                            WareHouse w1 = new WareHouse();

                                            w1.WareHouseFromID = t.WareHouseFromID.Value;
                                            w1.WareHouseID = t.WareHouseID.Value;
                                            w1.ShippingTypeID = t.ShippingTypeID.Value;

                                            if (t.SmallPackageID != null)
                                            {
                                                if (t.SmallPackageID > 0)
                                                {
                                                    List<Package> lp = new List<Package>();
                                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                    if (package != null)
                                                    {
                                                        double weight = 0;
                                                        if (package.Weight != null)
                                                        {
                                                            if (package.Weight > 0)
                                                            {
                                                                Package p = new Package();
                                                                weight = Math.Round(Convert.ToDouble(package.Weight), 2);
                                                                totalWeight += weight;
                                                                w1.TotalWeight = weight;
                                                                p.Weight = weight;
                                                                p.TransportationID = t.ID;
                                                                lp.Add(p);
                                                            }
                                                        }
                                                    }
                                                    w1.ListPackage = lp;
                                                    lw.Add(w1);                                                   
                                                }
                                            }
                                        }
                                    }
                                }

                                double addfeeVND = 0;
                                double addfeeCYN = 0;
                                double sensorVND = 0;
                                double sensorCYN = 0;
                                double TienBaoHiem = 0;

                                if (!string.IsNullOrEmpty(t.AdditionFeeVND))
                                    if (t.AdditionFeeVND.ToFloat(0) > 0)
                                        addfeeVND = Convert.ToDouble(t.AdditionFeeVND);

                                if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
                                    if (t.AdditionFeeCYN.ToFloat(0) > 0)
                                        addfeeCYN = Convert.ToDouble(t.AdditionFeeCYN);

                                if (!string.IsNullOrEmpty(t.SensorFeeeVND))
                                    if (t.SensorFeeeVND.ToFloat(0) > 0)
                                        sensorVND = Convert.ToDouble(t.SensorFeeeVND);

                                if (!string.IsNullOrEmpty(t.SensorFeeCYN))
                                    if (t.SensorFeeCYN.ToFloat(0) > 0)
                                        sensorCYN = Convert.ToDouble(t.SensorFeeCYN);

                                if (!string.IsNullOrEmpty(t.InsurrancePrice))
                                    if (t.InsurrancePrice.ToFloat(0) > 0)
                                        TienBaoHiem = Convert.ToDouble(t.InsurrancePrice);

                                TotalAdditionFeeCYN += addfeeCYN;
                                TotalAdditionFeeVND += addfeeVND;

                                TotalSensoredFeeVND += sensorVND;
                                TotalSensoredFeeCYN += sensorCYN;

                                FeeInsurrance += TienBaoHiem;
                            }
                        }


                        bool CheckIsDifference = true;
                        if (lw.Count > 0)
                        {
                            foreach (var item in lw)
                            {
                                if (item.WareHouseID != KhoVN || item.WareHouseFromID != KhoChina || item.ShippingTypeID != PTVC)
                                {
                                    CheckIsDifference = false;
                                    break;
                                }
                            }
                        }

                        if (CheckIsDifference == true)
                        {
                            double TotalFeeVND = 0;
                            if (acc.FeeTQVNPerWeight.ToFloat(0) > 0)
                            {
                                TotalFeeVND = Convert.ToDouble(acc.FeeTQVNPerWeight) * totalWeight;
                                totalWeightPriceVND = TotalFeeVND;
                            }
                            else
                            {
                                if (lw.Count > 0)
                                {
                                    var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(KhoChina, KhoVN, PTVC, true);
                                    if (fee.Count > 0)
                                    {
                                        foreach (var f in fee)
                                        {
                                            if (totalWeight >= f.WeightFrom && totalWeight <= f.WeightTo)
                                            {
                                                TotalFeeVND = Convert.ToDouble(f.Price) * totalWeight;
                                                totalWeightPriceVND = TotalFeeVND;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            totalPriceVND = Math.Round(totalWeightPriceVND + feeOutStockVND + TotalSensoredFeeVND + TotalAdditionFeeVND + FeeInsurrance, 0);
                            totalPriceCYN = Math.Round(totalPriceVND / currency, 2);

                            if (lw.Count > 0)
                            {
                                if (wallet >= totalPriceVND)
                                {
                                    //Lưu xuống 1 lượt yêu cầu xuất kho
                                    #region Tạo lượt xuất kho
                                    string note = hdfNote.Value;
                                    int shippingtype = hdfShippingType.Value.ToInt(0);
                                    int totalpackage = listID.Length - 1;
                                    string kq = ExportRequestTurnController.InsertWithUID(UID, username, 0, currentDate, totalPriceVND,
                                    totalPriceCYN, totalWeight, note, shippingtype, currentDate, username, totalpackage, 2, FeeInsurrance);
                                    int eID = kq.ToInt(0);
                                    for (int i = 0; i < listID.Length; i++)
                                    {
                                        int ID = listID[i].ToInt(0);
                                        var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
                                        if (t != null)
                                        {
                                            if (t.SmallPackageID != null)
                                            {
                                                if (t.SmallPackageID > 0)
                                                {
                                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                    if (package != null)
                                                    {
                                                        if (package.Status == 3)
                                                        {
                                                            var check = RequestOutStockController.GetBySmallpackageID(package.ID);
                                                            if (check == null)
                                                            {
                                                                RequestOutStockController.InsertT(package.ID, package.OrderTransactionCode, t.ID, Convert.ToInt32(package.MainOrderID), 1, DateTime.Now, username, eID);
                                                                //Update lại trạng thái từng đơn và ngày ship
                                                                TransportationOrderNewController.UpdateRequestOutStock(t.ID, 5, note, currentDate, shippingtype);
                                                            }
                                                        }
                                                    }
                                                }
                                            }                                           
                                        }
                                    }
                                    #endregion
                                    #region Trừ tiền xuất kho
                                    double walletLeft = wallet - totalPriceVND;
                                    AccountController.updateWallet(UID, walletLeft, currentDate, username);
                                    HistoryPayWalletController.Insert(UID, username, 0, totalPriceVND,
                                    username + " đã thanh toán đơn hàng vận chuyển hộ.", walletLeft, 1, 8, currentDate, username);
                                    #endregion
                                    #region Gửi thông báo
                                    //var admins = AccountController.GetAllByRoleID(0);
                                    //if (admins.Count > 0)
                                    //{
                                    //    foreach (var admin in admins)
                                    //    {
                                    //        NotificationsController.Inser(admin.ID, admin.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
                                    //        string strPathAndQuery = Request.Url.PathAndQuery;
                                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //        string datalink = "" + strUrl + "manager/OutStock/";
                                    //        PJUtils.PushNotiDesktop(admin.ID, "Có yêu cầu xuất kho của user: " + username, datalink);
                                    //    }
                                    //}

                                    //var managers = AccountController.GetAllByRoleID(2);
                                    //if (managers.Count > 0)
                                    //{
                                    //    foreach (var manager in managers)
                                    //    {
                                    //        NotificationsController.Inser(manager.ID, manager.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
                                    //        string strPathAndQuery = Request.Url.PathAndQuery;
                                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //        string datalink = "" + strUrl + "manager/OutStock/";
                                    //        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng của " + username + " đã yêu cầu xuất kho.", datalink);
                                    //    }
                                    //}
                                    //var khoVNs = AccountController.GetAllByRoleID(5);
                                    //if (khoVNs.Count > 0)
                                    //{
                                    //    foreach (var khoVN in khoVNs)
                                    //    {
                                    //        NotificationsController.Inser(khoVN.ID, khoVN.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
                                    //        string strPathAndQuery = Request.Url.PathAndQuery;
                                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                    //        string datalink = "" + strUrl + "manager/OutStock/";
                                    //        PJUtils.PushNotiDesktop(khoVN.ID, "Đơn hàng của " + username + " đã yêu cầu xuất kho.", datalink);
                                    //    }
                                    //}
                                    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                                    //hubContext.Clients.All.addNewMessageToPage("", "");
                                    #endregion
                                    PJUtils.ShowMessageBoxSwAlert("Bạn đã gửi yêu cầu xuất kho thành công. Xin chân thành cảm ơn", "s", true, Page);
                                }
                                else
                                {
                                    PJUtils.ShowMessageBoxSwAlert("Số tiền của bạn không đủ để thanh toán phiên xuất kho, vui lòng thử lại sau.", "e", true, Page);
                                }
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Không có kiện nào để gửi yêu cầu. Vui lòng kiểm tra lại hoặc liên hệ nhân viên", "e", true, Page);
                            }
                        }
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Các kiện hàng không cùng kho, nên không thể tạo yêu cầu xuất kho. Vui lòng liên hệ BQT GiangHuy", "e", true, Page);
                        }
                    }
                }
            }
        }

        //protected void btnPayExport_Click(object sender, EventArgs e)
        //{
        //    string username = Session["userLoginSystem"].ToString();
        //    var acc = AccountController.GetByUsername(username);
        //    if (acc != null)
        //    {

        //        int UID = acc.ID;
        //        DateTime currentDate = DateTime.Now;
        //        double wallet = Convert.ToDouble(acc.Wallet);
        //        string strListID = hdfListID.Value;
        //        if (!string.IsNullOrEmpty(strListID))
        //        {
        //            string[] listID = strListID.Split('|');
        //            if (listID.Length - 1 > 0)
        //            {
        //                double feeOutStockCYN = 0;
        //                double feeOutStockVND = 0;
        //                double feeWeightOutStock = 0;

        //                double totalWeight = 0;
        //                double currency = 0;

        //                double FeeInsurrance = 0;

        //                double TotalAdditionFeeCYN = 0;
        //                double TotalAdditionFeeVND = 0;

        //                double TotalSensoredFeeCYN = 0;
        //                double TotalSensoredFeeVND = 0;

        //                double totalWeightPriceVND = 0;
        //                double totalWeightPriceCYN = 0;

        //                double totalPriceVND = 0;
        //                double totalPriceCYN = 0;

        //                var config = ConfigurationController.GetByTop1();
        //                if (config != null)
        //                {
        //                    currency = Convert.ToDouble(config.AgentCurrency);
        //                    feeOutStockCYN = Convert.ToDouble(config.PriceCheckOutWareDefault);
        //                    feeOutStockVND = feeOutStockCYN * currency;
        //                }
        //                List<WareHouse> lw = new List<WareHouse>();

        //                List<int> lID = new List<int>();
        //                for (int i = 0; i < listID.Length - 1; i++)
        //                {
        //                    int ID = listID[i].ToInt(0);
        //                    var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
        //                    if (t != null)
        //                    {

        //                        //Fix lỗi không đổi trạng thái
        //                        lID.Add(t.ID);
        //                        var checkwh = lw.Where(x => x.WareHouseID == t.WareHouseID && x.WareHouseFromID == t.WareHouseFromID && x.ShippingTypeID == t.ShippingTypeID).FirstOrDefault();
        //                        if (checkwh != null)
        //                        {
        //                            if (t.SmallPackageID != null)
        //                            {
        //                                if (t.SmallPackageID > 0)
        //                                {
        //                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
        //                                    if (package != null)
        //                                    {
        //                                        double weight = 0;
        //                                        if (package.Weight != null)
        //                                        {
        //                                            if (package.Weight > 0)
        //                                            {
        //                                                Package p = new Package();
        //                                                weight = Convert.ToDouble(package.Weight);
        //                                                totalWeight += weight;
        //                                                p.Weight = weight;
        //                                                p.TransportationID = t.ID;
        //                                                checkwh.TotalWeight += weight;
        //                                                checkwh.ListPackage.Add(p);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            WareHouse w = new WareHouse();

        //                            w.WareHouseFromID = t.WareHouseFromID.Value;
        //                            w.WareHouseID = t.WareHouseID.Value;
        //                            w.ShippingTypeID = t.ShippingTypeID.Value;
        //                            if (t.SmallPackageID != null)
        //                            {
        //                                if (t.SmallPackageID > 0)
        //                                {
        //                                    List<Package> lp = new List<Package>();
        //                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
        //                                    if (package != null)
        //                                    {
        //                                        double weight = 0;
        //                                        if (package.Weight != null)
        //                                        {
        //                                            if (package.Weight > 0)
        //                                            {
        //                                                Package p = new Package();
        //                                                weight = Convert.ToDouble(package.Weight);
        //                                                totalWeight += weight;
        //                                                w.TotalWeight = weight;
        //                                                p.Weight = weight;
        //                                                p.TransportationID = t.ID;
        //                                                lp.Add(p);
        //                                            }
        //                                        }
        //                                    }
        //                                    w.ListPackage = lp;
        //                                    lw.Add(w);
        //                                }
        //                            }
        //                        }

        //                        double addfeeVND = 0;
        //                        double addfeeCYN = 0;
        //                        double sensorVND = 0;
        //                        double sensorCYN = 0;
        //                        double TienBaoHiem = 0;

        //                        if (!string.IsNullOrEmpty(t.AdditionFeeVND))
        //                            if (t.AdditionFeeVND.ToFloat(0) > 0)
        //                                addfeeVND = Convert.ToDouble(t.AdditionFeeVND);

        //                        if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
        //                            if (t.AdditionFeeCYN.ToFloat(0) > 0)
        //                                addfeeCYN = Convert.ToDouble(t.AdditionFeeCYN);

        //                        if (!string.IsNullOrEmpty(t.SensorFeeeVND))
        //                            if (t.SensorFeeeVND.ToFloat(0) > 0)
        //                                sensorVND = Convert.ToDouble(t.SensorFeeeVND);

        //                        if (!string.IsNullOrEmpty(t.SensorFeeCYN))
        //                            if (t.SensorFeeCYN.ToFloat(0) > 0)
        //                                sensorCYN = Convert.ToDouble(t.SensorFeeCYN);

        //                        if (!string.IsNullOrEmpty(t.InsurrancePrice))
        //                            if (t.InsurrancePrice.ToFloat(0) > 0)
        //                                TienBaoHiem = Convert.ToDouble(t.InsurrancePrice);

        //                        TotalAdditionFeeCYN += addfeeCYN;
        //                        TotalAdditionFeeVND += addfeeVND;

        //                        TotalSensoredFeeVND += sensorVND;
        //                        TotalSensoredFeeCYN += sensorCYN;

        //                        FeeInsurrance += TienBaoHiem;
        //                    }
        //                }
        //                double TotalFeeVND = 0;
        //                if (acc.FeeTQVNPerWeight.ToFloat(0) > 0)
        //                {
        //                    TotalFeeVND = Convert.ToDouble(acc.FeeTQVNPerWeight) * totalWeight;
        //                    totalWeightPriceVND += TotalFeeVND;
        //                }
        //                else
        //                {
        //                    if (lw.Count > 0)
        //                    {
        //                        foreach (var item in lw)
        //                        {
        //                            var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(
        //                    item.WareHouseFromID, item.WareHouseID, item.ShippingTypeID, true);
        //                            if (fee.Count > 0)
        //                            {
        //                                foreach (var f in fee)
        //                                {
        //                                    if (item.TotalWeight >= f.WeightFrom && item.TotalWeight <= f.WeightTo)
        //                                    {
        //                                        TotalFeeVND = Convert.ToDouble(f.Price) * item.TotalWeight;
        //                                        totalWeightPriceVND += TotalFeeVND;
        //                                        break;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                totalPriceVND = totalWeightPriceVND + feeOutStockVND + TotalSensoredFeeVND + TotalAdditionFeeVND + FeeInsurrance;
        //                totalPriceCYN = totalWeightPriceCYN + feeOutStockCYN + TotalSensoredFeeCYN + TotalAdditionFeeCYN;

        //                if (wallet >= totalPriceVND)
        //                {
        //                    //Lưu xuống 1 lượt yêu cầu xuất kho
        //                    #region Tạo lượt xuất kho

        //                    string note = hdfNote.Value;
        //                    int shippingtype = hdfShippingType.Value.ToInt(0);
        //                    int totalpackage = listID.Length - 1;
        //                    string kq = ExportRequestTurnController.InsertWithUID(UID, username, 0, currentDate, totalPriceVND,
        //                    totalPriceCYN, totalWeight, note, shippingtype, currentDate, username, totalpackage, 2, FeeInsurrance);

        //                    int eID = kq.ToInt(0);
        //                    //for (int i = 0; i < listID.Length - 1; i++)
        //                    //Fix lỗi không đổi trạng thái
        //                    for (int i = 0; i < lID.Count; i++)
        //                    {
        //                        //int ID = listID[i].ToInt(0);
        //                        //Fix lỗi không đổi trạng thái
        //                        int ID = lID[i];
        //                        var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
        //                        if (t != null)
        //                        {
        //                            double weight = 0;
        //                            if (t.SmallPackageID != null)
        //                            {
        //                                if (t.SmallPackageID > 0)
        //                                {
        //                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
        //                                    if (package != null)
        //                                    {
        //                                        if (package.Status == 3)
        //                                        {
        //                                            var check = RequestOutStockController.GetBySmallpackageID(package.ID);
        //                                            if (check == null)
        //                                            {
        //                                                RequestOutStockController.InsertT(package.ID,
        //                                                    package.OrderTransactionCode,
        //                                                    t.ID,
        //                                                    Convert.ToInt32(package.MainOrderID), 1,
        //                                                    DateTime.Now, username, eID);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                            //Update lại trạng thái từng đơn và ngày ship
        //                            TransportationOrderNewController.UpdateRequestOutStock(t.ID, 5, note, currentDate, shippingtype);
        //                        }
        //                    }
        //                    #endregion
        //                    //Trừ tiền 
        //                    #region Trừ tiền xuất kho
        //                    double walletLeft = wallet - totalPriceVND;
        //                    AccountController.updateWallet(UID, walletLeft, currentDate, username);
        //                    HistoryPayWalletController.Insert(UID, username, 0, totalPriceVND,
        //                        username + " đã thanh toán đơn hàng vận chuyển hộ.", walletLeft, 1, 8, currentDate, username);
        //                    #endregion
        //                    //gửi thông báo
        //                    #region Gửi thông báo
        //                    //var admins = AccountController.GetAllByRoleID(0);
        //                    //if (admins.Count > 0)
        //                    //{
        //                    //    foreach (var admin in admins)
        //                    //    {
        //                    //        NotificationsController.Inser(admin.ID, admin.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
        //                    //        string strPathAndQuery = Request.Url.PathAndQuery;
        //                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        //                    //        string datalink = "" + strUrl + "manager/OutStock/";
        //                    //        PJUtils.PushNotiDesktop(admin.ID, "Có yêu cầu xuất kho của user: " + username, datalink);
        //                    //    }
        //                    //}

        //                    //var managers = AccountController.GetAllByRoleID(2);
        //                    //if (managers.Count > 0)
        //                    //{
        //                    //    foreach (var manager in managers)
        //                    //    {
        //                    //        NotificationsController.Inser(manager.ID, manager.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
        //                    //        string strPathAndQuery = Request.Url.PathAndQuery;
        //                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        //                    //        string datalink = "" + strUrl + "manager/OutStock/";
        //                    //        PJUtils.PushNotiDesktop(manager.ID, "Đơn hàng của " + username + " đã yêu cầu xuất kho.", datalink);
        //                    //    }
        //                    //}
        //                    //var khoVNs = AccountController.GetAllByRoleID(5);
        //                    //if (khoVNs.Count > 0)
        //                    //{
        //                    //    foreach (var khoVN in khoVNs)
        //                    //    {
        //                    //        NotificationsController.Inser(khoVN.ID, khoVN.Username, 0, "Có yêu cầu xuất kho của user: " + username, 10, currentDate, username, false);
        //                    //        string strPathAndQuery = Request.Url.PathAndQuery;
        //                    //        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
        //                    //        string datalink = "" + strUrl + "manager/OutStock/";
        //                    //        PJUtils.PushNotiDesktop(khoVN.ID, "Đơn hàng của " + username + " đã yêu cầu xuất kho.", datalink);
        //                    //    }
        //                    //}
        //                    //var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        //                    //hubContext.Clients.All.addNewMessageToPage("", "");
        //                    #endregion
        //                    PJUtils.ShowMessageBoxSwAlert("Bạn đã gửi yêu cầu xuất kho thành công. Xin chân thành cảm ơn", "s", true, Page);
        //                }
        //                else
        //                {
        //                    PJUtils.ShowMessageBoxSwAlert("Số tiền của bạn không đủ để thanh toán phiên xuất kho, vui lòng thử lại sau.", "e", true, Page);
        //                }
        //            }
        //        }
        //    }
        //}


        public class WareHouse
        {
            public int WareHouseFromID { get; set; }
            public int WareHouseID { get; set; }
            public int ShippingTypeID { get; set; }
            public double TotalWeight { get; set; }
            public List<Package> ListPackage { get; set; }
        }

        public class Package
        {
            public int TransportationID { get; set; }
            public double Weight { get; set; }
        }

        protected void btnThanhToanTaiKho_Click(object sender, EventArgs e)
        {
            string username = Session["userLoginSystem"].ToString();
            var acc = AccountController.GetByUsername(username);
            if (acc != null)
            {
                int UID = acc.ID;
                DateTime currentDate = DateTime.Now;
                double wallet = Convert.ToDouble(acc.Wallet);
                string strListID = hdfListID.Value;
                if (!string.IsNullOrEmpty(strListID))
                {
                    string[] listID = strListID.Split('|');
                    if (listID.Length - 1 > 0)
                    {
                        double feeOutStockCYN = 0;
                        double feeOutStockVND = 0;
                        double FeeInsurrance = 0;
                        double totalWeight = 0;
                        double currency = 0;
                        double TotalAdditionFeeCYN = 0;
                        double TotalAdditionFeeVND = 0;
                        double TotalSensoredFeeCYN = 0;
                        double TotalSensoredFeeVND = 0;
                        double totalWeightPriceVND = 0;      
                        double totalPriceVND = 0;
                        double totalPriceCYN = 0;

                        var config = ConfigurationController.GetByTop1();
                        if (config != null)
                        {
                            currency = Convert.ToDouble(config.AgentCurrency);
                            feeOutStockCYN = Convert.ToDouble(config.PriceCheckOutWareDefault);
                            feeOutStockVND = feeOutStockCYN * currency;
                        }

                        int KhoChina = 0;
                        int KhoVN = 0;
                        int PTVC = 0;
                        List<WareHouse> lw = new List<WareHouse>();
                        for (int i = 0; i < listID.Length - 1; i++)
                        {
                            int ID = listID[i].ToInt(0);
                            var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
                            if (t != null)
                            {
                                KhoChina = t.WareHouseFromID.Value;
                                KhoVN = t.WareHouseID.Value;
                                PTVC = t.ShippingTypeID.Value;

                                var small = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                if (small != null)
                                {
                                    if (small.Status == 3)
                                    {
                                        var check = RequestOutStockController.GetBySmallpackageID(small.ID);
                                        if (check == null)
                                        {
                                            WareHouse w1 = new WareHouse();

                                            w1.WareHouseFromID = t.WareHouseFromID.Value;
                                            w1.WareHouseID = t.WareHouseID.Value;
                                            w1.ShippingTypeID = t.ShippingTypeID.Value;

                                            if (t.SmallPackageID != null)
                                            {
                                                if (t.SmallPackageID > 0)
                                                {
                                                    List<Package> lp = new List<Package>();
                                                    var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                    if (package != null)
                                                    {
                                                        double weight = 0;
                                                        if (package.Weight != null)
                                                        {
                                                            if (package.Weight > 0)
                                                            {
                                                                Package p = new Package();
                                                                weight = Math.Round(Convert.ToDouble(package.Weight), 2);
                                                                totalWeight += weight;
                                                                w1.TotalWeight = weight;
                                                                p.Weight = weight;
                                                                p.TransportationID = t.ID;
                                                                lp.Add(p);
                                                            }
                                                        }
                                                    }
                                                    w1.ListPackage = lp;
                                                    lw.Add(w1);
                                                }
                                            }
                                        }
                                    }
                                }


                                double addfeeVND = 0;
                                double addfeeCYN = 0;
                                double sensorVND = 0;
                                double sensorCYN = 0;
                                double TienBaoHiem = 0;

                                if (!string.IsNullOrEmpty(t.AdditionFeeVND))
                                    if (t.AdditionFeeVND.ToFloat(0) > 0)
                                        addfeeVND = Convert.ToDouble(t.AdditionFeeVND);

                                if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
                                    if (t.AdditionFeeCYN.ToFloat(0) > 0)
                                        addfeeCYN = Convert.ToDouble(t.AdditionFeeCYN);

                                if (!string.IsNullOrEmpty(t.SensorFeeeVND))
                                    if (t.SensorFeeeVND.ToFloat(0) > 0)
                                        sensorVND = Convert.ToDouble(t.SensorFeeeVND);

                                if (!string.IsNullOrEmpty(t.SensorFeeCYN))
                                    if (t.SensorFeeCYN.ToFloat(0) > 0)
                                        sensorCYN = Convert.ToDouble(t.SensorFeeCYN);

                                if (!string.IsNullOrEmpty(t.InsurrancePrice))
                                    if (t.InsurrancePrice.ToFloat(0) > 0)
                                        TienBaoHiem = Convert.ToDouble(t.InsurrancePrice);

                                TotalAdditionFeeCYN += addfeeCYN;
                                TotalAdditionFeeVND += addfeeVND;

                                TotalSensoredFeeVND += sensorVND;
                                TotalSensoredFeeCYN += sensorCYN;

                                FeeInsurrance += TienBaoHiem;
                            }
                        }

                        bool CheckIsDifference = true;
                        if (lw.Count > 0)
                        {
                            foreach (var item in lw)
                            {
                                if (item.WareHouseID != KhoVN || item.WareHouseFromID != KhoChina || item.ShippingTypeID != PTVC)
                                {
                                    CheckIsDifference = false;
                                    break;
                                }
                            }
                        }

                        if (CheckIsDifference == true)
                        {
                            double TotalFeeVND = 0;
                            if (acc.FeeTQVNPerWeight.ToFloat(0) > 0)
                            {
                                TotalFeeVND = Convert.ToDouble(acc.FeeTQVNPerWeight) * totalWeight;
                                totalWeightPriceVND = TotalFeeVND;
                            }
                            else
                            {
                                if (lw.Count > 0)
                                {
                                    var fee = WarehouseFeeController.GetByAndWarehouseFromAndToWarehouseAndShippingTypeAndAndHelpMoving(KhoChina, KhoVN, PTVC, true);
                                    if (fee.Count > 0)
                                    {
                                        foreach (var f in fee)
                                        {
                                            if (totalWeight >= f.WeightFrom && totalWeight <= f.WeightTo)
                                            {
                                                TotalFeeVND = Convert.ToDouble(f.Price) * totalWeight;
                                                totalWeightPriceVND = TotalFeeVND;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            totalPriceVND = Math.Round(totalWeightPriceVND + feeOutStockVND + TotalSensoredFeeVND + TotalAdditionFeeVND + FeeInsurrance, 0);
                            totalPriceCYN = Math.Round(totalPriceVND / currency, 2);

                            if (lw.Count > 0)
                            {
                                #region Tạo lượt xuất kho
                                string note = hdfNote.Value;
                                int shippingtype = hdfShippingType.Value.ToInt(0);
                                int totalpackage = listID.Length - 1;
                                string kq = ExportRequestTurnController.InsertWithUID(UID, username, 0, currentDate, totalPriceVND,
                                totalPriceCYN, totalWeight, note, shippingtype, currentDate, username, totalpackage, 1, FeeInsurrance);

                                int eID = kq.ToInt(0);
                                for (int i = 0; i < listID.Length - 1; i++)
                                {
                                    int ID = listID[i].ToInt(0);
                                    var t = TransportationOrderNewController.GetByIDAndUID(ID, acc.ID);
                                    if (t != null)
                                    {
                                        if (t.SmallPackageID != null)
                                        {
                                            if (t.SmallPackageID > 0)
                                            {
                                                var package = SmallPackageController.GetByID(Convert.ToInt32(t.SmallPackageID));
                                                if (package != null)
                                                {
                                                    if (package.Status == 3)
                                                    {
                                                        var check = RequestOutStockController.GetBySmallpackageID(package.ID);
                                                        if (check == null)
                                                        {
                                                            RequestOutStockController.InsertT(package.ID, package.OrderTransactionCode, t.ID, Convert.ToInt32(package.MainOrderID), 1, DateTime.Now, username, eID);
                                                            TransportationOrderNewController.UpdateRequestOutStock(t.ID, 5, note, currentDate, shippingtype);
                                                        }
                                                    }
                                                }
                                            }
                                        }                                       
                                    }
                                }
                                #endregion
                                PJUtils.ShowMessageBoxSwAlert("Bạn đã gửi yêu cầu xuất kho thành công. Xin chân thành cảm ơn", "s", true, Page);
                            }
                            else
                            {
                                PJUtils.ShowMessageBoxSwAlert("Không có kiện nào để gửi yêu cầu. Vui lòng kiểm tra lại hoặc liên hệ nhân viên", "e", true, Page);
                            }
                        }                        
                        else
                        {
                            PJUtils.ShowMessageBoxSwAlert("Các kiện hàng không cùng kho, nên không thể tạo yêu cầu xuất kho. Vui lòng liên hệ BQT hệ thống GiangHuy", "e", true, Page);
                        }                           
                    }
                }
            }
        }
    }
}