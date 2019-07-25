using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Network;
using EcsRx.Unity.Network;
using Zenject;

namespace EcsRx.Unity.Modules
{
    public class NetworkModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<HttpManager>();
            container.Bind<SocketManager>();
            container.Bind<IHttpProtocol, HttpProtocol>();
            container.Bind<ISocketProtocol, SocketProtocol>();
            var zenjectContainer = container.NativeContainer as DiContainer;
            zenjectContainer.BindFactory<string, int, IHttpRequest, HttpRequestFactory>().To<HttpRequest>();
            zenjectContainer.BindFactory<string, int, SocketChannel, SocketChannel.Factory>();
        }
    }
}
