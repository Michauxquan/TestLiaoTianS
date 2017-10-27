using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEntity.Manage;

namespace ProBusiness
{
    public class UserImgsBusiness
    {
        #region  查询

        public static List<M_Users> GetImgList(string userid, int sex, int pageIndex, int pageSize,ref int totalCount,ref int pageCount)
        {

            string whereSql = " a.Status<>9 and b.ImgCount>0";

            if (sex > -1)
            {
                whereSql += " and a.Sex=" + sex;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and a.UserID='" + userid + "' ";
            }
            string clumstr = "a.userID,a.Avatar,a.Name,a.Age,a.LoginName,a.MyService,a.Province,a.City,a.District,a.CreateTime,a.Status,a.Sex,a.IsMarry,a.Education," +
                "a.BHeight,a.Levelid,a.BWeight,a.MyContent,a.MyCharacter,a.TalkTo,a.BPay,a.Account,b.ImgCount,b.IsLogin,b.RecommendCount,b.SeeCount";
            DataTable dt = CommonBusiness.GetPagerData("M_Users a join userReport b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<M_Users> list = new List<M_Users>();
            foreach (DataRow item in dt.Rows)
            {
                M_Users model = new M_Users();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }

        public static List<UserImgs> GetNewImg(int tops,int status)
        {
            DataTable dt = UserImgsDAL.BaseProvider.GetNewImg(tops, status);
            List<UserImgs> list = new List<UserImgs>();
            foreach (DataRow item in dt.Rows)
            {
                UserImgs model = new UserImgs();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }
        public static List<UserImgs> GetUserImgList(string userid,int status, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            string whereSql = " a.Status<>9 ";

            if (status > -1)
            {
                whereSql += " and a.status=" + status;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                whereSql += " and a.UserID='" + userid + "' ";
            }
            string clumstr = "a.*,b.Name as UserName ";
            DataTable dt = CommonBusiness.GetPagerData("UserImgs a left join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
             
            List<UserImgs> list = new List<UserImgs>();
            foreach (DataRow item in dt.Rows)
            {
                UserImgs model = new UserImgs();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        public static List<UserImgs> GetUserImgList(string keywords, int status,string begintime,string endtime, int pageIndex, int pageSize, ref int totalCount, ref int pageCount)
        {
            string whereSql = " a.Status<>9 ";

            if (status > -1)
            {
                whereSql += " and a.status=" + status;
            }
            if (!string.IsNullOrEmpty(begintime))
            {
                whereSql += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                whereSql += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                whereSql += " and b.Name like '%" + keywords + "%' ";
            }
            string clumstr = "a.*,b.Name as UserName ";
            DataTable dt = CommonBusiness.GetPagerData("UserImgs a left join M_Users b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);

            List<UserImgs> list = new List<UserImgs>();
            foreach (DataRow item in dt.Rows)
            {
                UserImgs model = new UserImgs();
                model.FillData(item);
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region 操作

        public static bool Create(UserImgs userimg, string operateip)
        {
            var result = UserImgsDAL.BaseProvider.Create(userimg.UserID, userimg.ImgUrl, userimg.Size); 
            return result;
        }

        public static bool UpdateStatus(string  autoids, int status)
        {
            var result = UserImgsDAL.BaseProvider.UpdateStatus(autoids, status); 
            return result;
        }

        #endregion
    }
}
