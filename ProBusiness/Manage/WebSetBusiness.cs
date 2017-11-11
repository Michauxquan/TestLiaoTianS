using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web; 

namespace ProBusiness.Manage
{
    public class WebSetBusiness
    {
        public static string FILEPATH =ProTools.Common.GetKeyValue("UploadFilePath") + "Active/" + DateTime.Now.ToString("yyyyMM") + "/";
        public static string TempPath = ProTools.Common.GetKeyValue("UploadTempPath");
        #region 查询   

        public static List<Active> GetActiveList(string keyWords, int pageIndex, int pageSize, ref int totalCount, ref int pageCount, string btime = "", string etime = "", int type=-1)
        { 
            string whereSql = " a.Status<>9";
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.Title like '%" + keyWords + "%' or a.Tips like '%" + keyWords + "%' )";
            }
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and ( ( a.BTime<='" + etime + "' and a.BTime>='" + btime + "') or ( a.BTime>='" + btime + "' and a.ETime <='" + etime + "' ) or (a.Etime>='" + btime + "' and a.Etime<='" + etime + "'))";
            }
            if (type > -1)
            {
                whereSql += " and a.Type ="+ type +" ";
            }
            string cstr = @" a.*,b.UserName as CreateUser ";
            DataTable dt = CommonBusiness.GetPagerData(" Active a join M_Users b on a.CreateUserID=b.UserID ", cstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            
            List<Active> list = new List<Active>(); 
            foreach (DataRow dr in dt.Rows)
            {
                Active model = new Active();
                model.FillData(dr);
                list.Add(model);
            } 
            return list;
        }
        public static List<Active> GetActiveList(int type=0 ,int tops=9)
        {
            string sqltext = "select top " + tops + " a.*  from  Active a  where a.Status<>9 ";
            if (type > -1)
            {
                sqltext += " and Type=" + type;
            }
            DataTable dt = WebSetDAL.GetDataTable(sqltext);

            List<Active> list = new List<Active>();
            foreach (DataRow dr in dt.Rows)
            {
                Active model = new Active();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static Active GetActiveByID(string AutoID)
        {

            DataTable dt = WebSetDAL.BaseProvider.GetActiveByID(AutoID);
            Active model = null;
            if (dt.Rows.Count == 1)
            {
                model = new Active();
                model.FillData(dt.Rows[0]);
            }
            return model; 
        }
        public static List<ChargeSet> GetChargeSet(string keyWords, string userid,int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = " ChargeSet  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.Name like '%" + keyWords + "%' or a.Remark like '%" + keyWords + "%' or a.View like '%" + keyWords + "%')";
            } 
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
            } 
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.UserID='" + userid + "' ";
            }
            if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.Name as UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<ChargeSet> list = new List<ChargeSet>();
            foreach (DataRow dr in dt.Rows)
            {
                ChargeSet model = new ChargeSet();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static ChargeSet GetChargeSetDetail(string view)
        {
            DataTable dt = WebSetDAL.BaseProvider.GetChargeSetDetail(view.Trim());
            ChargeSet model = null;
            if (dt.Rows.Count == 1)
            {
                model = new ChargeSet();
                model.FillData(dt.Rows[0]);
            }
            return model;
        }

        public static List<LotteryResult> GetLotteryOpen(int pageIndex, ref int totalCount, ref int pageCount, string cpcode = "", int pageSize = 15, string btime = "", string etime = "", string status = "0,1", string keywords="")
        {
              string whereSql = " 1=1 ";
              if (!string.IsNullOrEmpty(status) && status!="-1")
            {
                whereSql += " and a.Status in (" + status + ")";
            }
            if (!string.IsNullOrEmpty(btime))
            {
                whereSql += " and a.OpenTime>='" + btime + "'";
            }
            if (!string.IsNullOrEmpty(etime))
            {
                whereSql += " and a.OpenTime<='" + etime + " 23:59:59:999'";
            }
            if (!string.IsNullOrEmpty(cpcode))
            {
                whereSql += " and a.CPCode='" + cpcode + "'";
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                whereSql += " and a.IssueNum like'%" + keywords + "%' ";
            }
            string cstr = @" a.AutoID,a.CPCode,a.IssueNum,c.ResultNum AutoNum,a.ResultNum ResultNum,a.Status,a.UpdateTime,a.CreateTime,a.PizeNum,a.UserNum,a.SumNum,a.OpenTime,a.Num,b.CPName ";
            DataTable dt = CommonBusiness.GetPagerData(" LotteryResult a join lottery b on a.CPCode=b.CPCode left join LotteryWaitPay c on a.CPCode=c.CPCode and a.IssueNum=c.IssueNum ", cstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);

            List<LotteryResult> list = new List<LotteryResult>(); 
            foreach (DataRow dr in dt.Rows)
            {
                LotteryResult model = new LotteryResult();
                model.FillData(dr);
                list.Add(model);
            } 
            return list;
        }

        public static Lottery GetLotteryDetail(string cpCode)
        {
            var model=CommonBusiness.LottertList.Where(x => x.CPCode == cpCode).FirstOrDefault();
            if (model != null && !string.IsNullOrEmpty(model.CPName))
            {
                return model;
            }
            else
            {
                DataTable dt = new CommonDAL().GetLotteryList();
                List<Lottery> lottertList=new List<Lottery>();
                foreach (DataRow dr in dt.Rows)
                {
                    Lottery mod = new Lottery();
                    mod.FillData(dr);
                    lottertList.Add(mod);
                }
                CommonBusiness.LottertList = lottertList;
                return CommonBusiness.LottertList.Where(x => x.CPCode == cpCode).FirstOrDefault(); 
            }
        }
        public static Lottery GetLotteryDetailByID(int autoid)
        {
            var model = CommonBusiness.LottertList.Where(x => x.AutoID == autoid).FirstOrDefault();
            if (model != null && !string.IsNullOrEmpty(model.CPName))
            {
                return model;
            }
            else
            {
                DataTable dt = new CommonDAL().GetLotteryList();
                List<Lottery> lottertList = new List<Lottery>();
                foreach (DataRow dr in dt.Rows)
                {
                    Lottery mod = new Lottery();
                    mod.FillData(dr);
                    lottertList.Add(mod);
                }
                CommonBusiness.LottertList = lottertList;
                return CommonBusiness.LottertList.Where(x => x.AutoID == autoid).FirstOrDefault();
            }
        }

        public static List<Plays> GetLotteryPlaysesByCode(string cpcode)
        {
            DataTable dt = WebSetDAL.BaseProvider.GetLotteryPlays(cpcode);
            List<Plays> list = new List<Plays>();
            foreach (DataRow dr in dt.Rows)
            {
                Plays model = new Plays();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        public static List<Plays> GetPlays()
        {
            DataTable dt = WebSetDAL.BaseProvider.GetPlays();
            List<Plays> list = new List<Plays>();
            foreach (DataRow dr in dt.Rows)
            {
                Plays model = new Plays();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
        #endregion
        #region 新增 
        public static bool InsertActive(Active model)
        {
            model.Img = GetUploadImgurl(model.Img);
            return WebSetDAL.BaseProvider.InsertActive(model.CreateUserID,model.Title, model.Content, model.Tips, model.Img, model.BTime,model.ETime,model.Type);
        }

        public static bool InsertChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.InsertChargeSet(model.UserID, model.View.ToLower(), model.Remark, model.Golds);
        }

        public static int CreateLottery(string cpname, string cpcode, int icontype, string resulturl, string userid,int openTimes,string closeTime,string onSaleTime,int sealTimes,int periodsNum,ref string  errmsg)
        {
            var result= WebSetDAL.BaseProvider.InsertLottery(cpname, cpcode, icontype, resulturl, userid, openTimes,closeTime, onSaleTime, sealTimes, periodsNum ,ref errmsg);
            if (result > 0)
            {
                GetLotteryDetailByID(result);
            }
            return result;

        }

        public static bool UpdateLotteryResult(string cpcode,string issuenum,string resultnum)
        {
            var result = WebSetDAL.BaseProvider.UpdateLotteryResult(cpcode, issuenum, resultnum); 
            return result;
        }

        public static bool ReturnLotteryResultMyselft(string lcode)
        {
            var result = WebSetDAL.BaseProvider.ReturnLotteryResultMyselft(lcode);
            return result;
        }

        public static bool CancelBett(string bcode)
        {
            var result = WebSetDAL.BaseProvider.CancelBett(bcode);
            return result;
        }

        #endregion

        #region 改 

        public static bool UpdateActive(Active model)
        {
            return WebSetDAL.BaseProvider.UpdateActive(model.AutoID, model.Title, model.Content, model.Tips, model.Img, model.BTime, model.ETime, model.UpdUserID);
        }
        public static bool UpdateChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.UpdateChargeSet(model.AutoID, model.View, model.Remark, model.Golds);
        }
        public static bool UpdateChargeSetStatus(int autoid,int status)
        {
            return CommonBusiness.Update("ChargeSet", "Status", status, " where Status<>9 and AutoID=" + autoid);
        }
        public static bool DeleteActive(int autoid)
        {
            return WebSetDAL.BaseProvider.DeleteActive(autoid);  
        } 
        public static bool UpdateUserLottery(int status, int autoid){
            bool bl = CommonBusiness.Update("Lottery","Status",status," AutoID="+autoid);
            if (bl)
            {
                var model = CommonBusiness.LottertList.Where(x => x.AutoID == autoid).FirstOrDefault();
                if (model != null && model.AutoID > 0)
                {
                    CommonBusiness.LottertList.Remove(model);
                    model.Status = status;
                    CommonBusiness.LottertList.Add(model);
                }
            }
            return bl;
        }

        public static bool UpdateLottery(string cpname, string cpcode, int icontype, string resulturl,int openTimes,string closeTime,string onSaleTime,int sealTimes,int periodsNum,int autoid)
        {
            bool bl = WebSetDAL.BaseProvider.UpdateLottery(cpname, cpcode, icontype, resulturl,openTimes,closeTime,onSaleTime,sealTimes,periodsNum, autoid);
            if (bl)
            {
                var model = CommonBusiness.LottertList.Where(x => x.AutoID == autoid).FirstOrDefault();
                if (model != null && model.AutoID>0)
                {
                    CommonBusiness.LottertList.Remove(model);
                    model.CPName = cpname;
                    model.CPCode = cpcode;
                    model.IconType = icontype;
                    model.ResultUrl = resulturl;
                    model.CloseTime = closeTime;
                    model.OnSaleTime = onSaleTime;
                    model.PeriodsNum = periodsNum;
                    model.SealTimes = sealTimes;
                    model.OpenTimes = openTimes;
                    CommonBusiness.LottertList.Add(model);
                }
            }
            return bl;
        }
        public static bool UpdateLotteryPlays(string lotterid, string permissions, string operateid, string ip)
        {
            return WebSetDAL.BaseProvider.UpdateLotteryPlays(lotterid, permissions, operateid);
        }

        public static bool ReturnLotteryResult(string cpcode,string issuenum)
        {
            return WebSetDAL.BaseProvider.ReturnLotteryResult(cpcode, issuenum);
        }
        public static bool ReturnLotteryResult(string cpcode, string issuenum,string userid,ref string mes)
        {
            return WebSetDAL.BaseProvider.ReturnLotteryResult(cpcode, issuenum,userid,ref mes);
        }
        public static bool UpdatePlayInfo(string cpcode, string pids, string playinfo,string bettnum,string content,string sapacerate)
        {
            return WebSetDAL.BaseProvider.UpdatePlayInfo(cpcode, pids, playinfo, bettnum,content, sapacerate);
        }

        #endregion
        public static string GetUploadImgurl(string imgurl)
        {
            if (!string.IsNullOrEmpty(imgurl) && imgurl.IndexOf(TempPath) >= 0)
            {
                DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(FILEPATH));
                if (!directory.Exists)
                {
                    directory.Create();
                }
                if (imgurl.IndexOf("?") > 0)
                {
                    imgurl = imgurl.Substring(0, imgurl.IndexOf("?"));
                }
                FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(imgurl));
                imgurl = FILEPATH + file.Name;
                if (file.Exists)
                {
                    file.MoveTo(HttpContext.Current.Server.MapPath(imgurl));
                }
            }
            return imgurl;
        }
    }
}
