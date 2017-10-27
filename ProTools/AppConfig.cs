using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ProTools
{
    public class AppConfig
    {
        public static string AppUrl = System.Configuration.ConfigurationManager.AppSettings["AppUrl"] ?? "http://t.apiplus.cn/";
        public static string AppKey = System.Configuration.ConfigurationManager.AppSettings["AppKey"] ?? "";

        public static string AppUrl1 = System.Configuration.ConfigurationManager.AppSettings["AppUrlBack"] ?? "http://t.apiplus.cn/";
        public static string AppKey1 = System.Configuration.ConfigurationManager.AppSettings["AppKeyBack"] ?? "";

        public static string AppUrl2 = System.Configuration.ConfigurationManager.AppSettings["AppUrlBack2"] ?? "http://47.89.48.203";
        public static string AppKey2 = System.Configuration.ConfigurationManager.AppSettings["AppKeyBack2"] ?? "8888";
        /// <summary>
        /// 布优支付
        /// </summary>
        public static string BUAppUrl = System.Configuration.ConfigurationManager.AppSettings["BUAppUrl"] ?? "https://submit.buucard.com/PayBank.aspx";
        public static string BUAppKey = System.Configuration.ConfigurationManager.AppSettings["BUAppKey"] ?? "22171";
        public static string BUAppSecretKey = System.Configuration.ConfigurationManager.AppSettings["BUAppSecretKey"] ?? "34e3b1ab58614577dd90cf730911d201";

        public static string callback = System.Configuration.ConfigurationManager.AppSettings["Url"] ?? "http://192.168.2.20:6323";

        /// <summary>
        /// 智汇付支付
        /// </summary>
        public static string ZHFAppUrl = System.Configuration.ConfigurationManager.AppSettings["ZHFAppUrl"] ?? "https://pay.zhihpay.com/gateway?input_charset=UTF-8";
        public static string ZHFAppKey = System.Configuration.ConfigurationManager.AppSettings["ZHFAppKey"] ?? "Z800021999888";

        public static string ZHFAppSecretKey =
            System.Configuration.ConfigurationManager.AppSettings["ZHFAppSecretKey"] ??
            @"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBANDpeXvHvEO1PEsWpt6ZtMYhaKqJmkW0e9RCKtcXrmN4eohZXBRDS8Nq80eHZU1jiW0/ye84DwKF+SdxR1CxmjiKNHtNVHITW/sUP1GGPzA7trpm3CqMdPqtso+YZLSM6XiimIOsZk8Zwt81Md9g0qdaAV3xTQuEymJ4uxIgBSOhAgMBAAECgYA5OBlb+8Lm0JbpAhVGVtcahUADpZipitt3sX/GVegfunnlKoR70ErKBVsItl7aqW6Ui6olaTdDO6qYUQB1a4+PefUh4BgQ8J/M6+WQc6asoiuiJP+p2AJ1apR8fbkmOPUgy8F3OAInYjxn3hlHySxSVvnhWHTWJT6E9RR35WWh+QJBAPUoyb/Sfz1pohqd+8OX5LhdQY7pXBv4RU6ng26so2u/hBNg4HPUSdt9RVHOvYSKR2n0YcWtR+oBsn4HHvzuIJ8CQQDaJl2XEFpAs0J5aMvNcqorzni/vA61OZJqOc+h+t7BVWZ/uanCqZb3FGzBrzThGgfd2Xd5NLTFatd2N/jQsRO/AkBGTd4dXlYS3HoaO/f5DVQP8t5cB1vcwYPOnIc6c9OhkJhlnkB/tv8/LFt2rFz451a3cdegAqM+3iG7tnsSeY9tAkAZRxzNJl3u5VasjtIeykyhqtkfDoF9ymAG4xAGKvWo3WZ1ImRjZBdUJg+8Gbs84jFLKanIZ60SuyCgWDgCpqItAkEArTRCtDCV3d4n6cnXXeysKx3N9AKVX2+kzCAV8ddHTCKkjmcwH4cTY4Vo5uSQJuXN1TYYHGEDJlh77KwGUc+OeA==";

        public static string ZHFPublicKey = System.Configuration.ConfigurationManager.AppSettings["ZHFPublicKey"] ?? 
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCJyObRvupLCiuQx8RL1nCD+dplN0aHA88jQniDW1COLF7RFr4GmTRvNr1/Timl+R9s3SYN4nVTuGNvW6nndNH6bPPaNvPlIRcA9xWXT41mYbALedQcfET67lP6p16IWaQF6QWiLwLMdTkEHjdlf6uwaeO2h5POYh9+u/BPW9dwOQIDAQAB";

    } 
    public enum KCWAppUrl
    {
        [Description("newly.do")]
        NewLy,
        [Description("daily.do")]
        Daily ,
        [Description("curly.do")]
        Curly
    }
}
