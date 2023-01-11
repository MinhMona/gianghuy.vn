using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST.manager
{
    public partial class PushMail : System.Web.UI.Page
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
                    string Username = Session["userLoginSystem"].ToString();
                    tbl_Account ac = AccountController.GetByUsername(Username);
                    if (ac.RoleID == 0)
                    {

                    }
                    else
                    {
                        Response.Redirect("/trang-chu");
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            var user = AccountController.GetAll_RoleID(1);
            foreach (var item in user)
            {
                PJUtils.SendMailGmail("gianghuycom@gmail.com.vn", "zioghzphiauqbdmw", item.Email,
                                               txtTitle.Text,
                                               txtMessage.Text, "");
            }
        
                PJUtils.ShowMessageBoxSwAlert("Thông báo thành công.", "s", true, Page);
           
        }
    }
}