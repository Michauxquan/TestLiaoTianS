using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class SysSetting
    {
        public int AutoID { get; set; } 
        public DateTime CreateTime { get; set; } 
        public string GGContent { get; set; }
        public int IsShowGG { get; set; }
        public int FHLock { get; set; }
        public int RGZLock { get; set; }
        public int BettLock { get; set; }
        public string RGZPoint { get; set; }
        public int MsgLock { get; set; }
        public decimal SysLotteryWin { get; set; }
        public decimal DrawRule { get; set; }
        public decimal DrawMin { get; set; }
        public decimal DrawMax { get; set; }
        public string DrawBTime { get; set; }
        public string DrawETime { get; set; }
        public string  KFUrl { get; set; }
        public decimal PayMin { get; set; }
        public decimal PayMax { get; set; }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(DataRow dr)
        {
            dr.FillData(this);
        }
    }

    public class AppSetting
    {
        public int AutoID { get; set; }
        public string Info { get; set; }
        public string KName { get; set; }
        public string KValue { get; set; }
        public string MinFee { get; set; }
        public string MaxFee { get; set; }
        public DateTime CreateTime { get; set; }
        public int Status { get; set; }
        public int IsShow { get; set; }

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
