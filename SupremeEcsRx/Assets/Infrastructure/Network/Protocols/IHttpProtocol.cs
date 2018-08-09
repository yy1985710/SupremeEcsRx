using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Network.Data;
using UniRx;

namespace EcsRx.Network
{
    public interface IHttpProtocol : IProtocol
    {
       string EncodeMessage<TIn>(HttpRequestMessage<TIn> message) where TIn : class;
       HttpResponseMessage<HttpResponseData<TOut>, TOut> DecodeMessage<TOut, TResponse>(string data) where TOut : class where TResponse: HttpResponseMessage<HttpResponseData<TOut>, TOut>, new();
    }
}
