using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class LotteryOrder
    {
        public long AutoID { get; set; }
        /// <summary>
        /// 投注单号
        /// </summary>
        public string LCode { get; set; }
        /// <summary>
        /// 追号单号
        /// </summary>
        public string BCode { get; set; }
        /// <summary>
        /// 投注ID
        /// </summary>
        public string LID { get; set; }
        public string CPCode { get; set; }
        public string CPName { get; set; }
        public string ModelName { get; set; }

        /// <summary>
        /// 期号
        /// </summary>
        public string IssueNum { get; set; }
        /// <summary>
        /// 开奖结果
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态 0 未开奖 1 开奖中 2 已开奖    --9 开奖失败  轮训处理再次开奖 
        /// </summary>
        public int Status { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 中奖总注数
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 倍数
        /// </summary>
        public int PMuch { get; set; } 
        /// <summary>
        /// 奖金
        /// </summary>
        public decimal WinFee { get; set; }
        /// <summary>
        /// 是付款
        /// </summary>
        public decimal PayFee { get; set; }
        /// <summary>
        /// 返点级别
        /// </summary>
        public decimal RPoint { get; set; }
        /// <summary>
        /// 用户ＩＤ
        /// </summary>
        public string  UserID  { get; set; }

        public string UserName { get; set; }
        public decimal Account { get; set; }
        public decimal AccountChange { get; set; }
        public int PlayType { get; set; }
        public string PlayTypeName { get; set; }
        public string ResultNum { get; set; }
        /// <summary>
        /// 帐变类型 0 收入 1支出
        /// </summary>
        public int AccountType { get; set; }

        /// <summary>
        /// 投注模式 0 元 1 角2 分
        /// </summary>
        public int MType { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int WinType { get; set; }
        public string IP { get; set; }
        public int BettStatus { get; set; }
        public int IsStart { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
