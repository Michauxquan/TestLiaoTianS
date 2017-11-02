﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Newtonsoft.Json; 
using ProEntity;

namespace ProBusiness
{
    public class TaskService
    {
        public static TaskService BasService = new TaskService();

        public void InsertAllLottery()
        {
            LotteryResultBusiness.InsertAllLottery();
        }

        public string BettAutoInsert()
        { 
            var list = LotteryOrderBusiness.GetBettAutoByStatus();
            string msg = "";
             list.ForEach(x =>
             {
                 string errmsg = "";
                
                 var issuenum = GetIssueNum(x.ComNum-1, x.JsonContent);
                 issuenum = string.IsNullOrEmpty(issuenum) ? "0" : issuenum;
                 var lottery = LotteryResultBusiness.GetLotteryResult(x.CPCode, "1,2","desc");
                 if (IsEquelNum(x.CPCode, lottery.IssueNum, issuenum))
                 {
                     var totalmuch = GetIssueNum(x.ComNum, x.JsonContent, 1);
                     var pMuch = string.IsNullOrEmpty(totalmuch) ? x.BMuch : Convert.ToInt32(totalmuch);
                     int comnum = x.ComNum + 1;
                     try
                     {
                         if (!string.IsNullOrEmpty(issuenum))
                         {
                             LotteryOrderBusiness.CreateLotteryOrder(x.BCode + comnum, issuenum, x.Type, x.TypeName, x.CPCode,
                                 x.CPName,
                                 x.Content, x.Num, x.PayFee * pMuch / x.PMuch, x.UserID, pMuch, x.RPoint, x.IP, 0, 5, x.BCode,x.ModelName,x.MType,
                                 ref errmsg);
                             if (!string.IsNullOrEmpty(errmsg))
                             {
                                 errmsg = issuenum + ":" + errmsg + ";";
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         errmsg = x.BCode + "第" + comnum + "期插入失败";

                        // L.Log("[BettAutoInsert] ", x.BCode + "第" + comnum + "期插入失败");
                     }
                     msg += errmsg;
                     LotteryOrderBusiness.UpdateBettAutoByCode(x.BCode, comnum, pMuch * x.PayFee, errmsg);
                 }
             });
             return msg;
        }


        public bool OpenLotteryResult(string result,string issnum,string cpcode)
        {
            var s= LotteryResultBusiness.UpdateSD11X5Result(result, issnum, cpcode); 
            return s;
        }

        public void CheckSysLottery()
        {
            LotteryResultBusiness.CheckSysLottery();
        
        }

        public bool OpenSysLotteryResult(string issnum, string cpcode)
        {
            var s = LotteryResultBusiness.UpdateSysLotteryResult(issnum, cpcode, 0);
            return s;
        }
        public bool UpdByStatusAndOpenTime(string cpcode,string opentime)
        {
            return LotteryResultBusiness.UpdateByStatusAndOpentTime(cpcode, opentime);
        }

        public bool IsEquelNum(string cpcode, string issuenum, string nowNum)
        {
            bool result =false;
            int num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 2, 2));
            var comissuenum = DateTime.Now.ToString("yyyyMMdd").Substring(2, 6);
            if (cpcode.IndexOf("FFC") > -1 || cpcode == "CQSSC" || cpcode == "KK11X5" || cpcode == "MK11X5")
            {
                if (cpcode == "CQSSC")
                {
                    comissuenum = DateTime.Now.ToString("yyyyMMdd");
                }
                else
                {
                    if (DateTime.Now.Hour < 2 || (DateTime.Now.Hour == 2 && DateTime.Now.Minute == 0))
                    {
                        comissuenum = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                    }
                    else
                    {
                        comissuenum = DateTime.Now.ToString("yyyyMMdd");
                    }
                   
                }
            }
            if (cpcode == "SD11X5")
            {
                if (num == 78)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 78;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "GD115" || cpcode == "TJSSC")
            {
                if (num == 84)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 84;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "JX115")
            {
                comissuenum = DateTime.Now.ToString("yyyyMMdd");
                if (num == 65)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 65;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "XJ115")
            {
                if (num == 65)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 65;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "HLJSSC")
            {
                num = Convert.ToInt32(issuenum) + 1;
                return num == Convert.ToInt32(nowNum);
            }
            else if (cpcode == "CQSSC")
            {
                num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 3, 3));
                if (num == 120)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 120;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(3, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "HLFFC" || cpcode == "KK11X5")
            {
                num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 3, 3));
                if (num == 960)
                {
                    
                    num = num - 960;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(3, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "MK11X5" || cpcode == "SFFFC")
            {
                num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 3, 3));
                if (num == 350)
                {
                    
                    num = num - 350;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(3, '0');
                return (comissuenum == nowNum);
            }
            else if (cpcode == "WFFFC")
            {
                num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 3, 3));
                if (num == 210)
                {
                    
                    num = num - 210;
                }
                comissuenum = comissuenum + num.ToString().PadLeft(3, '0');
                return (comissuenum == nowNum);
            }
            return result;
        }

        public string GetIssueNum(string cpcode, string issuenum, int comnum, string jsoncontent)
        {
            string comissuenum = DateTime.Now.ToString("yyyyMMdd");
            int num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 2, 2));
            if (cpcode.IndexOf("FFC") > -1 || cpcode == "CQSSC" || cpcode == "KK11X5" || cpcode == "MK11X5")
            {
                num = Convert.ToInt32(issuenum.Substring(issuenum.Length - 3, 3));
            }
            num = num + comnum;
            if (cpcode == "SD11X5")
            {
                comissuenum = comissuenum.Substring(2, 6);
                if (num > 78)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 78;
                }
            }
            else if (cpcode == "GD115" || cpcode == "TJSSC")
            {
                comissuenum = comissuenum.Substring(2, 6);
                if (num > 84)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd").Substring(2, 6);
                    num = num - 84;
                }
            }
            else if (cpcode == "JX115")
            {
                comissuenum = DateTime.Now.ToString("yyyyMMdd");
                if (num > 65)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 65;
                }
            }
            else if (cpcode == "XJSSC")
            {
                if (num > 96)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 96;
                }
            }
            else if (cpcode=="HLJSSC")
            {
                num = Convert.ToInt32(issuenum) + 1;
                return num.ToString().PadLeft(7, '0');
            }
            else if (cpcode == "CQSSC")
            {
                if (num > 120)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 120;
                }
            }
            else if (cpcode == "KK11X5" || cpcode=="HLFFC")
            {
                if (num > 960)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 960;
                }
            }
            else if (cpcode == "MK11X5" || cpcode == "SFFFC")
            {
                if (num > 350)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 350;
                }
            }
            else if (cpcode == "WFFFC")
            {
                if (num > 210)
                {
                    comissuenum = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
                    num = num - 210;
                }
            }
            if (cpcode.IndexOf("FFC") > -1 || cpcode == "CQSSC" || cpcode == "KK11X5" || cpcode == "MK11X5")
            {
                comissuenum = comissuenum + num.ToString().PadLeft(3, '0');
            }
            else
            {
                comissuenum = comissuenum + num.ToString().PadLeft(2, '0');
            }
            return comissuenum;
        }
        public string GetIssueNum(int comnum, string jsoncontent,int type=0)
        {
            string comissuenum = "";
            var jsonarr = jsoncontent.Split('|');
            if (jsonarr.Length > 0 && comnum>-1 && !string.IsNullOrEmpty(jsonarr[comnum]))
            {
                comissuenum= jsonarr[comnum].Split(',')[type];
            } 
            return comissuenum;
        }


        public string MessageInfo(string message,string cpcode,string issuenum,string uid) {
            string mes = "处理失败";
            List<Plays> plays = CommonBusiness.LottertPlays.Where(x => x.CPCode == cpcode).ToList();
            Lottery lottery = CommonBusiness.LottertList.Where(x => x.CPCode == cpcode).FirstOrDefault();
            var strs = message.Trim().Split('/');
            string[] list1 = new string[] { };
            string[] list2 = new string[] { };
            string[] list3 = new string[] { };
            bool istotal = false;
            if (strs.Length == 3)
            {
                var chars = strs[1].Trim().ToCharArray();
                if (strs[1].Trim().IndexOf("龙") > -1 || strs[1].Trim().IndexOf("虎") > -1 || strs[1].Trim().IndexOf("和") > -1)
                {
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if ("龙虎和".IndexOf(s) > -1)
                        {
                            list2[list2.Count()] = c.ToString();
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
                                if (mv.Length == 2)
                                {
                                    list1[list1.Count()] = mv;
                                    mv = s;
                                }
                                else
                                {
                                    mv = mv + s;
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
                                list1[list1.Count()] = c.ToString();
                            }
                        }
                    }
                    chars = strs[1].Trim().ToCharArray();
                    foreach (char c in chars)
                    {
                        string s = c.ToString();
                        if ("龙虎和大小单双1234567890".IndexOf(s) > -1)
                        {
                            list2[list2.Count()] = s;
                        }
                    }
                }
                chars = strs[2].Trim().ToCharArray();
                foreach (char c in chars)
                {
                    string s = c.ToString();
                    if ("共".IndexOf(s) > -1)
                    {
                        istotal = true;
                    }
                    list3[list3.Count()] = strs[2].Trim().Replace("共", "").Replace("各", "");
                }
            }
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
                        order.ModelName = "";
                        order.PayFee = Convert.ToDecimal(list3[list3.Count()]);
                        order.Content = t;
                        order.UserID = uid;
                        order.IssueNum = issuenum;
                        orders.Add(order);
                        tplays.Add(s + t, pid);
                    }
                }
            }

            return mes;
        }
    }
}
