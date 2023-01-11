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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NHST.UC
{
    public partial class uc_Sidebar : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadData();
            }
        }

        public void loadData()
        {
            var listpagetype = PageTypeController.GetAll();
            if (listpagetype.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                foreach (var t in listpagetype)
                {
                    html.Append("<div class=\"new-write\">");
                    html.Append("<div class=\"sidebar-img\"><img src=\"/App_Themes/GiangHuy/images/sv-icon.png\" alt=\"#\"></div>");
                    html.Append("<div class=\"sidebar-info\"><p><a href=\"" + t.NodeAliasPath + "\">" + t.PageTypeName + "</a></p></div>");
                    html.Append("</div>");
                }
                ltrCategory.Text = html.ToString();
            }

            var ListPages = PageController.GetAll("");
            var lps = ListPages.Take(6).ToList();
            if (lps.Count > 0)
            {
                StringBuilder html = new StringBuilder();
                foreach (var p in lps)
                {
                    int pagetypeid = Convert.ToInt32(p.PageTypeID);
                    html.Append("<div class=\"new-write\">");
                    html.Append("<div class=\"sidebar-img\"><img src=\"/App_Themes/GiangHuy/images/sv-icon.png\" alt=\"#\"></div>");
                    html.Append("<div class=\"sidebar-info\"><p><a href=\"" + p.NodeAliasPath + "\">" + p.Title + "</a></p></div>");
                    html.Append("</div>");
                }
                ltrList.Text = html.ToString();
            }
        }

        //public void loadData()
        //{
        //    StringBuilder html = new StringBuilder();
        //    var listpagetype = PageTypeController.GetAll();
        //    if (listpagetype.Count > 0)
        //    {
        //        foreach (var t in listpagetype)
        //        {
        //            html.Append("<div class=\"sidebar-item\">");
        //            html.Append("<p class=\"sidebar-item-title\">" + t.PageTypeName + "</p>");
        //            html.Append("<ul class=\"sidebar-item-nav\">");
        //            var listpage = PageController.GetByPagetTypeID(t.ID);
        //            if (listpage.Count > 0)
        //            {
        //                foreach (var temp in listpage)
        //                {
        //                    html.Append("<li><a href=\"" + temp.NodeAliasPath + "\">" + temp.Title + "</a></li>");
        //                }
        //            }
        //            else
        //            {
        //                html.Append("<li><a href=\"" + t.NodeAliasPath + "\">" + t.PageTypeName + "</a></li>");
        //            }
        //            html.Append("</ul>");
        //            html.Append("</div>");
        //        }
        //        ltrCategory.Text = html.ToString();
        //    }
        //}
    }
}