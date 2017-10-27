using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProEntity.UserAttr
{
    public  class UserAccount
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public decimal AccountFee { get; set; }
        public decimal DiscountFee { get; set; }
        public decimal InteFee { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
