using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class WebSetDAL : BaseDAL
    {
        public static WebSetDAL BaseProvider = new WebSetDAL();
       
        public DataTable GetChargeSetDetail(string view)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@View",view)
                                   };

            return GetDataTable("select * from ChargeSet where [View]=@View and Status=1", paras, CommandType.Text);
        }
        public DataTable GetActiveByID(string id)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@AutoID",id)
                                   };

            return GetDataTable("select * from Active where AutoID=@AutoID and Status<>9", paras, CommandType.Text);
        }
        public DataTable GetPlays()
        { 
            return GetDataTable("select c.* from   Plays c where  c.Status<>9 order by c.AutoID");
        }
        public DataTable GetLotteryPlays(string CPcode)
        {
            string sqlwhere = " where a.Status<> 9 ";
            if (!string.IsNullOrEmpty(CPcode))
            {
                sqlwhere += " and a.CPCode='" + CPcode + "' ";
            }

            return GetDataTable("select c.*,b.PIDS,b.Content,b.SpaceRate,isnull(d.PlayInfo,'') PlayInfo,isnull(d.bettmax,'')bettmax  from Lottery a join LotteryPlays  b on a.CPCode=b.CPCode " +
                                 "join Plays c on b.PID=c.PCode left join LotteryPlayInfo d on b.PIDs=d.pids and b.cpcode=d.cpcode  " + sqlwhere + " order by b.AutoID");
        }
        public bool UpdatePlayInfo(string cpcode, string pids, string playinfo,string bettnum,string content,string spacrate)
        {
            string sql = string.Format(@" declare @cpcode varchar(50)='{0}' , @pids varchar(300)='{1}' , @playinfo varchar(2000)='{2}' ,@content varchar(500)='{3}',@spacerate  varchar(500)='{4}',@bettnum  varchar(500)='{5}'
if exists ( select  1  from [dbo].[LotteryPlays]  where cpcode=@cpcode and pids=@pids )
begin

if exists(select  1  from [dbo].[LotteryPlayInfo]  where cpcode=@cpcode and pids=@pids )
begin
    update LotteryPlayInfo set playInfo=@playinfo,bettMax=@bettnum where cpcode=@cpcode and pids=@pids 
end
else 
begin
    insert into LotteryPlayInfo(PlayInfo,CPCode,PIDS,bettMax,CreateTime) values(@playinfo,@cpcode,@pids,@bettnum,getdate()) 
end

update lotteryplays set content=@content ,spacerate=@spacerate where pids=@pids  and cpcode=@cpcode

end
", cpcode, pids, playinfo, content, spacrate, bettnum);
            return ExecuteNonQuery(sql) > 1 ? true : false;
        }
        #region 新增 

        public bool InsertActive(string userid, string Title, string content, string Tips, string img, DateTime btime, DateTime etime,int type)
        {
            SqlParameter[] paras =
            { 
                new SqlParameter("@Title", Title),
                new SqlParameter("@Content", content),
                new SqlParameter("@Tips", Tips),
                new SqlParameter("@Img", img),
                new SqlParameter("@Type", type),
                new SqlParameter("@UserID", userid), 
                new SqlParameter("@ETime", etime),
                new SqlParameter("@BTime", btime)
            };
            return ExecuteNonQuery("Insert into  Active([Title],[Content],Tips,Img,CreateTime,CreateUserID,BTime,ETime,Type) values (@Title,@Content,@Tips,@Img,getDate(),@UserID,@BTime,@ETime,@Type)", paras, CommandType.Text) > 0;
        }
        public bool InsertChargeSet(string userid, string view, string remark, decimal golds)
        {
            SqlParameter[] paras =
            { 
                new SqlParameter("@View", view),
                new SqlParameter("@Remark", remark),
                new SqlParameter("@Golds", golds), 
                new SqlParameter("@UserID", userid)
            };
            return ExecuteNonQuery("Insert into  ChargeSet([View],[Remark],Golds,Status,CreateTime,UserID) values (@View,@Remark,@Golds,1,getDate(),@UserID)", paras, CommandType.Text) > 0;
        }

        public int InsertLottery(string cpname, string cpcode, int icontype, string resulturl, string userid, int openTimes, string closeTime, string onSaleTime, int sealTimes, int periodsNum, ref string errmsg)
        {
            SqlParameter[] paras =
            { 
                new SqlParameter("@Result",SqlDbType.Int),
                new SqlParameter("@ErrMsg",SqlDbType.NVarChar,300), 
                new SqlParameter("@CPName", cpname),
                new SqlParameter("@CPCode", cpcode),
                new SqlParameter("@IconType", icontype), 
                new SqlParameter("@ReturnUrl",resulturl),
                new SqlParameter("@OpenTimes",openTimes),
                new SqlParameter("@CloseTime",closeTime),
                new SqlParameter("@OnSaleTime",onSaleTime),
                new SqlParameter("@SealTimes",sealTimes),
                new SqlParameter("@PeriodsNum",periodsNum),
                new SqlParameter("@UserID", userid)
            };
            paras[0].Direction = ParameterDirection.Output;
            paras[1].Direction = ParameterDirection.Output;
            ExecuteNonQuery("P_InsertLottery", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[0].Value);
            errmsg = paras[1].Value.ToString();
            return result;
        }

        #endregion

        #region 修改  

        public bool UpdateActive(int autoid, string Title, string content, string Tips, string img, DateTime btime, DateTime etime, string upduserid)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", autoid),
                new SqlParameter("@Title", Title),
                new SqlParameter("@Content", content),
                new SqlParameter("@Img", img),
                new SqlParameter("@Tips", Tips),
                new SqlParameter("@ETime", etime),
                new SqlParameter("@UpdUserID", upduserid),
                new SqlParameter("@BTime", btime)
            };
            return ExecuteNonQuery("Update Active set UpdUserID=@UpdUserID,[Content]=@Content,Img=@Img,Tips=@Tips,Title=@Title,UpdTime=getdate(),BTime=@BTime,ETime=@ETime   where AutoID=@AutoID ", paras, CommandType.Text) > 0;
        }
        public bool UpdateChargeSet(int autoid, string view, string remark, decimal golds)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", autoid),
                new SqlParameter("@View", view),
                new SqlParameter("@Remark", remark),
                new SqlParameter("@Golds", golds)
            };
            return ExecuteNonQuery("Update ChargeSet set [View]=@View,[Remark]=@Remark,Golds=@Golds  where AutoID=@AutoID ", paras, CommandType.Text) > 0;
        }

        public bool UpdateLottery(string cpname, string cpcode, int icontype, string resulturl, int openTimes,string closeTime,string onSaleTime,int sealTimes,int periodsNum,int autoid)
        {
            SqlParameter[] paras =
            {  
                new SqlParameter("@AutoID",autoid), 
                new SqlParameter("@CPName", cpname),
                new SqlParameter("@CPCode", cpcode),
                new SqlParameter("@IconType", icontype), 
                new SqlParameter("@OpenTimes",openTimes),
                new SqlParameter("@CloseTime",closeTime),
                new SqlParameter("@OnSaleTime",onSaleTime),
                new SqlParameter("@SealTimes",sealTimes),
                new SqlParameter("@PeriodsNum",periodsNum),
                new SqlParameter("@ResultUrl",resulturl) 
            };
            return ExecuteNonQuery(@"Update Lottery set [CPName]=@CPName,[CPCode]=@CPCode,IconType=@IconType,ResultUrl=@ResultUrl,OpenTimes=@OpenTimes,CloseTime=@CloseTime,
OnSaleTime=@OnSaleTime,SealTimes=@SealTimes,PeriodsNum=@PeriodsNum where AutoID=@AutoID ", paras, CommandType.Text) > 0;
        }

        public bool DeleteActive(int autoid)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", autoid)
            };
            return ExecuteNonQuery("update Active set Status=9 where AutoID=@AutoID ", paras, CommandType.Text) > 0;
        }

        public bool UpdateLotteryPlays(string lotterid, string permissions, string userid)
        {
            SqlParameter[] paras = { 
                                       new SqlParameter("@CPCode",lotterid),
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@Permissions",permissions)
                                   };
            return ExecuteNonQuery("M_UpdateLotteryPlays", paras, CommandType.StoredProcedure) > 0;
        }


        public bool UpdateLotteryResult(string cpcode, string issuenum, string resultnum)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@CPCode", cpcode),
                new SqlParameter("@IssueNum", issuenum),
                new SqlParameter("@ResultNum", resultnum)
            };

            string sqltext = @" 
