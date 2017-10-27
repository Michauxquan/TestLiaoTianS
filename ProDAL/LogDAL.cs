using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class LogDAL : BaseDAL
    {


        public static Task<bool> AddLoginLog(string loginname, int type, string operateip, string userid, string remark = "用户登录")
        {
            string sqlText = "insert into UsersLog(Type,CreateTime,IP,UserID,Remark) " +
                            " values(@Type,GETDATE(),@OperateIP,@UserID,@Remark)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserName" , loginname),
                                     new SqlParameter("@Type" , type),   
                                     new SqlParameter("@Remark" , remark),
                                     new SqlParameter("@UserID" , userid),
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            var task = new Task<bool>(() =>{ return ExecuteNonQuery(sqlText, paras, CommandType.Text)>0; });
            task.Start();//异步执行
            return task;
            //return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }
        public static Task<bool> AddManageLoginLog(string loginname, int type, string operateip, string userid, string remark = "用户登录")
        {
            string sqlText = "insert into UsersOperatLog(Type,CreateTime,IP,UserID,Remark) " +
                            " values(@Type,GETDATE(),@OperateIP,@UserID,@Remark)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserName" , loginname),
                                     new SqlParameter("@Type" , type),   
                                     new SqlParameter("@Remark" , remark),
                                     new SqlParameter("@UserID" , userid),
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            var task = new Task<bool>(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
            task.Start();//异步执行
            return task;
            //return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }
        public static Task<bool> AddOperateLog(string userid,  int type,string message, string operateip)
        {
            string sqlText = "insert into UsersOperatLog(UserID,Remark,Type,CreateTime,IP) " +
                            " values(@UserID,@Message,@Type,GETDATE(),@OperateIP)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserID" , userid),
                                     new SqlParameter("@Type" , type),
                                     new SqlParameter("@Message" , message),
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            var task = new Task<bool>(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
            task.Start();//异步执行
            return task;
            //return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }

        public static Task<bool> UpdateLastIP(string userid, string operateip)
        {
            string sqlText = "Update M_Users set updateTime=getdate(), PrevLoginIP=LastLoginIP,LastLoginIP=@OperateIP where UserID=@UserID ";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserID" , userid), 
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            var task = new Task<bool>(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
            task.Start();//异步执行
            return task;
            //return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }

    }
}
