using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEntity.Manage;

namespace ProEntity
{
    public class LogEntity
    {
        [Property("Lower")]
        public int AutoID { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public string UserID { get; set; } 

        public string UserName { get; set; }

        public string IP { get; set; }

        public string Sheng { get; set; }

        public string Shi { get; set; }

        public string Qu { get; set; }

        public int Type { get; set; }
        public string Avatar { get; set; }

        public string IpName { get; set; } 

        public string LoginName { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
