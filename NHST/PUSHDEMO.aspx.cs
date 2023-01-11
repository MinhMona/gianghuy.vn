using NHST.Bussiness;
using NHST.Controllers;
using NHST.Models;
using Supremes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NHST
{
    public partial class PUSHDEMO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AutoConvertBase64ToImage();
            }
        }

        protected string AutoConvertBase64ToImage()
        {           
            var test = OrderController.getListOrderImagebase64();
            foreach (var item in test)
            {
                var itemtest = OrderController.GetAllByID(item.ID);
                Thread t = new Thread(CreateImage);
                t.Start(itemtest);
            }
            return "ok";
        }

        public void CreateImage(object ob)
        {
            tbl_Order dt = (tbl_Order)ob;

            var imagein = FileUploadCheck.ConvertBase64ToImageCustom(dt.image_model, dt.ID);           

            OrderController.UpdateLinkIMG(dt.ID, imagein);
        }
    }
}