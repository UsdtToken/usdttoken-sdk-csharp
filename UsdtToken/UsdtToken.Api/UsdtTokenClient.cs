using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsdtToken.Api.ResultModels;
using UsdtToken.Api.Utils;

namespace UsdtToken.Api
{
    public class ApiUrl
    {
        public static string CREATE_ADDRESS = "mch/address/create";
        public static string WITHDRAW = "mch/withdraw"; 
        public static string SUPPORT_COIN = "mch/support-coins"; 
        public static string CHECK_ADDRESS = "mch/check/address"; 
    }
    /// <summary>
    /// 
    /// </summary>
    public class UsdtTokenClient
    {
        private string gateway;
        private string merchantId;
        private string merchantKey;
        public UsdtTokenClient(string gateway, string merchantId, string key)
        {
             
            this.gateway = gateway;
            this.merchantId = merchantId;
            this.merchantKey = key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainCoinType"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public UsdtTokenResult<Address> CreateCoinAddress(string mainCoinType,string subCoinType, string alias)
        {
            Dictionary<string, object> requestParameters = new Dictionary<string, object>();
            requestParameters.Add("merchantId", this.merchantId);
            requestParameters.Add("mainCoinType", mainCoinType);
            requestParameters.Add("coinType", subCoinType);
            requestParameters.Add("callUrl", "");//V2
            if (alias != null && alias != "")
            {
                requestParameters.Add("alias", alias);
            }

            return OperateResult<Address>(requestParameters, ApiUrl.CREATE_ADDRESS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainCoinType"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public UsdtTokenResult<List<SupportCoin>> SupportCoin()
        { 
            Dictionary<string, object> requestParameters = new Dictionary<string, object>();
            requestParameters.Add("merchantId", this.merchantId);
            requestParameters.Add("showBalance", true);
            return OperateResult<List<SupportCoin>>(requestParameters, ApiUrl.SUPPORT_COIN,false);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="orderId"></param>
       /// <param name="mainCoinType"></param>
       /// <param name="subCoinType"></param>
       /// <param name="amount"></param>
       /// <param name="address"></param>
       /// <param name="memo"></param>
       /// <param name="remark"></param>
       /// <returns></returns>
        public UsdtTokenResult<string> Transfer(string orderId,string mainCoinType, string subCoinType, string amount, string address, string memo, string remark)
        {
            Transfer transfer = new Transfer();
            transfer.address = address;
            transfer.mainCoinType = mainCoinType;
            transfer.coinType = subCoinType;
            transfer.businessId = orderId;
            transfer.merchantId = this.merchantId;
            transfer.amount = amount;
            transfer.callUrl = "";
            transfer.memo = memo;
            transfer.remark = remark; 

            return OperateResult<string, Transfer>(transfer, ApiUrl.WITHDRAW);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainCoinType"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool CheckAddress(int mainCoinType, string address)
        {
            Dictionary<string, object> requestParameters = new Dictionary<string, object>();
            requestParameters.Add("merchantId", this.merchantId);
            requestParameters.Add("address", address);
            requestParameters.Add("mainCoinType", mainCoinType);
            if (OperateResult<string>(requestParameters, ApiUrl.CHECK_ADDRESS).code == "200")
                return true;
            else
                return false;
        }

        public UsdtTokenResult<T> OperateResult<T, T1>(T1 requestParameters, string operateUrl)
        {
            try
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(requestParameters);
                body = "[" + body + "]";
                string sendStr = Newtonsoft.Json.JsonConvert.SerializeObject(Common.Params(this.merchantKey, body));
                string host = this.gateway.EndsWith("/") ? this.gateway : (this.gateway + "/");
                string textReg = HttpService.PostDataGetHtml(host + operateUrl, sendStr);
                UsdtTokenResult<T> result = Newtonsoft.Json.JsonConvert.DeserializeObject<UsdtTokenResult<T>>(textReg);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UsdtTokenResult<T> OperateResult<T>(Dictionary<string, object> requestParameters, string operateUrl, bool needArry = true)
        {
            try
            {
                string body = Newtonsoft.Json.JsonConvert.SerializeObject(requestParameters);
                if (needArry)
                    body = "[" + body + "]";
                string sendStr = Newtonsoft.Json.JsonConvert.SerializeObject(Common.Params(this.merchantKey, body));
                string host = this.gateway.EndsWith("/") ? this.gateway : (this.gateway + "/");
                string textReg = HttpService.PostDataGetHtml(host + operateUrl, sendStr);
                UsdtTokenResult<T> result = Newtonsoft.Json.JsonConvert.DeserializeObject<UsdtTokenResult<T>>(textReg);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
