using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class BackModel
    {
        public int AutoID { get; set; }
        public decimal WinFee { get; set; }
        public decimal FCType { get; set; }
        public decimal FCFee { get; set; }
        public decimal SSCType { get; set; }
        public DateTime CreateTime { get; set; } 
        public decimal X5Type { get; set; }
        public decimal X5Fee { get; set; }
        public decimal MaxHigFee { get; set; }
        public decimal MaxLowFee { get; set; }  
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
