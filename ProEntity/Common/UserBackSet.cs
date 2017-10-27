using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class UserBackSet
    {
        public int AutoID { get; set; }
        public string UserID { get; set; }
        public int BackID { get; set; }
        public int SSCNum { get; set; } 
        public int SSCUseNum { get; set; }
        public int FCNum { get; set; } 
        public int FCUseNum { get; set; } 
        public int X5Num { get; set; }
        public int X5UseNum { get; set; } 
        public int Type { get; set; }
        public BackModel Bmodel { get; set; }

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
