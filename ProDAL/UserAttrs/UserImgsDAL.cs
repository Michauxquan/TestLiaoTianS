using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL 
{
   public  class UserImgsDAL : BaseDAL
    {
       public static UserImgsDAL BaseProvider = new UserImgsDAL();
       public bool Create(string userID, string imgurl, int size)
       {
           string sql = @"insert into UserImgs(UserID,ImgUrl,Size,Status,CreateTime,GoodCount)" +
                        "values(@UserID,@ImgUrl,@Size,0,getdate(),0)";
           SqlParameter[] paras = { 
                                    new SqlParameter("@UserID",userID), 
                                    new SqlParameter("@Size",size),
                                    new SqlParameter("@ImgUrl",imgurl) 
                                   };
           return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
       }

       public DataTable GetNewImg(int tops=0, int status=-1)
       {
           string sql = @"select "+(tops>0?"top "+tops.ToString():"")+@" *  from UserImgs Where AutoID in(
                select MAX(AutoID) from  (
                    select top 500  * from  UserImgs where 1=1 "+(status>-1?" and Status= "+status.ToString():"") +@" order by AutoID desc
                ) c  Group by UserID )";

           return GetDataTable(sql);

       }
       public bool UpdateStatus(string autoids, int status)
       { 
           SqlParameter[] paras =
           {
               new SqlParameter("@AutoIDS", autoids),
               new SqlParameter("@Status", status)
           };
           return ExecuteNonQuery("M_ImgUpdateStatus", paras, CommandType.StoredProcedure) > 0;

       }
    }
}
