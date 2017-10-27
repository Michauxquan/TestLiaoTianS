using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class Plays
    {
        public int AutoID { get; set; }
        public string PID { get; set; }
        public string PCode { get; set; }
        public string PName { get; set; }
        public DateTime CreateTime { get; set; }
        public int Layer { get; set; }
        public int PLen { get; set; }
        public string PIDS { get; set; }
        public int Status { get; set; }

        public List<Plays> ChildPlays{get;set;} 
        // , , ,拼接
        public string Content { get; set; }
        // , , ,拼接
        public string SpaceRate { get; set; }
        //玩法介绍
        public string PlayInfo { get; set; }

        ///单注限额
        public string BettMax { get; set; }

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
