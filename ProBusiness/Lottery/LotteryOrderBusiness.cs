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
   public class LotteryOrderBusiness
    {
       public static LotteryOrderBusiness BaseBusiness = new LotteryOrderBusiness();

        #region 查询

       public static List<LotteryOrder> GetLotteryOrder(string keyWords, string cpcode, string userid, string lcode, string issuenum, string type, string status, int winType, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int self = 0, string begintime = "", string endtime = "", string username = "", string orderby = " a.AutoID ", string bcode="",string winfee="",string opentime="",string opentimeend="" )
        {
            string tablename = "LotteryOrder (nolock) a left join M_Users b  on a.UserID =b.UserID " +
                               "left join lotteryResult (nolock) c on a.IssueNum=c.IssueNum   and a.CPCode=c.CPCode " +
                               " left join lotteryBettAuto (nolock) d on a.bcode=d.Bcode ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.LoginName like '%" + keyWords + "%' or a.IssueNum like '%" + keyWords + "%' or a.LCode like '%" + keyWords + "%'  or a.TypeName like '%" + keyWords + "%')";
            }
            if (!string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type like '%" + type+"%'";
            }
           if (!string.IsNullOrEmpty(bcode))
           {
               sqlwhere += " and a.bcode='" + bcode + "'";
           }
           if (!string.IsNullOrEmpty(status) && status != "-1")
            {
                sqlwhere += " and a.status in(" + status+")";
            }
            if (!string.IsNullOrEmpty(cpcode))
            {
                sqlwhere += " and a.cpcode='" + cpcode+"'";
            }
            if (!string.IsNullOrEmpty(username))
            {
                sqlwhere += " and b.LoginName='" + username + "'";
            }
           if (winType > -1)
           {
               sqlwhere += " and a.WinType=" + winType;
           }
           if (!string.IsNullOrEmpty(winfee))
           {
               sqlwhere += " and a.WinFee>" + Convert.ToDecimal(winfee);
           }
           if (!string.IsNullOrEmpty(opentime))
           {
               sqlwhere += " and c.opentime>='" + opentime+"'";
           }
           if (!string.IsNullOrEmpty(opentimeend))
           {
               sqlwhere += " and c.opentime<'" + opentimeend + "'";
           }
           if (!string.IsNullOrEmpty(userid))
            {
                if (self > 0)
                {
                    if (self == 1)
                    {
                        sqlwhere += " and a.UserID in(select UserID from UserRelation where ParentID='" + userid +"') ";
                    }
                    else if (self == 2)
                    {
                        sqlwhere += " and a.UserID in(select UserID from UserRelation where Parents like '%" + userid +"%')";
                    }
                    else
                    {
                        sqlwhere += " and a.UserID='" + userid + "' ";
                    }
                }
                else
                {
                    sqlwhere += " and (a.UserID in(select UserID from UserRelation where Parents like '%" + userid +"%') or  a.UserID='" + userid + "' )";
                }
            } 
           if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime +"'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
           if (!string.IsNullOrWhiteSpace(lcode))
           {
               sqlwhere += " and a.LCode ='" + lcode + "'";
           }
           if (!string.IsNullOrEmpty(issuenum))
           {
               sqlwhere += " and a.IssueNum ='" + issuenum + "'";
           }
            bool isdesc = true;
            if (!string.IsNullOrEmpty(orderby))
            {
                if (orderby.ToLower().IndexOf(" desc")>-1)
                {
                    isdesc = false;
                }
                orderby = orderby.Replace("desc", " ").Replace("asc", "");
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, " a.*,b.LoginName as UserName,c.ResultNum, isnull(d.IsStart,-1) IsStart,isnull(d.Status,-1) BettStatus", sqlwhere, orderby, pageSize, pageIndex, out totalCount, out pageCount, isdesc);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

       public static List<LotteryOrder> GetLotteryOrderNew(string keyWords, string cpcode, string userid, string lcode, string issuenum, string type, string status, int winType, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int self = 0, string begintime = "", string endtime = "", string username = "", string orderby = " a.AutoID ")
       {
           string tablename = "LotteryOrder (nolock)  a left join M_Users b  on a.UserID =b.UserID ";
           string sqlwhere = " a.status<>9 ";
           if (!string.IsNullOrEmpty(keyWords))
           {
               sqlwhere += " and (b.LoginName like '%" + keyWords + "%' or a.IssueNum like '%" + keyWords + "%' or a.LCode like '%" + keyWords + "%'  or a.TypeName like '%" + keyWords + "%')";
           }
           if (!string.IsNullOrEmpty(type))
           {
               sqlwhere += " and a.Type like '%" + type + "%'";
           } 
           if (!string.IsNullOrEmpty(status) && status != "-1")
           {
               sqlwhere += " and a.status in(" + status + ")";
           }
           if (!string.IsNullOrEmpty(cpcode))
           {
               sqlwhere += " and a.cpcode='" + cpcode + "'";
           }
           if (!string.IsNullOrEmpty(username))
           {
               sqlwhere += " and b.LoginName='" + username + "'";
           }
           if (winType > -1)
           {
               sqlwhere += " and a.WinType=" + winType;
           } 
           if (!string.IsNullOrEmpty(userid))
           {
               if (self > 0)
               {
                   if (self == 1)
                   {
                       sqlwhere += " and a.UserID in(select UserID from UserRelation where ParentID='" + userid + "') ";
                   }
                   else if (self == 2)
                   {
                       sqlwhere += " and a.UserID in(select UserID from UserRelation where Parents like '%" + userid + "%')";
                   }
                   else
                   {
                       sqlwhere += " and a.UserID='" + userid + "' ";
                   }
               }
               else
               {
                   sqlwhere += " and (a.UserID in(select UserID from UserRelation where Parents like '%" + userid + "%') or  a.UserID='" + userid + "' )";
               }
           }
           if (!string.IsNullOrEmpty(begintime))
           {
               sqlwhere += " and a.CreateTime>='" + begintime + "'";
           }
           if (!string.IsNullOrEmpty(endtime))
           {
               sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
           }
           if (!string.IsNullOrWhiteSpace(lcode))
           {
               sqlwhere += " and a.LCode ='" + lcode + "'";
           }
           if (!string.IsNullOrEmpty(issuenum))
           {
               sqlwhere += " and a.IssueNum ='" + issuenum + "'";
           }
           bool isdesc = true;
           if (!string.IsNullOrEmpty(orderby))
           {
               if (orderby.ToLower().IndexOf(" desc") > -1)
               {
                   isdesc = false;
               }
               orderby = orderby.Replace("desc", " ").Replace("asc", "");
           }
           DataTable dt = CommonBusiness.GetPagerData(tablename, " a.*,b.LoginName as UserName", sqlwhere, orderby, pageSize, pageIndex, out totalCount, out pageCount, isdesc);
           List<LotteryOrder> list = new List<LotteryOrder>();
           foreach (DataRow dr in dt.Rows)
           {
               LotteryOrder model = new LotteryOrder();
               model.FillData(dr);
               list.Add(model);
           }
           return list;
       }

       public static List<OrderReport> GetLotterySumOrder(string cpcode, string userid, string type, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, ref OrderReport listpage,int self =-1, string begintime = "", string endtime = "", string username = "")
       { 
           string sqlwhere = " status in(0,1,2,4) ";
           string uwhere = "";
           if (!string.IsNullOrEmpty(type))
           {
               sqlwhere += " and Type like '%" + type + "%'";
           }   
           if (!string.IsNullOrEmpty(cpcode))
           {
               sqlwhere += " and cpcode='" + cpcode + "'";
           }
           if (!string.IsNullOrEmpty(username))
           {
               if (self > 0)
               {
                   if (self == 0)
                   {
                       uwhere += " and  Userid in (select userid from Userrelation where ParentID='" + userid +"')  and LoginName = '" + username + "'";
                   }
                   else if (self == 1)
                   {
                       uwhere += " and  Userid in (select userid from Userrelation where parents like '%" + userid + "%') and Userid in (select userid from Userrelation where Parents like '%'+ (select top 1 UserID from M_Users where  LoginName = '" + username + "')+'%')";
                   } else{
                       uwhere += " and( Userid in (select userid from Userrelation where parents like'%" + userid +
                               "%') and  ( LoginName = '" + username +
                               "' or Userid in (select userid from Userrelation where parentid in (select top 1 UserID from M_Users where  LoginName = '" +
                               username + "') )) )";
                       
                   }
               }
               else
               {
                   uwhere += " and( Userid in (select userid from Userrelation where parents like'%" + userid +"%') and  ( LoginName = '" + username +
                              "' or Userid in (select userid from Userrelation where Parents like '%'+ (select top 1 UserID from M_Users where  LoginName ='" + username + "')+'%') ) )";
               }
           }
           else
           {
               if (!string.IsNullOrEmpty(userid))
               {
                   if (self > 0)
                   {
                       if (self == 0)
                       {
                           uwhere += " and UserID='" + userid + "' ";
                       }
                       if (self == 1)
                       {
                           uwhere += " and UserID in(select UserID from UserRelation where ParentID='" + userid + "') ";
                       }
                       else  
                       {
                           uwhere += " and UserID in(select UserID from UserRelation where Parents like '%" + userid +"%')";
                       } 
                   }
                   else
                   {
                       uwhere += " and (UserID in(select UserID from UserRelation where Parents like '%" + userid +"%') or UserID='" + userid + "' )";
                   }
               }
           }
           if (!string.IsNullOrEmpty(begintime))
           {
               sqlwhere += " and CreateTime>='" + begintime + "'";
           }
           if (!string.IsNullOrEmpty(endtime))
           {
               sqlwhere += " and CreateTime<='" + endtime + " 23:59:59:999'";
           } 
           bool isdesc = true;
           string sql = string.Format(@"
	
if OBJECT_ID('tempdb..#orderreport') is not null
drop table #orderreport

select  row_number() over( order by Autoid asc) Autoid,UserID,LoginName,UserName,cast(0 as decimal(18,4)) as DWDFee,cast(0 as decimal(18,4)) as DXDSFee,cast(0 as decimal(18,4)) as LHHFee  
into #orderreport
from M_Users 
where Status <>9 and UserID !='993b30f7-a8c4-49f9-b2bc-0b629d34bb76' {1}

update a set DWDFee= b.Num from  #orderreport a join (
	select  SUM(PayFee) as Num,UserID from LotteryOrder where  {0} and Type like '%1DWEID_%' 
	group by UserID 
) b on a.UserID=b.userID 

update a set DXDSFee= b.Num from  #orderreport a join (
	select  SUM(PayFee) as Num,UserID from LotteryOrder where  {0} and Type like '%1QUWEIX_2DXDS_%' 
	group by UserID 
) b on a.UserID=b.userID 

update a set LHHFee= b.Num from  #orderreport a join (
	select  SUM(PayFee) as Num,UserID from LotteryOrder where  {0} and Type like '%1QUWEIX_2LHDOU_%' 
	group by UserID 
) b on a.UserID=b.userID 



