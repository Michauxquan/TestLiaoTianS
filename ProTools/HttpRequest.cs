using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProTools
{
    public class HttpRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cpcode"></param>
        /// <param name="paras"> {row:"1-20",format:"json/xml",date:"2016-12-06",code:"彩票代码"}</param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public static T RequestServer<T>(KCWAppUrl etype, Dictionary<string, object> paras, RequestType requestType = RequestType.Get)
        { 
            string type=GetEnumDesc<KCWAppUrl>(etype);
            string url = AppConfig.AppUrl + "/" + type;
            string paraStr = string.Empty; 
            paras.Add("token",AppConfig.AppKey);
            if (paras != null && paras.Count > 0)
            {
                paraStr += CreateParameterStr(paras);
            }
            //签名认证
            //string signature = Signature.GetSignature(AppConfig.AppKey, AppConfig.AppSecret, userID);
            //paraStr += "signature=" + signature;

            string strResult = string.Empty;
            try
            {
                if (requestType == RequestType.Get)
                {
                    url += "?" + paraStr;
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";

                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }
                else
                {
                    byte[] postData = Encoding.UTF8.GetBytes(paraStr);
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "POST";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";
                    httpWebRequest.ContentLength = postData.Length;

                    System.IO.Stream outputStream = httpWebRequest.GetRequestStream();
                    outputStream.Write(postData, 0, postData.Length);
                    outputStream.Close();
                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }
                if (strResult.Contains("访问过快") || strResult.Contains("无效授权") || strResult.Contains("超出"))
                    return default(T);
                else
                    return JsonConvert.DeserializeObject<T>(strResult);
            }
            catch (System.Net.WebException webException)
            {
                HttpWebResponse response = webException.Response as HttpWebResponse;
                if (response != null)
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    strResult = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
                return default(T);
            }
           
        }

        public static T RequestServer<T>(string appUrl, string appKey, KCWAppUrl etype, Dictionary<string, object> paras, RequestType requestType = RequestType.Get)
        {
            string type = GetEnumDesc<KCWAppUrl>(etype);
            string url = appUrl + "/" + type;
            string paraStr = string.Empty;
            paras.Add("token", appKey);
            if (paras != null && paras.Count > 0)
            {
                paraStr += CreateParameterStr(paras);
            }
            //签名认证
            //string signature = Signature.GetSignature(AppConfig.AppKey, AppConfig.AppSecret, userID);
            //paraStr += "signature=" + signature;

            string strResult = string.Empty;
            try
            {
                if (requestType == RequestType.Get)
                {
                    url += "?" + paraStr;
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";

                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }
                else
                {
                    byte[] postData = Encoding.UTF8.GetBytes(paraStr);
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "POST";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";
                    httpWebRequest.ContentLength = postData.Length;

                    System.IO.Stream outputStream = httpWebRequest.GetRequestStream();
                    outputStream.Write(postData, 0, postData.Length);
                    outputStream.Close();
                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }
                if (strResult.Contains("访问过快") || strResult.Contains("无效授权") || strResult.Contains("超出"))
                    return default(T);
                else
                    return JsonConvert.DeserializeObject<T>(strResult);
            }
            catch (System.Net.WebException webException)
            {
                HttpWebResponse response = webException.Response as HttpWebResponse;
                if (response != null)
                {
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    strResult = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
                return default(T);
            }

        }

        public static string RequestServer(string name, Dictionary<string, object> paras, RequestType requestType = RequestType.Post)
        { 
            string url = AppConfig.AppUrl2 + ":" + AppConfig.AppKey2 + "/" + name;
            string paraStr = string.Empty; 
            if (paras != null && paras.Count > 0)
            {
                paraStr += CreateParameterStr(paras);
            }
            //签名认证
            //string signature = Signature.GetSignature(AppConfig.AppKey, AppConfig.AppSecret, userID);
            //paraStr += "signature=" + signature;

            string strResult = string.Empty;
            try
            {
                if (requestType == RequestType.Get)
                {
                    url += "?" + paraStr;
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";

                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }
                else
                {
                    byte[] postData = Encoding.UTF8.GetBytes(paraStr);
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "POST";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";
                    httpWebRequest.ContentLength = postData.Length;

                    System.IO.Stream outputStream = httpWebRequest.GetRequestStream();
                    outputStream.Write(postData, 0, postData.Length);
                    outputStream.Close();
                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }
                return strResult;
            }
            catch (System.Net.WebException webException)
            { 
                return webException.ToString();
            }

        }
        public static T RequestServer<T>(string name, Dictionary<string, object> paras, RequestType requestType = RequestType.Post)
        {
            string url = AppConfig.AppUrl2 + ":" + AppConfig.AppKey2 + "/" + name;
            string paraStr = string.Empty;
            if (paras != null && paras.Count > 0)
            {
                paraStr += CreateParameterStr(paras);
            }
            //签名认证
            //string signature = Signature.GetSignature(AppConfig.AppKey, AppConfig.AppSecret, userID);
            //paraStr += "signature=" + signature;

            string strResult = string.Empty;
            try
            {
                if (requestType == RequestType.Get)
                {
                    url += "?" + paraStr;
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "GET";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";

                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();
                }
                else
                {
                    byte[] postData = Encoding.UTF8.GetBytes(paraStr);
                    Uri uri = new Uri(url);
                    HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;

                    httpWebRequest.Method = "POST";
                    httpWebRequest.KeepAlive = false;
                    httpWebRequest.AllowAutoRedirect = true;
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                    httpWebRequest.UserAgent = "Ocean/NET-SDKClient";
                    httpWebRequest.ContentLength = postData.Length;

                    System.IO.Stream outputStream = httpWebRequest.GetRequestStream();
                    outputStream.Write(postData, 0, postData.Length);
                    outputStream.Close();
                    HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                    Stream responseStream = response.GetResponseStream();

                    System.Text.Encoding encode = Encoding.UTF8;
                    StreamReader reader = new StreamReader(response.GetResponseStream(), encode);
                    strResult = reader.ReadToEnd();

                    reader.Close();
                    response.Close();

                }
                return JsonConvert.DeserializeObject<T>(strResult);
            }
            catch (System.Net.WebException webException)
            {
                return default(T);
            }

        }

        private static String CreateParameterStr(Dictionary<String, Object> parameters)
        {
            StringBuilder paramBuilder = new StringBuilder();
            foreach (KeyValuePair<string, object> kvp in parameters)
            {
                String encodedValue = null;
                if (kvp.Value != null)
                {
                    String tempValue = kvp.Value.ToString();
                    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(tempValue);
                    encodedValue = System.Web.HttpUtility.UrlEncode(byteArray, 0, byteArray.Length);
                }
                paramBuilder.Append(kvp.Key).Append("=").Append(encodedValue);
                paramBuilder.Append("&");
            }
            return paramBuilder.ToString();
        }

        public static string GetEnumDesc<T>(T Enumtype)
        {
            if (Enumtype == null) throw new ArgumentNullException("Enumtype");
            if (!Enumtype.GetType().IsEnum) throw new Exception("参数类型不正确");
            return ((DescriptionAttribute)Enumtype.GetType().GetField(Enumtype.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
        }
    }


    public enum RequestType
    {
        Get = 1,
        Post = 2
    }
}
