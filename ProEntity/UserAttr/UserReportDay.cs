using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    [Serializable]
    public class UserReportDay
    {
        public int AutoID { get; set; }

        [Property("Lower")]
        public string UserID { get; set; }

        public string UserName { get; set; }

        public decimal UserPoint { get; set; }
        public decimal UserFHPoint { get; set; }
        public decimal TotalPay { get; set; }

        public decimal TotalDraw { get; set; }

        public decimal TotalPayMent { get; set; }

        public decimal TotalWin { get; set; }

        public decimal TotalReturn { get; set; }

        public DateTime ReportTime { get; set; }

        public decimal TotalRiGZ { get; set; }
        public decimal TotalFenH { get; set; }
        public decimal YL { get; set; }
        public decimal SumFee { get; set; }
        public decimal AccountFee { get; set; }
        public DateTime CreateTime { get; set; }
        public int Type { get; set; }
        public string LoginName { get; set; }
        public int SafeLevel { get; set; }

        /// <summary>
        /// 和局量
        /// </summary>
        public int TotalHJ { get; set; }
        public decimal GainFee { get; set; }
        public decimal TotalActive { get; set; }
        public decimal TotalXJ { get; set; }
        public DateTime Update { get; set; }
        /// <summary>
        /// 0 未结算 1已结算 2 已作废
        /// </summary>
        public int Status { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }

    [Serializable]
    public class UserAccountDay
    {
        public decimal Account { get; set; }
        public int  AllCount { get; set; }
        public int Type { get;set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}

