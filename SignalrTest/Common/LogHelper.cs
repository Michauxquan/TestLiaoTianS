using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalrTest.Common
{
    public class LogHelper
    { //将日记对象缓存起来
        static string root = HttpRuntime.AppDomainAppPath + "logs";
        static object _islock = new object();

        public static void Info(string client, string name, object message)
        {
            LogClass.Log.WriteLogs(client, "", client, client + " " + message.ToString(), root + "\\" + name + "\\", false);

        }
        public static void Debug(string client, string name, object message)
        {
            LogClass.Log.WriteLogs(client, "", client, client + " " + message.ToString(), root + "\\Debug" + name + "\\", false);
        }
        public static void Error(string client, string name, object message)
        {
            LogClass.Log.WriteLogs(client, "", client, client + " " + message.ToString(), root + "\\Error" + name + "\\", false);
        }
    }
}