using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness.Manage;
using ProBusiness;

namespace SignalrTest.Controllers
{
    public class BbaseController : Controller
    {
        protected int PageSize = 12;

        protected Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();

        /// <summary>
        /// 登录IP
        /// </summary>
        protected string OperateIP
        {
            get
            {
                return string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            }
        }
        

        protected ProEntity.Manage.M_Users CurrentUser
        {
            get
            {
                if (Session["Manager"] == null)
                {
                    return null;
                }
                else
                {
                    return (ProEntity.Manage.M_Users)Session["Manager"];
                }
            }
            set { Session["Manager"] = value; }
        }
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool IsLimits(string menucode)
        {
            if (Session["Manager"] != null)
            {
                ProEntity.Manage.M_Users model = (ProEntity.Manage.M_Users)Session["Manager"];
                if (model.Menus.Where(m => m.MenuCode == menucode).Count() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>  
        /// 功能：产生数字和字符混合的随机字符串  
        /// </summary>  
        /// <param name="codecount">字符串的个数</param>  
        /// <returns></returns>  
        public string CreateRandomCode(int codecount)
        {

            // 数字和字符混合字符串  
            string allchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n";
            //分割成数组  
            string[] allchararray = allchar.Split(',');
            string randomcode = "";

            //随机数实例  
            System.Random rand = new System.Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codecount; i++)
            {
                //获取一个随机数  
                int t = rand.Next(allchararray.Length);
                //合成随机字符串  
                randomcode += allchararray[t];
            }
            return randomcode;
        }

        public bool checkGolds(string route)
        {
            var model = WebSetBusiness.GetChargeSetDetail(route.ToLower());
            if (model == null)
            {
                return true;
            }
            if (model.Golds == 0 || model.Status == 0)
            {
                return true;
            }
            return CommonBusiness.UpdateUserAccount(model.Golds, CurrentUser.UserID, 1,
                string.IsNullOrEmpty(model.Remark) ? "查看信息扣除" : model.Remark); ;
        }
        [Serializable]
        public class SessionAccount
        {
            public bool Status { get; set; }
            public DateTime ExpTime { get; set; }
        }

        public bool getAccountSession(string keyname)
        {
            bool result = false;
            if (Session[keyname] != null)
            {
                var model = (SessionAccount)Session[keyname];
                if (DateTime.Now.CompareTo(model.ExpTime) == -1)
                {
                    result = model.Status;
                }
            }
            return result;
        }
    }
}
