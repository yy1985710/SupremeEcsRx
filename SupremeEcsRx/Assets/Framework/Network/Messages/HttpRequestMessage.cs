using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public abstract class HttpRequestMessage<T> : IRequestMessage<T> where T : class 
    {
        public abstract string Path { get;}
        public T Data { get; set; }

        protected HttpRequestMessage(T data)
        {
            Data = data;
        }

    }
}
