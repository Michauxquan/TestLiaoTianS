using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class LotteryResultDAL:BaseDAL
    {
        public static LotteryResultDAL BaseProvider = new LotteryResultDAL();



        public void InsertLotteryResultAll()
        {
            SqlParameter[] paras = {};
            ExecuteNonQuery("InsertLotteryResultAll", paras, CommandType.StoredProcedure);
        }

        public bool UpdateLotteryResult(string issuenum, string cpcode, int status)
        {
            SqlParameter[] paras = {new SqlParameter("@IssueNum",issuenum),
                                       new SqlParameter("@Status",status),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("Update LotteryResult set Status=@Status  where IssueNum=@IssueNum and CPCode=@CPCode", paras, CommandType.Text) > 0;
        }
        public bool UpdateLotteryStatus(string cpcode, int status)
        {
            SqlParameter[] paras = {
                                       new SqlParameter("@Status",status),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("UpdateLotteryStatus", paras, CommandType.StoredProcedure) > 0;
        }
        public bool UpdateSD11X5Result(string result, string issnum, string cpcode)
        {
            SqlParameter[] paras = {
                                       new SqlParameter("@Content",result),
                                        new SqlParameter("@IssueName",issnum),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            return ExecuteNonQuery("UpdateSD11X5ResultAsync", paras, CommandType.StoredProcedure) > 0;
        }
        public bool UpdateSysLotteryResult(string issnum, string cpcode, int lay)
        {
            SqlParameter[] paras = {
                                        new SqlParameter("@result",SqlDbType.Int),
                                       new SqlParameter("@lay",lay),
                                        new SqlParameter("@IssueName",issnum),
                                       new SqlParameter("@CPCode",cpcode)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            return ExecuteNonQuery("A_SysLotteryResult", paras, CommandType.StoredProcedure) > 0;
        }

        public void CheckSysLottery()
        {
            SqlParameter[] paras = {};
            ExecuteNonQuery("checkSysLottery", paras, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpcode">以,结尾</param>
        /// <param name="openTime"></param>
        /// <returns></returns>
        public bool UpdateByStatusAndOpentTime(string cpcode, string openTime)
        {
            cpcode = cpcode.TrimEnd(',');
            cpcode ="'"+ cpcode.Replace(",", "','")+"'"; 
            return ExecuteNonQuery("Update LotteryResult set Status=1  where Opentime<'" + openTime + "' and CPCode in (" + cpcode + ") and Status=0") > 0;
        }
        public DataTable GetNowLottery(string cpcode)
        {
            SqlParameter[] paras = {};
            string commandText = string.Format(@"
begin try
 declare @Opentime datetime 
 select top 1 @Opentime= Opentime from lotteryresult (nolock) where  CPCode='{0}'  and Status=2 order by Opentime desc 

select top 1 datediff(second,getdate(),opentime) time, *  from lotteryresult (nolock) where CPCode='{0}'   and Opentime>@Opentime  order by Opentime asc
end try
begin catch 
select ERROR_MESSAGE() state 
end catch ", cpcode);

            return GetDataTable(commandText, paras, CommandType.Text);
        }
        public DataTable GetNowLottery(string cpcode,string sqlwhere)
        {
            SqlParameter[] paras = { };
            string commandText = string.Format(@"
select top 1 datediff(second,getdate(),a.opentime) time, *  
from lotteryresult (nolock) a join Lottery b on a.cpcode=b.cpcode
where a.CPCode='{0}'   and a.Opentime>getdate() {1}  order by a.Opentime asc
  ", cpcode, sqlwhere);

            return GetDataTable(commandText, paras, CommandType.Text);
        }
          public DataTable GetOpenLottery(string cpcode,string sqlwhere,int top=1)
        {
            SqlParameter[] paras = { };
            string commandText = string.Format(@" 
select top {2} * from lotteryresult (nolock) where CPCode='{0}' {1}
   order by Opentime desc 
  ", cpcode, sqlwhere, top);

            return GetDataTable(commandText, paras, CommandType.Text);
        } 
    }
}
