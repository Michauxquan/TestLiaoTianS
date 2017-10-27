using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ProDAL;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness.UserAttrs
{
    public class UserReportBussiness
    {
        public static List<UserReportDay> GetReportList(string btime, string etime,string userid, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            string whereSql = " b.Status<>9 ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and a.UserID='" + userid + "'";
            }
            string clumstr = "a.*,b.LoginName as UserName";
            DataTable dt = CommonBusiness.GetPagerData("UserReportDay a join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }
        public static List<UserReportDay> GetReporSumtList(string btime, string etime, string userid, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            string whereSql = " b.Status<>9 ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and ( b.userid='" + userid + "' or a.Userid in(select userid  from dbo.UserRelation where ParentID ='" + userid + "' ) ) ";
            }
            whereSql += "  group by  a.UserID,b.LoginName ";
            string clumstr = "a.UserID,b.loginName username,SUM(UserPoint) UserPoint,SUM(TotalPay) TotalPay,0-SUM(TotalDraw) TotalDraw,0-SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn ,SUM(TotalPayMent)-SUM(UserPoint)-SUM(TotalWin)-SUM(TotalReturn) YL,Sum(TotalActive) TotalActive,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH ";
            DataTable dt = CommonBusiness.GetPagerData("UserReportDay a join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "b.LoginName", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }
        public static List<UserReportDay> GetReportAllDays(string btime, string etime,string userid)
        {
            string whereSql = " b.status<>9  ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and ( a.userid='" + userid + "' or a.Userid in(select userid  from dbo.UserRelation where ParentID ='" + userid + "' ) ) ";
            }
            string clumstr = "1 as AutoID ,SUM(UserPoint) UserPoint,SUM(TotalPay) TotalPay,0-SUM(TotalDraw) TotalDraw,0-SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn ,SUM(TotalPayMent)-SUM(UserPoint)-SUM(TotalWin)-SUM(TotalReturn) YL,Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH ";
            int totalCount = 0;
            DataTable dt = CommonBusiness.GetPagerData(" UserReportDay a join M_users b on a.Userid=b.userid  ", clumstr, whereSql, " AutoID", 1, 1, out totalCount, out totalCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 日工资
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="status"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public static List<UserReportDay> GetRGZList(string keywords, int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string btime = "", string etime = "")
        {
            string whereSql = " b.status<>9  ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                whereSql += " and ( a.userid='" + keywords + "' or b.LoginName like '%" + keywords + "%' ) ";
            }
            string clumstr = " a.*,b.LoginName UserName ";
            DataTable dt = CommonBusiness.GetPagerData(" UserDayWage a join M_users b on a.Userid=b.userid  ", clumstr, whereSql, " a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }
        /// <summary>
        /// 分红列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="status"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public static List<UserReportDay> GetFHList(string keywords, int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string btime = "", string etime = "")
        {
            string whereSql = " b.status<>9  ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                whereSql += " and ( a.userid='" + keywords + "' or b.LoginName like '%" + keywords + "%' ) ";
            }
            string clumstr = " a.*,b.LoginName UserName ";
            DataTable dt = CommonBusiness.GetPagerData(" UserProfitShare a join M_users b on a.Userid=b.userid  ", clumstr, whereSql, " a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 分红统计列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="status"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public static List<UserReportDay> GetFHSumList(string keywords, int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string btime = "", string etime = "")
        {
            string whereSql = " b.status<>9  ";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                whereSql += " and ( a.userid='" + keywords + "' or b.LoginName like '%" + keywords + "%' ) ";
            }
            whereSql += "  group by  a.UserID,b.LoginName ";
            string clumstr = " a.UserID,b.LoginName username,SUM(UserPoint) UserPoint ,SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn ,SUM(YL) YL,Sum(GainFee) GainFee";
            DataTable dt = CommonBusiness.GetPagerData(" UserProfitShare a join M_users b on a.Userid=b.userid  ", clumstr, whereSql, " b.LoginName", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>

        public static List<UserReportDay> GetReporSumtList(string btime, string etime, int pageIndex, int pageSize, ref int totalCount, ref int pageCount,string userid="")
        {
            string whereSql = " 1=1";
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.ReportTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.Userid='" + userid + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.ReportTime<'" + etime + "'";
            }
            //whereSql += " and (a.UserPoint>0 or a.TotalPay>0 or a.TotalDraw>0 or a.TotalPayMent>0 or a.TotalWin>0 or a.TotalReturn>0) ";
            whereSql += "  group by  a.ReportTime ";
            string clumstr = "ReportTime, SUM(UserPoint) UserPoint,SUM(TotalPay) TotalPay,SUM(TotalDraw) TotalDraw,SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn, SUM(TotalPayMent)-SUM(UserPoint)-SUM(TotalWin)-SUM(TotalReturn) YL ,Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH";
            DataTable dt = CommonBusiness.GetPagerData("UserReportDay a  ", clumstr, whereSql, "a.ReportTime", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 月 周 日 统计
        /// </summary>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="sqlwhere"> MONTH(ReportTime)=MONTH(dateadd(month,-1,getdate()))</param>
        /// <param name="groupby"> 上月:convert(varchar(7),ReportTime,120) </param>
        /// <returns></returns>
        public static List<UserReportDay> GetReporTimeList(string sqlwhere,string groupby)
        {
            string whereSql = " where 1=1 ";
            whereSql +=" and "+sqlwhere+ "  group by  " + groupby;
            string clumstr =
                groupby + "  as ReportTime,SUM(UserPoint) UserPoint,SUM(TotalPay) TotalPay,0-SUM(TotalDraw) TotalDraw,0-SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn, SUM(TotalPayMent)-SUM(UserPoint)-SUM(TotalWin)-SUM(TotalReturn) YL,Sum(TotalActive) TotalActive,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH ";
            DataTable dt = UserOrdersDAL.GetDataTable("SET DATEFIRST 1  select " + clumstr + " from UserReportDay   " + whereSql);
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        public static List<LotteryOrder> GetLotteryOrderReportList(string btime, string etime, int playtype, string cpcode, string userid, string lcode, string issuenum, string type, string state, int winType, int pageIndex, int pageSize, ref int totalCount, ref int pageCount, int self = 0, string username="")
        {

            string whereSql = " a.AutoID>0 ";

            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.CreateTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.CreateTime<'" + etime + "'";
            }
            if (playtype > -1)
            {
                whereSql += " and a.playtype="+playtype;
            }
            if (!string.IsNullOrEmpty(type))
            {
                whereSql += " and b.Type like '%" + type + "%'";
            } 
            if (!string.IsNullOrEmpty(cpcode))
            {
                whereSql += " and b.cpcode='" + cpcode + "'";
            }
            if (winType > -1)
            {
                whereSql += " and b.WinType=" + winType;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                if (self > 0)
                {
                    if (self == 1)
                    {
                        whereSql += " and a.UserID in(select UserID from UserRelation where ParentID='" + userid + "')";
                    }
                    else if (self == 2)
                    {
                        whereSql += " and a.UserID in(select UserID from UserRelation where Parents like '%" + userid +
                                    "%')";
                    }
                    else
                    {
                        whereSql += " and a.UserID='" + userid + "' ";
                    }
                }
                else
                {
                    whereSql += " and ( a.UserID in(select UserID from UserRelation where Parents like '%" + userid + "%') or  a.UserID='" + userid + "')";
                }
            }
            if (!string.IsNullOrEmpty(username))
            {
                whereSql += " and c.LoginName='" + username.Trim() + "' "; 
            }
            if (!string.IsNullOrWhiteSpace(lcode))
            {
                if (!string.IsNullOrEmpty(state) )
                { 
                    whereSql += " and b." + state + " ='" + lcode + "'"; 
                } 
                else
                {
                    whereSql += " and (b.LCode like '%" + lcode + "%' or b.BCode like '%" + lcode + "%') ";
                }
            }
            if (!string.IsNullOrEmpty(issuenum))
            {
                whereSql += " and b.IssueNum ='" + issuenum + "'";
            }
            string clumstr = @" isnull(b.LCode,'--') as LCode,b.IssueNum,b.Type,b.TypeName,b.CPCode,b.CPName,a.AccountChange, a.Type AccountType,
case when a.Type=0 then a.AccountChange else b.WinFee end WinFee,
case when a.Type<2 then a.CreateTime else b.CreateTime end CreateTime,
case when a.Type=1 then a.AccountChange else isnull(b.PayFee,0.00) end PayFee ,
isnull(b.Remark,'') Remark, a.AutoID,a.Account ,a.PlayType,a.Remark PlayTypeName,c.LoginName UserName ,a.Type ChangeType ";
            DataTable dt = CommonBusiness.GetPagerData(" AccountOperateRecord a join M_Users c on a.UseriD=c.Userid left join LotteryOrder b on a.Userid=b.Userid and a.FkCode=b.LCode and b.Status<>9 ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow item in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }


        public static List<LotteryOrder> GetAccReportList(string btime, string etime, int playtype,  string userid, int pageIndex, int pageSize, ref int totalCount, ref int pageCount, int self = 0, string username = "")
        {

            string whereSql = " a.AutoID>0 ";

            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.CreateTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.CreateTime<'" + etime + "'";
            }
            if (playtype > -1)
            {
                whereSql += " and a.playtype=" + playtype;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                if (self > 0)
                {
                    if (self == 1)
                    {
                        whereSql += " and a.UserID in(select UserID from UserRelation where ParentID='" + userid + "')";
                    }
                    else if (self == 2)
                    {
                        whereSql += " and a.UserID in(select UserID from UserRelation where Parents like '%" + userid +
                                    "%')";
                    }
                    else
                    {
                        whereSql += " and a.UserID='" + userid + "' ";
                    }
                }
                else
                {
                    whereSql += " and ( a.UserID in(select UserID from UserRelation where Parents like '%" + userid + "%') or  a.UserID='" + userid + "')";
                }
            }
            if (!string.IsNullOrEmpty(username))
            {
                whereSql += " and c.LoginName='" + username.Trim() + "' ";
            }
            string clumstr = @" a.AccountChange, a.Type AccountType,  a.CreateTime  , '' Remark, a.AutoID,a.Account,a.PlayType,a.Remark PlayTypeName,c.LoginName UserName ,a.Type ChangeType ";
            DataTable dt = CommonBusiness.GetPagerData(" AccountOperateRecord a join M_Users c on a.UseriD=c.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount,false);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow item in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }


        public static List<UserAccountDay> GetUserAccountDay(string userid, string btime, string etime)
        {
            string str =
                string.Format(
                    @"select  sum(AccountChange) Account ,count(type) AllCount,0 type  from [dbo].[AccountOperateRecord] where type=0
and userid='{0}' and Createtime>='{1}'  and CreateTime<='{2}'  and AccountChange>0
union all 
select (0- sum(AccountChange)) Account ,count(type) AllCount,1 type  from [dbo].[AccountOperateRecord] where type=1 and userid='{0}' and Createtime>'{1}' and CreateTime<='{2}' and AccountChange>0 ", userid, btime, etime);
            DataTable dt = UserReplyDAL.GetDataTable(str);
            List<UserAccountDay> list = new List<UserAccountDay>();
            foreach (DataRow item in dt.Rows)
            {
                UserAccountDay model = new UserAccountDay();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        public static List<UserReportDay> GetUserRPTList( string userid,int pageindex,int pagesize,int isxj,ref int totalcount,ref int pagecount,string btime,string endtime,
            ref UserReportDay listpage, string username="")
        {
            string whereSql = " ";

            if (!string.IsNullOrEmpty(username))
            {
                if (isxj == 0)
                {
                    whereSql += " and  Userid in (select userid from Userrelation where ParentID='" + userid + "')  and LoginName = '" + username + "'";
                }
                else if (isxj == 1)
                {
                    whereSql += " and( Userid in (select userid from Userrelation where parents like'%" + userid + "%') and  ( LoginName = '" + username + "' or Userid in (select userid from Userrelation where parentid in (select top 1 UserID from M_Users where  LoginName = '" + username + "') )) )";
                }
                else
                {
                    whereSql += " and  Userid in (select userid from Userrelation where parents like '%" + userid +
                                "%') and Userid in (select userid from Userrelation where Parents like '%'+ (select top 1 UserID from M_Users where  LoginName = '" +
                                username + "')+'%')";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    if (isxj == 0)
                    {
                        whereSql += " and UserID = '" + userid + "'";
                    }
                    else if (isxj == 1)
                    {
                        whereSql += " and( UserID = '" + userid + "' or Userid in (select userid from Userrelation where ParentID='" + userid + "') ) ";
                    }
                    else
                    {
                        whereSql += " and  Userid in (select userid from Userrelation where Parents like '%" + userid + "%') ";
                    }
                }
            }
            string sql = string.Format(@"
declare @today nvarchar(25)='{0}'
declare @endtoday nvarchar(25)='{1}'
	
if OBJECT_ID('tempdb..#userreport') is not null
drop table #userreport

select UserID,LoginName,cast(0 as decimal(18,4)) UserPoint,cast(0 as decimal(18,4)) TotalPay,
 cast(0 as decimal(18,4)) TotalDraw,cast(0 as decimal(18,4))  TotalPayMent, cast(0 as decimal(18,4)) TotalWin,
cast(0 as decimal(18,4))  TotalReturn,cast(0 as decimal(18,4))  TotalZHReturn,cast(0 as decimal(18,4))  TotalActive,
cast(0 as decimal(18,4))  TotalXJ,cast(0 as decimal(18,4))  TotalFenH,cast(0 as decimal(18,4)) TotalRiGZ,cast(0 as decimal(18,4)) SumFee 
into #userreport
from M_Users 
where Status <>9 and UserID !='993b30f7-a8c4-49f9-b2bc-0b629d34bb76' {2}



update a set TotalPay= b.Num from  #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(1,14) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
update a set TotalPayMent= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(4,5) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID
update a set TotalWin= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType=8 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID

update a set TotalDraw= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType=2 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--下级返点
update a set TotalReturn= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType=17 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--用户返点
update a set UserPoint= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType=7 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--追号撤单返款
update a set TotalZHReturn= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(6,9) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID  
--活动奖金
update a set TotalActive= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(20,21,22) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--转账下级
update a set TotalXJ= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(23) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--日工资
update a set TotalRiGZ= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock) where  PlayType in(18) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 

--分红
update a set TotalFenH= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord (nolock)  where  PlayType in(19) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 

--团队总额
update a set SumFee= b.Num from   #userreport a join (
	select sum(isnull(c.AccountFee,'0'))+min(a.AccountFee) as Num,a.Userid from UserAccount (nolock)  a left join 
    Userrelation b on charindex(a.userid,Parents)>0 
    left join UserAccount c   on c.UserID=b.UserID 
	group  by a.userid

) b on a.UserID=b.userID 

update  #userreport set TotalPayMent=TotalPayMent-TotalZHReturn


--统计 
if object_id('tempdb..#list') is not null
drop table #list

select row_number() over( order by b.Autoid asc) id,b.Type,b.AutoID,b.UserID,b.SafeLevel,
a.LoginName,AccountFee,UserPoint,TotalPay,TotalDraw,b.CreateTime,SumFee,
TotalPayMent,TotalWin,TotalReturn,YL ,TotalActive ,TotalXJ,TotalRiGZ,TotalFenH
into #list
from
(
select 
b.LoginName,min(c.AccountFee) AccountFee,SUM(UserPoint) UserPoint,
SUM(TotalPay) TotalPay,0-SUM(TotalDraw) TotalDraw,SUM(TotalPayMent) TotalPayMent,
SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn, max(isnull(SumFee,0)) SumFee,
0-SUM(TotalPayMent)+SUM(UserPoint)+SUM(TotalWin)+SUM(TotalReturn)+Sum(TotalActive)+Sum(TotalRiGZ)+Sum(TotalFenH) YL ,
Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH
from #userreport a  
join M_Users b on a.userid=b.userid 
join UserAccount c on b.userid=c.userid 
group by  b.LoginName  
) a
join M_Users b on a.LoginName=b.LoginName 

declare @totalcount int=0,@pagecount int=0

select @totalcount=count(1) from #list

set @pagecount=CEILING(@totalCount * 1.0/{4})

select @totalcount totalcount, @pagecount pagecount

select *
from #list where id>({3}-1)*{4} and id<={3}*{4}

select 
sum(AccountFee) AccountFee,SUM(UserPoint) UserPoint,max(SumFee) SumFee,0 SafeLevel,
SUM(TotalPay) TotalPay,SUM(TotalDraw) TotalDraw,SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,
SUM(TotalReturn) TotalReturn, Sum(YL) YL ,Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,
Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH 
from #list
", btime+" 00:00:00",endtime+ " 23:59:59",  whereSql, pageindex, pagesize);

            DataSet ds = UserOrdersDAL.GetDataSet(sql);
            DataTable dt1 = ds.Tables[0];
            totalcount = int.Parse(dt1.Rows[0]["totalcount"].ToString());
            pagecount = int.Parse(dt1.Rows[0]["pagecount"].ToString());
            List<UserReportDay> list = new List<UserReportDay>();
            foreach (DataRow item in ds.Tables[1].Rows)
            {
                UserReportDay model = new UserReportDay();
                model.FillData(item);
                list.Add(model);
            }

            listpage = new UserReportDay();
            foreach (DataRow item in ds.Tables[2].Rows)
            {
                listpage.FillData(item);
            } 
            return list;
        }
        public static UserReportDay GetUserTeamRPT(string userid, string btime, string endtime, string username = "")
        {
            string whereSql = " ";

            if (!string.IsNullOrEmpty(username))
            {  
                    whereSql += " and  Userid in (select userid from Userrelation where parents like '%" + userid +
                                "%') and Userid in (select userid from Userrelation where Parents like '%'+ (select top 1 UserID from M_Users where  LoginName = '" +
                                username + "')+'%')";
                 
            }
            else
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    whereSql += "  and( UserID ='" + userid + "' or Userid in (select userid from Userrelation where Parents like'%" + userid + "%') )";
                    
                }
            }
            string sql = string.Format(@"
declare @today nvarchar(25)='{0}'
declare @endtoday nvarchar(25)='{1}'
	
if OBJECT_ID('tempdb..#userreport') is not null
drop table #userreport

select UserID,LoginName,cast(0 as decimal(18,4)) UserPoint,cast(0 as decimal(18,4)) TotalPay,
 cast(0 as decimal(18,4)) TotalDraw,cast(0 as decimal(18,4))  TotalPayMent, cast(0 as decimal(18,4)) TotalWin,
cast(0 as decimal(18,4))  TotalReturn,cast(0 as decimal(18,4))  TotalZHReturn,cast(0 as decimal(18,4))  TotalActive,
cast(0 as decimal(18,4))  TotalXJ,cast(0 as decimal(18,4))  TotalFenH,cast(0 as decimal(18,4)) TotalRiGZ,cast(0 as decimal(18,4)) SumFee 
into #userreport
from M_Users 
where Status <>9 and UserID !='993b30f7-a8c4-49f9-b2bc-0b629d34bb76' {2}


update a set TotalPay= b.Num from  #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(1,14) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
update a set TotalPayMent= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(4,5) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID
update a set TotalWin= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType=8 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID

update a set TotalDraw= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType=2 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--下级返点
update a set TotalReturn= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType=17 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--用户返点
update a set UserPoint= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType=7 and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--追号撤单返款
update a set TotalZHReturn= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(6,9) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID  
--活动奖金
update a set TotalActive= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(20,21,22) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--转账下级
update a set TotalXJ= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(23) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 
--日工资
update a set TotalRiGZ= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(18) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 

--分红
update a set TotalFenH= b.Num from   #userreport a join (
	select  SUM(AccountChange) as Num,UseriD from AccountOperateRecord where  PlayType in(19) and 
	CreateTime >=@today and CreateTime<=@endtoday
	group by UseriD 
) b on a.UserID=b.userID 

--团队总额
update a set SumFee= b.Num from   #userreport a join (
	select sum(isnull(c.AccountFee,'0'))+min(a.AccountFee) as Num,a.Userid from UserAccount a left join 
    Userrelation b on charindex(a.userid,Parents)>0 
    left join UserAccount c   on c.UserID=b.UserID 
	group  by a.userid

) b on a.UserID=b.userID 

update  #userreport set TotalPayMent=TotalPayMent-TotalZHReturn


--统计 
if object_id('tempdb..#list') is not null
drop table #list

select row_number() over( order by b.Autoid desc) id,b.Type,b.AutoID,b.UserID,b.SafeLevel,
a.LoginName,AccountFee,UserPoint,TotalPay,TotalDraw,b.CreateTime,SumFee,
TotalPayMent,TotalWin,TotalReturn,YL ,TotalActive ,TotalXJ,TotalRiGZ,TotalFenH
into #list
from
(
select 
b.LoginName,min(c.AccountFee) AccountFee,SUM(UserPoint) UserPoint,
SUM(TotalPay) TotalPay,0-SUM(TotalDraw) TotalDraw,SUM(TotalPayMent) TotalPayMent,
SUM(TotalWin) TotalWin,SUM(TotalReturn) TotalReturn, max(isnull(SumFee,0)) SumFee,
0-SUM(TotalPayMent)+SUM(UserPoint)+SUM(TotalWin)+SUM(TotalReturn)+Sum(TotalActive)+Sum(TotalRiGZ)+Sum(TotalFenH) YL ,
Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH
from #userreport a  
join M_Users b on a.userid=b.userid 
join UserAccount c on b.userid=c.userid 
group by  b.LoginName  
) a
join M_Users b on a.LoginName=b.LoginName 


select '{3}' as UserID,
sum(AccountFee) AccountFee,SUM(UserPoint) UserPoint,max(SumFee) SumFee,0 SafeLevel,
SUM(TotalPay) TotalPay,SUM(TotalDraw) TotalDraw,SUM(TotalPayMent) TotalPayMent,SUM(TotalWin) TotalWin,
SUM(TotalReturn) TotalReturn, Sum(YL) YL ,Sum(TotalActive) TotalActive ,Sum(TotalXJ) TotalXJ,
Sum(TotalRiGZ) TotalRiGZ,Sum(TotalFenH) TotalFenH 
from #list
", btime + " 00:00:00", endtime + " 23:59:59", whereSql, userid);

            DataSet ds = UserOrdersDAL.GetDataSet(sql); 
            UserReportDay listpage = new UserReportDay();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                listpage.FillData(item);
            }
            return listpage;
        }
        #region 操作

        public static bool BoutDayWages(string autoid)
        {
            return CommonBusiness.Update("UserDayWage", "Status", 2, " Status=0 and Autoid in(" + autoid.Trim(',')+" ) ");
        }

        public static bool WagesAuditting(string ids, string ip)
        {
            bool bl = UserOrdersDAL.BaseProvider.WagesAuditting(ids, ip);
            return bl;
        }

        /// <summary>
        /// 作废分红
        /// </summary>
        /// <param name="autoid"></param>
        /// <returns></returns>
        public static bool BoutProfitShare(string autoid)
        {
            return CommonBusiness.Update("UserProfitShare", "Status", 2, " Status=0 and  Autoid in(" + autoid.Trim(',') + " ) ");
        }

        public static bool ProfitShareAuditting(string ids, string ip)
        {
            bool bl = UserOrdersDAL.BaseProvider.ProfitShareAuditting(ids, ip);
            return bl;
        }
        public static bool UpdatePMoney(int autoid, decimal money,string tablename)
        {
            return CommonBusiness.Update(tablename, "GainFee", money, " Status=0 and  Autoid= " + autoid);
        }
        #endregion
    }
}
