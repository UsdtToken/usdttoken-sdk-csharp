using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsdtToken.Api
{
    public class UsdtTokenResult<T>
    {

        public string code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
