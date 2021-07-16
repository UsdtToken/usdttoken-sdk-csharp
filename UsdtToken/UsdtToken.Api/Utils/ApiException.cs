using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsdtToken.Api.Utils
{
    public class ApiException : Exception
    {
        public ApiException(string msg) : base(msg) 
        {

        }
    }
}