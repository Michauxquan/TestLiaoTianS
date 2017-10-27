using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
//using System.Web.ModelBinding;
using ProBusiness.Common;
using ProBusiness.Manage;
using ProDAL;
using ProEntity.Manage;
using ProDAL.Manage;
using ProEntity.UserAttr;
using ProEnum;
using Menu = ProEntity.Menu;


namespace ProBusiness
{
    public class M_UsersBusiness
    {
        #region 查询
        /// <summary>
        /// 根据账号密码获取信息
        /// </summary>
        /// <param name="loginname">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static M_Users GetM_UserByUserName(string loginname, string pwd, string operateip)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            DataTable dt = new M_UsersDAL().GetM_UserByUserName(loginname, pwd);
            M_Users model = null;
            if (dt.Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
                if (!string.IsNullOrEmpty(model.RoleID))
                {
                    model.Role = ManageSystemBusiness.GetRoleByID(model.RoleID); 
                }
                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ClientMenus;
                }
                else if (model.IsAdmin == 1)
                {
                    model.Menus = CommonBusiness.ClientMenus;
                }
                else
                {
                    model.Menus = model.Role.Menus;
                }
            }
            return model;
        }
        /// <summary>
        /// 根据账号密码获取信息（登录）
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <param name="operateip"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static M_Users GetM_UserByProUserName(string loginname, string pwd, string operateip, out int result,EnumUserOperateType type=EnumUserOperateType.Login,int sourceType=0)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            DataSet ds = new M_UsersDAL().GetM_UserByProUserName(loginname, pwd,sourceType, out result);
            M_Users model = null;
            if (ds.Tables.Contains("M_User") && ds.Tables["M_User"].Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(ds.Tables["M_User"].Rows[0]);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {

                    model.Menus = sourceType == 1 ? CommonBusiness.ManageMenus : CommonBusiness.ClientMenus;
                }
                else if (model.IsAdmin == 1)
                {
                    model.Menus = sourceType == 1 ? CommonBusiness.ManageMenus : CommonBusiness.ClientMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            if (model != null && model.Status==0)
            { 
                LogBusiness.UpdateLastIP(model != null ? model.UserID : "", operateip);
            }
            return model;
        }
        public static int GetM_UserCountByLoginName(string loginname)
        {
            DataTable dt = new M_UsersDAL().GetM_UserByLoginName(loginname);
            return dt.Rows.Count;
        }
        public static List<M_Users> GetUsers(int pageSize, int pageIndex, ref int totalCount, ref int pageCount, int type = -1, int status = -1,int sourcetype=-1, string keyWords = "", string colmonasc = "", bool isasc = false,
            string rebatemin="",string rebatemax="",string accountmin="",string accountmax="")
        {
            string whereSql = " a.Status<>9 and isadmin=0 ";
            if (!string.IsNullOrEmpty(rebatemax))
            {
                whereSql += " and a.Rebate<='" + rebatemax + "' ";
            }
            if (!string.IsNullOrEmpty(rebatemin))
            {
                whereSql += " and a.Rebate>'" + rebatemin + "' ";
            }
            if (sourcetype > -1)
            {
                whereSql += " and a.sourcetype=" + sourcetype + " ";
            }
            if (type > -1)
            {
                whereSql += " and a.type=" + type +" ";
            }
            if (!string.IsNullOrEmpty(accountmax))
            {
                whereSql += " and b.AccountFee<='" + accountmax + "' ";
            }
            if (!string.IsNullOrEmpty(accountmin))
            {
                whereSql += " and b.AccountFee>'" + accountmin + "' ";
            }
            if (status > -1)
            {
                whereSql += " and a.Status=" + status;
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.UserName like '%" + keyWords + "%' or a.LoginName like'%" + keyWords + "%') ";
            } 
            string cstr = @" a.*,b.AccountFee ";
            DataTable dt = CommonBusiness.GetPagerData("M_Users a left join UserAccount b on a.UserID=b.UserID ", cstr, whereSql, "a.AutoID", colmonasc, pageSize, pageIndex, out totalCount, out pageCount, isasc);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                list.Add(model);
            }

            return list;
        }
        public static M_Users GetUserDetail(int autoID)
        {

            DataTable dt = M_UsersDAL.BaseProvider.GetUserDetail(autoID);

            M_Users model = null;
            if (dt.Rows.Count == 1)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }

            return model;
        }
        public static M_Users GetUserDetail(string userID)
        {
            
            DataTable dt = M_UsersDAL.BaseProvider.GetUserDetail(userID);

            M_Users model=null;
            if (dt.Rows.Count == 1)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }
            
            return model;
        }
        public static M_Users GetUserDetailByLoginName(string loginName,int sourcetype=0)
        {

            DataTable dt = M_UsersDAL.GetDataTable("select *  from M_Users where Status<>9 and LoginName='" + loginName + "' and sourcetype=" + sourcetype);

            M_Users model = null;
            if (dt.Rows.Count == 1)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }

            return model;
        }

        public static List<M_Users> GetUsersRelationList(int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string userid,int type = -1, int status = -1, string keyWords = "", string colmonasc = " a.AutoID ", bool isasc = false,
           string accountmin = "", string accountmax = "", bool myselft = false, int sourcetype = -1,int isline=-1)
        {
            string whereSql = " a.layers>0 and a.Status<>9   "; 
            if (type > -1)
            {
                whereSql += " and a.type=" + type + " ";
            }
            if (isline > -1)
            {
                whereSql += " and a.isline=" + isline + " ";
            }
            if (!string.IsNullOrEmpty(accountmax))
            {
                whereSql += " and b.AccountFee<='" + accountmax + "' ";
            }
            if (!string.IsNullOrEmpty(accountmin))
            {
                whereSql += " and b.AccountFee>'" + accountmin + "' ";
            }
            if (sourcetype > -1)
            {
                whereSql += " and a.sourcetype=" + sourcetype + " ";
            }
            if (status > -1)
            {
                whereSql += " and a.Status=" + status;
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.UserName like '%" + keyWords + "%' or a.LoginName like'%" + keyWords + "%') ";
            }
            string orswhere = "";
            if (!string.IsNullOrEmpty(userid))
            {
                orswhere = " and ( c.ParentID='" + userid + "' " + (myselft ? " or a.userid='" + userid + "'" : "")+" ) "; ;
            }
            if (string.IsNullOrEmpty(colmonasc))
            {
                colmonasc = " a.AutoID ";
            }
            //string clumstr = @" select  a.*,b.AccountFee,f.*  from M_Users a join UserAccount b on a.UserID=b.Userid left join UserRelation c on a.UserID=c.UserID  " + orswhere + " left join (select  d.userid as uid,d.BackID,d.SSCNum,d.SSCUseNum,d.FCNum,d.FCUseNum,d.X5Num,d.X5UseNum,e.SSCType,e.FCType,e.X5Type,e.WinFee  " +
            //                 "from UserBackSet d join  BackModel e on d.Backid=e.AutoID  where d.type=0) f on f.uid=a.userid  " + whereSql;

            DataTable dt = CommonBusiness.GetPagerData("  M_Users a join UserAccount b on a.UserID=b.Userid left join UserRelation c on a.UserID=c.UserID   left join (select  d.userid as uid,d.BackID,d.SSCNum,d.SSCUseNum,d.FCNum,d.FCUseNum,d.X5Num,d.X5UseNum,e.SSCType,e.FCType,e.X5Type,e.WinFee  " +
                             "from UserBackSet d join  BackModel e on d.Backid=e.AutoID  where d.type=0) f on f.uid=a.userid  ", " a.*,b.AccountFee,f.*  ", whereSql + orswhere, colmonasc, pageSize, pageIndex, out totalCount, out pageCount, isasc);
            //DataTable dt = M_UsersDAL.GetDataTable(clumstr);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }
        public static List<M_Users> GetUsersRelationList(string userid,bool myselft=false)
        {
            string whereSql = myselft ? " or a.userid='" + userid + "'" : "";
            string clumstr = " select  a.*,b.AccountFee  from M_Users a join UserAccount b on a.UserID=b.Userid join UserRelation c on a.UserID=c.UserID and ( c.ParentID='" + userid + "' "+whereSql +"  ) where a.layers>0 and a.Status<>9";
            DataTable dt = M_UsersDAL.GetDataTable(clumstr);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }
        public static List<M_UserRelation> GetUsersListByParent(string parentid)
        {
            string clumstr = " select  c.*,a.UserName  from M_Users a join UserRelation c on a.UserID=c.UserID and  c.ParentID='" + parentid + "' where a.layers>0 and a.Status<>9";
            DataTable dt = M_UsersDAL.GetDataTable(clumstr);
            List<M_UserRelation> list = new List<M_UserRelation>();
            M_UserRelation model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_UserRelation();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }
        public static M_UserRelation GetParentByChildID(string childID)
        {
            string clumstr = " select  c.*,a.UserName  from M_Users a join UserRelation c on a.UserID=c.UserID and  c.UserID='" + childID + "' where a.Status<>9";
            DataTable dt = M_UsersDAL.GetDataTable(clumstr);

            M_UserRelation model = new M_UserRelation();
            foreach (DataRow item in dt.Rows)
            { 
                model.FillData(item); 
            }

            return model;
        }

        public static UserAccount GetUserAccount(string id)
        {
            string clumstr = " select a.*  from UserAccount a where  a.UserID='" + id + "'";
            DataTable dt = M_UsersDAL.GetDataTable(clumstr);
            UserAccount model = new UserAccount();
            foreach (DataRow item in dt.Rows)
            {
                model.FillData(item);
            }
            return model;
        }

        public static List<UserAccount> GetTeamAccount(string userid, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            string whereSql = " b.status<>9 ";
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and ( b.userid='" + userid + "' or a.Userid in(select userid  from dbo.UserRelation where ParentID ='" + userid + "' ) ) ";
            }
            string clumstr = " a.*,b.LoginName UserName ";
            DataTable dt = CommonBusiness.GetPagerData(" UserAccount a join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "b.autoid ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserAccount> list = new List<UserAccount>();
            foreach (DataRow item in dt.Rows)
            {
                UserAccount model = new UserAccount();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region 改
        /// <summary>
        /// 修改管理员账户
        /// </summary>
        public static bool SetAdminAccount(string userid, string loginname, string pwd)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);

            return M_UsersDAL.BaseProvider.SetAdminAccount(userid, loginname, pwd);
        } 

        /// <summary>
        /// 新增或修改用户信息
        /// </summary>
        public static string CreateM_User(M_Users musers,ref string errormsg,string parentid="",int backid=0)
        {
            string userid = Guid.NewGuid().ToString();
            musers.LoginPwd = ProBusiness.Encrypt.GetEncryptPwd(musers.LoginPwd, musers.LoginName); 
            bool bl = M_UsersDAL.BaseProvider.CreateM_User(userid, musers.LoginName, musers.LoginPwd, musers.UserName, backid, musers.SourceType, musers.Type, parentid, musers.RoleID, out errormsg, musers.Description); 
            return bl ? userid : "";
        }

        public static string CreateM_UserBase(string loginname, string loginpwd)
        {
            string userid = Guid.NewGuid().ToString();
            string userPwd = ProBusiness.Encrypt.GetEncryptPwd(loginpwd, loginname);
            bool bl = M_UsersDAL.BaseProvider.CreateM_UserBase(userid, loginname, userPwd);
            return bl ? userid : "";
        }  
        /// <summary>
        /// 修改用户户信息
        /// </summary>
        public static bool UpdateM_User(string userid,string avatar)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_User(userid, avatar); 
            return bl;
        }
        public static bool UpdateM_UserRole(string userid, string RoleID,string Description="")
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_UserRole(userid, RoleID, Description);
            return bl;
        }
        public static bool UpdateM_UserName(string userid, string username)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_User(userid, username);
            return bl;
        }
        public static bool UpdatePwd(string loginname, string pwd)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            bool bl = M_UsersDAL.BaseProvider.UpdatePwd(loginname, pwd);
            return bl;
        }

        public static bool saveEditUsers(M_Users model)
        {
            if (!string.IsNullOrEmpty(model.LoginPwd))
            {
                model.LoginPwd = ProBusiness.Encrypt.GetEncryptPwd(model.LoginPwd, model.LoginName);
            }
            if (!string.IsNullOrEmpty(model.AccountPwd))
            {
                model.AccountPwd = ProBusiness.Encrypt.GetEncryptPwd(model.AccountPwd, model.LoginName);
            }

            bool bl = M_UsersDAL.BaseProvider.saveEditUsers(model.UserID, model.AccountPwd, model.LoginPwd, model.Status, model.Backid, model.LoginName,
                model.IsRestBank, model.AccountFee, model.rgzStatus, model.fhStatus, model.fhPoint, model.AskType, model.AnswerContent, model.RealName, model.Type,model.SafeLevel);
            return bl;
        }

        public static string GetUserParentsName(string userid)
        {
            return M_UsersDAL.BaseProvider.GetUserParentsName(userid);
        }

        public static string GetUserAllParentsName(string userid)
        {
            return M_UsersDAL.BaseProvider.GetUserAllParentsName(userid);
        }
        public static bool UpdateAccountPwd(string userid,string loginname, string pwd)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            bool bl = M_UsersDAL.BaseProvider.UpdateAccountPwd(userid, pwd);
            return bl;
        }
        public static bool BindOtherAccount(int  type,string userid, string accountcode,ref string errmsg)
        {
            bool bl = M_UsersDAL.BaseProvider.BindOtherAccount(userid, type, accountcode, ref errmsg);
            return bl;
        }

        public static bool UpdateUserInfo(string userid, string username, string realname, string accountpwd,
            int asktype, string answercontent)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateUserInfo(userid, username, realname, accountpwd, asktype, answercontent);
            return bl;
        }

        public static bool DeleteUser(string userid)
        {
            return M_UsersDAL.BaseProvider.DeleteUser(userid);
        }

        public static bool M_DelUsers(string userid, string operater, string ip = "")
        {
            return M_UsersDAL.BaseProvider.M_DelUsers(userid, operater, ip);
        }

        public static  bool DeleteM_User(string userid, int status) {
            return M_UsersDAL.BaseProvider.DeleteM_User(userid, status);
        }
        public static bool UpdateM_UserStatus(string userid, int status)
        {
            return M_UsersDAL.BaseProvider.UpdateM_UserStatus(userid, status);
        }
        public static bool UpdateUserLevel(string userid, int status)
        {
            return M_UsersDAL.BaseProvider.UpdateUserLevel(userid, status);
        }
        public static bool UpdateM_UserRebate(string userid,string parentid, decimal point)
        {
            SqlConnection conn = new SqlConnection(M_UsersDAL.ConnectionString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            if (!M_UsersDAL.BaseProvider.UpdateM_UserRebate(userid, parentid, point, tran))
                {
                    tran.Rollback();
                    conn.Dispose();
                    return false;
                } 

            tran.Commit();
            conn.Dispose();

            return true;
             
        } 
        public static bool CheckEmail(string loginname, string email)
        {
           var result= CommonBusiness.Select("M_Users", "count(1)", " LoginName='" + loginname + "' and email='" + email + "' ");
            return Convert.ToInt32(result) > 0;
        }
        public static bool CheckEmail(string email)
        {
            var result = CommonBusiness.Select("M_Users", "count(1)", " email='" + email + "' ");
            return Convert.ToInt32(result) > 0;
        }

        public static bool UpdateLower(string uid, string lower)
        {
            return M_UsersDAL.BaseProvider.UpdateLower(uid, lower); 
        }

        public static bool UpdLine(string id, int isline=0)
        {
            return M_UsersDAL.BaseProvider.UpdLine(id, isline); 
        }

        public static bool UserChangeMoney(string userid,string puserid, decimal changemoney,  
            string remark, ref string msg)
        {
            return M_UsersDAL.BaseProvider.UserChangeMoney(userid,puserid, changemoney,  remark,ref msg); 
        }

        #endregion

    }

    

    
}
