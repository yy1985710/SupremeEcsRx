using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Unity.Loader;
using EcsRx.Views.ViewHandlers;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Handlers
{
    public class AssetbundleViewHandler : IViewHandler
    {
        private IInstantiator instantiator;
        private IResourceLoader resourceLoader;

        protected string AssetBundleTemplate { get; }

        public AssetbundleViewHandler(IInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader, string assetBundleTemplate)
        {
            this.instantiator = instantiator;
            this.resourceLoader = resourceLoader;
            AssetBundleTemplate = assetBundleTemplate;
        }

        public void DestroyView(object view)
        {
            Object.Destroy(view as GameObject);
        }

        public void SetActiveState(object view, bool isActive)
        {
            (view as GameObject).SetActive(isActive);
        }

        public object CreateView()
        {

        }

        private async void LoadResource()
        {
            await test();
        }

        private async Task<int> test()
        {
            return 1;
        }
    }
}
