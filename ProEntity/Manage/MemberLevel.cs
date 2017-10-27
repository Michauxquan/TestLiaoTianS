using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class MemberLevel
    {
        public int AutoID { get; set; }
        public string LevelID { get; set; }
        public string Name { get; set; }
        public decimal IntegFeeMore { get; set; }
        public decimal DiscountFee { get; set; }
        public decimal Golds { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public int Origin { get; set; }
        public string ImgUrl { get; set; }
        public string CreateUserID { get; set; }
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
