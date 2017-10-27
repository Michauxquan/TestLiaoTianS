using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class Lottery
    {
        [Property("Lower")]
        public int AutoID { get; set; }
        /// <summary>
        /// 彩种代码
        /// </summary>
        public string CPCode { get; set; }
        /// <summary>
        /// 彩种名称
        /// </summary>
        public string CPName { get; set; }
        /// <summary>
        /// 状态 1 正常 9 删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 图标 0 无 1 新品 2 火热
        /// </summary>
        public int IconType { get; set; }
        /// <summary>
        /// 开奖地址
        /// </summary>
        public string ResultUrl { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 开奖间隔
        /// </summary>
        public int OpenTimes { get; set; }
        /// <summary>
        /// 停售时间
        /// </summary>
        public string CloseTime { get; set; }
        /// <summary>
        /// 销售时间
        /// </summary>
        public string OnSaleTime { get; set; }
        /// <summary>
        /// 封盘间隔
        /// </summary>
        public int SealTimes { get; set; }
        /// <summary>
        /// 期数
        /// </summary>
        public int PeriodsNum { get; set; } 

        public string CreateUserID { get; set; }

        public DateTime CreateTime { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
