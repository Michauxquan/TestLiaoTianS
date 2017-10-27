using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProDAL;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness
{
   public class UserOrdersBusiness
    {
       public static UserOrdersBusiness BaseBusiness = new UserOrdersBusiness();

        #region 查询

       public static List<UserOrders> GetUserOrders(string keyWords, string userid, int type, int status, int payway, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = "UserOrders  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.UserName like '%" + keyWords + "%' or a.BankName like '%" + keyWords + "%' or a.OrderCode like '%" + keyWords + "%'  or a.Sku like '%" + keyWords + "%' or a.OtherCode like '%" + keyWords + "%')";
            }
            if (type > -1)
            {
                sqlwhere += " and a.Type=" + type;
            }
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
            }
            if (payway > -1)
            {
                sqlwhere += " and a.PayType=" + payway;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.UserID='" + userid + "' ";
            } 
           if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.LoginName UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserOrders> list = new List<UserOrders>();
            foreach (DataRow dr in dt.Rows)
            {
                UserOrders model = new UserOrders();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
       public static UserOrders GetUserOrderDetail(string ordercode)
       {
           DataTable dt = UserOrdersDAL.BaseProvider.GetUserOrderDetail(ordercode);
           UserOrders model = null;
           if (dt.Rows.Count == 1)
           {
               model = new UserOrders();
               model.FillData(dt.Rows[0]);
           }
           return model;
       } 

        public static List<UserOrders> GetUserDraws(string keyWords, string userid, int type, int status, int payway, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
       {
           string tablename = "UserDraws  a left join M_Users b  on a.UserID =b.UserID ";
           string sqlwhere = " a.status<>9 ";
           if (!string.IsNullOrEmpty(keyWords))
           {
               sqlwhere += " and (b.UserName like '%" + keyWords + "%' or a.BankName like '%" + keyWords + "%' or a.OrderCode like '%" + keyWords + "%'  or a.Sku like '%" + keyWords + "%' or a.OtherCode like '%" + keyWords + "%')";
           }
           if (type > -1)
           {
               sqlwhere += " and a.Type=" + type;
           }
           if (status > -1)
           {
               sqlwhere += " and a.status=" + status;
           }
           if (payway > -1)
           {
               sqlwhere += " and a.PayType=" + payway;
           }
           if (!string.IsNullOrEmpty(userid))
           {
               sqlwhere += " and a.UserID='" + userid + "' ";
           }
           if (!string.IsNullOrEmpty(begintime))
           {
               sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
           }
           if (!string.IsNullOrEmpty(endtime))
           {
               sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
           }
           DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.LoginName UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
           List<UserOrders> list = new List<UserOrders>();
           foreach (DataRow dr in dt.Rows)
           {
               UserOrders model = new UserOrders();
               model.FillData(dr);
               list.Add(model);
           }
           return list;
       }
       public static UserOrders GetUserDrawDetail(string ordercode)
       {
           DataTable dt = UserOrdersDAL.BaseProvider.GetUserDrawDetail(ordercode);
           UserOrders model = null;
           if (dt.Rows.Count == 1)
           {
               model = new UserOrders();
               model.FillData(dt.Rows[0]);
           }
           return model;
       }
        #endregion

        #region 添加.删除

       public static bool CreateUserOrder(string levelid, int paytype, string orderCode, string UserID,ref string msg)
        {
            return UserOrdersDAL.BaseProvider.CreateUserOrder(levelid, paytype, orderCode, UserID,out msg);
        }
       public static bool CreateUserOrder(string ordercode, int paytype, string spname, string bankinfo,string sku, string content, decimal totalfee, string othercode, int type, decimal num, decimal payfee,string userID,string createuserid,string operatip)
       {
           return UserOrdersDAL.BaseProvider.CreateUserOrder(ordercode, paytype, spname, bankinfo, sku, content, totalfee, othercode, type, num, payfee, userID, createuserid, operatip);
       }
        public static bool DeleteOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("UserOrders", "Status", 9, "ordercode='" + ordercode + "'");
            return bl;
        }
        public static bool BoutOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("UserOrders", "Status", 3, "ordercode='" + ordercode + "' and Status=0");
            return bl;
        }

       public static bool OrderAuditting(string ordercode, string othercode,decimal payfee)
       {
           bool bl = UserOrdersDAL.BaseProvider.OrderAuditting(ordercode, othercode, payfee);
           return bl;
       }
       public static string CreateUserDraw(string ordercode, int bankid, decimal totalfee, string createuserid, string operatip)
       {
           return UserOrdersDAL.BaseProvider.CreateUserDraw(ordercode, bankid, totalfee, createuserid, operatip);
       }
       public static bool DeleteDraw(string ordercode)
       {
           bool bl = CommonBusiness.Update("UserDraws", "Status", 9, "ordercode='" + ordercode + "'");
           return bl;
       }
       public static bool BoutDraw(string ordercode)
       {
           bool bl = CommonBusiness.Update("UserDraws", "Status", 3, "ordercode='" + ordercode + "' and Status=0");
           return bl;
       }

       public static bool DrawAuditting(string ordercode, int status)
       {
           bool bl = UserOrdersDAL.BaseProvider.DrawAuditting(ordercode, status);
           return bl;
       }

       #endregion 
    }
}
