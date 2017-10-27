using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
   public  class ChargeSet
    {
        public int AutoID{get;set;}
        public string UserID{get;set;}
        public decimal Golds{get;set;}
        public DateTime CreateTime{get;set;}
        public int Status{get;set;}
        public string View{get;set;}
        public string Remark{get;set;}
        public string UserName { get; set; }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(DataRow dr)
        {
            dr.FillData(this);
        }

    }
}
