using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProTools
{
    public class KCWBase<T>
    {
        public int Rows { get; set; }
        public string Code { get; set; }
        public string Remain { get; set; }
        public List<T> Data { get; set; }
    }

    public class DataResult
    {
        public string Code { get; set; }
        public string Expect { get; set; }
        public string OpenCode { get; set; }
        public DateTime OpenTime { get; set; }
        public long OpenTimestamp { get; set; } 
    }
    public class StatusResult
    {
        public string Code { get; set; }
        public string Open_Phase { get; set; }
        public string Open_Result { get; set; }
        public int Open_Index { get; set; }
        public DateTime Open_Time { get; set; }
        public DateTime Now { get; set; }
        public DateTime Load_Time { get; set; }
        public DateTime Next_Time { get; set; }
        public string Next_Phase { get; set; }
        public int Next_Index { get; set; }
    }
}
