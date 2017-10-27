using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class Active
    {
        public int AutoID { get; set; }
        public string Title { get; set; }
        public string Tips { get; set; }
        public string CreateUserID { get; set; }
        public string CreateUser{ get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime BTime { get; set; }
        public DateTime ETime { get; set; } 
        public string Img { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public string UpdUserID { get; set; } 
        public DateTime UpdTime { get; set; }
        public int Type { get; set; }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
