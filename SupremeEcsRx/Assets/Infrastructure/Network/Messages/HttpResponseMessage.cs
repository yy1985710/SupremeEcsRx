using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Network
{
    public abstract class HttpResponseMessage<T, TU> : IResponseMessage<TU>
    {
        protected Dictionary<int, string> errorMessages; 
        public abstract bool IsOK { get; }
        public abstract string ErrorMessage { get; }
        public abstract int ErrorCode { get; }
        public T Response { get; set; }
        public abstract TU Data { get; }
    }
}
