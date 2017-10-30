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

        public void SendMessage(string nickName, string message)
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

        public void SendMessage(string nickName, string message)
        {
            var strs=message.Trim().Split('/');
            if (strs.Length == 3) {
                foreach (string item in strs)
                {
                    if (!string.IsNullOrEmpty(item)) {

                    }
                }
            }

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

        public void SendMessage(string nickName, string message)
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

        public void SendMessage(string nickName, string message)
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
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }

        public ActionResult CQLottery() {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "CQSSC";
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }
        public ActionResult HGLottery()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "NHGKLB";
            ViewBag.UserName = CurrentUser.LoginName;
            return View();
        }
        public ActionResult DJLottery()
        {
            if (Session["Manager"] == null)
            {
                return Redirect("/Home/Login");
            }
            ViewBag.CPCode = "NDJKLB";
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