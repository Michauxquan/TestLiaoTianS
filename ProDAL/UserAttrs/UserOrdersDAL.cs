using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class UserOrdersDAL : BaseDAL
    {
        public static UserOrdersDAL BaseProvider = new UserOrdersDAL();
        public bool CreateUserOrder(string levelid, int paytype, string orderCode, string userID,out string msg)
        {
            string result = "";
            SqlParameter[] paras = { 
                                    new SqlParameter("@Result",result),
                                    new SqlParameter("@UserID",userID),
                                    new SqlParameter("@LevelID",levelid),
                                    new SqlParameter("@PayType",paytype),
                                    new SqlParameter("@OrderCode",orderCode),    
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("M_InserUserOrder", paras, CommandType.StoredProcedure) ;
            msg = paras[0].Value.ToString();
            return string.IsNullOrEmpty(result);
        }
        public bool CreateUserOrder(string ordercode, int paytype, string spname, string bankinfo, string sku, string content, decimal totalfee, string othercode, int type, decimal num, decimal paytfee, string userID, string createid, string operatip)
        {
            string sql = @"INSERT INTO [UserOrders]([OrderCode],[BankName],[SPName],[Sku],[Content],[CreateTime],[Status],[UserID],[PayType],[TotalFee],[OtherCode],[Type],[Num],[PayFee],CreateUserID,IP)
                    VALUES (@OrderCode,@BankName,@SPName,@Sku,@Content,getdate(), 0,@UserID, @PayType, @TotalFee, @OtherCode, @Type, @Num,@PayFee,@CreateUserID,@IP)";
            SqlParameter[] paras = { 
                                    new SqlParameter("@SPName",spname),
                                    new SqlParameter("@BankName",bankinfo),
                                    new SqlParameter("@UserID",userID),
                                    new SqlParameter("@CreateUserID",createid),
                                    new SqlParameter("@IP",operatip), 
                                    new SqlParameter("@Sku",sku),
                                    new SqlParameter("@PayType",paytype),
                                    new SqlParameter("@PayFee",paytfee),
                                    new SqlParameter("@Content",content),
                                    new SqlParameter("@TotalFee",totalfee),
                                    new SqlParameter("@OtherCode",othercode),
                                    new SqlParameter("@Type",type),
                                    new SqlParameter("@Num",num),
                                    new SqlParameter("@OrderCode",ordercode),    
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0; 
        }
        public string CreateUserDraw(string ordercode, int bankid, decimal totalfee, string createuserid, string operatip)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@msg",SqlDbType.VarChar,300),
                                    new SqlParameter("@BankID",bankid), 
                                    new SqlParameter("@CreateUserID",createuserid), 
                                    new SqlParameter("@TotalFee",totalfee), 
                                    new SqlParameter("@IP",operatip),
                                    new SqlParameter("@OrderCode",ordercode),    
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("P_InsertUserDraw", paras, CommandType.StoredProcedure);
            return paras[0].Value.ToString();
        }

        public DataTable GetUserOrderDetail(string ordercode)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@OrderCode",ordercode)
                                   };

            return GetDataTable("select * from UserOrders where OrderCode=@OrderCode", paras, CommandType.Text);
        }

        public DataTable GetUserDrawDetail(string ordercode)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@OrderCode",ordercode)
                                   };

            return GetDataTable("select * from UserDraws where OrderCode=@OrderCode", paras, CommandType.Text);
        }

        public bool OrderAuditting(string ordercode, string othercode, decimal payFee)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@OrderCode",ordercode),
                                    new SqlParameter("@OtherCode",othercode),
                                    new SqlParameter("@PayFee",payFee),    
                                   };
            return ExecuteNonQuery("M_OrderAuditting", paras, CommandType.StoredProcedure) > 0; 
        }

        public bool DrawAuditting(string ordercode, int status)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@OrderCode",ordercode),
                                     new SqlParameter("@Status",status)    
                                   };
            return ExecuteNonQuery("M_DrawAuditting", paras, CommandType.StoredProcedure) > 0;
        }
        public bool WagesAuditting(string ids, string ip)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ids",ids), 
                                    new SqlParameter("@IP",ip),    
                                   };
            return ExecuteNonQuery("M_WagesAuditting", paras, CommandType.StoredProcedure) > 0;
        }


        public bool ProfitShareAuditting(string ids, string ip)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ids",ids), 
                                    new SqlParameter("@IP",ip),    
                                   };
            return ExecuteNonQuery("M_ProfitShareAuditting", paras, CommandType.StoredProcedure) > 0;
        }
    }
}
