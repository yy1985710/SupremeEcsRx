using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.ErrorHandle;
using EcsRx.Network;
using EcsRx.Network.Data;
using EcsRx.Unity.Exception;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class HttpRequest : IHttpRequest
    {
        public Dictionary<string, string> Header { get; set; }
        public string Url { get; set; }

        private IHttpProtocol protocol;
        private IErrorHandle errorHandle;

        public HttpRequest(string ip, int port, IHttpProtocol protocol, IErrorHandle errorHandle)
        {
            Header = new Dictionary<string, string>();
            Url = $"http://{ip}:{port}";
            this.protocol = protocol;
            this.errorHandle = errorHandle;
            Header["Content-Type"] = "application/json";
        }

        public IObservable<TOut> Post<TIn, TOut, TResponse>(HttpRequestMessage<TIn> message) where TIn : class where TOut : class where TResponse : HttpResponseMessage<HttpResponseData<TOut>, TOut>, new()
        {
            Debug.Log("Http Path: " + Url + message.Path);
            string request = protocol.EncodeMessage(message);
            var subject = new Subject<TOut>();
            ObservableWWW.Post(Url + message.Path, Encoding.UTF8.GetBytes(request), Header).CatchIgnore(
                (WWWErrorException ex) =>
                {
                    Debug.Log(ex.RawErrorMessage);
                    errorHandle.Handel(new HttpException(ex.RawErrorMessage,
                        -1));
                    subject.OnError(new HttpException(ex.RawErrorMessage,
                        -1));
                }).Subscribe(data =>
                {
                    HttpResponseMessage<HttpResponseData<TOut>, TOut> response = protocol.DecodeMessage<TOut, TResponse>(data);
                   
                    if (response.IsOK)
                    {
                        Debug.Log("HttpRequest Response: " + data);
                        subject.OnNext(response.Data);
                    }
                    else
                    {
                        Debug.LogError("HttpRequest Response: " + data);
                        errorHandle.Handel(new HttpException(response.ErrorMessage,
                            Convert.ToInt32(response.ErrorCode)));
                        subject.OnError(new HttpException(response.ErrorMessage,
                            Convert.ToInt32(response.ErrorCode)));
                    }
                    subject.OnCompleted();
                }
            );

            return subject;
        }
    }
}
