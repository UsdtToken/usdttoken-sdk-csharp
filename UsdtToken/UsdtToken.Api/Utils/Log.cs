using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace UsdtToken.Api.Utils
{
    public class Log
    {  
        public static string path = HttpContext.Current.Request.PhysicalApplicationPath + "Logs\\";
 
        public static void Debug(string className, string content)
        {
            if (ApiConfig.LOG_LEVENL >= 3)
            {
                WriteLog("DEBUG", className, content);
            }
        }
 
        public static void Info(string className, string content)
        {
            if (ApiConfig.LOG_LEVENL >= 2)
            {
                WriteLog("INFO", className, content);
            }
        }

     
        public static void Error(string className, string content)
        {
            if (ApiConfig.LOG_LEVENL >= 1)
            {
                WriteLog("ERROR", className, content);
            }
        }

     
        protected static void WriteLog(string type, string className, string content)
        {
            if (!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); 
            string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"; 
 
            StreamWriter mySw = File.AppendText(filename);
             
            string write_content = time + " " + type + " " + className + ": " + content;
            mySw.WriteLine(write_content);
             
            mySw.Close();
        }
    }
}