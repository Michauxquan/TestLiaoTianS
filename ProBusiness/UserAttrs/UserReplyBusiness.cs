using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ProBusiness.Manage;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness.UserAttrs
{
    public class UserReplyBusiness 
    {
        public static UserReplyBusiness BaseBusiness = new UserReplyBusiness();

        #region 查询 

        public static List<UserReply> GetUserReplys(string guid, string userid, int type, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int sourcetype = -1, bool ht = false, string keyWords = "")
        {
            string tablename = "UserReply  a left join M_Users b  on a.Guid =b.UserID left join M_Users c  on a.CreateUserID =c.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and ( b.username like'%" + keyWords + "%' or  c.username like'%" + keyWords + "%')";
            }
            if (!string.IsNullOrEmpty(guid))
            {
                sqlwhere += " and a.guid='" + guid + "' ";
            }
            if (type>-1)
            {
                sqlwhere += " and a.Type=" + type;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.CreateUserID='" + userid + "' ";
            }
            if (ht)
            {
                sqlwhere += " and a.guid in(select userid  from M_users where status<>9 and SourceType=1) ";
            }
            if (sourcetype > -1)
            {
                //1后台 0前台
                sqlwhere += " and c.SourceType=" + sourcetype+" ";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.LoginName as UserName,b.Avatar as UserAvatar,c.LoginName as FromName,c.Avatar as FromAvatar ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserReply> list = new List<UserReply>();
            foreach (DataRow dr in dt.Rows)
            {
                UserReply model = new UserReply();
                model.FillData(dr);
                list.Add(model);
            }
            return list; 
        }

        public static int GetNotReadReplay(int type, string userid, int status)
        {
            string sqlwhere = " Status<>9 ";
            if (type > -1)
            {
                sqlwhere += " and Type=" + type;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and Guid='" + userid + "'";
            }
            if (status > -1)
            {
                sqlwhere += " and Status=" + status;
            }

            return Convert.ToInt32(CommonBusiness.Select("UserReply", "count(1)", sqlwhere));
        }

        public static UserReply GetReplyDetail(string replyid)
        {
            DataTable dt = UserReplyDAL.BaseProvider.GetReplyDetail(replyid);
            UserReply model = null;
            if (dt.Rows.Count == 1)
            {
                model = new UserReply();
                model.FillData(dt.Rows[0]);
            }
            return model;
        }
        #endregion

        #region 添加.删除
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="userID"></param>
        /// <param name="fromReplyID"></param>
        /// <param name="fromReplyUserID"></param>
        /// <param name="type"></param>
        /// <param name="haschilds">-1 无 0 直属下级 1所有下级 2 一级代理 3 全部</param>
        /// <param name="errormsg"></param>
        /// <returns></returns>
        public static bool CreateUserReply(string guid, string content,string title, string userID, string fromReplyID, string fromReplyUserID, int type, int haschilds, ref string errormsg,int isreply=0)
        {
            return UserReplyDAL.BaseProvider.CreateUserReply(guid.Trim(','), content,title, userID, fromReplyID, fromReplyUserID, type, haschilds,isreply, ref errormsg);
        }  
        public static bool DeleteReply(string replyid)
        {
            replyid = replyid.TrimEnd(',').Replace(",","','");
            bool bl = CommonBusiness.Update("UserReply", "Status", 9, "ReplyID in('" + replyid + "')");
            return bl;
        }
        public static bool DeleteReplyByID(string replyid)
        {
            replyid = replyid.TrimEnd(',');
            bool bl = CommonBusiness.Update("UserReply", "Status", 9, "AutoID in(" + replyid + ")");
            return bl;
        }
        public static bool UpdateReplyStatus(string replyid,int status)
        {
            bool bl = CommonBusiness.Update("UserReply", "Status", status, "ReplyID in('" + replyid + "') and Status<>9");
            return bl;
        }
        #endregion 
    }
}
