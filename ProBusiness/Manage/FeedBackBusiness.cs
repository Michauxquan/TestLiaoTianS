using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ProDAL.Mannge;
using ProEntity.Manage;

namespace ProBusiness
{
    public  class FeedBackBusiness
    {
        #region 增
        /// <summary>
        /// 新增建议反馈举报
        /// </summary>
        public static bool InsertFeedBack(FeedBack model)
        { 
            return FeedBackDAL.BaseProvider.InsertFeedBack(model.Title,  model.TipedName,model.TipedID, model.Type, model.Remark, model.CreateUserID);
        }
        #endregion

        #region 查
        public static List<FeedBack> GetFeedBacks(string keywords, string beginDate, string endDate, int type, int status, string userID, int pageSize, int pageIndex, out int totalCount, out int pageCount)
        {
            string sqlWhere = "1=1";
            if (type != -1)
            {
                sqlWhere += " and type=" + type;
            }

            if (status != -1)
            {
                sqlWhere += " and status=" + status;
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                sqlWhere += " and Reamrk like '%"+keywords+"%' ";
            }
            if (status>-1)
            {
                sqlWhere += " and status=" + status;
            }
            if (!string.IsNullOrEmpty(userID))
            {
                sqlWhere += " and CreateUserID='" + userID + "'";
            }

            if (!string.IsNullOrEmpty(beginDate))
            {
                sqlWhere += " and createtime>='" + beginDate + "'";
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlWhere += " and createtime<='" + DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            DataTable dt = CommonBusiness.GetPagerData("FeedBack", "*", sqlWhere, "AutoID", pageSize, pageIndex, out totalCount, out pageCount);

            List<FeedBack> list = new List<FeedBack>();
            FeedBack model;
            foreach (DataRow item in dt.Rows)
            {
                model = new FeedBack();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }



        public static int GetFeedBacksCount(string beginDate, string endDate, int status)
        {
            string sqlWhere = "1=1";
            if (status != -1)
            {
                sqlWhere += " and status=" + status;
            }

            if (!string.IsNullOrEmpty(beginDate))
            {
                sqlWhere += " and createtime>='" + beginDate + "'";
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                sqlWhere += " and createtime<='" + DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            int result = Convert.ToInt32(CommonBusiness.Select("FeedBack", "count(1)", sqlWhere));

            return result;
        }


        public static FeedBack GetFeedBackDetail(string id)
        {
            DataTable dt = FeedBackDAL.BaseProvider.GetFeedBackDetail(id);
            FeedBack model = new FeedBack();
            if (dt.Rows.Count > 0)
                model.FillData(dt.Rows[0]);

            return model;
        }
        #endregion

        #region 改
        public static bool UpdateFeedBackStatus(string id, int status, string content)
        {
            return FeedBackDAL.BaseProvider.UpdateFeedBackStatus(id, status, content);
        }
        #endregion
    }
}
