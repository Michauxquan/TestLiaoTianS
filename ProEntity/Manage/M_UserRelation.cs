using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProEntity.Manage
{
   public  class M_UserRelation
    {
       public M_UserRelation()
		{}
       /// <summary>
       /// 
       /// </summary>
       public int AutoID{ get; set; } 

       public string UserID { get; set; }
       public string UserName { get; set; }
       public string ParentID { get; set; }
       public string Parents { get; set; }
       public DateTime CreateTime { get; set; }
       public int Layers { get; set; }
       public void FillData(System.Data.DataRow dr)
       {
           dr.FillData(this);
       }
    }
}
