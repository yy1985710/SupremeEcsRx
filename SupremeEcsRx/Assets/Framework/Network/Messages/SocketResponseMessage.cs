using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public abstract class SocketResponseMessage<T> : IResponseMessage where T : class 
    {
        public T Data { get { return RawData as T; } }
        public object RawData { get; set; }
    }
}
