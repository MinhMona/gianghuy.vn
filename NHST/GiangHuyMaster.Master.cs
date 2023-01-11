using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static Telerik.Web.UI.OrgChartStyles;

namespace NHST
{
    public partial class GiangHuyMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMenu();
                LoadData();
            }
        }

        public void LoadMenu()
        {
            string html = "";
            var categories = MenuController.GetByLevel(0);
            if (categories != null)
            {

                foreach (var c in categories)
                {
                    var categories2 = MenuController.GetByLevel(c.ID);
                    if (categories2 != null)
                    {
                        html += " <li>";

                        if (!string.IsNullOrEmpty(c.MenuLink))
                        {
                            if (Convert.ToBoolean(c.Target))
                                html += "<a target=\"_blank\" href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                            else
                                html += "<a href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        }
                        else
                        {
                            html += "<a href=\"javascript:;\">" + c.MenuName + "</a>";
                        }
                        html += "<div class=\"sub-menu-wrap\">";
                        html += "<ul class=\"sub-menu\">";
                        foreach (var item in categories2)
                        {
                            html += " <li>";
                            if (Convert.ToBoolean(c.Target))
                                html += "   <a target=\"_blank\" href =\"" + item.MenuLink + "\">" + item.MenuName + "</a>";
                            else
                                html += "   <a href =\"" + item.MenuLink + "\">" + item.MenuName + "</a>";
                            html += "</li>";
                        }
                        html += " </ul>";
                        html += " </div>";
                    }
                    else
                    {
                        html += " <li>";
                        if (Convert.ToBoolean(c.Target))
                            html += "<a target=\"_blank\" href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        else
                            html += "<a href=\"" + c.MenuLink + "\">" + c.MenuName + "</a>";
                        html += "</li>";
                    }
                }

                ltrMenu.Text = html;
            }
        }

        public void LoadData()
        {
            var confi = ConfigurationController.GetByTop1();
            if (confi != null)
            {
                #region lấy meta
                HtmlHead objHeader = (HtmlHead)Page.Header;
                HtmlMeta meta = new HtmlMeta();
                meta = new HtmlMeta();
                meta.Attributes.Add("name", "description");
                meta.Content = confi.MetaDescription;
                objHeader.Controls.Add(meta);

                meta = new HtmlMeta();
                meta.Attributes.Add("name", "keyword");
                meta.Content = confi.MetaKeyword;
                objHeader.Controls.Add(meta);
                ltrSEO.Text += "<script>" + confi.GoogleAnalytics + "</script>";
                ltrSEO.Text += "<script>" + confi.WebmasterTools + "</script>";

                ltrHeader.Text += "<script>" + confi.HeaderScriptCode + "</script>";
                //ltrFooterScript.Text += "<script>" + confi.FooterScriptCode + "</script>";
                #endregion
                //ltrFooter.Text = confi.FooterLeft;
                string email = confi.EmailSupport;
                string hotline = confi.Hotline;
                string timework = confi.TimeWork;

                //ltrFooter.Text = confi.FooterLeft;

                //ltrHotPhone.Text = " <a class=\"icon-dt\" href =\"tel:" + hotline + "\"><img src=\"/App_Themes/GiangHuy/images/icon-dt.png\" alt=\"\"></a>";
                //ltrZalo.Text = " <a class=\"icon-zalo\" href=\"" + confi.ZaloLink + "\" target=\"_blank\"><img src=\"/App_Themes/GiangHuy/images/icon-zalo.png\" alt=\"\"></a>";

                //ltrFooter.Text = "<a href=\"/\"><img src=\"" + confi.LogoIMG + "\" alt=\"\"></a>";
                ltrLogo.Text = "  <img src=\"" + confi.LogoIMG + "\" alt=\"\">";
                //ltrLogoFooter.Text = "<a href=\"/\"><img src=\"" + confi.LogoIMG + "\" alt=\"\"></a>";

                ////ltrCurrency.Text += "<li>Tỉ giá: 1¥ = <span class=\"icon - circle\"><i class=\"fa fa-usd\"></i>" + string.Format("{0:N0}", Convert.ToDouble(confi.Currency)) + "</span></li>";
                //ltrCurrency.Text += "<a href=\"/\"><span class=\"icon-circle\"><i class=\"fa fa-usd\"></i></span><span class=\"hidden-mobile\"> Tỉ giá: 1 =" + string.Format("{0:N0}", Convert.ToDouble(confi.Currency)) + "</span></a>";
                //ltrEmail.Text += "<a href=\"/\"><span class=\"icon-circle\"><i class=\"fa fa-envelope\"></i></span>" + email + "</a>";
                ////ltrTop.Text += "<li>Hotline: <span><a href=\"tel:" + hotline + "\">" + hotline + "</a></span></li>";


                ltrTyGia.Text += "<p class=\"m-g-r-30\">Tỉ giá Mua Hộ: 1¥ = " + string.Format("{0:N0}", Convert.ToDouble(confi.Currency)) + " - Tỉ giá TTH: 1¥ = " + string.Format("{0:N0}", Convert.ToDouble(confi.PricePayHelpDefault)) + " </p>";
                //ltrTop.Text += "<li><span><a href=\"mailto:" + email + "\">" + email + "</a></span></li>";
                ltrHotLine.Text += "<a class=\"m-g-r-30 display-none\" href=\"tel:" + hotline + "\">Hotline: " + hotline + "</a>";
                //ltrTop.Text += "<li>Giờ làm việc: <span class=\"color-yellow\">" + timework + "</span></li>";

                ////ltrTopLeft.Text += "<li>Email: <span><a href=\"mailto:" + email + "\">" + email + "</a></span></li>";

                //ltrAddress.Text = "<div class=\"box-text\"> "+confi.Address+"</div>";
                //ltrPhone.Text = "<div class=\"box-text\"><a href=\"tel:" + hotline + "\">Hotline: </br>" + hotline + "</a></div>";
                //ltrMail.Text = "<div class=\"box-text\"><a href=\"mailto:" + email + "\">Email: </br>" + email + "</a></div>";

                //ltrLienHe.Text = "<li><a href=\"#\">" + confi.Address + "</a></li>";
                //ltrLienHe.Text = "<li><a href=\"tel:" + hotline + "\">" + hotline + "</a></li>";
                //ltrLienHe.Text = "<li><a href=\"mailto:" + email + "\">" + email + "</a></li>";

                //ltrTime.Text = " <p>" + timework + "</p>";

                ltrSocial.Text += "<li><a href=\"" + confi.WechatLink + "\" target=\"_blank\"><img src=\"/App_Themes/GiangHuy/images/chat.png\" alt=\"\"></a></li>";
                ltrSocial.Text += "<li><a href=\"" + confi.ZaloLink + "\" target=\"_blank\"> <img src=\"/App_Themes/GiangHuy/images/zalo.png\" alt=\"\"></a></li>";
                ltrSocial.Text += "<li><a href=\"" + confi.Facebook + "\" target=\"_blank\"><i class=\"fa fa-facebook\"></i></a></li>";


                //ltrSocialFooter.Text += "<li><a href=\"" + confi.Facebook + "\" target=\"_blank\"><i class=\"fa fa-facebook\" aria-hidden=\"true\"></i></a></li>";
                //ltrSocialFooter.Text += "<li><a href=\"" + confi.Twitter + "\" target=\"_blank\"><i class=\"fa fa-twitter\" aria-hidden=\"true\"></i></a></li>";
                //ltrSocialFooter.Text += "<li><a href=\"" + confi.LinkedIn + "\" target=\"_blank\"><i class=\"fa fa-linkedin\" aria-hidden=\"true\"></i></a></li>";
                //ltrSocialFooter.Text += "<li><a href=\"" + confi.YoutubeLink + "\" target=\"_blank\"><i class=\"fa fa-youtube-play\" aria-hidden=\"true\"></i></a></li>";

                string infocontent = confi.InfoContent;
                if (Session["infoclose"] == null)
                {
                    if (!string.IsNullOrEmpty(infocontent))
                    {
                        ltr_infor.Text += "<div class=\"sec webinfo\">";
                        ltr_infor.Text += "<div class=\"all-info\">";
                        ltr_infor.Text += "<div class=\"main-info\">";
                        ltr_infor.Text += "<div class=\"textcontent\">";
                        ltr_infor.Text += "<span>" + infocontent + "</span>";
                        ltr_infor.Text += "<a href=\"javascript:;\" onclick=\"closewebinfo()\" class=\"icon-close-info\"><i class=\"fa fa-times\"></i></a>";
                        ltr_infor.Text += "</div></div></div></div>";
                    }
                }

                ltrSection.Text = "";
                //var news = NewsController.GetAllNotHiddenType(1);
                var news = PageController.GetTopByPagetTypeID(9, 14);
                foreach (var item in news)
                {
                    ltrSection.Text += "<div class=\"swiper-slide\">";
                    ltrSection.Text += $"<a href=\"{item.NodeAliasPath}\" class=\"article-item\">";
                    ltrSection.Text += "<div class=\"article-image\">";
                    ltrSection.Text += $"<img src=\"{item.IMG}\" alt=\"{item.Title}\">";
                    ltrSection.Text += "</div>";
                    ltrSection.Text += "<div class=\"article-content\">";
                    ltrSection.Text += $"<div class=\"article-title\">{item.Title}</div>";
                    ltrSection.Text += $"<div class=\"article-para \" style=\"display: -webkit-box; -webkit-line-clamp: 4; -webkit-box-orient: vertical; overflow: hidden;\">{item.Summary}</div>";
                    ltrSection.Text += "<div class=\"article-more\">xem chi tiết</div>";
                    ltrSection.Text += "</div></a></div>";
                }

                ltrFooterMenu.Text = "";
                var gioiThieu = MenuController.GetAll("Giới thiệu");
                ltrFooterMenu.Text += $"<li class=\"li-item\"><a href=\"{gioiThieu.FirstOrDefault().MenuLink}\">Giới thiệu</a></li>";
                var huongDan = MenuController.GetAll("Hướng dẫn");
                ltrFooterMenu.Text += $"<li class=\"li-item\"><a href=\"{huongDan.FirstOrDefault().MenuLink}\">Hướng dẫn</a></li>";
                var tinTuc = MenuController.GetAll("Tin tức");
                ltrFooterMenu.Text += $"<li class=\"li-item\"><a href=\"{tinTuc.FirstOrDefault().MenuLink}\">Tin tức</a></li>";
                var chinhSach = MenuController.GetByLevel(5);
                foreach (var item in chinhSach)
                {
                    ltrFooterMenu.Text += $"<li class=\"li-item\"><a href=\"{item.MenuLink}\">{item.MenuName}</a></li>";
                }

                ltrFooterSupport.Text = "";
                var huongDans = new List<tbl_Menu>();
                huongDans.Add(MenuController.GetAll("Hướng Dẫn Tạo Tài Khoản").FirstOrDefault());
                huongDans.Add(MenuController.GetAll("Hướng Dẫn tạo đơn hàng sử dụng công cụ đặt hàng Giang Huy").FirstOrDefault());
                foreach (var item in huongDans)
                {
                    ltrFooterSupport.Text += $"<li class=\"li-item\"><a href=\"{item.MenuLink}\">{item.MenuName}</a></li>";
                }
                ltrFooterSupport.Text += $"<li class=\"li-item\"><a href=\"\">Trung tâm trợ giúp</a></li>";
                ltrFooterSupport.Text += $"<li class=\"li-item\"><a href=\"\">Trả hàng & vận chuyển</a></li>";
                ltrFooterSupport.Text += $"<li class=\"li-item\"><a href=\"\">Các câu hỏi thường gặp</a></li>";

                ltrWebChat.Text += "<a href=\"" + confi.WechatLink + "\" target=\"_blank\"><img src=\"/App_Themes/GiangHuy/images/chat.png\" alt=\"\"></a>";
                ltrLinkZalo.Text += "<a href=\"" + confi.ZaloLink + "\" target=\"_blank\"> <img src=\"/App_Themes/GiangHuy/images/zalo.png\" alt=\"\"></a>";
                ltrLinkFB.Text += "<a href=\"" + confi.Facebook + "\" target=\"_blank\"> <img src=\"/App_Themes/GiangHuy/images/facebook.png\" alt=\"\"></a>";
                ltrYoutube.Text = "<a href=\"" + confi.YoutubeLink + "\" target=\"_blank\"><img src=\"/App_Themes/GiangHuy/images/youtube.png\" alt=\"\"></a>";
            }
            if (Session["userLoginSystem"] != null)
            {
                string username = Session["userLoginSystem"].ToString();
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    var ordershoptemp = OrderShopTempController.GetByUID(acc.ID);
                    int count = 0;
                    if (ordershoptemp.Count > 0)
                        count = ordershoptemp.Count;
                    #region phần thông báo
                    decimal levelID = Convert.ToDecimal(acc.LevelID);
                    int levelID1 = Convert.ToInt32(acc.LevelID);
                    string level = "1 Vương Miện";
                    var userLevel = UserLevelController.GetByID(levelID1);
                    if (userLevel != null)
                    {
                        level = userLevel.LevelName;
                    }
                    string userIMG = "/App_Themes/CIQOrder/images/user-icon.png";
                    var ai = AccountInfoController.GetByUserID(acc.ID);
                    if (ai != null)
                    {
                        if (!string.IsNullOrEmpty(ai.IMGUser))
                            userIMG = ai.IMGUser;
                    }

                    decimal countLevel = UserLevelController.GetAll("").Count();
                    decimal te = levelID / countLevel;
                    te = Math.Round(te, 2, MidpointRounding.AwayFromZero);
                    decimal tile = te * 100;
                    string levelIconList = "";
                    string levelIconSingle = "";
                    var userLevels = UserLevelController.GetAll("");
                    if (userLevels.Count > 0)
                    {
                        foreach (var item in userLevels)
                        {
                            if (item.ID <= levelID)
                            {
                                levelIconList += "<img style=\"margin-right:5px;width:15%\" src=\"/App_Themes/GiangHuy/images/vm-active.png\">";
                                //levelIconSingle += "<img src=\"/App_Themes/CIQOrder/images/vm-active.png\">";
                            }
                            else
                            {
                                levelIconList += "<img style=\"margin-right:5px;width:15%\" src=\"/App_Themes/GiangHuy/images/vm-inactive.png\">";
                            }
                        }
                    }
                    #endregion

                    #region New
                    ltrLogin.Text += "<div class=\"acc-info\">";
                    ltrLogin.Text += "<a class=\"acc-info-btn\" href=\"#\"><i class=\"icon fa fa-user\"></i><span>" + username + "</span></a>";
                    ltrLogin.Text += "<div class=\"status-desktop\">";
                    ltrLogin.Text += "<div class=\"status-wrap\">";
                    ltrLogin.Text += "<div class=\"status__header\">";
                    ltrLogin.Text += "<h4>" + level + "</h4>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status__body\">";
                    ltrLogin.Text += "<div class=\"level\">";
                    ltrLogin.Text += "<div class=\"level__info\">";
                    ltrLogin.Text += "<p>Level</p>";
                    ltrLogin.Text += "<p class=\"rank\">" + level + "</p>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"level__process\"><span style=\"width: " + tile + "%\"></span></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"balance\">";
                    ltrLogin.Text += "<p>Số dư:</p>";
                    ltrLogin.Text += "<div class=\"balance__number\"><p class=\"vnd\">" + string.Format("{0:N0}", acc.Wallet) + " vnđ</p></div>";
                    ltrLogin.Text += "</div>";
                    if (acc.RoleID != 1)
                        ltrLogin.Text += "<div class=\"links\"><a href=\"/manager/login\">Quản trị<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"#\">Thông tin tài khoản<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"/danh-sach-don-hang?t=1\">Đơn hàng của bạn<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "<div class=\"links\"><a href=\"/lich-su-giao-dich\">Lịch sử giao dịch<i class=\"fa fa-caret-right\"></i></a></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status__footer\"><a href=\"/dang-xuat\" class=\"ft-btn\">ĐĂNG XUẤT</a></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"status-mobile\">";
                    ltrLogin.Text += "<a href=\"/thong-tin-nguoi-dung\" class=\"user-menu-logo\"><img src=\"" + userIMG + "\" alt=\"\"></a>";
                    ltrLogin.Text += "<h3 class=\"username\">" + username + "</h3>";
                    ltrLogin.Text += "<div class=\"user-info\">Số tiền: <span class=\"money\">" + string.Format("{0:N0}", acc.Wallet) + "</span> vnđ | Level <span class=\"vip\">" + level + "</span></div>";
                    ltrLogin.Text += "<div class=\"nav-percent\">";
                    ltrLogin.Text += "<div class=\"nav-percent-ok\" style=\"width: " + tile + "%\"></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "<div class=\"profile-bottom\">";
                    ltrLogin.Text += "<ul class=\"menu-in-profile\">";
                    ltrLogin.Text += "<li><a href=\"/\"><i class=\"fa fa-home\"></i>TRANG CHỦ</a></li>";
                    ltrLogin.Text += "<li><a href=\"/theo-doi-mvd\"><i class=\"fa fa-search\"></i>TRACKING</a></li>";
                    ltrLogin.Text += "<li><a href=\"/danh-sach-don-hang?t=1\"><i class=\"fa fa-home\"></i>MUA HÀNG HỘ</a></li>";
                    ltrLogin.Text += "<li><a href=\"/lich-su-giao-dich\"><i class=\"fa fa-money\"></i>TÀI CHÍNH</a></li>";
                    ltrLogin.Text += "<li><a href=\"/khieu-nai\"><i class=\"fa fa-exclamation\"></i>KHIẾU NẠI</a></li>";
                    ltrLogin.Text += "<li><a href=\"/thong-tin-nguoi-dung\"><i class=\"fa fa-user\"></i>QUẢN LÝ TÀI KHOẢN</a></li>";
                    ltrLogin.Text += "</ul>";
                    ltrLogin.Text += "</div><a href=\"/dang-xuat\" class=\"main-btn\">Đăng xuất</a></div>";
                    ltrLogin.Text += "<div class=\"overlay-status-mobile\"></div>";
                    ltrLogin.Text += "</div>";
                    ltrLogin.Text += "</div>";
                    //ltrLogin.Text += "<a class=\"hl\"><span class=\"icon\"><i class=\"fa fa-user\" aria-hidden=\"true\"></i></span><span>Đăng xuất</span></a>";
                    //ltrLogin.Text += "<div class=\"status\">";
                    //ltrLogin.Text += "<div class=\"status-wrap\">";

                    //ltrLogin.Text += "<div class=\"status__header\">";
                    //ltrLogin.Text += "<h4>" + level + "</h4>";
                    //ltrLogin.Text += "</div>";//end status__header

                    //ltrLogin.Text += "<div class=\"status__body\">";

                    //ltrLogin.Text += "<div class=\"level\">";

                    //ltrLogin.Text += "<div class=\"level__info\">";
                    //ltrLogin.Text += "<p>Level</p>";
                    //ltrLogin.Text += "<p class=\"rank\">" + level + "</p>";
                    //ltrLogin.Text += "</div>"; // end level__info

                    //ltrLogin.Text += "<div class=\"level__process\">";
                    //ltrLogin.Text += "<span style=\"width: " + tile + "%\"></span>";
                    //ltrLogin.Text += "</div>"; // end level__process

                    //ltrLogin.Text += "</div>"; // end level
                    //ltrLogin.Text += "<div class=\"balance\">";
                    //ltrLogin.Text += "<p> Số dư: </p>";
                    //ltrLogin.Text += "<div class=\"balance__number\">";
                    //ltrLogin.Text += "<p class=\"vnd\">" + string.Format("{0:N0}", acc.Wallet) + "vnđ</p>";
                    ////ltrLogin.Text += "<p class=\"vnd\">" + string.Format("{0:N0}", acc.WalletCYN) + "y</p>";
                    //ltrLogin.Text += "</div>";//end balance__number

                    //ltrLogin.Text += "</div>"; //end balance
                    //ltrLogin.Text += "<div class=\"links\">";
                    //ltrLogin.Text += "<a href=\"/thong-tin-nguoi-dung\">Thông tin tài khoản<i class=\"fa fa-caret-right\"></i></a>";
                    //ltrLogin.Text += "</div>"; //end links tài khoản
                    //ltrLogin.Text += "<div class=\"links\">";
                    //ltrLogin.Text += "<a href=\"/\">Đơn hàng của bạn<i class=\"fa fa-caret-right\"></i></a>";
                    //ltrLogin.Text += "</div>"; //end links đơn hàng
                    //ltrLogin.Text += "<div class=\"links\">";
                    //ltrLogin.Text += "<a href=\"/\">Lịch sử giao dịch<i class=\"fa fa-caret-right\"></i></a>";
                    //ltrLogin.Text += "</div>"; //end links lịch sử giao dịch
                    //ltrLogin.Text += "</div>"; //end tatus__body
                    //ltrLogin.Text += "<div class=\"status__footer\">";
                    //ltrLogin.Text += "<a href =\"/dang-xuat\" class=\"ft-btn\">Đăng xuất</a>";
                    //ltrLogin.Text += "</div>"; // end status__footer
                    //ltrLogin.Text += "</div>"; // end status-wrap
                    //ltrLogin.Text += "</div>";//end status
                    #endregion

                }
            }
            else
            {
                ltrLogin.Text = "<div class=\"login-register\"><a href=\"/dang-ky\">ĐĂNG KÝ</a> <span> / </span> <a href=\"/dang-nhap\">ĐĂNG NHẬP</a></div>";
            }
        }
    }
}