using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Crypto;
using EcsRx.ErrorHandle;
using EcsRx.Network.Data;
using EcsRx.Serialize;
using UniRx;
using UnityEngine;

namespace EcsRx.Network
{
    public class HttpProtocol : IHttpProtocol
    {
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }

        public HttpProtocol(ISerialize serialize, IDeserialize deserialize, ICrypto crypto)
        {
            Serialize = serialize;
            Deserialize = deserialize;
            Crypto = crypto;
        }

        public string EncodeMessage<TIn>(HttpRequestMessage<TIn> message) where TIn : class
        {
            byte[] data = Serialize.Serialize(message.Data);
            var str = Encoding.UTF8.GetString(data);
            Debug.Log("HttpRequest Request: " + str);
            byte[] encryptedData = Crypto.Encryption(data);
            var encode = Convert.ToBase64String(encryptedData);
            return encode;
        }

        public HttpResponseMessage<HttpResponseData<TOut>, TOut> DecodeMessage<TOut, TResponse>(string data) where TOut : class where TResponse : HttpResponseMessage<HttpResponseData<TOut>, TOut>, new()
        {
            var response = new TResponse();
            var base64Data = Convert.FromBase64String(data);
            var decryptionData = Crypto.Decryption(base64Data);
            var dataString = System.Text.Encoding.UTF8.GetString(decryptionData);
            response.Response = Deserialize.Deserialize<HttpResponseData<TOut>>(dataString);
            return response;
        }

    }
}
