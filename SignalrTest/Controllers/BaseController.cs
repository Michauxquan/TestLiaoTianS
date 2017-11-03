using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalrTest.Controllers;
//工具 -> 库程序包管理器 -> 程序包管理器控制台 输入下面命令  
//install-package Microsoft.AspNet.SignalR -Version 1.1.4  
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Hubs;
using ProBusiness;
using ProEntity;
using ProBusiness.UserAttrs;

namespace SignalR.Controllers 
{

    //所有 hub  
    public class AllHub : Hub 
    {

        /// <summary>
        /// The count of users connected.
        /// </summary>
        public static List<string> Users = new List<string>();

        public void SendSingle()
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<AllHub>();

        }

        /// <summary>
        /// Sends the update user count to the listening view.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void Send(int count)
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<AllHub>();
            context.Clients.All.updateUsersOnlineCount(count);
        }

        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }

            // Send the current count of users
            Send(Users.Count);

            var context = GlobalHost.ConnectionManager.GetHubContext<AllHub>();
            context.Clients.Client(clientId).updateUserName(clientId);


            return base.OnConnected();
        }

        /// <summary>
        /// The OnReconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            string clientId = GetClientId();
            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
            }

            // Send the current count of users
            Send(Users.Count);

            return base.OnReconnected();
        }

        /// <summary>
        /// The OnDisconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnDisconnected()
        {
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) > -1)
            {
                Users.Remove(clientId);
            }

            // Send the current count of users
            Send(Users.Count);

            return base.OnDisconnected();
        }

        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify
        /// a connection.
        /// </summary>
        /// <returns>The client Id.</returns>
        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                // clientId passed from application 
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.ConnectionId;
            }

            return clientId;
        }
    }

    //当前 hub  
    public class CurHub : Hub
    {
        public void SetRecGroup(string id)//设置接收组  
        {
            this.Groups.Add(this.Context.ConnectionId, id);
        }

        

    }

    [HubName("ChatRoomHub")]
    public class ChatHub : Hub
    {
        static List<UserEntity> users = new List<UserEntity>();
        public void UserEnter(string nickName)
        {
            UserEntity userEntity = new UserEntity
            {
                NickName = nickName,
                ConnectionId = Context.ConnectionId
            };
            if (!users.Where(x => x.NickName == "系统管理员").Any()) {
                UserEntity userEntity2 = new UserEntity
                {
                    NickName = "系统管理员",
                    ConnectionId = Context.ConnectionId
                };
                users.Add(userEntity2);
            }
            if (!users.Where(x => x.NickName == nickName).Any())
            {
                users.Add(userEntity);                
            }
            Clients.All.NotifyUserEnter(nickName, users);
        }

        public void SendMessage(string nickName, string message, string issuenum, string uid)
        { 
            Clients.All.NotifySendMessage(nickName, message);
        }

        public override Task OnDisconnected()
        {
            var currentUser = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (currentUser != null)
            {
                users.Remove(currentUser);
                Clients.Others.NotifyUserLeft(currentUser.NickName, users);
            }
            return base.OnDisconnected();
        }
    }

    [HubName("CQRoomHub")]
    public class CQChatHub : Hub
    {
        static List<UserEntity> users = new List<UserEntity>();
        public void UserEnter(string nickName)
        {
            UserEntity userEntity = new UserEntity
            {
                NickName = nickName,
                ConnectionId = Context.ConnectionId
            };
            if (!users.Where(x => x.NickName == "系统管理员").Any())
            {
                UserEntity userEntity2 = new UserEntity
                {
                    NickName = "系统管理员",
                    ConnectionId = Context.ConnectionId
                };
                users.Add(userEntity2);
            }
            if (!users.Where(x => x.NickName == nickName).Any())
            {
                users.Add(userEntity);
            }
            Clients.All.NotifyUserEnter(nickName, users);
        }

        public void SendMessage(string nickName, string message, string issuenum, string uid)
        {
       
            Clients.All.NotifySendMessage(nickName, message);
        }

        public override Task OnDisconnected()
        {
            var currentUser = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (currentUser != null)
            {
                users.Remove(currentUser);
                Clients.Others.NotifyUserLeft(currentUser.NickName, users);
            }
            return base.OnDisconnected();
        }
    }
    [HubName("HGRoomHub")]
    public class HGChatHub : Hub
    {
        static List<UserEntity> users = new List<UserEntity>();
        public void UserEnter(string nickName)
        {
            UserEntity userEntity = new UserEntity
            {
                NickName = nickName,
                ConnectionId = Context.ConnectionId
            };
            if (!users.Where(x => x.NickName == "系统管理员").Any())
            {
                UserEntity userEntity2 = new UserEntity
                {
                    NickName = "系统管理员",
                    ConnectionId = Context.ConnectionId
                };
                users.Add(userEntity2);
            }
            if (!users.Where(x => x.NickName == nickName).Any())
            {
                users.Add(userEntity);
            }
            Clients.All.NotifyUserEnter(nickName, users);
        }

        public void SendMessage(string nickName, string message, string issuenum, string uid)
        {
            Clients.All.NotifySendMessage(nickName, message);
        }

        public override Task OnDisconnected()
        {
            var currentUser = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (currentUser != null)
            {
                users.Remove(currentUser);
                Clients.Others.NotifyUserLeft(currentUser.NickName, users);
            }
            return base.OnDisconnected();
        }
    }

    [HubName("DJRoomHub")]
    public class DJChatHub : Hub
    {
        static List<UserEntity> users = new List<UserEntity>();
        public void UserEnter(string nickName)
        {
            UserEntity userEntity = new UserEntity
            {
                NickName = nickName,
                ConnectionId = Context.ConnectionId
            };
            if (!users.Where(x => x.NickName == "系统管理员").Any())
            {
                UserEntity userEntity2 = new UserEntity
                {
                    NickName = "系统管理员",
                    ConnectionId = Context.ConnectionId
                };
                users.Add(userEntity2);
            }
            if (!users.Where(x => x.NickName == nickName).Any())
            {
                users.Add(userEntity);
            }
            Clients.All.NotifyUserEnter(nickName, users);
        }

        public void SendMessage(string nickName, string message,string issuenum,string uid)
        {
           // TaskService.BasService.MessageInfo(message, "NDJKLB", issuenum, uid);
            Clients.All.NotifySendMessage(nickName, message);
        }

        public override Task OnDisconnected()
        {
            var currentUser = users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (currentUser != null)
            {
                users.Remove(currentUser);
                Clients.Others.NotifyUserLeft(currentUser.NickName, users);
            }
            return base.OnDisconnected();
        }
    }
    public class UserEntity
    {
        public string NickName { get; set; }

        public string ConnectionId { get; set; }
        public string Actar { get; set; }
    }


    public class BaseController : BbaseController
    {

        public ActionResult ProgressBar()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            return View();
        }

        public ActionResult Broadcast()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            
            return View();
        }
        //
        public ActionResult BJLottery()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "BJSC";
            ViewBag.Uid = CurrentUser.UserID;
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }

        public ActionResult CQLottery() {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "CQSSC";
            ViewBag.Uid = CurrentUser.UserID;
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }
        public ActionResult HGLottery()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "XHGKLB";
            ViewBag.UserName = CurrentUser.LoginName;
            ViewBag.Uid = CurrentUser.UserID;
            return View();
        }
        public ActionResult DJLottery()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "XDJKLB";
            ViewBag.Uid = CurrentUser.UserID;
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }
        //进度条  
        public void fnProgressBar()
        {
            for (int i = 0; i < 100; i++)
            {
                IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<CurHub>();
                chat.Clients.Group("clientId").notify(i);//向指定组发送   
                System.Threading.Thread.Sleep(100);
            }
        }
        public string fnBroadcastMsg(string msg,string hubname="cqssc")
        {
            string result = "发送失败!";
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<CQChatHub>();
                if (hubname.ToLower() == "xhgklb")
                {
                    hub = GlobalHost.ConnectionManager.GetHubContext<HGChatHub>();
                }
                else if (hubname.ToLower() == "bjsc") {
                    hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                }
                else if (hubname.ToLower() == "xdjklb") {
                    hub = GlobalHost.ConnectionManager.GetHubContext<DJChatHub>();
                }
                hub.Clients.All.notice("【系统消息】"+ msg); 
                result = "发送成功!";
            }
            catch (Exception e)
            {
                result = "发送失败!\n失败原因:\n" + e.Message;
            }
            return result;
        }

        //广播  
        public string fnBroadcast(string msg)
        {
            string result = "发送失败!";
            try
            {
                IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<AllHub>();
                chat.Clients.All.notice(msg);//向所有组发送   
                result = "发送成功!";
            }
            catch (Exception e)
            {
                result = "发送失败!\n失败原因:\n" + e.Message;
            }
            return result;
        }

        public JsonResult GetTodyRopert(string cpcode="cqssc", string issuenum="")
        { 
            var msg = "网络延迟,请稍后再试";
            int totalCount = 0;
            var bibett = LotteryOrderBusiness.GetIssueNumFee(cpcode, issuenum.Trim(), "");
            var fee=M_UsersBusiness.GetUserAccount(CurrentUser.UserID);
            var result = UserReportBussiness.GetReportList(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00", DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59", CurrentUser.UserID, 1, 1, ref totalCount, ref totalCount);
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("bettfee", bibett);
            JsonDictionary.Add("fee", fee);
            JsonDictionary.Add("Errmsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetlotteryResult(string cpcode)
        { 
            JsonDictionary.Add("item", LotteryResultBusiness.GetNowLottery(cpcode, " and a.status!=2 "));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetIssueNumFee(string cpcode, string issuenum = "", string type = "")
        {
            var result = LotteryOrderBusiness.GetIssueNumFee(cpcode, issuenum.Trim(), type);
            JsonDictionary.Add("PayFee", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserAccount()
        {
            var model = M_UsersBusiness.GetUserAccount(CurrentUser.UserID);
            JsonDictionary.Add("Fee", model.AccountFee); 
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult MessageInfo(string message, string cpcode, string issuenum)
        {
            string mes = "";
            var result = -1;
            var strs = message.Trim().Split('/');         
            if (strs.Length == 3)
            {
                List<Plays> plays = CommonBusiness.LottertPlays.Where(x => x.CPCode == cpcode).ToList();
                Lottery lottery = CommonBusiness.LottertList.Where(x => x.CPCode == cpcode.ToUpper()).FirstOrDefault();
                List<string> list1 = new List<string>();
                List<string> list2 = new List<string>();
                List<string> list3 = new List<string>();
                bool istotal = false;
                var chars = strs[1].Trim().ToCharArray();
                if (strs[1].Trim().IndexOf("龙") > -1 || strs[1].Trim().IndexOf("虎") > -1 || strs[1].Trim().IndexOf("和") > -1)
                {
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if ("龙虎和".IndexOf(s) > -1)
                        {
                            list2.Add(s);
                        }
                    }
                    chars = strs[0].Trim().ToCharArray();
                    string mv = "";
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if (cpcode.ToLower() == "bjsc")
                        {

                        }
                        else
                        {
                            if ("万千百十个".IndexOf(s) > -1)
                            {
                                mv = mv + s;
                                if (mv.Length == 2)
                                {
                                    list1.Add(mv);
                                    mv ="";
                                }
                            }
                        }
                    }
                }
                else
                {
                    chars = strs[0].Trim().ToCharArray();
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if (cpcode.ToLower() == "bjsc")
                        {

                        }
                        else
                        {
                            if ("万千百十个".IndexOf(s) > -1)
                            {
                                list1.Add(s);
                            }
                        }
                    }
                    chars = strs[1].Trim().ToCharArray();
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if ("龙虎和大小单双1234567890".IndexOf(s) > -1)
                        {
                            list2.Add(s);
                        }
                    }
                }
              
                if (strs[2].IndexOf("共") > -1)
                {
                    istotal = true;
                }  
                list3.Add(System.Text.RegularExpressions.Regex.Replace(strs[2], @"[^0-9]+", "")); 
                if (list1.Count > 0 && list2.Count > 0 && list3.Count > 0)
                {
                    List<LotteryOrder> orders = new List<LotteryOrder>();
                    Dictionary<string, string> tplays = new Dictionary<string, string>();
                    foreach (string s in list1)
                    {
                        foreach (string t in list2)
                        {
                            string key = s + t;
                            string pid = plays.Where(x => x.OutName.IndexOf(key) > -1).FirstOrDefault().PIDS;
                            if (!tplays.ContainsKey(s + t) && !string.IsNullOrWhiteSpace(pid))
                            {
                                LotteryOrder order = new LotteryOrder();
                                order.CPCode = cpcode.ToUpper();
                                order.CPName = lottery.CPName;
                                order.Status = 0;
                                order.TypeName = key;
                                order.Type = pid;
                                order.Num = 1;
                                order.PMuch = 1;
                                order.RPoint = 0;
                                order.MType = 0;
                                order.ModelName = "0/0%";
                                order.PayFee = Convert.ToDecimal(list3[list3.Count-1]);
                                order.Content = t;
                                order.UserID = CurrentUser.UserID;
                                order.IssueNum = issuenum;
                                orders.Add(order);
                                tplays.Add(s + t, pid);
                            }
                        }
                    }
                    result = LotteryOrderBusiness.CreateUserOrderList(orders, CurrentUser, OperateIP, 0, 4, ref mes);
                }
                else {
                    mes = "输入格式不正确";
                }
            }
            
            JsonDictionary.Add("result", result>0?"投注成功":mes);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #region Api
        public string fnSendSysMsg(string msg, string hubname = "cqssc")
        {
            string result = "发送失败!";
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<CQChatHub>();
                if (hubname.ToLower() == "xhgklb")
                {
                    hub = GlobalHost.ConnectionManager.GetHubContext<HGChatHub>();
                }
                else if (hubname.ToLower() == "bjsc")
                {
                    hub = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                }
                else if (hubname.ToLower() == "xdjklb")
                {
                    hub = GlobalHost.ConnectionManager.GetHubContext<DJChatHub>();
                }
                hub.Clients.All.notice("【系统消息】" + msg);
                result = "发送成功!";
            }
            catch (Exception e)
            {
                result = "发送失败!\n失败原因:\n" + e.Message;
            }
            return result;
        }
        #endregion

    }
}