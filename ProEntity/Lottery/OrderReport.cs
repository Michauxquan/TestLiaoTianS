using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProEntity
{
    public class OrderReport
    {
        public long AutoID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string CPCode { get; set; }
        public string CPName { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; } 
        public decimal DWDFee { get; set; }
        public decimal DXDSFee { get; set; }
        public decimal LHHFee { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
