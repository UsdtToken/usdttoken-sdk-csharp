using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsdtToken.Api.ResultModels
{
    public class SupportCoin
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public string mainCoinType { get; set; }
        public string coinType { get; set; }
        public string decimals { get; set; }
        public int tokenStatus { get; set; }
        public string mainSymbol { get; set; }
        public decimal balance { get; set; }
    }
}
