using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Unity.Exception
{
    public class SocketException : System.Exception
    {
        public int ErrorCode { get; set; }
        public int MessageID { get; set; }

        public SocketException(string message, int code, int msgId) : base(message)
        {
            ErrorCode = code;
            MessageID = msgId;
        }
    }
}
