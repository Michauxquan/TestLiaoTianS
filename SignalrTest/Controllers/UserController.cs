using Newtonsoft.Json;
using ProBusiness;
using ProBusiness.UserAttrs;
using ProEntity;
using ProEntity.Manage;
using ProEntity.UserAttr;
using SignalrTest.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SignalrTest.Controllers
{
    [UserAuthorize]
    public class UserController : BbaseController
    {

        public ActionResult UserIndex(string type = "record")
        {
            UserAccount usacc = M_UsersBusiness.GetUserAccount(CurrentUser.UserID);
            ViewBag.lgname = CurrentUser.LoginName;
            ViewBag.accountfee = usacc.AccountFee;
            ViewBag.Type = type;
            return View();
        }
        //
        // GET: /User/

        public ActionResult UserList(string id = "")
        {
            ViewBag.UserID = id;
            ViewBag.NUID = CurrentUser.AutoID;
            ViewBag.Layers = CurrentUser.Layers;
            ViewBag.CUserID = CurrentUser.UserID;
            ViewBag.CloginName = CurrentUser.LoginName;
            return View();
        }

        public ActionResult UserAdd()
        {
            var model = M_UsersBusiness.GetUserDetail(CurrentUser.UserID);
            ViewBag.Rebate = model.Rebate;
            ViewBag.UsableRebate = model.UsableRebate;
            return View();
        }
        public ActionResult UserDefault()
        {
            ViewBag.Type = 0;
            if (CurrentUser != null)
                ViewBag.Type = CurrentUser.Type;
            return View();
        }

        #region Ajax

        public JsonResult UserInfoList(int type, bool orderby, string username, string userid, string accountmin, string accountmax, string clumon, int pageIndex, int pageSize, bool mytype = false, int soucetype = 0)
        {
            int total = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetUsersRelationList(pageSize, pageIndex, ref total, ref pageCount, string.IsNullOrEmpty(userid) ? CurrentUser.UserID : userid, type, -1, username, clumon, orderby, accountmin, accountmax, mytype, soucetype);

            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult UserAdd(int type, string username, string loginpwd, string loginname, int backid)
        {
            string Errmsg = "";
            M_Users user = new M_Users()
            {
                Type = type,
                SourceType = 0,
                UserName = username,
                LoginName = loginname,
                LoginPwd = loginpwd,
                Description = "用户新增",
                Rebate = 0,
                RoleID = ""
            };
            if (backid == -1)
            {
                backid = 41;
            }
            var result = M_UsersBusiness.CreateM_User(user, ref Errmsg, CurrentUser.UserID, backid);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Errmsg", Errmsg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult UpdUserBackSet(string userid, int backid)
        {
            var result = false;
            string msg = "操作失败";
            if (backid < 41)
            {
                int totalcount = 0;
                var mback = BackModelSetBusiness.GetBackSet(CurrentUser.UserID, 0, 1, 1, ref totalcount, ref totalcount)
                    .FirstOrDefault();
                if (mback != null && mback.BackID != 0 && mback.BackID < backid)
                {
                    if (userid.ToLower() != CurrentUser.UserID.ToLower())
                    {
                        result = BackModelSetBusiness.UpdUserBackSet(userid, backid);
                    }
                }
                else
                {
                    msg = "下级返点模式不能大于本级返点模式";
                }
            }
            else
            {
                msg = "所在用户组无权限修改为1920以下模式";
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("msg", result ? "修改成功" : msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UserMoneyChange(int id, decimal changemoney, string accountpwd, string remark = "")
        {
            string msg = "";
            var result = false;
            var user = M_UsersBusiness.GetUserDetail(id);
            if (user != null && user.AutoID > 0)
            {
                if (CurrentUser.AccountPwd.ToLower() !=
                    ProBusiness.Encrypt.GetEncryptPwd(accountpwd, CurrentUser.LoginName).ToLower())
                {
                    msg = "资金密码错误,转账失败";
                }
                else if (CurrentUser.UserID.ToLower() == user.UserID.ToLower())
                {
                    msg = "不能对自己进行转账";
                }
                else
                {
                    result = M_UsersBusiness.UserChangeMoney(user.UserID, CurrentUser.UserID, changemoney, remark,
                        ref msg);
                }
            }
            else
            {
                msg = "用户不存在";
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Msg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult UserUpdPoint(string id, decimal addpoint)
        {
            var result = M_UsersBusiness.UpdateM_UserRebate(id, CurrentUser.UserID, addpoint);
            if (result)
            {
                var model = CurrentUser;
                model.UsableRebate = model.UsableRebate - addpoint;
                Session["Manage"] = model;
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("ErrMsg", result ? "" : "配置失败,请稍后再试");
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpPost]
        public JsonResult GetChildList(string userid = "", bool type = false)
        {
            var list = M_UsersBusiness.GetUsersRelationList(userid == "" ? CurrentUser.UserID : userid, type);
            JsonDictionary.Add("items", list);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserAccount()
        {
            var model = M_UsersBusiness.GetUserAccount(CurrentUser.UserID);
            JsonDictionary.Add("item", model);
            JsonDictionary.Add("LoginName", CurrentUser != null ? CurrentUser.UserName : "");
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetMsgList(int type, int pageIndex)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserReplyBusiness.GetUserReplys(CurrentUser.UserID, "", type, PageSize, pageIndex, ref total,
                ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }

        public JsonResult SaveReply(string entity)
        {
            var result = false;
            string msg = "提交失败，请稍后再试！";
            UserReply model = JsonConvert.DeserializeObject<UserReply>(entity);
            model.FromReplyID = string.IsNullOrEmpty(model.FromReplyID) ? "" : model.FromReplyID;
            model.FromReplyUserID = string.IsNullOrEmpty(model.FromReplyUserID) ? "" : model.FromReplyUserID;
            int haschild = model.GUID.IndexOf("ZSXJ,");
            if (haschild > -1)
            {
                haschild = 1;
            }
            result = UserReplyBusiness.CreateUserReply(model.GUID.Replace("ZSXJ,", ""), model.Content, model.Title, CurrentUser.UserID, model.FromReplyID, model.FromReplyUserID, model.Type, model.GUID.IndexOf("ZSXJ,"), ref msg);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("ErrMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdStatus(string ids, int status)
        {
            var result = false;
            string msg = "提交失败，请稍后再试！";
            result = UserReplyBusiness.UpdateReplyStatus(ids.TrimEnd(',').Replace(",", "','"), status);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("ErrMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult ReplyDetail(string id)
        {
            var model = UserReplyBusiness.GetReplyDetail(id);
            JsonDictionary.Add("Item", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetMsgCount()
        {
            var cint = UserReplyBusiness.GetNotReadReplay(-1, CurrentUser.UserID, 0);
            JsonDictionary.Add("result", cint);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveOrders(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            UserOrders model = serializer.Deserialize<UserOrders>(entity);
            model.SPName = "";
            model.UserName = CurrentUser.LoginName;
            model.BankName = CurrentUser.LoginName + " " + model.BankName;
            string error = "操作成功";
            var result = false;
            if (model.AutoID == -1)
            {
                var tempu = M_UsersBusiness.GetUserDetailByLoginName(model.UserName, 0);
                string ordercode = DateTime.Now.ToString("yyMMddhhmmss") + CurrentUser.AutoID;
                if (tempu != null && !string.IsNullOrEmpty(tempu.UserID))
                {
                    result = UserOrdersBusiness.CreateUserOrder(ordercode, model.PayType, model.SPName,
                        model.BankName, model.Sku, model.Content, model.TotalFee, model.OtherCode,
                        Convert.ToInt32(model.TotalFee), model.Type, model.PayFee, tempu.UserID, CurrentUser.UserID, OperateIP);
                }
                else
                {
                    error = "登陆账号不存在,订单登记失败";
                }
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("Msg", error);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DrawAppSet()
        {
            SysSetting syset = BackModelSetBusiness.GetSysSetting();
            JsonDictionary.Add("btime", syset.DrawBTime);
            JsonDictionary.Add("etime", syset.DrawETime);
            JsonDictionary.Add("drawmin", syset.DrawMin);
            JsonDictionary.Add("drawmax", syset.DrawMax);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }



        public JsonResult UserRPTList(int pageIndex = 1, string userid = "", int isxj = 1, string btime = "", string etime = "")
        {
            int totalCount = 0;
            int pageCount = 0;
            if (string.IsNullOrEmpty(userid))
            {
                userid = CurrentUser.UserID;
            }
            if (string.IsNullOrEmpty(btime))
            {
                btime = "2017-01-01";
            }
            if (string.IsNullOrEmpty(etime))
            {
                etime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }
            UserReportDay listpage = new UserReportDay();
            var list = UserReportBussiness.GetUserRPTList(userid, pageIndex, PageSize, isxj, ref totalCount, ref pageCount, btime, etime, ref listpage);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("itempage", listpage);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        public JsonResult UpdateUserStatus(int id, int status)
        {
            bool bl = false;
            var user = M_UsersBusiness.GetUserDetail(id);
            if (user != null && user.AutoID > 0)
            {
                bl = M_UsersBusiness.UpdateM_UserStatus(user.UserID, status);
            }
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdPwd(int id, int type, string pwd)
        {
            bool bl = false;
            var user = M_UsersBusiness.GetUserDetail(id);
            if (user != null && user.AutoID > 0)
            {
                if (type == 1)
                {
                    bl = M_UsersBusiness.UpdatePwd(user.LoginName, pwd);
                }
                else
                {
                    bl = M_UsersBusiness.UpdateAccountPwd(user.UserID, user.LoginName, pwd);
                }
            }
            JsonDictionary.Add("result", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserTeamRPT(string userid = "", string btime = "", string etime = "")
        {
            if (string.IsNullOrEmpty(userid))
            {
                userid = CurrentUser.UserID;
            }
            if (string.IsNullOrEmpty(btime))
            {
                btime = "2017-01-01";
            }
            if (string.IsNullOrEmpty(etime))
            {
                etime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            List<UserReportDay> list = new List<UserReportDay>();
            string[] ulist = userid.Split(',');
            foreach (string ss in ulist)
            {
                if (!string.IsNullOrEmpty(ss.Trim()))
                {
                    list.Add(UserReportBussiness.GetUserTeamRPT(ss, btime, etime));
                }
            }
            JsonDictionary.Add("Item", list);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetAllParents(string userid)
        {
            if (string.IsNullOrEmpty(userid))
            {
                userid = CurrentUser.UserID;
            }
            string result = M_UsersBusiness.GetUserAllParentsName(userid);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion

    }
}
