using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Framework.Exceptions
{
    public class HttpException : System.Exception
    {
        public int ErrorCode { get; set; }

        public HttpException(string message, int code) : base(message)
        {
            ErrorCode = code;
        }
    }
}
