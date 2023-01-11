using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHST.Models;
using NHST.Bussiness;
using MB.Extensions;
using WebUI.Business;
using System.Data;
using System.Globalization;

namespace NHST.Controllers
{
    public class ComplainPayHelpController
    {
        #region CRUD
        public static string Insert(int UID, int PayHelpID, string Amount, string IMG, string ComplainText, int Status, DateTime CreatedDate, string CreatedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                tbl_ComplainPayHelp c = new tbl_ComplainPayHelp();
                c.UID = UID;
                c.PayHelpID = PayHelpID;
                c.Amount = Amount;
                c.IMG = IMG;
                c.ComplainText = ComplainText;
                c.Status = Status;
                c.CreatedDate = CreatedDate;
                c.CreatedBy = CreatedBy;
                dbe.tbl_ComplainPayHelp.Add(c);
                dbe.SaveChanges();
                string kq = c.ID.ToString();
                return kq;
            }
        }
        public static string Update(int ID, string Amount, int Status, DateTime ModifiedDate, string ModifiedBy)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_ComplainPayHelp.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    c.Amount = Amount;
                    c.Status = Status;
                    c.ModifiedDate = ModifiedDate;
                    c.ModifiedBy = ModifiedBy;
                    string kq = dbe.SaveChanges().ToString();
                    return kq;
                }
                else
                    return null;
            }
        }
        #endregion
        #region Select
        public static List<tbl_ComplainPayHelp> GetByUID(int UID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_ComplainPayHelp> cs = new List<tbl_ComplainPayHelp>();
                cs = dbe.tbl_ComplainPayHelp.Where(c => c.UID == UID).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static List<tbl_ComplainPayHelp> GetAll(string s)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_ComplainPayHelp> cs = new List<tbl_ComplainPayHelp>();
                cs = dbe.tbl_ComplainPayHelp.Where(c => c.CreatedBy.Contains(s)).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static List<tbl_ComplainPayHelp> GetAllByPayHelpIDAndUID(int UID, int PayHelpID)
        {
            using (var dbe = new NHSTEntities())
            {
                List<tbl_ComplainPayHelp> cs = new List<tbl_ComplainPayHelp>();
                cs = dbe.tbl_ComplainPayHelp.Where(c => c.UID == UID && c.PayHelpID == PayHelpID).OrderByDescending(c => c.ID).ToList();
                return cs;
            }
        }
        public static tbl_ComplainPayHelp GetByID(int ID)
        {
            using (var dbe = new NHSTEntities())
            {
                var c = dbe.tbl_ComplainPayHelp.Where(p => p.ID == ID).FirstOrDefault();
                if (c != null)
                {
                    return c;
                }
                else
                    return null;
            }
        }
        #endregion

        public static List<tbl_ComplainPayHelp> GetByUID_SQL(int UID)
        {
            var list = new List<tbl_ComplainPayHelp>();
            var sql = @"select * from tbl_ComplainPayHelp ";
            sql += " where UID = " + UID + "";
            sql += " Order By ID desc";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);

            int i = 1;
            while (reader.Read())
            {
                var entity = new tbl_ComplainPayHelp();
                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);
                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);
                if (reader["PayHelpID"] != DBNull.Value)
                    entity.PayHelpID = reader["PayHelpID"].ToString().ToInt(0);

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                if (reader["Amount"] != DBNull.Value)
                    entity.Amount = reader["Amount"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                i++;
                list.Add(entity);
            }
            reader.Close();
            return list;

        }

        public static int GetTotal(string s, string fd, string td, int Status)
        {
            var sql = @"select Total=Count(*) ";
            sql += "from tbl_ComplainPayHelp ";
            sql += "Where CreatedBy LIKE N'%" + s + "%' ";
            if (Status > -1)
            {
                sql += " And Status=" + Status + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            int a = 0;
            while (reader.Read())
            {
                if (reader["Total"] != DBNull.Value)
                    a = reader["Total"].ToString().ToInt(0);

            }
            reader.Close();
            return a;
        }
        public static List<tbl_ComplainPayHelp> GetAllBySQL(string s, int pageIndex, int pageSize, string fd, string td, int Status)
        {
            var sql = @"select * ";
            sql += "from tbl_ComplainPayHelp ";
            sql += "Where CreatedBy LIKE N'%" + s + "%' ";
            if (Status > -1)
            {
                sql += " And Status=" + Status + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += "AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += "order by id DESC OFFSET " + pageIndex + "*" + pageSize + " ROWS FETCH NEXT " + pageSize + " ROWS ONLY";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_ComplainPayHelp> a = new List<tbl_ComplainPayHelp>();
            while (reader.Read())
            {
                var entity = new tbl_ComplainPayHelp();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["PayHelpID"] != DBNull.Value)
                    entity.PayHelpID = reader["PayHelpID"].ToString().ToInt(0);

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());

                if (reader["Amount"] != DBNull.Value)
                {
                    entity.Amount =  Convert.ToDouble(reader["Amount"].ToString()).ToString() + " ¥";
                    //entity.Amount = Convert.ToDouble(reader["Amount"].ToString()).ToString("0,0", CultureInfo.InvariantCulture) + " ¥";
                }
                //if (reader["Status"] != DBNull.Value)
                //{
                //    entity.Status = reader["Status"].ToString().ToInt(0);
                //    //if (reader["Status"].ToString().ToInt(0) == 1)
                //    //    entity.StatusName = "<span class=\"badge orange darken-2 white-text border-radius-2\">Đang chờ</span>";
                //    //if (reader["Status"].ToString().ToInt(0) == 2)
                //    //    entity.StatusName = "<span class=\"badge green darken-2 white-text border-radius-2\">Duyệt</span>";
                //    //if (reader["Status"].ToString().ToInt(0) == 3)
                //    //    entity.StatusName = "<span class=\"badge red darken-2 white-text border-radius-2\">Hủy</span>";
                //}

                if (reader["CreatedDate"] != DBNull.Value)
                {
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                    //entity.CreatedDateString = Convert.ToDateTime(reader["CreatedDate"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                }
                a.Add(entity);
            }
            reader.Close();
            return a;
        }

        public static List<tbl_ComplainPayHelp> GetAllBySQLExcel(string s, string fd, string td, int Status)
        {
            var sql = @"select * ";
            sql += "from tbl_ComplainPayHelp ";
            sql += "Where CreatedBy LIKE N'%" + s + "%' ";
            if (Status > -1)
            {
                sql += " AND Status=" + Status + " ";
            }
            if (!string.IsNullOrEmpty(fd))
            {
                var df = DateTime.ParseExact(fd, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate >= CONVERT(VARCHAR(24),'" + df + "',113) ";
            }
            if (!string.IsNullOrEmpty(td))
            {
                var dt = DateTime.ParseExact(td, "dd/MM/yyyy HH:mm", null);
                sql += " AND CreatedDate <= CONVERT(VARCHAR(24),'" + dt + "',113) ";
            }
            sql += "order by id DESC ";
            var reader = (IDataReader)SqlHelper.ExecuteDataReader(sql);
            List<tbl_ComplainPayHelp> a = new List<tbl_ComplainPayHelp>();
            while (reader.Read())
            {
                var entity = new tbl_ComplainPayHelp();

                if (reader["ID"] != DBNull.Value)
                    entity.ID = reader["ID"].ToString().ToInt(0);

                if (reader["PayHelpID"] != DBNull.Value)
                    entity.PayHelpID = reader["PayHelpID"].ToString().ToInt(0);

                if (reader["UID"] != DBNull.Value)
                    entity.UID = reader["UID"].ToString().ToInt(0);

                if (reader["CreatedBy"] != DBNull.Value)
                    entity.CreatedBy = reader["CreatedBy"].ToString();

                if (reader["ComplainText"] != DBNull.Value)
                    entity.ComplainText = reader["ComplainText"].ToString();

                if (reader["Status"] != DBNull.Value)
                    entity.Status = Convert.ToInt32(reader["Status"].ToString());

                if (reader["Amount"] != DBNull.Value)
                    entity.Amount = reader["Amount"].ToString();

                if (reader["CreatedDate"] != DBNull.Value)
                    entity.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());

                a.Add(entity);
            }
            reader.Close();
            return a;
        }
    }
}