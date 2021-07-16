using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http;
using System.Threading.Tasks;

namespace UsdtToken.Api.Utils
{
    public class HttpService
    {
        public static string ToUrl(Dictionary<string, string> dict)
        {
             
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in dict)
            { 
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    str.Append(kv.Key + "=" + kv.Value + "&");
                }
            }
            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            if (!string.IsNullOrEmpty(result))
                return result;
            else
                return  "";

        }
    
        public static string PostDataGetHtml(string url, Dictionary<string, string> dd)
        {
            try
            {
                string postData = ToUrl(dd);
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return "Network error:" + new ArgumentNullException("httpWebRequest").Message;
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "Text";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;
                req.ServicePoint.Expect100Continue = false;
                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

                return htmlResult;
            }
            catch (Exception ex)
            {
                return "(Network error)：" + ex.Message;
            }
        }
      
        public static string PostDataGetHtml(string url, string postData)
        {
            try
            {
                
                byte[] data = Encoding.UTF8.GetBytes(postData);

                Uri uri = new Uri(url);
                HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
                if (req == null)
                {
                    return "Network error:" + new ArgumentNullException("httpWebRequest").Message;
                }
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "text/plain";
                req.ContentLength = data.Length;
                req.AllowAutoRedirect = true;
                req.ServicePoint.Expect100Continue = false;
                Stream outStream = req.GetRequestStream();
                outStream.Write(data, 0, data.Length);
                outStream.Close();

                var res = req.GetResponse() as HttpWebResponse;
                if (res == null)
                {
                    return "Network error:" + new ArgumentNullException("HttpWebResponse").Message;
                }
                Stream inStream = res.GetResponseStream();
                var sr = new StreamReader(inStream, Encoding.UTF8);
                string htmlResult = sr.ReadToEnd();

               return htmlResult;
            }
            catch (Exception ex)
            {
                return " (Network error)：" + ex.Message;
            }
        }
 
        public static async Task<string> PostDataToApi(string url, Dictionary<string, string> dd)
        { 
            try
            {
                 
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip }; 
                using (var http = new HttpClient(handler))
                { 
                    var content = new FormUrlEncodedContent(dd ); 
                    var response = await http.PostAsync(url, content); 
                    response.EnsureSuccessStatusCode(); 
                    string result=await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                    return result;
                } 
            }
            catch (Exception ex)
            {
                return "(Network error)：" + ex.Message;
            }
        } 
        public static string GetDataGetHtml(string url)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "text/plain";
                httpWebRequest.Method = "GET"; 
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return "(Network error)：" + new ArgumentNullException("webStream");
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();

                webRespon.Close();
                streamReader.Close();

                return responseContent;
            }
            catch (Exception ex)
            {
                return "(Network error)：" + ex.Message;
            }
        }

        
        public static bool GetDataGetHtml(string url, string filePath, ref string mg)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = "text/plain";
                httpWebRequest.Method = "GET"; 
                httpWebRequest.AllowWriteStreamBuffering = false;
                httpWebRequest.Timeout = 300000;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    return false;
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                string responseContent = streamReader.ReadToEnd();
                mg = responseContent;
                webRespon.Close();
                streamReader.Close();
                if (responseContent.ToUpper().IndexOf("NULL") > -1)
                {
                    return false;
                }
                else
                {
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    var buff = Encoding.UTF8.GetBytes(responseContent);
                    fs.Write(buff, buff.Length, 0);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { 
            return true;
        }
         
        public static string Post(string xml, string url, bool isUseCert=false, int timeout=10)
        {
            System.GC.Collect(); 

            string result = ""; 

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            { 
                ServicePointManager.DefaultConnectionLimit = 200; 
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                } 
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;
                 
                request.ContentType = "text/plain";
                byte[] data=new  byte[0];
                if (!string.IsNullOrEmpty(xml))
                {
                   data = System.Text.Encoding.UTF8.GetBytes(xml);
                    request.ContentLength = data.Length;
                }else
                {
                    request.ContentLength = 0;
                }
                 
                if (!string.IsNullOrEmpty(xml))
                { 
                    reqStream = request.GetRequestStream();
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                } 
                response = (HttpWebResponse)request.GetResponse(); 
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new ApiException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new ApiException(e.ToString());
            }
            finally
            { 
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
         
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;
 
            try
            { 
                ServicePointManager.DefaultConnectionLimit = 200; 
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                } 
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET"; 
                response = (HttpWebResponse)request.GetResponse(); 
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                Log.Error("HttpService", "Thread - caught ThreadAbortException - resetting.");
                Log.Error("Exception message: {0}", e.Message);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                Log.Error("HttpService", e.ToString());
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                throw new ApiException(e.ToString());
            }
            catch (Exception e)
            {
                Log.Error("HttpService", e.ToString());
                throw new ApiException(e.ToString());
            }
            finally
            { 
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
    }
}