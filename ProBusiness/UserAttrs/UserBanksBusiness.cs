using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness.UserAttrs
{
    public class UserBanksBusiness
    {
        #region 查询
        /// <summary>
        /// 获取银行卡
        /// </summary>
        /// <returns></returns>
        public static List<UserBanks> GetBanks(string type, string userid,int pageSize, int pageIndex, ref int totalCount, ref int pageCount,string keyword="")
        {
            string tablename = "UserBanks  a join M_Users b on a.userid=b.userid ";

            string sqlwhere = " 1=1 "; 
            if (!string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type in(" + type + ")";
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.userid='"+userid+"' ";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                sqlwhere += " and b.LoginName='" + keyword + "' ";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.LoginName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserBanks> list = new List<UserBanks>();
            foreach (DataRow dr in dt.Rows)
            {
                UserBanks model = new UserBanks();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static UserBanks GetBankDetail(int autoid)
        {
            DataTable dt = UserBanksDAL.BaseProvider.GetBankDetail(autoid);
            UserBanks model = new UserBanks();
            foreach (DataRow dr in dt.Rows)
            { 
                model.FillData(dr); 
            }
            return model;
        }

        public static bool InsertBanks(UserBanks model,ref string errmsg)
        {
            return UserBanksDAL.BaseProvider.Create(model.UserID, model.CardCode, model.BankName, model.BankChild,
                model.TrueName, model.BankPre, model.BankCity, model.Type,model.IDCard, ref errmsg);
        }

        public static int GetCount(string  userid)
        {
            return Convert.ToInt32(CommonBusiness.Select( "UserBanks","count(1)", " UserID='" + userid + "'"));
        }

        public static bool UpdateStatus(string autoids, int status)
        {
            return UserBanksDAL.BaseProvider.UpdateStatus(autoids, status);
        }
        public static bool UpdateStatus(string userid)
        {
            return UserBanksDAL.BaseProvider.UpdateStatus(userid);
        }

        public static bool DeleteBanks(int autoid)
        {
            return CommonBusiness.Delete(" UserBanks"," Autoid="+autoid);
        }


        public static bool UpdateBanks(UserBanks model, ref string errmsg)
        {
            return UserBanksDAL.BaseProvider.UpdateModel(model.AutoID, model.CardCode, model.BankName, model.BankChild,
                model.TrueName, model.BankPre, model.BankCity, model.Type, model.IDCard, ref errmsg);
        }

        #endregion
    }
}
