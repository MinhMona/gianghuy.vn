using MB.Extensions;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class danh_sach_vch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userLoginSystem"] == null)
                {
                    Response.Redirect("/trang-chu");
                }
                else
                {
                    string username_current = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(username_current);
                    if (ac.RoleID == 1)
                    {
                        Response.Redirect("/trang-chu");
                    }
                    string page1 = Request.Url.ToString();
                    string page2 = Request.UrlReferrer.ToString();
                    if (page1 != page2)
                        Session["PrePage"] = page2;
                    LoadData();
                }
            }
        }

        private void LoadData()
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            if (ac != null)
            {
                string searchid = "";
                if (!string.IsNullOrEmpty(Request.QueryString["sid"]))
                {
                    searchid = Request.QueryString["sid"].ToString().Trim();
                    search_id.Text = searchid;
                }
                string search = "";
                if (!string.IsNullOrEmpty(Request.QueryString["s"]))
                {
                    search = Request.QueryString["s"].ToString().Trim();
                    search_mvd.Text = search;
                }

                string searchname1 = "";
                if (!string.IsNullOrEmpty(Request.QueryString["sn"]))
                {
                    searchname1 = Request.QueryString["sn"].ToString().Trim();
                    search_name.Text = searchname1;
                }
                string fd = Request.QueryString["fd"];
                if (!string.IsNullOrEmpty(fd))
                    rFD.Text = fd;
                string td = Request.QueryString["td"];
                if (!string.IsNullOrEmpty(td))
                    rTD.Text = td;

                int st = -1;

                if (!string.IsNullOrEmpty(Request.QueryString["st"]))
                {
                    st = Request.QueryString["st"].ToString().ToInt(-1);
                }

                List<tbl_TransportationOrderNew> oAll = new List<tbl_TransportationOrderNew>();
                var os = TransportationOrderNewController.GetAllWithFilter_SqlHelper(searchid, search, searchname1, st, fd, td, Convert.ToInt32(ac.RoleID), Convert.ToInt32(ac.ID));
                if (os.Count > 0)
                {
                    pagingall(os.OrderByDescending(x => x.ID).ToList());
                }
            }

        }
        #region Pagging
        public void pagingall(List<tbl_TransportationOrderNew> acs)
        {
            string username_current = Session["userLoginSystem"].ToString();
            tbl_Account ac = AccountController.GetByUsername(username_current);
            int PageSize = 20;
            if (acs.Count > 0)
            {
                int TotalItems = acs.Count;
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
                StringBuilder hcm = new StringBuilder();
                for (int i = FromRow; i < ToRow + 1; i++)
                {
                    var item = acs[i];
                    string UserName = "";
                    var obj_user = AccountController.GetByID(Convert.ToInt32(item.UID));
                    if (obj_user != null)
                    {
                        UserName = obj_user.Username;
                    }
                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + UserName + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div>");
                    hcm.Append("<select name=\"\" onchange=\"ChooseSaler('" + item.ID + "', $(this))\"  id=\"\">");
                    hcm.Append("  <option value=\"0\">Chọn nhân viên kinh doanh</option>");
                    var salers = AccountController.GetAllByRoleID(6);
                    if (salers.Count > 0)
                    {
                        foreach (var temp in salers)
                        {
                            if (temp.ID == item.SaleID)
                                hcm.Append("  <option selected value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                            else
                                hcm.Append("  <option value=\"" + temp.ID + "\">" + temp.Username + "</option>");
                        }
                    }
                    hcm.Append("</select>");
                    hcm.Append("</div>");
                    hcm.Append("</td>");
                    hcm.Append("<td>" + item.BarCode + "</td>");
                    hcm.Append("<td>" + item.Weight + "</td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt blue-text no-wrap\"><span class=\"total\">Bảo hiểm:</span><span> " + string.Format("{0:N0}", Convert.ToDouble(item.InsurrancePrice)) + " đ</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Cước vật tư:</span><span> " + string.Format("{0:N0}", Convert.ToDouble(item.SensorFeeeVND)) + " đ</span></p>");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Phụ phí:</span><span> " + string.Format("{0:N0}", Convert.ToDouble(item.AdditionFeeVND)) + " đ</span></p>");
                    hcm.Append("</td>");

                    hcm.Append("<td style=\"font-weight:bold\">");
                    hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Ngày tạo:</span><span> " + string.Format("{0:dd/MM/yyyy HH:mm}", Convert.ToDateTime(item.CreatedDate)) + "</span></p>");

                    var warehouse = WarehouseController.GetByID(item.WareHouseID ?? 0);

                    if (item.DateInVNWareHouse != null)
                    {
                        hcm.Append($"<p class=\"s-txt no-wrap\"><span class=\"total\">Hàng về kho {warehouse.WareHouseName}:</span><span> " + string.Format("{0:dd/MM/yyyy HH:mm}", Convert.ToDateTime(item.DateInVNWareHouse)) + "</span></p>");
                    }
                    else
                    {
                        hcm.Append("<p></p>");
                    }
                    if (item.DateExportRequest != null)
                    {
                        hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Yêu cầu xuất kho:</span><span> " + string.Format("{0:dd/MM/yyyy HH:mm}", Convert.ToDateTime(item.DateExportRequest)) + "</span></p>");
                    }
                    else
                    {
                        hcm.Append("<p></p>");
                    }
                    if (item.DateExport != null)
                    {
                        hcm.Append("<p class=\"s-txt no-wrap\"><span class=\"total\">Ngày xuất kho:</span><span> " + string.Format("{0:dd/MM/yyyy HH:mm}", Convert.ToDateTime(item.DateExport)) + "</span></p>");
                    }
                    else
                    {
                        hcm.Append("<p></p>");
                    }
                    hcm.Append("</td>");

                    hcm.Append("<td>" + PJUtils.GeneralTransportationOrderNewStatus(Convert.ToInt32(item.Status)) + "</td>");
                    if (ac.RoleID == 0 || ac.RoleID == 2)
                    {
                        hcm.Append("<td><a class=\"btn btn-info btn-sm\" href=\"/manager/chi-tiet-vch-admin.aspx?id=" + item.ID + "\">Chi tiết</a></td>");
                    }
                    else
                    {
                        hcm.Append("<td><a class=\"btn btn-info btn-sm\" href=\"/manager/chi-tiet-vch.aspx?id=" + item.ID + "\">Chi tiết</a></td>");
                    }

                    hcm.Append("</tr>");
                }
                ltr.Text = hcm.ToString();
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
                if (pageUrl.IndexOf("Page=") > 0)
                {
                    int a = pageUrl.IndexOf("Page=");
                    int b = pageUrl.Length;
                    pageUrl.Remove(a, b - a);
                }
                else
                {
                    pageUrl += "&Page={0}";
                }

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

        #region button event
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchid = search_id.Text.Trim();
            string searchmvd = search_mvd.Text.Trim();
            string searchname = search_name.Text.Trim();
            string fd = "";
            string td = "";
            int st = status.SelectedValue.ToInt(-1);
            if (!string.IsNullOrEmpty(rFD.Text))
            {
                fd = rFD.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rTD.Text))
            {
                td = rTD.Text.ToString();
            }

            //if (fd == "" && td == "" && st == -1)
            //{
            //    Response.Redirect("danh-sach-vch");
            //}
            //else
            //{
            //    Response.Redirect("danh-sach-vch?&fd=" + fd + "&td=" + td + "&st=" + st);
            //}
            if (!string.IsNullOrEmpty(searchmvd) || !string.IsNullOrEmpty(searchname) || !string.IsNullOrEmpty(searchid))
            {
                Response.Redirect("danh-sach-vch?s=" + searchmvd + "&sn=" + searchname + "&sid=" + searchid + "&fd=" + fd + "&td=" + td + "&st=" + st);
            }
            else
            {
                Response.Redirect("danh-sach-vch?fd=" + fd + "&td=" + td + "&st=" + st);
            }
        }
        #endregion

        [WebMethod]
        public static string UpdateStaff(int OrderID, int StaffID, int Type)
        {
            string username = HttpContext.Current.Session["userLoginSystem"].ToString();
            var obj_user = AccountController.GetByUsername(username);
            if (obj_user != null)
            {
                if (obj_user.RoleID == 0 || obj_user.RoleID == 2)
                {
                    var mo = TransportationOrderNewController.GetByID(OrderID);
                    if (mo != null)
                    {
                        if (Type == 1) //1:saler
                        {
                            string salerName = "";
                            var sale = AccountController.GetByID(StaffID);
                            if (sale != null)
                            {
                                salerName = sale.Username;
                            }
                            TransportationOrderNewController.UpdateStaff(mo.ID, StaffID, salerName);
                        }
                        return "ok";
                    }
                }
                else
                {
                    return "notpermision";
                }
            }
            return "null";
        }

    }
}