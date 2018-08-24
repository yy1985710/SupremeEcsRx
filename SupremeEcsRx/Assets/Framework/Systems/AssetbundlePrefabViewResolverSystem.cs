using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Framework.Components;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.Loader;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public abstract class AssetBundlePrefabViewResolverSystem : PrefabViewResolverSystem
    {
        private GameObject prefab;
        private AssetBundleInfo assetBundleInfo;
        public IEntityCollectionManager CollectionManager { get; }
        public IInstantiator Instantiator { get; }

        public IResourceLoader ResourceLoader { get; }

        protected abstract string AssetBundleTemplate { get; }

        protected override GameObject PrefabTemplate => prefab;

        protected AssetBundlePrefabViewResolverSystem(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator, IResourceLoader resourceLoader) 
            : base(collectionManager, eventSystem, instantiator)
        {
            ResourceLoader = resourceLoader;
        }

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            assetBundleInfo.Require(view);
            entity.GetComponent<DummyViewComponent>().AsyncView.Value = view;
            entity.AddComponent<AsyncComponent>();
        }

        public override async void Setup(IEntity entity)
        {
            entity.AddComponent<DummyViewComponent>();
            prefab = await GetPrefab() as GameObject;
            base.Setup(entity);
        }

        protected async Task<Object> GetPrefab()
        {
            assetBundleInfo = await ResourceLoader.LoadAsyn(AssetBundleTemplate);
            return assetBundleInfo.mainObject;
        }
    }
}
