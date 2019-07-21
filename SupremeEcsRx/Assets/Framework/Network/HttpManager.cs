using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class HttpManager : IDisposable
    {
        private List<IHttpRequest> httpRequests;

        private HttpRequestFactory factory;
        public IHttpRequest DefaultHttpRequest => httpRequests.FirstOrDefault();
        public IHttpRequest WechatHttpRequest { get; set; }
        public HttpManager(HttpRequestFactory factory)
        {
            httpRequests = new List<IHttpRequest>();
            this.factory = factory;
        }

        public IHttpRequest Create(string ip, int port)
        {
            var httpRequest = factory.Create(ip, port);
            httpRequests.Add(httpRequest);
            return httpRequest;
        }

        public IHttpRequest Get(int index)
        {
            return httpRequests[index];
        }

        public void Dispose()
        {
            httpRequests.Clear();
        }
    }
}
