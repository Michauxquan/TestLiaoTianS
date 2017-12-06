using Newtonsoft.Json;
using ProBusiness;
using ProBusiness.Manage;
using ProBusiness.UserAttrs;
using ProEntity.Manage;
using ProTools;
using SignalrTest.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SignalrTest.Controllers
{
    [UserAuthorize]
    public class HelpController : BbaseController
    {
        //
        // GET: /Help/ 
        public ActionResult ZSRecord(string cpcode = "CQSSC")
        {
            return View();
        }
        public ActionResult ChangePsw()
        {
            return View();
        }
        public ActionResult MoneyInOut()
        {
            return View();
        }
        public ActionResult AddMember()
        {
            if (CurrentUser == null || CurrentUser.Type == 0)
                return Content("暂无权限");
            int total = 0;
            ViewBag.BackList = BackModelSetBusiness.GetBackSet(CurrentUser.UserID, 1, 300, 1, ref total, ref total);
            return View();
        }

        public ActionResult MoneyRecord(string start = "", string end = "", string ordertype = "-1", int pageIndex = 1)
        {
            int total = 0;
            int pageTotal = 0;
            ViewBag.Start = start == "" ? DateTime.Now.ToString("yyyy-MM-dd") : start;
            ViewBag.End = end == "" ? DateTime.Now.ToString("yyyy-MM-dd") : end;
            var list = UserReportBussiness.GetLotteryOrderReportList(ViewBag.Start + " 00:00:00", ViewBag.End + " 23:59:59", int.Parse(ordertype), "", CurrentUser.UserID,
                "", "", "", "", -1, pageIndex, 15,
                ref total, ref pageTotal, 3);
            ViewData["items"] = list;
            ViewData["totalCount"] = total;
            ViewData["pageCount"] = pageTotal;
            ViewData["pageIndex"] = pageIndex;

            ViewBag.Type = ordertype;
            return View();
        }

        public ActionResult BettRecord(string cpcode, string issuenum,
             string btime, string etime, string status = "-1", string lcode = "", string type = "",
            int pageIndex = 1, string username = "", int selfrange = 3, int winType = -1, string bcode = "")
        {

            int total = 0;
            int pageTotal = 0;
            if (!string.IsNullOrEmpty(etime))
            {
                etime = Convert.ToDateTime(etime).ToString("yyyy-MM-dd");
            }
            else
                etime = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(btime))
            {
                btime = Convert.ToDateTime(btime).ToString("yyyy-MM-dd");
            }
            else
                btime = DateTime.Now.ToString("yyyy-MM-dd");

            var items = LotteryOrderBusiness.GetLotteryOrder("", cpcode, CurrentUser.UserID, lcode, issuenum, type, status, winType, 15, pageIndex, ref total,
                ref pageTotal, selfrange, btime, etime, username, " a.AutoID desc", bcode);

            ViewData["items"] = items;
            ViewData["totalCount"] = total;
            ViewData["pageCount"] = pageTotal;
            ViewData["pageIndex"] = pageIndex;

            ViewBag.Start = btime;
            ViewBag.End = etime;
            ViewBag.Type = type;
            ViewBag.CPCode = cpcode; ViewBag.status = status; ViewBag.issuenum = issuenum;


            return View();
        }
        public ActionResult UserReport(string btime, string etime, int pageIndex = 1, int pageSize = 15)
        {
            int total = 0;
            int pageTotal = 0;
            if (!string.IsNullOrEmpty(etime))
            {
                etime = Convert.ToDateTime(etime).ToString("yyyy-MM-dd");
            }
            else
                etime = DateTime.Now.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(btime))
            {
                btime = Convert.ToDateTime(btime).ToString("yyyy-MM-dd");
            }
            else
                btime = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");


            var list = UserReportBussiness.GetReporSumtList(btime, etime, pageIndex, pageSize,
                ref total, ref pageTotal);
            ViewData["items"] = list;
            ViewData["totalCount"] = total;
            ViewData["pageCount"] = pageTotal;
            ViewData["pageIndex"] = pageIndex;
            ViewBag.Start = btime;
            ViewBag.End = etime;
            return View();
        }

        public JsonResult RestPwd(string loginname, string useremail)
        {
            string newpwd = CreateRandomCode(6);
            string errorMsg = "";
            var result = false;
            if (M_UsersBusiness.CheckEmail(loginname, useremail))
            {
                result = M_UsersBusiness.UpdatePwd(loginname, newpwd);
                if (result)
                {
                    StringBuilder bodyInfo = new StringBuilder();
                    bodyInfo.Append("亲爱会员：<br/>");
                    bodyInfo.Append("    您好！<br/>你于");
                    bodyInfo.Append(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"));
                    bodyInfo.Append("通过<a href='#'>"+ ConfigurationManager.AppSettings["WebUrl"] + "/Help/Forget</a>忘记密码功能，审请重置密码。<br/>");
                    bodyInfo.Append("　　　重置之后的个人密码为：" + newpwd + "<br/>请妥善保管");
                    SendMail email = new SendMail();
                    email.mailFrom = ConfigurationManager.AppSettings["EmailAccount"];
                    email.mailPwd = ConfigurationManager.AppSettings["EmailPwd"];
                    email.isEnableSsl = true;
                    email.mailSubject = "会员中心--充值密码";
                    email.mailBody = bodyInfo.ToString();
                    email.isbodyHtml = true; //是否是HTML
                    email.host = ConfigurationManager.AppSettings["EmailHost"]; //如果是QQ邮箱则：smtp:qq.com,依次类推
                    email.mailToArray = new string[] { useremail }; //接收者邮件集合 
                    result = email.Send();
                }
                else
                {
                    errorMsg = "发送邮件失败，请稍后再试！";
                }
            }
            else
            {
                errorMsg = "注册邮箱与用户不符！";
            }
            JsonDictionary.Add("errorMsg", errorMsg);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveFeedBack(string entity)
        {
            var result = false;
            string msg = "提交失败，请稍后再试！";
            if (CurrentUser == null)
            {
                msg = "您尚未登录，请登录后在操作！";
            }
            else
            {
                FeedBack model = JsonConvert.DeserializeObject<FeedBack>(entity);
                model.CreateUserID = CurrentUser.CreateUserID;
                result = FeedBackBusiness.InsertFeedBack(model);
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("errorMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateUserInfo(string username, string realname = "", string accountpwd = "",
            int asktype = 1, string answercontent = "")
        {
            M_Users model = CurrentUser;
            model.UserName = username;
            if (!string.IsNullOrEmpty(username))
            {
                model.UserName = username;
            }
            if (!string.IsNullOrEmpty(realname))
            {
                model.RealName = realname;
            }
            if (!string.IsNullOrEmpty(accountpwd))
            {
                model.AccountPwd = Encrypt.GetEncryptPwd(accountpwd, CurrentUser.LoginName);
            }
            if (!string.IsNullOrEmpty(answercontent))
            {
                model.AnswerContent = answercontent;
            }
            if (asktype > 0)
            {
                model.AskType = asktype;
            }
            var result = M_UsersBusiness.UpdateUserInfo(model.UserID, model.UserName, model.RealName, model.AccountPwd,
                model.AskType, model.AnswerContent);
            JsonDictionary.Add("content", result ? "修改成功" : "修改失败");
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetActive(int type = 0, int tops = 9)
        {
            var list = WebSetBusiness.GetActiveList(type, tops);
            JsonDictionary.Add("Items", list);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ReplyList(int type, int pageIndex, int pageSize)
        {

            int total = 0;
            int pageCount = 0;
            var list = UserReplyBusiness.GetUserReplys(type == 1 ? "" : CurrentUser.UserID,
                type == 1 ? CurrentUser.UserID : "", type == 2 ? 1 : type, pageSize, pageIndex, ref total, ref pageCount);
            if (type == 2)
            {
                list.ForEach(x =>
                {
                    if (x.Status == 0)
                    {
                        x.Content = "该信息共有" + x.Content.Length + "个字...";
                    }
                }
            );
            }
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteReply(string replyid)
        {
            JsonDictionary.Add("result", UserReplyBusiness.DeleteReply(replyid));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #region 团队 


        public JsonResult GetUserNextBackSet(bool iseque = false, string userid = "")
        {
            int totalcoount = 0, pagecount = 0;
            if (iseque)
            {
                JsonDictionary.Add("Items",
               BackModelSetBusiness.GetBackModel(5000, 1, ref totalcoount, ref pagecount,
                   " and a.WinFee<=( select isnull(WinFee,0) from BackModel where autoid in( select backid from UserBackSet where Userid='" + CurrentUser.UserID + "' and type=0 ) )", true));
            }
            else
            {
                JsonDictionary.Add("Items",
               BackModelSetBusiness.GetBackModel(5000, 1, ref totalcoount, ref pagecount,
                   " and a.WinFee<( select isnull(WinFee,0) from BackModel where autoid in( select backid from UserBackSet where Userid='" + CurrentUser.UserID + "' and type=0 ) )", true));
            }
            if (string.IsNullOrEmpty(userid))
            {
                userid = CurrentUser.UserID;
            }
            JsonDictionary.Add("Item",
               BackModelSetBusiness.GetBackModel(1, 1, ref totalcoount, ref pagecount,
                   " and  autoid in( select backid from UserBackSet where Userid='" + userid + "' and type=0  )"));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserPeiErBackSet()
        {
            int totalcoount = 0, pagecount = 0;
            JsonDictionary.Add("Items",
                BackModelSetBusiness.GetBackModel(5000, 1, ref totalcoount, ref pagecount,
                    " and a.WinFee<( select isnull(WinFee,0) from BackModel where autoid in( select backid from UserBackSet where Userid='" + CurrentUser.UserID + "' and type=1 ) )", true));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetBackModelDetail(int autoid)
        {
            var list = BackModelSetBusiness.GetBackById(autoid);
            JsonDictionary.Add("Item", list);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult GetUserReportDay(int pageIndex, int pagesize, string username, string userid, string btime, string etime)
        {
            int totalCount = 0;
            int pageCount = 0;
            var list = UserReportBussiness.GetReporSumtList(btime, etime, userid, pageIndex, pagesize, ref totalCount,
                ref pageCount);
            var item = UserReportBussiness.GetReportAllDays(btime, etime, userid);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("item", item);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetTeamAccount(int pageIndex, int pagesize, string userid)
        {
            int totalCount = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetTeamAccount(userid, pageIndex, pagesize, ref totalCount,
                ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
