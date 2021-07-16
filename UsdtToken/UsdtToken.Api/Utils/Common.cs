using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;


namespace UsdtToken.Api.Utils
{
    public class Common
    {

        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }


        public static Dictionary<string, string> Params(string key, string body)
        {
            string timestamp = GetTimeStamp().ToString();
            string nonce = GetNonceString(8);
            String sign = Sign(key, timestamp, nonce, body);
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("body", body);
            map.Add("sign", sign);
            map.Add("timestamp", timestamp);
            map.Add("nonce", nonce);
            return map;
        }

        static Random random = new Random(10);
        public static string GetNonceString(int len)
        {
            string tmp = "";
            for (int i = 0; i < len; i++)
            {
                int seed = random.Next(0, 10);
                tmp += seed.ToString();
            }
            return tmp;
        }

        public static bool CheckSign(string key, string timestamp, string nonce, string body, string sign)
        {
            string checkSign = Sign(key, timestamp, nonce, body);
            return checkSign == sign;
        }


        public static string Sign(Dictionary<string, string> dict, string mchkey)
        {
            var vDic = (from objDic in dict orderby objDic.Key ascending select objDic);
            StringBuilder str = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vDic)
            {
                string pkey = kv.Key;
                if (pkey.Equals("sign"))
                    continue;
                string pvalue = kv.Value;
                if (!string.IsNullOrEmpty(pvalue))
                {
                    str.Append(pkey + "=" + pvalue + "&");
                }
            }
            String result = str.ToString().Substring(0, str.ToString().Length - 1);
            if (string.IsNullOrEmpty(mchkey))
                return result;
            else
                return result + "&key=" + mchkey;
        }

        public static string MD5(string source)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(source);
                return BitConverter.ToString(md5.ComputeHash(bytes)).Replace("-", "");
            }
        }


        public static string Sign(string key, string timestamp, string nonce, string type, string body)
        {
            try
            {
                return Md5Helper.Md5Hex(body + key + nonce + timestamp + type).ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Sign(string key, string timestamp, string nonce, string body)
        {
            try
            {
                string raw = body + key + nonce + timestamp;
                string sign = Md5Helper.Md5Hex(raw).ToLower();
                return sign;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string Sign(string key, string timestamp, string nonce)
        {
            try
            {
                return Md5Helper.Md5Hex(key + nonce + timestamp).ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}