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
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST.manager
{
    public partial class ComplainPayHelpList1 : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 2 && ac.RoleID != 7)
                        Response.Redirect("/trang-chu");
                    LoadData();
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchname = search_name.Text.Trim();
            string status = ddlStatus.SelectedValue;
            string fd = "";
            string td = "";
            if (!string.IsNullOrEmpty(rdatefrom.Text))
            {
                fd = rdatefrom.Text.ToString();
            }
            if (!string.IsNullOrEmpty(rdateto.Text))
            {
                td = rdateto.Text.ToString();
            }

            if (!string.IsNullOrEmpty(searchname))
            {
                Response.Redirect("ComplainPayHelpList?s=" + searchname + "&stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
            else
            {
                Response.Redirect("ComplainPayHelpList?stt=" + status + "&fd=" + fd + "&td=" + td + "");
            }
        }
        private void LoadData()
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }

            int status = Request.QueryString["stt"].ToInt(-1);
            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];

            ddlStatus.SelectedValue = status.ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
            {
                rdatefrom.Text = fd;
            }
            if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                rdateto.Text = td;

            int page = 0;
            Int32 Page = GetIntFromQueryString("Page");
            if (Page > 0)
            {
                page = Page - 1;
            }
            var total = ComplainPayHelpController.GetTotal(search, fd, td, status);
            var la = ComplainPayHelpController.GetAllBySQL(search, page, 20, fd, td, status);
            pagingall(la, total);
        }

        [WebMethod]
        public static string loadinfoComplainPayHelp(string ID)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var com = ComplainPayHelpController.GetByID(ID.ToInt(0));
            if (com != null)
            {
                ComplainListShow l = new ComplainListShow();
                if (com != null)
                {

                    var payHelp = PayhelpController.GetByID(Convert.ToInt32(com.PayHelpID));
                    if (payHelp != null)
                    {
                        l.TiGia = string.Format("{0:N0}", Convert.ToDouble(payHelp.Currency)).Replace(",", ".");
                        l.AmountCNY = string.Format("{0:N0}", Convert.ToDouble(payHelp.TotalPrice)).Replace(",", ".");
                        l.UserName = com.CreatedBy;
                        l.ShopID = com.PayHelpID.ToString();
                        l.AmountVND = Convert.ToDouble(com.Amount).ToString();
                        l.ComplainText = com.ComplainText;
                        l.Status = com.Status.ToString();

                        if (!string.IsNullOrEmpty(com.IMG))
                        {
                            var b = com.IMG.Split('|').ToList();
                            l.ListIMG = b;
                        }
                    }
                }
                return serializer.Serialize(l);
            }
            return serializer.Serialize(null);
        }

        public partial class ComplainListShow
        {
            public string TiGia { get; set; }
            public string UserName { get; set; }
            public string AmountCNY { get; set; }
            public string AmountVND { get; set; }
            public string ComplainText { get; set; }
            public string Status { get; set; }
            public string UrlIMG { get; set; }
            public List<string> ListIMG { get; set; }
            public string ShopID { get; set; }

        }

        #region Pagging
        public void pagingall(List<tbl_ComplainPayHelp> acs, int total)
        {
            int PageSize = 20;
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
                StringBuilder hcm = new StringBuilder();
                for (int i = 0; i < acs.Count; i++)
                {
                    var item = acs[i];
                    var payHelp = PayhelpController.GetByID(Convert.ToInt32(item.PayHelpID));
                    string statusString = "";
                    if (item.Status == 0)
                        statusString = "<span class=\"badge red darken-2 white-text border-radius-2\">Đã hủy</span></td>";
                    if (item.Status == 1)
                        statusString = "<span class=\"badge orange  darken-2 white-text border-radius-2\">Chờ duyệt</span></td>";
                    if (item.Status == 2)
                        statusString = "<span class=\"badge yellow darken-2 white-text border-radius-2\">Đang xử lý</span></td>";
                    if (item.Status == 3)
                        statusString = "<span class=\"badge green darken-2 white-text border-radius-2\">Đã xử lý</span></td>";

                    hcm.Append("<tr>");
                    hcm.Append("<td>" + item.ID + "</td>");
                    hcm.Append("<td>" + item.CreatedBy + "</td>");
                    hcm.Append("<td>" + item.PayHelpID + "</td>");
                    hcm.Append("<td>" + item.Amount + "</td>");
                    hcm.Append("<td>" + item.ComplainText + "</td>");
                    hcm.Append("<td>" + statusString + "</td>");
                    hcm.Append("<td>" + item.CreatedDate.ToString().Remove(item.CreatedDate.ToString().Length - 6, 6) + "</td>");
                    hcm.Append("<td>");
                    hcm.Append("<div class=\"action-table\">");
                    hcm.Append("<a href=\"javascript:;\" onclick=\"Complain(" + item.ID + ")\" class=\"edit-mode\" data-position=\"top\"> ");
                    hcm.Append(" <i class=\"material-icons\">remove_red_eye</i><span>Xem</span></a>");
                    if (payHelp != null)
                        hcm.Append("<a href =\"chi-tiet-thanh-toan-ho.aspx?ID=" + item.PayHelpID + "\" target=\"_blank\" data-position=\"top\" ><i class=\"material-icons\">edit</i><span>Chi tiết đơn</span></a>");

                    hcm.Append("</div>");
                    hcm.Append("</td>");
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int ID = hdfID.Value.ToInt(0);
            string username_current = Session["userLoginSystem"].ToString();
            var ac = AccountController.GetByUsername(username_current);
            DateTime currentDate = DateTime.Now;
            string BackLink = "/manager/ComplainPayHelpList.aspx";
            if (ID > 0)
            {
                var com = ComplainPayHelpController.GetByID(ID);
                if (com != null)
                {
                    var setNoti = SendNotiEmailController.GetByID(10);
                    int status = lbStatus.SelectedValue.ToInt();
                    int status_current = Convert.ToInt32(com.Status);

                    ComplainPayHelpController.Update(com.ID, txtAmountRefundCYN.Text.ToString(), status, DateTime.Now, username_current);
                    if (status == 3)
                    {
                        if (status_current != 3)
                        {
                            string uReceive = hdfUserName.Value.Trim().ToLower();
                            var u = AccountController.GetByUsername(uReceive);
                            if (u != null)
                            {
                                int UID = u.ID;
                                double walletCYN = Convert.ToDouble(u.WalletCYN);
                                double rf = Convert.ToDouble(txtAmountRefundCYN.Text);
                                walletCYN = walletCYN + Convert.ToDouble(txtAmountRefundCYN.Text);

                                AccountController.updateWalletCYN(u.ID, walletCYN);
                                PayhelpController.UpdateStatus(com.PayHelpID ?? 0, 3, currentDate, username_current);
                                HistoryPayWalletCYNController.Insert(u.ID, u.Username, Convert.ToDouble(txtAmountRefundCYN.Text), walletCYN, 3, 7,
                                u.Username + " đã được hoàn tiền khiếu nại của đơn thanh toán hộ: " + com.PayHelpID + " vào tài khoản.", currentDate, username_current);
                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(Convert.ToInt32(u.ID),
                                        AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.PayHelpID),
                                        "Admin đã duyệt khiếu nại đơn thanh toán hộ: " + com.PayHelpID + "  của bạn.",
                                        5, currentDate, ac.Username, false);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "khieu-nai-than-toan-ho/";
                                        PJUtils.PushNotiDesktop(u.ID, "Admin đã duyệt khiếu nại đơn thanh toán hộ: " + com.PayHelpID + "  của bạn.", datalink);
                                    }

                                    if (setNoti.IsSendEmailUser == true)
                                    {
                                        try
                                        {
                                            PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", u.Email,
                                                "Thông báo tại GIANGHUY.COM.",
                                                "Admin đã duyệt khiếu nại đơn thanh toán hộ: "
                                                + com.PayHelpID + "  của bạn.", "");
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                    else if (status == 0)
                    {
                        if (status_current != 0)
                        {
                            string uReceive = hdfUserName.Value.Trim().ToLower();
                            var u = AccountController.GetByUsername(uReceive);
                            if (u != null)
                            {
                                PayhelpController.UpdateStatus(com.PayHelpID ?? 0, 3, currentDate, username_current);

                                if (setNoti != null)
                                {
                                    if (setNoti.IsSentNotiUser == true)
                                    {
                                        NotificationsController.Inser(Convert.ToInt32(u.ID),
                                        AccountController.GetByID(Convert.ToInt32(u.ID)).Username, Convert.ToInt32(com.PayHelpID), "Admin đã hủy khiếu nại đơn thanh toán hộ: " + com.PayHelpID + "  của bạn.", 5, currentDate, ac.Username, false);
                                        string strPathAndQuery = Request.Url.PathAndQuery;
                                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                                        string datalink = "" + strUrl + "khieu-nai-thanh-toan-ho/";
                                        PJUtils.PushNotiDesktop(u.ID, "Admin đã hủy khiếu nại đơn thanh toán hộ: " + com.PayHelpID + "  của bạn.", datalink);
                                    }

                                    if (setNoti.IsSendEmailUser == true)
                                    {
                                        try
                                        {
                                            PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw",
                                                u.Email, "Thông báo tại GIANGHUY.COM.", "Admin đã hủy khiếu nại đơn thanh toán hộ: "
                                                + com.PayHelpID + "  của bạn.", "");
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }

                    PJUtils.ShowMessageBoxSwAlertBackToLink("Cập nhật thành công", "s", true, BackLink, Page);
                }
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string search = "";
            if (!string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                search = Request.QueryString["s"].ToString().Trim();
                search_name.Text = search;
            }
            int status = Request.QueryString["stt"].ToInt(-1);
            ddlStatus.SelectedValue = status.ToString();

            string fd = Request.QueryString["fd"];
            string td = Request.QueryString["td"];

            if (!string.IsNullOrEmpty(Request.QueryString["fd"]))
                rdatefrom.Text = fd;
            if (!string.IsNullOrEmpty(Request.QueryString["td"]))
                rdateto.Text = td;

            var la = ComplainPayHelpController.GetAllBySQLExcel(search, fd, td, status);
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StrExport.Append("<table border=\"1\">");
            StrExport.Append("  <tr>");
            StrExport.Append("      <th><strong>ID</strong></th>");
            StrExport.Append("      <th><strong>Username</strong></th>");
            StrExport.Append("      <th><strong>Mã đơn hàng</strong></th>");
            StrExport.Append("      <th><strong>Tiền hoàn</strong></th>");
            StrExport.Append("      <th><strong>Nội dung</strong></th>");
            StrExport.Append("      <th><strong>Trạng thái</strong></th>");
            StrExport.Append("      <th><strong>Ngày tạo</strong></th>");
            StrExport.Append("  </tr>");
            foreach (var item in la)
            {
                double Amount = 0;
                if (item.Amount.ToFloat(0) > 0)
                    Amount = Convert.ToDouble(item.Amount);

                string statusString = "";
                if (item.Status == 0)
                    statusString = "<span>Đã hủy</span></td>";
                if (item.Status == 1)
                    statusString = "<span>Chờ duyệt</span></td>";
                if (item.Status == 2)
                    statusString = "<span>Đang xử lý</span></td>";
                if (item.Status == 3)
                    statusString = "<span>Đã xử lý</span></td>";

                StrExport.Append("  <tr>");
                StrExport.Append("      <td>" + item.ID + "</td>");
                StrExport.Append("      <td>" + item.CreatedBy + "</td>");
                StrExport.Append("      <td>" + item.PayHelpID + "</td>");
                StrExport.Append("      <td style=\"mso-number-format:'\\@'\">" + string.Format("{0:N0}", Convert.ToDouble(Amount)) + "</td>");
                StrExport.Append("      <td>" + item.ComplainText + "</td>");
                StrExport.Append("      <td>" + statusString + "</td>");
                StrExport.Append("      <td>" + string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate) + "</td>");
                StrExport.Append("  </tr>");
            }
            StrExport.Append("</table>");
            StrExport.Append("</div></body></html>");
            string strFile = "ExcelComplainPayHelpList.xls";
            string strcontentType = "application/vnd.ms-excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(StrExport.ToString());
            Response.Flush();
            Response.End();
        }
    }
}