declare @totalcount int=0,@pagecount int=0

select @totalcount=count(1) from #orderreport

set @pagecount=CEILING(@totalCount * 1.0/{3})

select @totalcount totalcount ,@pagecount pagecount 

select * from #orderreport where Autoid>=({2}-1)*{3} and Autoid<={2}*{3}

select 0 autoid, '' UserName,'' LoginName,
sum(DXDSFee) DXDSFee,SUM(LHHFee) LHHFee,SUM(DWDFee) DWDFee,0 SafeLevel
from #orderreport
", sqlwhere, uwhere,pageIndex,pageSize);
           DataSet ds = UserOrdersDAL.GetDataSet(sql);
           DataTable dt1 = ds.Tables[0];
           totalCount = int.Parse(dt1.Rows[0]["totalcount"].ToString());
           pageCount = int.Parse(dt1.Rows[0]["pagecount"].ToString());
           List<OrderReport> list = new List<OrderReport>();
           foreach (DataRow item in ds.Tables[1].Rows)
           {
               OrderReport model = new OrderReport();
               model.FillData(item);
               list.Add(model);
           }

           listpage = new OrderReport();
           foreach (DataRow item in ds.Tables[2].Rows)
           {
               listpage.FillData(item);
           } 
           return list;
       }

       public static LotteryOrder GetUserOrderDetail(string lcode)
       {
           DataTable dt = LotteryOrderDAL.BaseProvider.GetLotteryOrderDetail(lcode);
           LotteryOrder model = null;
           if (dt.Rows.Count == 1)
           {
               model = new LotteryOrder();
               model.FillData(dt.Rows[0]);
           }
           return model;
       }

       public static List<LotteryBettAuto> GetBettAutoRecord(string keyWords, string cpcode, string userid, string bCode, string issuenum, string type, int status, int winType, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int self = 0, string begintime = "", string endtime = "")
        {
            string tablename = "LotteryBettAuto  a left join M_Users b  on a.UserID =b.UserID   ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.LoginName like '%" + keyWords + "%' or a.StartNum like '%" + keyWords + "%' or a.BCode like '%" + keyWords + "%'  or a.TypeName like '%" + keyWords + "%')";
            }
            if (!string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type like '%" + type+"%'";
            }
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
            }
           if (winType > -1)
           {
               sqlwhere += " and a.WinType=" + winType;
           }
           if (!string.IsNullOrEmpty(userid))
            {
                if (self > 0)
                {
                    if (self == 1)
                    {
                        sqlwhere += " and a.UserID in(select UserID from UserRelation where ParentID='" + userid + "')";
                    }
                    else if (self == 2)
                    {
                        sqlwhere += " and a.UserID in(select UserID from UserRelation where Parents like '%" + userid + "%')";
                    }
                    else
                    {
                        sqlwhere += " and a.UserID='" + userid + "' ";
                    }
                }
            } 
           if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime +"'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            if (!string.IsNullOrWhiteSpace(bCode))
           {
               sqlwhere += " and a.BCode ='" + bCode + "'";
           }
           if (!string.IsNullOrEmpty(issuenum))
           {
               sqlwhere += " and a.StartNum ='" + issuenum + "'";
           }
           DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.LoginName as UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
           List<LotteryBettAuto> list = new List<LotteryBettAuto>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryBettAuto model = new LotteryBettAuto();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
       public static List<LotteryBettAuto> GetBettAutoByStatus()
       { 
           DataTable dt =LotteryOrderDAL.GetDataTable(
                   "select  a.*,b.LoginName as UserName from LotteryBettAuto  a left join M_Users b  on a.UserID =b.UserID where a.Status=0 and b.Status<>9");
           List<LotteryBettAuto> list = new List<LotteryBettAuto>();
           foreach (DataRow dr in dt.Rows)
           {
               LotteryBettAuto model = new LotteryBettAuto();
               model.FillData(dr);
               list.Add(model);
           }
           return list;
       }

       public static UserReportDay GetUserWinDay(string userid)
       {
           DataTable dt = LotteryOrderDAL.GetDataTable("select sum(PayFee) as TotalPayMent,sum(WinFee) TotalWin from LotteryOrder where UserID='" + userid + "' and CreateTime>=convert(varchar(10),getdate(),120)");
           UserReportDay model = new UserReportDay();
           foreach (DataRow dr in dt.Rows)
           {
               model.FillData(dr);
           }
           return model;
       }

       public static List<LotteryOrder> GetTodayBett(string btime, string etime)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(btime))
            {
                sqlwhere+=" and CreateTime >='"+btime+"'";
            }
             if (!string.IsNullOrEmpty(etime))
            {
                sqlwhere+=" and CreateTime <'"+etime+"'";
            }
             string sqlstr = @" select a.CPCode,a.CPName,ISNULL(b.PayFee,0) PayFee from Lottery a left join (select CPCode,SUM(PayFee) PayFee  from LotteryOrder (nolock)  where Status<>3 " + sqlwhere + " group by CPCode ) b on a.CPCode=b.CPCode ";
            DataTable dt = LotteryOrderDAL.GetDataTable(sqlstr);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
       public static List<LotteryOrder> GetTodayUserBett(string btime, string etime,string cpcode="")
       {
           string sqlwhere = "";
           if (!string.IsNullOrEmpty(btime))
           {
               sqlwhere += " and CreateTime >='" + btime + "'";
           }
           if (!string.IsNullOrEmpty(etime))
           {
               sqlwhere += " and CreateTime <'" + etime + "'";
           } if (!string.IsNullOrEmpty(cpcode))
           {
               sqlwhere += " and CpCode in ('" + cpcode.TrimEnd(',').Replace(",","','") + "')";
           }
           string sqlstr = @" select a.CPCode,a.CPName ,b.UserID ,c.LoginName as UserName,ISNULL(b.PayFee,0) PayFee from Lottery a 
                              join (select  UserID,SUM(PayFee) PayFee,CPcode  from LotteryOrder (nolock)   where Status<>3 " + sqlwhere + " group by UserID,CPCode ) b on a.CPCode=b.CPCode" +
                           " join M_Users c on c.UserID=b.UserID where c.Status<>9 order by a.CPCode  ";
           DataTable dt = LotteryOrderDAL.GetDataTable(sqlstr);
           List<LotteryOrder> list = new List<LotteryOrder>();
           foreach (DataRow dr in dt.Rows)
           {
               LotteryOrder model = new LotteryOrder();
               model.FillData(dr);
               list.Add(model);
           }
           return list;
       }

       public static string GetIssueNumFee(string cpcode,string userid,string issuenum="", string type = "")
       {
           string where = " CPCode='" + cpcode + "' and Status in(0,1,2,4) and UserID='" + userid + "'";
           //if (!string.IsNullOrEmpty(issuenum))
           //{
               where += " and issuenum='" + issuenum + "' ";
           //}
           if (!string.IsNullOrEmpty(type))
           {
               where += " and type='" + type + "' ";
           }
           return CommonBusiness.Select("lotteryorder (nolock) ", "Sum(PayFee)", where).ToString();
       }

       public static List<LotteryOrder> GetIssueNumReport(string cpcode, string issuenum = "")
       {
           string swhere = "";
           if (!string.IsNullOrEmpty(issuenum))
           {
               swhere = "  and issuenum='" + issuenum + "'";
           }
           DataTable dt =
               LotteryOrderDAL.GetDataTable(
                   "select   CPCOde,CPName,IssueNum,Type+'_'+Content Type,TypeName,Sum(payFee) payFee   from LotteryOrder (nolock) where CPCode='" + cpcode + "' " + swhere + " group by  CPCOde,CPName, IssueNum,Type+'_'+Content,TypeName    ");
           List<LotteryOrder> list = new List<LotteryOrder>();
           foreach (DataRow dr in dt.Rows)
           {
               LotteryOrder model = new LotteryOrder();
               model.FillData(dr);
               list.Add(model);
           }
           return list;
       }

       #endregion

        #region 添加.删除


       public static string Getordercode()
       {
           System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
           long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
           return timeStamp.ToString();
       }

       public static int CreateUserOrderList(List<LotteryOrder> models, M_Users user,string ip,int usedisFee,int palytype,ref string errmsg )
       {
           int k = 0;
           string msg = "";
           models.ForEach(x =>
           {
               string errormsg = "";
               var result = false;
               if (x.ModelName.ToLower().IndexOf("undefined") == -1)
               {

                   string orderCode = Getordercode() + user.AutoID;
                   Console.WriteLine(orderCode);
                   x.TypeName = x.TypeName.Replace("大小单双", "").Replace("趣味型", "趣味");
                   if (x.CPCode.ToLower() != "bjsc")
                   {
                       x.TypeName = x.TypeName.Replace("龙虎斗", "");
                   }
                   result = CreateLotteryOrder(orderCode, x.IssueNum, x.Type.Trim(), x.TypeName, x.CPCode, x.CPName,
                       x.Content.Replace("\"", ""),
                       x.Num, x.PayFee, user.UserID, x.PMuch<1?1:x.PMuch, x.RPoint, ip, usedisFee, palytype, "", x.ModelName,
                       x.MType, ref errormsg);
               }
               else
               {
                   errormsg = "奖金模式选择不正确,请确认后再提交";
               }
               if (!result)
               {
                   if (msg.IndexOf(errormsg.Trim()) == -1)
                   {
                       msg += errormsg + " ";
                   }
               }
               else
               {
                   k++;
               }
           });
           errmsg = msg;
           return k;
       }

       public static bool CreateLotteryOrder(string ordercode, string issueNum, string type, string typename,string cpcode, string cpname, string content,  int num,
           decimal payfee, string userID, int pmuch, decimal rpoint, string operatip,int usedisFee,int palytype, string bCode,string ModelName,int mtype,ref string errormsg)
       {
           var orderid = Guid.NewGuid().ToString();
           return LotteryOrderDAL.BaseProvider.CreateLotteryOrder(ordercode, orderid,issueNum, type, cpcode, cpname, content, typename, num,
            payfee, userID, pmuch, rpoint, operatip, usedisFee, palytype, bCode,ModelName,mtype,ref errormsg);
       }
        public static bool DeleteOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("LotteryOrder", "Status", 9, "LCode='" + ordercode + "'");
            return bl;
        }
        public static bool BoutOrder(string ordercode)
        {
            bool bl = CommonBusiness.Update("LotteryOrder", "Status", 3, "LCode='" + ordercode + "' and Status=0");
            return bl;
        }


        public static int CreateBettOrderList(List<LotteryBettAuto> models, M_Users user, string ip, int isStart, ref string errmsg)
        {
            int k = 0;
            string msg = "";
            models.ForEach(x =>
            {
                string errormsg = "";
                string orderCode = Getordercode() + user.AutoID;
                x.TypeName = x.TypeName.Replace("大小单双", "").Replace("趣味型", "趣味");
                if (x.CPCode.ToLower() != "bjsc")
                {
                    x.TypeName = x.TypeName.Replace("龙虎斗", "");
                }
                var result = CreateBettOrder(orderCode, x.StartNum, x.Type, x.TypeName, x.CPCode, x.CPName, x.Content.Replace("\"", ""),
                    x.Num, x.PayFee, user.UserID, x.PMuch, x.RPoint, ip, isStart,x.BettNum,x.BMuch,x.TotalFee,x.Profits,x.WinFee, x.BettType,x.JsonContent,x.ModelName,x.MType,ref errormsg);
                if (!result)
                {
                    //msg += x.Content + "    " + errormsg + "/n";
                    msg += errormsg;
                }
                else
                {
                    k++;
                }
            });
            errmsg = msg;
            return k;
        }
        public static bool CreateBettOrder(string ordercode, string issueNum, string type, string typename, string cpcode, string cpname, string content, int num, decimal payfee, string userID,
           int pmuch, decimal rpoint, string operatip, int isStart, int bettnum, int bmuch, decimal totalfee, decimal profits, decimal winfee, int bettType,string jsonContent,string modelName,int mtype, ref string errormsg)
        { 
            return LotteryOrderDAL.BaseProvider.CreateBettOrder(ordercode,  issueNum, type, typename, cpcode, cpname, content, num,
             payfee, userID, pmuch, rpoint, operatip, isStart, bettnum, bmuch, totalfee, profits, winfee,bettType,jsonContent, modelName, mtype,ref errormsg);
        }
       #endregion 

        #region 修改

       public static bool UpdateBettAutoByCode(string bCode, int comnum,decimal comfee, string remark)
       {
           return LotteryOrderDAL.BaseProvider.UpdateBettAutoByCode(bCode, comnum,comfee, remark);
       }


        public static bool SaveLotteryUpdate(LotteryOrder model, ref string msg )
        {
            return LotteryOrderDAL.BaseProvider.SaveLotteryUpdate(model.AutoID, model.Type, model.TypeName, model.Content, ref msg);
        }

        public static bool DeleteLotteryOrder(long autoid)
        {
            return LotteryOrderDAL.BaseProvider.DeleteLotteryOrder(autoid);
        }

        #endregion
        
    }
}
