using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public  abstract class SocketRequestMessage<T> : IRequestMessage<T>
    {
        public T Data { get; set; }

        public SocketRequestMessage(T data)
        {
            Data = data;
        }

    }
}
