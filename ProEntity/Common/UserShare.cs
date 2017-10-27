using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class UserShare
    {
        public int AutoID { get; set; } 
        public DateTime CreateTime { get; set; } 
        public string userid { get; set; }
        public int backid { get; set; }
        public string qrcode { get; set; } 
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
