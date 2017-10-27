using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using ProDAL; 
using ProEntity;
using ProEnum;


namespace ProBusiness
{
    public class LotteryResultBusiness
    {
        #region 查询   
        #endregion
        public static List<LotteryResult> GetPagList(string cpCode,int status,bool orderby, int pageSize, int pageIndex, ref int totalCount, ref int pageCount,string btime="",string etime="")
        {
            string sqlwhere = " b.cpCode='" + cpCode + "'";
            if (status > -1)
            {
                sqlwhere += " and b.Status=" + status;
            }
            if (!string.IsNullOrEmpty(btime))
            {
                sqlwhere += " and b.CreateTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                sqlwhere += " and b.CreateTime<='" + etime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(" LotteryResult (nolock) b ",
                "b.*", sqlwhere, "b.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount, orderby);
            List<LotteryResult> list = new List<LotteryResult>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryResult model = new LotteryResult();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static List<LotteryOrder> GetLotteryWin(string cpCode, int status, decimal winFee, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string btime = "", string etime = "")
        {
            string sqlwhere = " 1=1 ";
            if (!string.IsNullOrEmpty(cpCode))
            {
                 sqlwhere += " and b.cpCode='" + cpCode + "'";
            }
            if (status > -1)
            {
                sqlwhere += " and b.Status=" + status;
            }
            if (!string.IsNullOrEmpty(btime))
            {
                sqlwhere += " and b.CreateTime>='" + btime + "'";
            }
            if (winFee > 0)
            {
                sqlwhere += " and b.WinFee>="+winFee;
            }
            if (!string.IsNullOrEmpty(etime))
            {
                sqlwhere += " and b.CreateTime<='" + etime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(" LotteryOrder (nolock) b join M_Users a on a.UserID=b.UserID ",
                " b.* ,a.LoginName as UserName", sqlwhere, " b.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount);
            List<LotteryOrder> list = new List<LotteryOrder>();
            foreach (DataRow dr in dt.Rows)
            {
                LotteryOrder model = new LotteryOrder();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static LotteryResult GetLotteryResult(string cpCode, int status, string btime, string etime, string orderasc = "asc")
        {
            string sqlwhere = " where  b.cpCode='" + cpCode + "'";
            if (status > -1)
            {
                sqlwhere += " and b.Status=" + status;
            }

            if (!string.IsNullOrEmpty(btime))
            {
                sqlwhere += " and b.opentime>='" + btime + "'";
            } 
            if (!string.IsNullOrEmpty(etime))
            {
                sqlwhere += " and b.opentime<='" + etime + " 23:59:59:999'";
            }

            DataTable dt = LotteryResultDAL.GetDataTable("select top 1 *  from  LotteryResult (nolock) b " + sqlwhere + " Order by AutoID " + orderasc);
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr); 
            }
            return model;
        }
        public static LotteryResult GetLotteryResult(string cpCode, string status, string orderasc = "asc")
        {
            string sqlwhere = " where  b.cpCode='" + cpCode + "'";
            if (!string.IsNullOrEmpty(status ))
            {
                sqlwhere += " and b.Status in (" + status+") ";
            }

            DataTable dt = LotteryResultDAL.GetDataTable("select top 1 *  from  LotteryResult (nolock) b " + sqlwhere + " Order by AutoID " + orderasc);
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr);
            }
            return model;
        }
        public static LotteryResult GetNowLottery(string cpcode)
        {
            DataTable dt= LotteryResultDAL.BaseProvider.GetNowLottery(cpcode);
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr);
            }
            return model;
        }
        public static LotteryResult GetNowLottery(string cpcode, string sqlwhere)
        {
            DataTable dt = LotteryResultDAL.BaseProvider.GetNowLottery(cpcode, sqlwhere);
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr);
            }
            return model;
        }
        public static LotteryResult GetOpenLottery(string cpcode, string sqlwhere,int top=1)
        {
            DataTable dt = LotteryResultDAL.BaseProvider.GetOpenLottery(cpcode, sqlwhere, top);
            LotteryResult model = new LotteryResult();
            foreach (DataRow dr in dt.Rows)
            {
                model.FillData(dr);
            }
            return model;
        }
        #region Insert

        public static void InsertAllLottery()
        {
            LotteryResultDAL.BaseProvider.InsertLotteryResultAll();
        
        }

        #endregion

        #region 改
        public static  bool UpdateLotteryResult(string issuenum,string cpcode,int status) {
            return LotteryResultDAL.BaseProvider.UpdateLotteryResult(issuenum, cpcode,status);
        }
        public static bool UpdateStatus(string cpcode,int status){
            return LotteryResultDAL.BaseProvider.UpdateLotteryStatus(cpcode,status);
        }

        public static bool UpdateSD11X5Result(string result, string issnum, string cpcode)
        {
            return LotteryResultDAL.BaseProvider.UpdateSD11X5Result(result, issnum,cpcode);
        }
        public static bool UpdateSysLotteryResult(string issnum, string cpcode,int  lay =0)
        {
            return LotteryResultDAL.BaseProvider.UpdateSysLotteryResult(issnum, cpcode, lay);
        }


        public static bool UpdateByStatusAndOpentTime(string cpcode, string opentime)
        {
            return LotteryResultDAL.BaseProvider.UpdateByStatusAndOpentTime(cpcode, opentime);
        }

        public static void CheckSysLottery()
        {
            LotteryResultDAL.BaseProvider.CheckSysLottery();
        }

        #endregion

    }

    

    
}
