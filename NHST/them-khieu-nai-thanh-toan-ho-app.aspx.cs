using MB.Extensions;
using Microsoft.AspNet.SignalR;
using NHST.Bussiness;
using NHST.Controllers;
using NHST.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace NHST
{
    public partial class them_khieu_nai_thanh_toan_ho_app : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        public void LoadData()
        {
            string Key = Request.QueryString["Key"];
            int UID = Convert.ToInt32(Request.QueryString["UID"]);
            int orderid = Convert.ToInt32(Request.QueryString["o"]);
            if (UID > 0)
            {
                var tk = DeviceTokenController.GetByToken(UID, Key);
                if (tk != null)
                {
                    ViewState["UID"] = UID;
                    ViewState["Key"] = Key;
                    pnMobile.Visible = true;
                    var u = AccountController.GetByID(UID);
                    if (u != null)
                    {
                        var payHelp = PayhelpController.GetByIDAndUID(orderid,u.ID);
                        if (payHelp != null)
                        {
                            txtOrderID.Text = orderid.ToString();
                        }
                    }
                }
                else
                {
                    pnShowNoti.Visible = true;
                }
            }
            else
            {
                pnShowNoti.Visible = true;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int orderid = txtOrderID.Text.ToInt(0);
            DateTime currentDate = DateTime.Now;
            int UID = Convert.ToInt32(ViewState["UID"]);
            var u = AccountController.GetByID(UID);
            if (u != null)
            {
                var payHelp = PayhelpController.GetByIDAndUID(orderid,UID);
                if (payHelp != null)
                {
                    string IMG = "";
                    string KhieuNaiIMG = "/Uploads/KhieuNaiIMG/";
                    if (hinhDaiDien.UploadedFiles.Count > 0)
                    {
                        foreach (UploadedFile f in hinhDaiDien.UploadedFiles)
                        {
                            if (f.FileName.ToLower().Contains(".jpg") || f.FileName.ToLower().Contains(".png") || f.FileName.ToLower().Contains(".jpeg"))
                            {
                                if (f.ContentType == "image/png" || f.ContentType == "image/jpeg" || f.ContentType == "image/jpg")
                                {
                                    var o = KhieuNaiIMG + Guid.NewGuid() + f.GetExtension();
                                    try
                                    {
                                        f.SaveAs(Server.MapPath(o));
                                        IMG = o;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    else
                        IMG = imgDaiDien.ImageUrl;
                    PayhelpController.UpdateStatus(orderid, 4, DateTime.Now, u.Username);

                    string kq = ComplainPayHelpController.Insert(UID, orderid, pAmount.Value.ToString(), IMG, txtNote.Text, 1, DateTime.Now, u.Username);
                    if (kq.ToInt(0) > 0)
                    {
                        OrderCommentController.Insert(UID, "Bạn vừa tạo 1 khiếu nại", true, 1, DateTime.Now, u.ID);

                        var p = PayhelpController.GetByIDAndUID(orderid,UID);
                        if (p != null)
                        {
                            var setNoti = SendNotiEmailController.GetByID(10);
                            if (setNoti != null)
                            {
                                if (setNoti.IsSentNotiAdmin == true)
                                {
                                    var admins = AccountController.GetAllByRoleID(0);
                                    if (admins.Count > 0)
                                    {
                                        foreach (var admin in admins)
                                        {
                                            NotificationsController.Inser(admin.ID, admin.Username, orderid, "Đã có khiếu nại mới cho đơn thanh toán hộ #" + orderid + ". CLick vào để xem",
                    7, currentDate, u.Username, false);
                                        }
                                    }

                                    var managers = AccountController.GetAllByRoleID(2);
                                    if (managers.Count > 0)
                                    {
                                        foreach (var manager in managers)
                                        {

                                            NotificationsController.Inser(manager.ID, manager.Username, orderid, "Đã có khiếu nại mới cho đơn thanh toán hộ #" + orderid + ". CLick vào để xem",
                                                  7, currentDate, u.Username, false);
                                        }
                                    }
                                }

                                if (setNoti.IsSentEmailAdmin == true)
                                {
                                    var admins = AccountController.GetAllByRoleID(0);
                                    if (admins.Count > 0)
                                    {
                                        foreach (var admin in admins)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", admin.Email,
                                                    "Thông báo tại Giang Huy.", "Đã có khiếu nại mới cho đơn thanh toán hộ #" + orderid + ". CLick vào để xem", "");
                                            }
                                            catch { }
                                        }
                                    }

                                    var managers = AccountController.GetAllByRoleID(2);
                                    if (managers.Count > 0)
                                    {
                                        foreach (var manager in managers)
                                        {
                                            try
                                            {
                                                PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", manager.Email,
                                                    "Thông báo tại Giang Huy.", "Đã có khiếu nại mới cho đơn thanh toán hộ #" + orderid + ". CLick vào để xem", "");
                                            }
                                            catch { }
                                        }
                                    }

                                }
                            }



                        }

                        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "signalRNow()", true);
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>signalRNow();</script>", false);

                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                        hubContext.Clients.All.addNewMessageToPage("", "");

                        PJUtils.ShowMessageBoxSwAlert("Tạo khiếu nại thanh toán hộ thành công", "s", true, Page);
                    }
                }
                else
                {
                    lblError.Text = "Không tìm thấy đơn thanh toán hộ";
                    lblError.Visible = true;

                    //PJUtils.ShowMessageBoxSwAlert("Không tìm thấy đơn hàng", "e", true, Page);
                }
            }
        }
    }
}