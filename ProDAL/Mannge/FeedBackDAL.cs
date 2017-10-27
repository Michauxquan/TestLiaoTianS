using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL.Mannge
{
    public class FeedBackDAL:BaseDAL
    {
        public static FeedBackDAL BaseProvider = new FeedBackDAL();

        #region 添加

        public bool InsertFeedBack(string title, string tipedName, string tipedid, int type, string remark,
                                   string createUserID)
        {
            SqlParameter[] parms = { 
                                       new SqlParameter("@Title",title),
                                       new SqlParameter("@TipedName",tipedName),
                                       new SqlParameter("@TipedID",tipedid),
                                       new SqlParameter("@Type",type), 
                                       new SqlParameter("@Remark",remark),
                                       new SqlParameter("@CreateUserID",createUserID)
                                   };

            string cmdText = "insert into feedback(Title,TipedName,TipedID,Type,Remark,CreateUserID) values(@Title,@TipedName,@TipedID,@Type,@Remark,@CreateUserID)";
            return ExecuteNonQuery(cmdText, parms, CommandType.Text) > 0;
        }
        #endregion

        public DataTable GetFeedBackDetail(string id)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@AutoID",id)
                                   };
            return GetDataTable("select * from FeedBack where AutoID=@AutoID", paras, CommandType.Text);
        }

        public bool UpdateFeedBackStatus(string id, int status, string content)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@AutoID",id),
                                    new SqlParameter("@Status",status),
                                    new SqlParameter("@Content",content)
                                   };
            return ExecuteNonQuery("M_UpdateFeedBackStatus", paras, CommandType.StoredProcedure) > 0;
        }
    }
}
