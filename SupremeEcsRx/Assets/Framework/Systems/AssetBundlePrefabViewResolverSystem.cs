using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.Components;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Loader;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public abstract class AssetBundlePrefabViewResolverSystem : DynamicPrefabViewResolverSystem
    {
        private IResourceLoader resourceLoader;

        public AssetBundlePrefabViewResolverSystem(IEntityCollectionManager collectionManager, IUnityInstantiator instantiator, IResourceLoader resourceLoader) : base(collectionManager, instantiator)
        {
            this.resourceLoader = resourceLoader;
        }

        protected override void OnViewCreated(IEntity entity, ViewComponent viewComponent)
        {
            var assetBundleComponent = entity.GetComponent<AssetBundleComponent>();
            assetBundleComponent.AssetBundle.Require(viewComponent.View as GameObject);
        }

        public override async Task<Object> CreateView(IEntity entity)
        {
            var resource = await resourceLoader.LoadAsyn(ResourcePath);
            var assetBundleComponent = entity.AddComponent<AssetBundleComponent>();
            assetBundleComponent.AssetBundle = resource;
            return resource.mainObject;
        }

        
        
    }
}
