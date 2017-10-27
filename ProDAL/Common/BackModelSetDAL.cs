using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ProDAL
{
    public class BackModelSetDAL : BaseDAL
    {
        public static BackModelSetDAL BaseProvider = new BackModelSetDAL();

        #region 新增
        public bool CreateBackModel(decimal ssctype, decimal sscfee, decimal fctype, decimal fcfee, decimal x5type, decimal x5fee, decimal maxhigfee, decimal maxlowfee)
        {
            SqlParameter[] paras =
            { 
                new SqlParameter("@ssctype", ssctype),
                new SqlParameter("@sscfee", sscfee),
                new SqlParameter("@fctype", fctype),
                new SqlParameter("@fcfee", fcfee),
                new SqlParameter("@x5type", x5type),
                new SqlParameter("@x5fee", x5fee), 
                new SqlParameter("@maxhigfee", maxhigfee),
                new SqlParameter("@maxlowfee", maxlowfee)
            };
            return ExecuteNonQuery("Insert into  BackModel([ssctype],[sscfee],fctype,fcfee,x5type,x5fee,maxhigfee,maxlowfee) values (@ssctype,@sscfee,@fctype,@fcfee,@x5type,@x5fee,@maxhigfee,@maxlowfee)", paras, CommandType.Text) > 0;
        }
        public bool CreateUserShare( int backid,string uid,ref string errmsg)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@ErrorMsg",SqlDbType.VarChar,100), 
                new SqlParameter("@uid", uid), 
                new SqlParameter("@backid", backid) 
            };
            paras[0].Direction = ParameterDirection.Output;
            int result= ExecuteNonQuery("M_CreateUserShare", paras, CommandType.StoredProcedure);
            errmsg = paras[0].Value.ToString();
            return result>0;
        }

        #endregion

        #region 修改

        public bool UpdateBackModel(int autoid,decimal ssctype, decimal sscfee, decimal fctype, decimal fcfee, decimal x5type, decimal x5fee, decimal maxhigfee, decimal maxlowfee)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@autoid", autoid), 
                new SqlParameter("@sscfee", sscfee),
                new SqlParameter("@fctype", fctype),
                new SqlParameter("@fcfee", fcfee),
                new SqlParameter("@x5type", x5type),
                new SqlParameter("@x5fee", x5fee), 
                new SqlParameter("@maxhigfee", maxhigfee),
                new SqlParameter("@maxlowfee", maxlowfee)
            };
            return ExecuteNonQuery("Update BackModel set sscfee=@sscfee,[ssctype]=@ssctype,fctype=@fctype,fcfee=@fcfee,x5type=@x5type,x5fee=@x5fee,maxhigfee=@maxhigfee,maxlowfee=@maxlowfee   where autoid=@autoid ", paras, CommandType.Text) > 0;
        }

        public bool UpdUserModel(string uid, string info, int type)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@uid", uid), 
                new SqlParameter("@info", info),
                new SqlParameter("@type", type)
            };
            return ExecuteNonQuery("M_UpdateUserModel", paras, CommandType.StoredProcedure) > 0;
        }


        public bool UserShareDel(int autoid)
        {
            return ExecuteNonQuery(" delete from UserShare where autoid=" + autoid) > 0;
        }

        public bool UpdSysSet(int autoid, int bettlock, int fhlock,int rgzlock,string rgzpoint,decimal syswinpoint,string ggcontent,decimal drawrule,decimal drawmin ,decimal drawmax,string drawbtime,
            string drawetime,int isshow,string kfurl,int msglock,decimal paymin,decimal paymax)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@autoid", autoid), 
                new SqlParameter("@bettlock",bettlock), 
                new SqlParameter("@fhlock", fhlock),
                new SqlParameter("@rgzlock", rgzlock),
                new SqlParameter("@rgzpoint", rgzpoint),
                new SqlParameter("@syswinpoint", syswinpoint),
                new SqlParameter("@ggcontent", ggcontent),
                new SqlParameter("@drawrule", drawrule),
                new SqlParameter("@drawmin", drawmin),
                new SqlParameter("@drawmax", drawmax),
                new SqlParameter("@drawbtime", drawbtime),
                new SqlParameter("@drawetime", drawetime),
                new SqlParameter("@isshowgg", isshow),
                new SqlParameter("@kfurl", kfurl),
                new SqlParameter("@msglock", msglock),
                new SqlParameter("@paymin", paymin),
                new SqlParameter("@paymax", paymax)
            };
            return ExecuteNonQuery(" update SysSetting set BettLock=@bettlock, FHLock=@fhlock,RGZLock=@rgzlock,RGZPoint=@rgzpoint,SysLotteryWin=@syswinpoint,GGContent=@ggcontent" +
                                   ",DrawRule=@drawrule,DrawMin=@drawmin,DrawMax=@drawmax,DrawBTime=@drawbtime,DrawETime=@drawetime,IsShowGG=@isshowgg,KFUrl=@kfurl" +
                                   ",MsgLock=@msglock,PayMax=@paymin,PayMin=@paymax " +
                                   "  where Autoid=@autoid", paras, CommandType.Text) > 0;
        }

        public bool ClearData(int type,string btime)
        {
            string sqltext = " delete from LotteryResult where CreateTime<'" + btime + "' " +
                             " delete from SyslotteryCPYK where CreateTime<<'" + btime + "' or CreateTime is null ";
            if (type == 1)
            {
                sqltext += " delete from AccountOperateRecord where CreateTime<'" + btime + "' ";
                sqltext += " delete from UsersLog where CreateTime<'" + btime + "' ";
                sqltext += " delete from LotteryOrder where CreateTime<'" + btime + "' ";
                sqltext += " delete from UserOrders where CreateTime<'" + btime + "' ";
                sqltext += " delete from UserDraws where CreateTime<'" + btime + "' ";
                sqltext += " delete from LotteryBettAuto where UpdateTime<'" + btime + "'  or UpdateTime is null";
            }
            SqlParameter[] para = {};
            return ExecuteNonQuery(sqltext, para, CommandType.Text) > 0;
        }

        public bool UpdUserBackSet(string uid, int backid)
        {
            SqlParameter[] para = { };
            return ExecuteNonQuery("update UserbackSet set backid="+backid+" where userid='"+uid+"' and type=0 ", para, CommandType.Text) > 0;
        }

        public bool UpdAppsetting(int autoid, string kvalue,string minfee,string maxfee)
        {
            SqlParameter[] para = { };
            return ExecuteNonQuery("update APPSetting set kvalue='" + kvalue + "',minfee='" + minfee + "',MaxFee='" + maxfee + "' where AutoID=" + autoid + " ", para, CommandType.Text) > 0;
        }

        public bool UpdSetStatus(int autoid, string clumname, int status)
        {
            SqlParameter[] para = { };
            return ExecuteNonQuery("update APPSetting set " + clumname + "=" + status + " where AutoID=" + autoid + " ", para, CommandType.Text) > 0;
        }
        #endregion
    }
}
