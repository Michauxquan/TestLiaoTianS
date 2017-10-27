using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class LotteryResult
    {
        public long AutoID { get; set; } 
        public string CPCode { get; set; }
        public string CPName { get; set; } 
        /// <summary>
        /// 期号
        /// </summary>
        public string IssueNum { get; set; }

        /// <summary>
        /// 官方开奖结果
        /// </summary>
        public string ResultNum { get; set; }

        /// <summary>
        /// 手动开奖结果
        /// </summary>
        public string AutoNum { get; set; }
        /// <summary>
        /// 状态 0 未开奖 1 开奖中 2 已开奖    --9 开奖失败  轮训处理再次开奖 
        /// </summary>
        public int Status { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime OpenTime { get; set; }
        public int Num { get; set; }
        /// <summary>
        /// 中奖总注数
        /// </summary>
        public int PizeNum { get; set; }
        /// <summary>
        /// 中奖人数
        /// </summary>
        public int UserNum { get; set; }
        /// <summary>
        /// 剩余开奖时间
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 和值 开奖结果的和值
        /// </summary>
        public int SumNum { get; set; } 

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
