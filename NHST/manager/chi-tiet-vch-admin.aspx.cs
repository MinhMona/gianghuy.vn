using MB.Extensions;
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
    public partial class chi_tiet_vch_admin : System.Web.UI.Page
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
                    if (ac.RoleID != 0 && ac.RoleID != 2)
                        Response.Redirect("/trang-chu");
                }
                string page1 = Request.Url.ToString();
                string page2 = Request.UrlReferrer.ToString();
                if (page1 != page2)
                    Session["PrePage"] = page2;
                LoadShippingType();
                LoadData();
                LoadDDL();
            }
        }
        public void LoadShippingType()
        {
            ddlShippingType.Items.Clear();
            ddlShippingType.Items.Insert(0, "Chưa chọn hình thức vận chuyển");
            var s = ShippingTypeVNController.GetAllWithIsHidden("", false);
            if (s.Count > 0)
            {
                foreach (var item in s)
                {
                    ListItem listitem = new ListItem(item.ShippingTypeVNName.ToString(), item.ID.ToString());
                    ddlShippingType.Items.Add(listitem);
                }
            }
            ddlShippingType.DataBind();
        }

        public void LoadDDL()
        {
            var warehousefrom = WarehouseFromController.GetAllWithIsHidden(false);
            if (warehousefrom.Count > 0)
            {
                ddlWarehouseFrom.DataSource = warehousefrom;
                ddlWarehouseFrom.DataBind();
            }
            var warehouse = WarehouseController.GetAllWithIsHidden(false);
            if (warehouse.Count > 0)
            {
                ddlReceivePlace.DataSource = warehouse;
                ddlReceivePlace.DataBind();
            }

            var shippingtype = ShippingTypeToWareHouseController.GetAllWithIsHidden(false);
            if (shippingtype.Count > 0)
            {
                ddlShippingTypeKyGui.DataSource = shippingtype;
                ddlShippingTypeKyGui.DataBind();
            }
        }

        public void LoadData()
        {
            var id = Request.QueryString["ID"].ToInt(0);
            if (id > 0)
            {
                ViewState["ID"] = id;
                var config = ConfigurationController.GetByTop1();
                if (config != null)
                {
                    hdfCurrency.Value = config.AgentCurrency;
                }
                var user = AccountController.GetAll_RoleID_All("");
                if (user.Count > 0)
                {
                    ddlUsername.DataSource = user;
                    ddlUsername.DataBind();
                }
                var t = TransportationOrderNewController.GetByID(id);
                if (t != null)
                {
                    int index = 0;
                    var a = AccountController.GetByUsername(t.Username);
                    if (a != null)
                    {
                        if (user.Count > 0)
                        {
                            index = user.FindIndex(n => n.ID == a.ID);
                        }
                    }

                    ddlUsername.SelectedIndex = index;
                    ddlUsername.DataBind();

                    txtBarcode.Text = t.BarCode;
                    double weight = 0;
                    int spID = 0;
                    if (t.SmallPackageID != null)
                        spID = Convert.ToInt32(t.SmallPackageID);
                    var package = SmallPackageController.GetByID(spID);
                    if (package != null)
                    {
                        if (package.Weight != null)
                            if (package.Weight.ToString().ToFloat(0) > 0)
                                weight = Convert.ToDouble(package.Weight);
                    }
                    rWeight.Value = weight;
                    double addFeeCYN = 0;
                    double addFeeVND = 0;
                    if (!string.IsNullOrEmpty(t.AdditionFeeCYN))
                    {
                        if (t.AdditionFeeCYN.ToFloat(0) > 0)
                            addFeeCYN = Convert.ToDouble(t.AdditionFeeCYN);
                    }
                    if (!string.IsNullOrEmpty(t.AdditionFeeVND))
                    {
                        if (t.AdditionFeeVND.ToFloat(0) > 0)
                            addFeeVND = Convert.ToDouble(t.AdditionFeeVND);
                    }
                    rAdditionFeeCYN.Value = addFeeCYN;
                    rAdditionFeeVND.Value = addFeeVND;

                    double sensorCYN = 0;
                    double sensorVND = 0;
                    if (!string.IsNullOrEmpty(t.SensorFeeCYN))
                    {
                        if (t.SensorFeeCYN.ToFloat(0) > 0)
                            sensorCYN = Convert.ToDouble(t.SensorFeeCYN);
                    }
                    if (!string.IsNullOrEmpty(t.SensorFeeeVND))
                    {
                        if (t.SensorFeeeVND.ToFloat(0) > 0)
                            sensorVND = Convert.ToDouble(t.SensorFeeeVND);
                    }

                    rSensorFeeCYN.Value = sensorCYN;
                    rSensorFeeeVND.Value = sensorVND;                  
                    txtSummary.Text = t.Note;
                    txtStaffNote.Text = t.StaffNote;
                    ddlStatus.SelectedValue = t.Status.ToString();
                    ddlShippingType.SelectedValue = t.ShippingTypeVN.ToString();
                    txtExportRequestNote.Text = t.ExportRequestNote;
                    ddlShippingTypeKyGui.SelectedValue = t.ShippingTypeID.ToString();
                    ddlWarehouseFrom.SelectedValue = t.WareHouseFromID.ToString();
                    ddlReceivePlace.SelectedValue = t.WareHouseID.ToString();

                    string dateexre = "";
                    string dateout = "";
                    if (t.DateExportRequest != null)
                        dateexre = string.Format("{0:dd/MM/yyyy}", t.DateExportRequest);
                    if (t.DateExport != null)
                        dateout = string.Format("{0:dd/MM/yyyy}", t.DateExport);
                    txtDateExportRequest.Text = dateexre;
                    txtDateExport.Text = dateout;
                    txtCancelReason.Text = t.CancelReason;

                    txtName.Text = t.ProductName;
                    if (!string.IsNullOrEmpty(t.PriceVND))
                    {
                        txtPriceVND.Text = string.Format("{0:N0}", Convert.ToDouble(t.PriceVND));
                    }
                    else
                    {
                        txtPriceVND.Text = "0";
                    }
                    if (!string.IsNullOrEmpty(t.InsurrancePrice))
                    {
                        txtInsurrancePrice.Text = string.Format("{0:N0}", Convert.ToDouble(t.InsurrancePrice));
                    }
                    else
                    {
                        txtInsurrancePrice.Text = "0";
                    }
                    if (t.IsInsurrance == true)
                        hdfCheckBox.Value = t.IsInsurrance.ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            string username_current = Session["userLoginSystem"].ToString();
            DateTime currentDate = DateTime.Now;
            int ID = ViewState["ID"].ToString().ToInt(0);
            double currency = 0;
            double percent = 0;
            var config = ConfigurationController.GetByTop1();
            if (config != null)
            {
                currency = Convert.ToDouble(config.AgentCurrency);
                percent = Convert.ToDouble(config.InsurancePercentTrans);
            }
            var a = AccountController.GetByID(Convert.ToInt32(ddlUsername.Text));
            var t = TransportationOrderNewController.GetByID(ID);
            if (t != null)
            {
                int packageID = Convert.ToInt32(t.SmallPackageID);
                int statusNew = ddlStatus.SelectedValue.ToInt();
                double PriceVND = Convert.ToDouble(txtPriceVND.Text);

                double InsurranceMoney = 0;
                bool IsInsurrance = false;                         
                var listCheck = hdfListCheckBox.Value.Split('|').ToList();
                foreach (var item in listCheck)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var ck = item.Split(',').ToList();
                        if (ck != null)
                        {
                            if (ck[0] == "1")
                            {
                                IsInsurrance = Convert.ToBoolean(ck[1].ToInt(0));
                            }                            
                        }
                    }    
                }
                if (IsInsurrance == true)
                {                   
                    InsurranceMoney = Math.Round(PriceVND * percent / 100, 0);
                }    

                TransportationOrderNewController.UpdateNew(ID, a.ID, a.Username, rWeight.Value.ToString(), currency.ToString(),
                rAdditionFeeCYN.Value.ToString(), rAdditionFeeVND.Value.ToString(), "", "", "", "", rSensorFeeCYN.Value.ToString(),
                rSensorFeeeVND.Value.ToString(), packageID, txtBarcode.Text, statusNew, txtSummary.Text,
                txtStaffNote.Text, "", "", currentDate, username_current, IsInsurrance, InsurranceMoney.ToString(), PriceVND.ToString());
                SmallPackageController.UpdateUIDandUserName(Convert.ToInt32(t.SmallPackageID), a.ID, a.Username);
                TransportationOrderNewController.UpdateKho(ID, Convert.ToInt32(ddlWarehouseFrom.SelectedValue), Convert.ToInt32(ddlReceivePlace.SelectedValue), Convert.ToInt32(ddlShippingTypeKyGui.SelectedValue));
                PJUtils.ShowMessageBoxSwAlert("Cập nhật thành công", "s", true, Page);
            }
        }

        protected void btnBackLink_Click(object sender, EventArgs e)
        {
            string prepage = Session["PrePage"].ToString();
            if (!string.IsNullOrEmpty(prepage))
            {
                Response.Redirect(prepage);
            }
            else
            {
                Response.Redirect(Request.Url.ToString());
            }
        }
    }
}