using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsdtToken.Common
{
    public class OrderNo
    {
        public static string NewOrderNo( int OrderType=1)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long f = Convert.ToInt64(ts.TotalSeconds);
            Random ram = new Random();
            return "" + (int)OrderType +f + ram.Next(111, 999);
        }
    }
}