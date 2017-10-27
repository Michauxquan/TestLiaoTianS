using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class LotteryBettAuto
    {
        public long AutoID { get; set; }
        public string BCode { get; set; } 
        public string CPCode { get; set; }
        public string CPName { get; set; } 
        /// <summary>
        /// 期号
        /// </summary>
        public string StartNum { get; set; }
        /// <summary>
        /// 投注期数
        /// </summary>
        public int BettNum { get; set; }
        /// <summary>
        /// 完成期数
        /// </summary>
        public int ComNum { get; set; }
        /// <summary>
        /// 开奖结果
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态 0 未同步 1 同步中 2 已停止
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 中奖后是否停止
        /// </summary>
        public int IsStart { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 中奖总注数
        /// </summary>
        public int Num { get; set; }
        
        /// <summary>
        /// 基础倍数
        /// </summary>
        public int BMuch { get; set; }

        /// <summary>
        /// 倍数
        /// </summary>
        public int PMuch { get; set; } 
        /// <summary>
        /// 奖金
        /// </summary>
        public decimal WinFee { get; set; }
        /// <summary>
        /// 奖金模式
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal TotalFee { get; set; }
        /// <summary>
        /// 完成金额
        /// </summary>
        public decimal ComFee { get; set; }

        /// <summary>
        /// 是付款
        /// </summary>
        public decimal PayFee { get; set; }
        /// <summary>
        /// 返点级别
        /// </summary>
        public decimal RPoint { get; set; }
        /// <summary>
        /// 利润比例
        /// </summary>
        public decimal Profits { get; set; }
        /// <summary>
        ///  自动投注模式 0 利率 1 同倍 3 翻倍
        /// </summary>
        public int BettType { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string  UserID  { get; set; }

        public string UserName { get; set; }

        public string ResultNum { get; set; }
        /// <summary>
        /// 期数 倍数 总金额
        /// </summary>
        public string JsonContent { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 投注模式 0 元 1 角2 分
        /// </summary>
        public int MType { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public int WinType { get; set; }
        public string IP { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