if exists( select 1 from LotteryWaitPay  where CPCode=@CPCode and  IssueNum=@IssueNum )
begin
    update LotteryWaitPay  set ResultNum=@resultnum where  CPCode=@CPCode and  IssueNum=@IssueNum
end
else 
begin
    insert into LotteryWaitPay( [cpcode] ,[IssueNum],[ResultNum],[AutoNum],[isread] ,[opentime],[addtime]) 
    select @CPCode,@IssueNum,@resultnum,'',0, (select top 1 OpenTime from LotteryResult  where CPCode=@CPCode and  IssueNum=@IssueNum ) ,getdate() 
end
--update LotteryResult set resultnum=@resultnum where CPCode=@CPCode and  IssueNum=@IssueNum and status=2
";

            return ExecuteNonQuery(sqltext, paras, CommandType.Text) > 0;
        }

        public bool ReturnLotteryResult(string cpcode, string issuenum)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@CPCode", cpcode),
                new SqlParameter("@IssueNum", issuenum) 
            };
            return ExecuteNonQuery(@"
begin try
begin tran 

if not exists(select 1 from lotteryorder where cpcode=@CPCode and IssueNum=@IssueNum )
begin
    select 1
end
update LotteryResult set status=3 where CPCode=@CPCode and  IssueNum=@IssueNum 

declare @ttt table (ordercode varchar(60))

if  exists(select 1 from lotteryorder where cpcode=@CPCode and IssueNum=@IssueNum and status=0)
begin
insert into @ttt  select LCode from LotteryOrder   where  CPCode=@CPCode and  IssueNum=@IssueNum  and status=0 
update LotteryOrder set status=3 where  LCOde in(select ordercode from @ttt)  and Status=0

insert into AccountOperateRecord (UserID,AccountChange,Account,PlayType,CreateTime,	Remark,CreateUserID,IP,InAccount,Type,FKCode)

select a.UserID,PayFee,PayFee+b.AccountFee,9,GETDATE(),'撤单返款,单号'+a.LCode,a.UserID,IP,PayFee,0,a.LCode from LotteryOrder a join UserAccount b on a.UserID=b.UserID where  CPCode=@CPCode and  IssueNum=@IssueNum  and Lcode in ( select ordercode from @ttt)


update a set AccountFee =AccountFee+isnull(b.payFee,0) from UserAccount a join (select  userid ,sum(isnull(payFee,0)) payFee from  lotteryorder where  CPCode=@CPCode and  IssueNum=@IssueNum  and lcode in(select ordercode from @ttt) group by userid ) b on a.userid=b.userid

select 1
end
else 
    select 0
commit tran
end try
begin catch
rollback tran
select -1
end catch

", paras, CommandType.Text) > 0;
        }

        public bool ReturnLotteryResultMyselft(string lcode)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@lcode", lcode)
            };
            return ExecuteNonQuery(@"
begin try
begin tran 

if not exists(select 1 from lotteryorder where Lcode=@lcode and status=0 ) 
or exists(select 1 from lotteryorder a join  LotteryResult b on a.CPCode=b.CPCode and a.IssueNum=b.IssueNum
 where a.Lcode=@lcode and b.Status!=0)
begin
    select -1
commit tran 
return
end

update lotteryorder set status=3 where  Lcode=@lcode and status=0 
 
if (select isnull(bcode,'') from lotteryorder where Lcode=@lcode  and status=3 )!=''
begin
    update a set LockAccountFee=isnull(LockAccountFee,0)-b.PayFee,AccountFee=AccountFee+b.PayFee
    from UserAccount a join LotteryOrder b	on a.UserID=b.UserID
    where  Lcode=@lcode and status=3 
end
else
begin

    update a set AccountFee=AccountFee+b.PayFee
    from UserAccount a join LotteryOrder b	on a.UserID=b.UserID
    where  Lcode=@lcode and status=3 
end

insert into AccountOperateRecord (UserID,AccountChange,Account,PlayType,CreateTime,	Remark,CreateUserID,IP,InAccount,Type,FkCode)

select a.UserID,PayFee,b.AccountFee,9,GETDATE(),'撤单返款'+@lcode,a.UserID,IP,PayFee,0 ,@lcode
from LotteryOrder a join UserAccount b on a.UserID=b.UserID where  Lcode=@lcode and status=3 
select 1
 
commit tran
end try
begin catch
rollback tran
select -1
end catch

", paras, CommandType.Text) > 0;
        }

        public bool CancelBett(string bcode)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@bcode", bcode)
            };
            return ExecuteNonQuery(@"
begin try
begin tran 
--1 进行中 2 已完成 3(已完成或中奖停止) 4 撤单

if not exists(select 1 from LotteryBettAuto where Bcode=@bcode and status=1 )
begin
    select -1
    commit tran 
    return
end


if OBJECT_ID('tempdb..#list') is not null
drop table #list

declare @comnum int=0,@issuenum nvarchar(64)='',@cpcode nvarchar(64)='',
@userid nvarchar(64)='',@payfee decimal(18,4)=0.0000,
@totalfee  decimal(18,4)=0.0000,@Lcode nvarchar(64)=''



select @comnum=ComNum,@cpcode=CPCode,@userid=UserID,@payfee=PayFee
from LotteryBettAuto where Bcode=@bcode
declare @index int=@comnum,@count int=0

select * into #list
from SplitToTable((select JsonContent from LotteryBettAuto where Bcode=@bcode),'|')
select @count=COUNT(1) from #list 
while(@index<=@count)
begin

set @Lcode=''

select @issuenum=substring(value,1,charindex(',',value)-1),
@totalfee=@payfee*(substring(value,charindex(',',value)+1,LEN(value)-charindex(',',value)))
 from #list where id=@index
 
if exists(select 1 from LotteryResult where CPCode=@cpcode and IssueNum=@issuenum and status=0)
begin
if exists(select 1 from LotteryOrder a  where CPCode=@cpcode 
and IssueNum=@issuenum and UserID=@userid and BCode=@bcode and Status=0)
begin

select @Lcode =lcode from LotteryOrder a  where CPCode=@cpcode 
and IssueNum=@issuenum and UserID=@userid and BCode=@bcode

if not exists(select 1 from LotteryBettAuto where BCode=@bcode and Status=4)
begin
--撤追号单
update  a set a.status=4 from LotteryBettAuto a where BCode=@bcode
end

--撤订单
update a
set a.status=3
from LotteryOrder a  where CPCode=@cpcode 
and IssueNum=@issuenum and UserID=@userid and BCode=@bcode
 

--返款
update a 
set a.LockAccountFee=a.LockAccountFee-@totalfee,a.AccountFee=a.AccountFee+@totalfee
from UserAccount a where UserID=@userid

insert into AccountOperateRecord (UserID,AccountChange,Account,PlayType,CreateTime,
	Remark,CreateUserID,IP,InAccount,Type,FkCode)
select @userid,@totalfee,a.AccountFee,9,GETDATE(),'撤单返款',a.UserID,'',@totalfee,0 
,(case when @Lcode!='' then @Lcode else @bcode end)
from UserAccount a where UserID=@userid

end
else if not exists(select 1 from LotteryOrder a  where CPCode=@cpcode 
and IssueNum=@issuenum and UserID=@userid and BCode=@bcode)
begin

if not exists(select 1 from LotteryBettAuto where BCode=@bcode and Status=4)
begin
--撤追号单
update  a set a.status=4 from LotteryBettAuto a where BCode=@bcode
end

--返款
update a 
set a.LockAccountFee=a.LockAccountFee-@totalfee,a.AccountFee=a.AccountFee+@totalfee
from UserAccount a where UserID=@userid

insert into AccountOperateRecord (UserID,AccountChange,Account,PlayType,CreateTime,
	Remark,CreateUserID,IP,InAccount,Type,FkCode)
select @userid,@totalfee,a.AccountFee,9,GETDATE(),'撤单返款',a.UserID,'',@totalfee,0 
,(case when @Lcode!='' then @Lcode else @bcode end)
from UserAccount a where UserID=@userid

end

end

set @index=@index+1

end

select 1
 
commit tran
end try
begin catch
rollback tran
select -1
end catch

", paras, CommandType.Text) > 0;
        }
        #endregion
    }
}
