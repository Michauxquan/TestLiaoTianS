using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
   public class UserOrders
    {
        public int AutoID { get; set; }
        public string OrderCode { get; set; }
        public string SPName { get; set; }
        public string BankName { get; set; }
        public string Sku { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int PayType { get; set; }
        public decimal TotalFee { get; set; }
        public decimal PayFee { get; set; }
        public string OtherCode { get; set; }
        public int Type { get; set; }
        public decimal Num { get; set; }
        public DateTime UpdTime { get; set; }
        public string UpdUserID { get; set; } 
        public string CreateUserID { get; set; }
        public int BankID { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
