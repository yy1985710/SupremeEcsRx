using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public interface IResponseMessage<T> : IResponseMessage
    {
        T Data { get; }
    }

    public interface IResponseMessage : INetworkMessage
    {
        object RawData { get; set; }
    }
}
