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
            var httpRequest = factory.Create(ip, port, false);
            httpRequests.Add(httpRequest);
            return httpRequest;
        }

        public IHttpRequest CreateWechat(string ip, int port)
        {
			WechatHttpRequest = factory.Create(ip, port, true);
            httpRequests.Add(WechatHttpRequest);
            return WechatHttpRequest;
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
