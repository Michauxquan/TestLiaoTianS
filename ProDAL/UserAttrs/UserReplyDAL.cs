using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL.UserAttrs
{
    public class UserReplyDAL:BaseDAL
    {
        public static UserReplyDAL BaseProvider = new UserReplyDAL();

        public bool CreateUserReply(string guid, string content,string title, string userID, string fromReplyID, string fromReplyUserID, int type, int haschilds,int isreply, ref string errormsg)
        {
            string replyID = Guid.NewGuid().ToString(); 
            SqlParameter[] paras = { 
                                    new SqlParameter("@ErrorMsg" , SqlDbType.VarChar,300),
                                    new SqlParameter("@Result",SqlDbType.Int),
                                     new SqlParameter("@ReplyID",replyID),
                                     new SqlParameter("@Title",title), 
                                     new SqlParameter("@GUID",guid),
                                     new SqlParameter("@Content",content),
                                     new SqlParameter("@FromReplyID",fromReplyID),
                                     new SqlParameter("@CreateUserID" , userID), 
                                     new SqlParameter("@Type" , type), 
                                     new SqlParameter("@HasChilds",haschilds), 
                                     new SqlParameter("@IsReply",isreply), 
                                     new SqlParameter("@FromReplyUserID" , fromReplyUserID),
                                   };
            paras[0].Direction = ParameterDirection.Output;
            paras[1].Direction = ParameterDirection.Output;
            ExecuteNonQuery("P_InsertUserReplay", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[1].Value);
            errormsg = paras[0].Value.ToString();
            return result > 0;
        }
        public DataTable GetReplyDetail(string replyid)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ReplyID",replyid)
                                   };

            return GetDataTable("select a.*,b.UserName from UserReply a join M_Users b on a.CreateUserID=b.UserID where ReplyID=@ReplyID ", paras, CommandType.Text);
        }
    }
}
