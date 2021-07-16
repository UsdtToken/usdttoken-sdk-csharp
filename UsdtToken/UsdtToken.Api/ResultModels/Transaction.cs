﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsdtToken.Api.ResultModels
{
    public class Transaction
    {
        public string txid { get; set; }
        public string tradeId { get; set; }
        public string mchid { get; set; }
        public int coinType { get; set; }
        public string tradeAddress { get; set; }
        public string tradeAmount { get; set; }
        public string fee { get; set; }
        public int tradeType { get; set; }
        public int tradeStatus { get; set; }
        public long createTime { get; set; }
        public long updateTime { get; set; }
        public string callUrl { get; set; }
        public string memo { get; set; }
    }
}
