using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST
{
    public partial class them_khieu_nai : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session["userLoginSystem"] = "admin";
                if (Session["userLoginSystem"] != null)
                {
                    LoadData();
                }
                else
                {
                    Response.Redirect("/dang-nhap");
                }
            }

        }
        public void LoadData()
        {
            if (RouteData.Values["id"] != null)
            {
                int MainOrderID = RouteData.Values["id"].ToString().ToInt(0);
                if (MainOrderID > 0)
                {
                    string username = Session["userLoginSystem"].ToString();
                    var u = AccountController.GetByUsername(username);
                    if (u != null)
                    {
                        int UID = u.ID;

                        var mainorder = MainOrderController.GetAllByUIDAndID(UID, MainOrderID);
                        if (mainorder != null)
                        {
                            txtOrderID.Text = MainOrderID.ToString();
                        }
                        var payHelp = PayhelpController.GetByIDAndUID(MainOrderID, UID);
                        if (payHelp != null)
                        {
                            txtOrderID.Text = MainOrderID.ToString();
                        }
                    }
                }
            }
            else
                Response.Redirect("/danh-sach-don-hang");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int orderid = txtOrderID.Text.ToInt(0);
            DateTime currentDate = DateTime.Now;
            string username = Session["userLoginSystem"].ToString();
            var u = AccountController.GetByUsername(username);
            if (u != null)
            {
                int UID = u.ID;

                string value = hdfListIMG.Value;



                var shops = MainOrderController.GetAllByUIDAndID(UID, orderid);
                var payHelp = PayhelpController.GetByIDAndUID(orderid, UID);
                if (shops != null)
                {
                    string kq = CreateComplainResult(value, UID, orderid, username, false);
                    SendNotiCreateComplainSuccess(kq, orderid, u, UID, currentDate);
                    #region SendNotiOld
                    //if (kq.ToInt(0) > 0)
                    //{
                    //    OrderCommentController.Insert(orderid, "Bạn vừa tạo 1 khiếu nại", true, 1, DateTime.Now, u.ID);

                    //    var o = MainOrderController.GetAllByUIDAndID(UID, orderid);
                    //    if (o != null)
                    //    {
                    //        var setNoti = SendNotiEmailController.GetByID(10);
                    //        if (setNoti != null)
                    //        {
                    //            if (setNoti.IsSentNotiAdmin == true)
                    //            {
                    //                var admins = AccountController.GetAllByRoleID(0);
                    //                if (admins.Count > 0)
                    //                {
                    //                    foreach (var admin in admins)
                    //                    {
                    //                        NotificationsController.Inser(admin.ID,
                    //                                                           admin.Username, orderid,
                    //                                                           "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", 5,
                    //                                                           currentDate, u.Username, false);
                    //                        string strPathAndQuery = Request.Url.PathAndQuery;
                    //                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                    //                        string datalink = "" + strUrl + "manager/OrderDetail/" + orderid;
                    //                        PJUtils.PushNotiDesktop(admin.ID, "Đã có khiếu nại mới cho đơn hàng #" + orderid, datalink);
                    //                    }
                    //                }

                    //                var managers = AccountController.GetAllByRoleID(2);
                    //                if (managers.Count > 0)
                    //                {
                    //                    foreach (var manager in managers)
                    //                    {
                    //                        NotificationsController.Inser(manager.ID,
                    //                                                           manager.Username, orderid,
                    //                                                           "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", 5,
                    //                                                           currentDate, u.Username, false);
                    //                        string strPathAndQuery = Request.Url.PathAndQuery;
                    //                        string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                    //                        string datalink = "" + strUrl + "manager/OrderDetail/" + orderid;
                    //                        PJUtils.PushNotiDesktop(manager.ID, "Đã có khiếu nại mới cho đơn hàng #" + orderid, datalink);
                    //                    }
                    //                }
                    //            }

                    //            if (setNoti.IsSentEmailAdmin == true)
                    //            {
                    //                var admins = AccountController.GetAllByRoleID(0);
                    //                if (admins.Count > 0)
                    //                {
                    //                    foreach (var admin in admins)
                    //                    {
                    //                        try
                    //                        {
                    //                            PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", admin.Email,
                    //                                "Thông báo tại GIANGHUY.COM.", "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", "");
                    //                        }
                    //                        catch { }
                    //                    }
                    //                }

                    //                var managers = AccountController.GetAllByRoleID(2);
                    //                if (managers.Count > 0)
                    //                {
                    //                    foreach (var manager in managers)
                    //                    {
                    //                        try
                    //                        {
                    //                            PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", manager.Email,
                    //                                "Thông báo tại GIANGHUY.COM.", "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", "");
                    //                        }
                    //                        catch { }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "signalRNow()", true);
                    //    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>signalRNow();</script>", false);

                    //    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    //    hubContext.Clients.All.addNewMessageToPage("", "");

                    //    PJUtils.ShowMessageBoxSwAlert("Tạo khiếu nại thành công", "s", true, Page);
                    //}
                    #endregion
                }
                else if (payHelp != null)
                {
                    string kq = CreateComplainResult(value, UID, orderid, username, true);
                    SendNotiCreateComplainSuccess(kq, orderid, u, UID, currentDate);
                }
                else
                {
                    lblError.Text = "Không tìm thấy đơn hàng";
                    lblError.Visible = true;

                    //PJUtils.ShowMessageBoxSwAlert("Không tìm thấy đơn hàng", "e", true, Page);
                }
            }
        }

        private string CreateComplainResult(string value, int UID, int orderid, string username, bool isPayHelp)
        {
            string link = "";
            string[] listIMG = value.Split('|');

            for (int i = 0; i < listIMG.Length - 1; i++)
            {
                string imageData = listIMG[i];
                string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/KhieuNaiIMG/");
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string time = DateTime.Now.ToString("hh:mm tt");
                Page page = (Page)HttpContext.Current.Handler;
                //  TextBox txtCampaign = (TextBox)page.FindControl("txtCampaign");
                string k = i.ToString();
                string fileNameWitPath = path + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";
                string linkIMG = "/Uploads/KhieuNaiIMG/" + k + "-" + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "") + ".png";

                //   string fileNameWitPath = path + s + ".png";
                byte[] data;
                string convert;
                string contenttype;

                using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        if (imageData.Contains("data:image/png"))
                        {
                            convert = imageData.Replace("data:image/png;base64,", String.Empty);
                            data = Convert.FromBase64String(convert);
                            contenttype = ".png";
                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                            if (result)
                            {
                                bw.Write(data);
                                link += linkIMG + "|";
                            }
                        }
                        else if (imageData.Contains("data:image/jpeg"))
                        {
                            convert = imageData.Replace("data:image/jpeg;base64,", String.Empty);
                            data = Convert.FromBase64String(convert);
                            contenttype = ".jpeg";
                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                            if (result)
                            {
                                bw.Write(data);
                                link += linkIMG + "|";
                            }
                        }
                        else if (imageData.Contains("data:image/gif"))
                        {
                            convert = imageData.Replace("data:image/gif;base64,", String.Empty);
                            data = Convert.FromBase64String(convert);
                            contenttype = ".gif";
                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                            if (result)
                            {
                                bw.Write(data);
                                link += linkIMG + "|";
                            }
                        }
                        else if (imageData.Contains("data:image/jpg"))
                        {
                            convert = imageData.Replace("data:image/jpg;base64,", String.Empty);
                            data = Convert.FromBase64String(convert);
                            contenttype = ".jpg";
                            var result = FileUploadCheck.isValidFile(data, contenttype, contenttype); // Validate Header
                            if (result)
                            {
                                bw.Write(data);
                                link += linkIMG + "|";
                            }
                        }
                    }
                }
            }
            if (!isPayHelp)
                return ComplainController.Insert(UID, orderid, pAmount.Value.ToString(), link, txtNote.Text, 1, DateTime.Now, username);
            else
            {
                PayhelpController.UpdateStatus(orderid, 4, DateTime.Now, username);
                return ComplainPayHelpController.Insert(UID, orderid, pAmount.Value.ToString(), link, txtNote.Text, 1, DateTime.Now, username);
            }
        }
        private void SendNotiCreateComplainSuccess(string kq, int orderid, tbl_Account u, int UID, DateTime currentDate)
        {
            if (kq.ToInt(0) > 0)
            {
                OrderCommentController.Insert(orderid, "Bạn vừa tạo 1 khiếu nại", true, 1, DateTime.Now, u.ID);

                var o = MainOrderController.GetAllByUIDAndID(UID, orderid);
                var payHelp = PayhelpController.GetByIDAndUID(orderid, UID);
                if (o != null)
                {
                    var setNoti = SendNotiEmailController.GetByID(10);
                    string content = "Đã có khiếu nại mới cho đơn hàng #";
                    string link = "manager/OrderDetail/";
                    SendNotiFunction(setNoti, orderid, currentDate, u, content, link);
                }
                else if (payHelp != null)
                {
                    var setNoti = SendNotiEmailController.GetByID(10);
                    string content = "Đã có khiếu nại mới cho đơn thanh toán hộ #";
                    string link = "manager/chi-tiet-thanh-toan-ho.aspx?ID=";
                    SendNotiFunction(setNoti, orderid, currentDate, u, content, link);
                }
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "signalRNow()", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>signalRNow();</script>", false);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                hubContext.Clients.All.addNewMessageToPage("", "");

                PJUtils.ShowMessageBoxSwAlert("Tạo khiếu nại thành công", "s", true, Page);
            }
        }
        private void SendNotiFunction(tbl_SendNotiEmail setNoti, int orderid, DateTime currentDate, tbl_Account u, string content, string link)
        {
            if (setNoti != null)
            {
                if (setNoti.IsSentNotiAdmin == true)
                {
                    var admins = AccountController.GetAllByRoleID(0);
                    if (admins.Count > 0)
                    {
                        foreach (var admin in admins)
                        {
                            NotificationsController.Inser(admin.ID,
                                                               admin.Username, orderid,
                                                               content + orderid + ". CLick vào để xem", 5,
                                                               //"Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", 5,
                                                               currentDate, u.Username, false);
                            string strPathAndQuery = Request.Url.PathAndQuery;
                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                            string datalink = "" + strUrl + link + orderid;
                            //string datalink = "" + strUrl + "manager/OrderDetail/" + orderid;
                            PJUtils.PushNotiDesktop(admin.ID, content + orderid, datalink);
                            //PJUtils.PushNotiDesktop(admin.ID, "Đã có khiếu nại mới cho đơn hàng #" + orderid, datalink);
                        }
                    }

                    var managers = AccountController.GetAllByRoleID(2);
                    if (managers.Count > 0)
                    {
                        foreach (var manager in managers)
                        {
                            NotificationsController.Inser(manager.ID,
                                                               manager.Username, orderid,
                                                               content + orderid + ". CLick vào để xem", 5,
                                                               //"Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", 5,
                                                               currentDate, u.Username, false);
                            string strPathAndQuery = Request.Url.PathAndQuery;
                            string strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
                            string datalink = "" + strUrl + link + orderid;
                            //string datalink = "" + strUrl + "manager/OrderDetail/" + orderid;
                            PJUtils.PushNotiDesktop(manager.ID, content + orderid, datalink);
                            //PJUtils.PushNotiDesktop(manager.ID, "Đã có khiếu nại mới cho đơn hàng #" + orderid, datalink);
                        }
                    }
                }

                //if (setNoti.IsSentEmailAdmin == true)
                //{
                //    var admins = AccountController.GetAllByRoleID(0);
                //    if (admins.Count > 0)
                //    {
                //        foreach (var admin in admins)
                //        {
                //            try
                //            {
                //                PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", admin.Email,
                //                    "Thông báo tại GIANGHUY.COM.", content + orderid + ". CLick vào để xem", "");
                //                //"Thông báo tại GIANGHUY.COM.", "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", "");
                //            }
                //            catch { }
                //        }
                //    }

                //    var managers = AccountController.GetAllByRoleID(2);
                //    if (managers.Count > 0)
                //    {
                //        foreach (var manager in managers)
                //        {
                //            try
                //            {
                //                PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", manager.Email,
                //                    "Thông báo tại GIANGHUY.COM.", content + orderid + ". CLick vào để xem", "");
                //                //"Thông báo tại GIANGHUY.COM.", "Đã có khiếu nại mới cho đơn hàng #" + orderid + ". CLick vào để xem", "");
                //            }
                //            catch { }
                //        }
                //    }
                //}
            }
        }
    }
}