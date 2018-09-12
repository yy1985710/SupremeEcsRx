using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Network;
using EcsRx.Unity.Network;

namespace EcsRx.Unity.Modules
{
    public class NetworkModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<HttpManager>();
            container.Bind<SocketManager>();
            container.Bind<IHttpProtocol, DefaultHttpProtocol>();
            container.Bind<ISocketProtocol, SocketProtocol>();
        }
    }
}
