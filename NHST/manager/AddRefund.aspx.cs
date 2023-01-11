using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MB.Extensions;

namespace NHST.manager
{
    public partial class AddRefund : System.Web.UI.Page
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
                    if (ac != null)
                    {
                        if (ac.RoleID == 1 || ac.RoleID == 3)
                            Response.Redirect("/trang-chu");
                    }
                    loaddata();
                }
            }
        }
        public void loaddata()
        {
            var id = Request.QueryString["i"].ToInt(0);
            if (id > 0)
            {
                string username_current = Session["userLoginSystem"].ToString();
                tbl_Account ac = AccountController.GetByUsername(username_current);
                int role = ac.RoleID.ToString().ToInt();


                ViewState["UID"] = id;
                var a = AccountController.GetByID(id);
                if (a != null)
                {
                    rp_username.Text = a.Username;
                    rp_textarea.Text = a.Username + " đã rút tiền CNY.";
                }
                else
                {
                    Response.Redirect("/manager/userlist.aspx");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            string uReceive = rp_username.Text.Trim().ToLower();
            int uidReceive = ViewState["UID"].ToString().ToInt(0);
            var admin = AccountController.GetByUsername(username);
            var u = AccountController.GetByID(uidReceive);
            string BackLink = "/manager/refund-cyn";
            if (!string.IsNullOrEmpty(rp_vnd.Text))
            {
                double money = Convert.ToDouble(rp_vnd.Text);
                DateTime currentdate = DateTime.Now;
                if (u != null)
                {
                    double WalletCYN = Convert.ToDouble(u.WalletCYN);
                    int UID = u.ID;
                    if (money > 0)
                    {
                        if (money > WalletCYN)
                        {
                            PJUtils.ShowMessageBoxSwAlert("Số tiền nhập vào lớn hơn số tiền trong ví hiện tại của khách.", "i", false, Page);
                        }    
                        else
                        {
                            int status = ddlStatus.SelectedValue.ToInt(0);
                            string kq = RefundController.Insert(UID, u.Username, Convert.ToDouble(rp_vnd.Text), rp_textarea.Text, status, DateTime.Now, username);
                            if (kq.ToInt(0) > 0)
                            {
                                if (status == 2)
                                {                                    
                                    WalletCYN = WalletCYN - money;
                                    AccountController.updateWalletCYN(u.ID, WalletCYN);
                                    HistoryPayWalletCYNController.Insert(u.ID, u.Username, money, WalletCYN, 2, 2,
                                    u.Username + " đã được hoàn lại tiền mua hộ vào tài khoản.", currentdate, username);
                                }
                                PJUtils.ShowMessageBoxSwAlertBackToLink("Tạo lệnh hoàn tiền thành công", "s", true, BackLink, Page);
                            }
                        }
                    }
                }
                else
                {
                    PJUtils.ShowMessageBoxSwAlert("Tên đăng nhập không tồn tại.", "e", false, Page);
                }
            }
            else
            {
                PJUtils.ShowMessageBoxSwAlert("Chưa nhập số tiền cần rút.", "e", false, Page);
            }
        }
    }
}