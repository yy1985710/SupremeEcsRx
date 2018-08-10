using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Network;
using EcsRx.Unity.Network;
using Zenject;
using cocosocket4unity;
using EcsRx.Serialize;

namespace EcsRx.Unity.Installers
{
    public class NetworkInstaller : Installer<NetworkInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HttpManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SocketManager>().AsSingle();
            Container.Bind<IHttpProtocol>().To<DefaultHttpProtocol>().AsSingle();
            Container.Bind<ISocketProtocol>().To<SocketProtocol>().AsSingle();

        }
    }
}
