using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public abstract class SocketResponseMessage<T> : IResponseMessage<T>
    {
        public T Data { get; set; }
    }
}
