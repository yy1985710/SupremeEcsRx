using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Unity.Components;
using EcsRx.Unity.Loader;
using UniRx;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public abstract class AssetBundleRawResourceSystem : DynamicRawResourceResolverSystem
    {
        private IResourceLoader resourceLoader;
        public AssetBundleRawResourceSystem(IEntityCollectionManager collectionManager, IResourceLoader resourceLoader) : base(collectionManager)
        {
            this.resourceLoader = resourceLoader;
        }

        public override async Task<Object> CreateView(IEntity entity)
        {
            var resource = await resourceLoader.LoadAsyn(ResourcePath);
            var assetBundleComponent = entity.AddComponent<AssetBundleComponent>();
            assetBundleComponent.AssetBundle = resource;
            return resource.mainObject;
        }

        public override void DestroyView(IEntity entity)
        {
            var assetBundleComponent = entity.GetComponent<AssetBundleComponent>();
            assetBundleComponent.AssetBundle.Release();
        }

        public override void OnViewCreated(IEntity entity, Object viewObject)
        {
            var assetBundleComponent = entity.GetComponent<AssetBundleComponent>();
            assetBundleComponent.AssetBundle.Retain();
        }
    }
}
