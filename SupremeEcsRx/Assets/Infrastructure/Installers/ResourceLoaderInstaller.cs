using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Unity.Loader;
using Zenject;

namespace EcsRx.Unity.Installers
{
    public class ResourceLoaderInstaller : Installer<ResourceLoaderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ILoadStrategy>().To<LocalPriorLoadStrategy>().AsSingle();
            Container.Bind<LocalFileLoader>().AsSingle();
            Container.Bind<RemoteFileLoader>().AsSingle();
            Container.Bind<ImageLoader>().AsSingle();
            Container.Bind<IResourceLoader>().To<ResourceLoader>().AsSingle();
            Container.Bind<IResourceLoader>().WithId("AssetBundle").To<AssetBundleLoader>().AsSingle();
        }
    }
}
