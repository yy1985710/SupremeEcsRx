using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Unity.Loader;
using Zenject;

namespace EcsRx.Unity.Modules
{
    public class ResourceLoaderModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<ILoadStrategy, LocalPriorLoadStrategy>();
            container.Bind<LocalFileLoader>();
            container.Bind<RemoteFileLoader>();
            container.Bind<ImageLoader>();
            container.Bind<IResourceLoader, ResourceLoader>();

            var zenjectContainer = container.NativeContainer as DiContainer;
            zenjectContainer.Bind<IResourceLoader>().WithId("AssetBundle").To<AssetBundleLoader>().AsSingle();
        }
    }
}
