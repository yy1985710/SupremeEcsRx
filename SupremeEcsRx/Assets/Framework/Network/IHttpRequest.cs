using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EcsRx.Network;
using EcsRx.Network.Data;
using Zenject;

namespace EcsRx.Unity.Network
{
    public interface IHttpRequest
    {
        Dictionary<string, string> Header { get; set; }
        string Url { get; set; }

        IObservable<TOut> Post<TIn, TOut, TResponse>(HttpRequestMessage<TIn> message) where TIn : class
            where TOut : class
            where TResponse : HttpResponseMessage<HttpResponseData<TOut>, TOut>, new();
    }

    public class HttpRequestFactory : PlaceholderFactory<string, int, IHttpRequest>
    {
    }

}

