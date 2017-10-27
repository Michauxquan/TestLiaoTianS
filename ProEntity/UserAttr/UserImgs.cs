using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class UserImgs
    {

        public int AutoID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string ImgUrl { get; set; }
        public int Size { get; set; }
        public int GoodCount { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
