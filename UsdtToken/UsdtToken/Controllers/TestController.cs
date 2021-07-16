using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsdtToken.Api;
using UsdtToken.Api.ResultModels;

namespace UsdtToken.Controllers
{
    public class TestController : Controller
    {
        string gateway = System.Configuration.ConfigurationManager.AppSettings["Gateway"].ToString();
        string merchantId = System.Configuration.ConfigurationManager.AppSettings["MerchantId"].ToString();
        string merchantKey = System.Configuration.ConfigurationManager.AppSettings["MerchantKey"].ToString();
        public ActionResult Index()
        {
            return View() ;
        }

        /// <summary>
        /// SupportCoin
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportCoin()
        {
            UsdtTokenClient client = new UsdtTokenClient(gateway, merchantId, merchantKey);
            List<SupportCoin> res = client.SupportCoin().data;
            if (res == null)
            {
                return Content("fail");
            }
            foreach(var item in res)
            {
                Response.Write("Name:"+item.symbol+ "<br/>MainCoinType:" + item.mainCoinType + "<br/>CoinType:" + item.coinType + "<br/>Balance:" + item.balance);
                Response.Write("<br/><br/>");
            }
            return Content("");

        }

        /// <summary>
        /// CheckAddress
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckAddress(string address = "0x3eb3ecc863d756825e80594e46f3d82abec47a97")
        {
            UsdtTokenClient client = new UsdtTokenClient(gateway, merchantId, merchantKey); 
            bool res = client.CheckAddress( 1, address);
           
            return Content(res.ToString());
        }

        /// <summary>
        /// get eth address
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCoinAddress()
        {
            UsdtTokenClient client = new UsdtTokenClient(gateway, merchantId, merchantKey); 
            Address res = client.CreateCoinAddress("1", "1", "0001").data;
            if (res == null)
            {
                return Content("fail");
            }
            return Content(res.address);
        }



        /// <summary>
        /// Transfer
        /// withdraw
        /// </summary>
        /// <returns></returns>
        public ActionResult Transfer(decimal Amount=100m)
        {
            string orderId = Common.OrderNo.NewOrderNo();
            UsdtTokenClient client = new UsdtTokenClient(gateway, merchantId, merchantKey);
            List<SupportCoin> res = client.SupportCoin().data;
            string message = "";
            if (res != null)
            {
                SupportCoin item = res.FirstOrDefault(o => o.symbol == "USDT_ERC20");
                if (item == null) return null;
                if (item.balance < Amount) return Content("not money");
                UsdtTokenResult<string> res2 = client.Transfer(orderId, item.mainCoinType,item.coinType, Amount.ToString("0.00"), "0x3eb3ecc863d756825e80594e46f3d82abec47a97", "someone withdraw", "remark");
                if (res2.code == "200")
                    res2.data = orderId.ToString();
                message = res2.message;
            }
           
            return Content(message);

        }


        /// <summary>
        ///  Callback 
        ///  https://www.usdttoken.com/login
        ///  Log in to the user center to set the callback url
        /// </summary>
        /// <returns></returns>
        public ActionResult Callback(string timestamp, string nonce, string body, string sign)
        {
            
            if (!UsdtToken.Api.Utils.Common.CheckSign(merchantKey, timestamp, nonce, body, sign))
            {
                //or
                //Response.Write("error");
                return Content("error");
            }
            Trade trade = Newtonsoft.Json.JsonConvert.DeserializeObject<Trade>(body);

            //TODO 业务处理
            if (trade.tradeType == 1)
            {
                //logger.Info("=====get deposit callback ======");
                //logger.Info("=====收到充币通知======");
                //logger.InfoFormat("address:{0},amount:{1},mainCoinType:{2},fee:{3}", trade.address, trade.amount, trade.mainCoinType, trade.fee);
                //金额为最小单位，需要转换,包括amount和fee字段 

            }
            else if (trade.tradeType == 2)
            {
                //logger.Info("=====get withdraw callback ======");
                //logger.Info("=====收到提币处理通知=====");
                //logger.InfoFormat("address:{0},amount:{1},mainCoinType:{2},businessId:{3}", trade.address, trade.amount, trade.mainCoinType, trade.businessId);
                if (trade.status == 1)
                {
                    //TODO: 提币已到账
                }
                else if (trade.status == 2)
                {  
                }
            }
            return Content("success"); 

        }
    }
}