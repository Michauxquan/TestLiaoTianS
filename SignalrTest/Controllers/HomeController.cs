using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrTest.Common;
using ProBusiness;
using ProEntity.Manage;


namespace SignalrTest.Controllers
{
    public class HomeController : BbaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login1");
            }
            return View();
        }

        public ActionResult SecondPage()
        {
            return View();
        }
        public ActionResult Login()
        {

            return Redirect("/Home/Login1");
            //if (Session["IsLogin"] != null)
            //{
            //    return Redirect("/Home/Login1");
            //}
            //return View();
        }
        public ActionResult LogOut()
        {
            HttpCookie cook = Request.Cookies["cp"];
            if (cook != null)
            {
                cook["status"] = "0";
                Response.Cookies.Add(cook);
                if (CurrentUser != null)
                {
                    M_UsersBusiness.UpdLine(CurrentUser.UserID, 0);
                }
            }
            Session.RemoveAll(); 
            return Redirect("/Home/Login1"); 
        }
        public ActionResult Login1()
        {
            //if (Session["IsLogin"] == null || Session["IsLogin"].ToString() != CommonBusiness.GetAppSet().Where(x => x.KName == "Login").FirstOrDefault().KValue)
            //{
            //    Session.RemoveAll();
            //    return Redirect("/Home/Login");
            //}
            if (CurrentUser != null)
            {
                return Redirect("/Home/Index");
            }
            HttpCookie cook = Request.Cookies["cp"];
            if (cook != null)
            {
                if (cook["status"] == "1")
                {
                    string operateip = OperateIP;
                    int result;
                    M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(cook["username"], cook["pwd"], operateip, out result);
                    if (model != null)
                    {
                        model.LastLoginIP = OperateIP;
                        Session["Manager"] = model;
                        Session["KFUrl"] = CommonBusiness.KFUrl;
                        M_UsersBusiness.UpdLine(model.UserID, 1);
                        return Redirect("/Home/Index");
                    }
                }
            }
            return View();
        }

        public JsonResult LoginCheck(string txtUname, string txtPWD)
        {
            var result = false;
            var msg = "操作失败";
            var Dt = CommonBusiness.GetAppSet().Where(x => x.KName == "Login").FirstOrDefault();
            if (Dt != null && Dt.AutoID > 0)
            {
                if (Dt.KValue == txtUname && txtPWD == Dt.KValue)
                {
                    Session["IsLogin"] = txtPWD;
                    result = true;
                }
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Errmsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd, string remember = "")
        {
            bool bl = false;
            string operateip = OperateIP;
            int result = 0;
            string msg = "";
            ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip, out result);
            if (model != null)
            {
                if (model.Status < 1)
                {
                    model.LastLoginIP = OperateIP;
                    HttpCookie cook = new HttpCookie("cp");
                    cook["username"] = userName;
                    cook["pwd"] = pwd;
                    if (remember == "1")
                    {
                        cook["status"] = remember;
                    }
                    cook.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cook);
                    CurrentUser = model;
                    Session["Manager"] = model;
                    Session["KFUrl"] = CommonBusiness.Select("SysSetting", "top 1 KFUrl", " 1=1 ").ToString();
                    result = 1;
                    M_UsersBusiness.UpdLine(model.UserID, 1);
                }
                else
                {
                    msg = result == 3 ? "用户已被禁闭，请联系管理员" : "用户名或密码错误！";
                }
            }
            else
            {
                msg = result == 3 ? "用户已被禁闭，请联系管理员" : result == 2 ? "用户名不存在" : "用户名或密码错误！";
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Errmsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult Error(string msg = "", string refurl = "/Home/Index", bool qc = true)
        {
            if (qc)
            {
                Session.RemoveAll();
            }
            ViewBag.Url = refurl;
            ViewBag.ErrMsg = !string.IsNullOrEmpty(msg) ? msg : "Sorry..页面没有找到！";
            return View();
        }
        
    }
}
