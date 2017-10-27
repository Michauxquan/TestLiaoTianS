using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProDAL;
using ProDAL.UserAttrs;
using ProEntity;
using ProEntity.Manage;

namespace ProBusiness
{
    public class BackModelSetBusiness
    {
        public static BackModelSetBusiness BaseBusiness = new BackModelSetBusiness();

        #region 查询

        public static List<BackModel> GetBackModel(int pageSize, int pageIndex, ref int totalCount, ref int pageCount,string sqlwhere="",bool asc=false)
        {
            string tablename = " BackModel a ";
            string sql = " 1=1 ";

            DataTable dt = CommonBusiness.GetPagerData(tablename, " a.* ", sql + sqlwhere, " a.WinFee ", pageSize, pageIndex, out totalCount, out pageCount, asc);
           List<BackModel> list = new List<BackModel>();
            foreach (DataRow dr in dt.Rows)
            {
                BackModel model = new BackModel();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static List<UserBackSet> GetBackSet(string userid,int type,int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            //string tablename = " UserBackSet a  join BackModel b on a.Autoid=b.Backid ";
            string tablename = " UserBackSet a ";
            string sql = " 1=1 ";
            if (!string.IsNullOrEmpty(userid))
            {
                sql += " and a.UserID='" + userid + "'";
            }
            if (type>-1)
            {
                sql += " and a.Type="+type;
            }
           // DataTable dt = CommonBusiness.GetPagerData(tablename, "a.* ,b.WinFee,b.SSCType,b.X5Type,b.FCType ", sql, " a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount, true);
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.* ", sql, " a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount, true);
            List<UserBackSet> list = new List<UserBackSet>();
            foreach (DataRow dr in dt.Rows)
            {
                UserBackSet model = new UserBackSet();
                model.FillData(dr);
                BackModel bmodel = GetBackById(model.BackID);
                model.Bmodel = bmodel;
                list.Add(model);
            }
            return list;
        }
        public static BackModel GetBackById(int BackID )
        {
            DataTable dt = BackModelSetDAL.GetDataTable("select *  from BackModel where AutoID=" + BackID); 
            List<BackModel> list = new List<BackModel>();
            foreach (DataRow dr in dt.Rows)
            {
                BackModel model = new BackModel();
                model.FillData(dr);
                list.Add(model);
            }
            return list.FirstOrDefault();
        }
        /// <summary>
        /// 获取用户分享
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<UserShare> GetUserShareList(string userid)
        { 
            DataTable dt = BackModelSetDAL.GetDataTable("select *  from UserShare where userid='" + userid + "'");
            List<UserShare> list = new List<UserShare>();
            foreach (DataRow dr in dt.Rows)
            {
                UserShare model = new UserShare();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        /// 根据二维码获取分享
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public static UserShare GetUserShare(string qrcode)
        {
            DataTable dt = BackModelSetDAL.GetDataTable("select top 1 *  from UserShare where qrcode='" + qrcode + "'");
            UserShare model = new UserShare();
            foreach (DataRow dr in dt.Rows)
            { 
                model.FillData(dr); 
            }
            return model;
        }

        public static SysSetting GetSysSetting()
        {
            DataTable dt = BackModelSetDAL.GetDataTable("select top 1 *  from SysSetting ");
            SysSetting model = new SysSetting();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr);
            }
            return model;
        }

        #endregion

        #region 添加.删除

        public static bool CreateBackModel(BackModel model)
        {
            return BackModelSetDAL.BaseProvider.CreateBackModel( model.SSCType, model.WinFee, model.FCType, model.FCFee, model.X5Type, model.X5Fee, model.MaxHigFee, model.MaxLowFee);
        }

        public static bool CreateUserShare(int backid, string uid,ref string errmsg)
        {
            return BackModelSetDAL.BaseProvider.CreateUserShare(backid, uid, ref errmsg);
        
        }


        #endregion 

        #region 修改

        public static bool UpdateBackModel(BackModel model)
        {
            return BackModelSetDAL.BaseProvider.UpdateBackModel(model.AutoID, model.SSCType, model.WinFee, model.FCType, model.FCFee, model.X5Type, model.X5Fee, model.MaxHigFee, model.MaxLowFee);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="info"></param>
        /// <param name="type">1代理配额 0自身配额</param>
        /// <returns></returns>
        public static bool UpdUserModel(string uid, string info, int type)
        {
            return BackModelSetDAL.BaseProvider.UpdUserModel(uid, info, type);
        }
        public static bool UpdUserBackSet(string uid, int backid)
        {
            return BackModelSetDAL.BaseProvider.UpdUserBackSet(uid, backid);
        }
        public static bool UserShareDel(int autoid)
        {
            return BackModelSetDAL.BaseProvider.UserShareDel(autoid);
        }

        public static bool UpdSysSet(SysSetting model)
        {
            return BackModelSetDAL.BaseProvider.UpdSysSet(model.AutoID, model.BettLock ,model.FHLock, model.RGZLock, model.RGZPoint, model.SysLotteryWin, model.GGContent, model.DrawRule, model.DrawMin, model.DrawMax, model.DrawBTime, model.DrawETime
                ,model.IsShowGG,model.KFUrl,model.MsgLock,model.PayMax,model.PayMin);
        }

        public static bool ClearData(int type,string btime)
        {
            return BackModelSetDAL.BaseProvider.ClearData(type, btime);
        }

        public static bool UpdAppsetting(int autoid, string kvalue,string minfee,string maxfee)
        {
            return BackModelSetDAL.BaseProvider.UpdAppsetting(autoid, kvalue, minfee, maxfee);
        }

        public static bool UpdSetStatus(int autoid, string clumname, int status)
        {
            return BackModelSetDAL.BaseProvider.UpdSetStatus(autoid, clumname, status);
        }

        #endregion
        
    }
}
