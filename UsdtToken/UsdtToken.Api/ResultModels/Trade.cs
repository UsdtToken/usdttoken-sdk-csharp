
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsdtToken.Api.ResultModels
{
    public class Trade
    { 
        public string txId { get; set; } 
        public string tradeId { get; set; } 
        public string address { get; set; } 
        public string mainCoinType { get; set; } 
        public string coinType { get; set; } 
        public string amount { get; set; }
        //   1-充值 2-提款(转账)
        public int tradeType { get; set; }
        //  0-padding 1-success 2-fail
        public int status { get; set; }
        //fee
        public string fee { get; set; }
        public int decimals { get; set; } 
        public string businessId { get; set; } 
        public string memo { get; set; }

        public bool isErcToken()
        {
            return mainCoinType != coinType &&
                    this.mainCoinType == "60";
        }

        public bool isUsdt()
        {
            return this.mainCoinType == "0"
                    && this.coinType == "31";
        }

        public bool isTrcToken()
        {
            return mainCoinType != coinType &&
                    this.mainCoinType == "195";
        }
    }
}
