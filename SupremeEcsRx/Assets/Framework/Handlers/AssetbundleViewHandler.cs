using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Unity.Loader;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;
using UnityEngine;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Handlers
{
    public class AssetBundleViewHandler : IViewHandler
    {
        private IInstantiator instantiator;
        private IResourceLoader resourceLoader;

        protected string AssetBundleTemplate { get; }

        public AssetBundleViewHandler(IInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader, string assetBundleTemplate)
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
            return null;
        }

        public void CreateView(Action<Object> action)
        {
            resourceLoader.LoadAsyn(AssetBundleTemplate).Subscribe(info =>
            {
                var createdPrefab = instantiator.InstantiatePrefab(info.mainObject);
                info.Require(createdPrefab);
                action(createdPrefab);
            });
        }
    }
}
