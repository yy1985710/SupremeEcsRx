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
    public abstract class AsyncPrefabViewResolverSystem : PrefabViewResolverSystem
    {
        private GameObject prefab;
        private AssetBundleInfo assetBundleInfo;

        public IResourceLoader ResourceLoader { get; }

        

        protected override GameObject PrefabTemplate => prefab;

        protected AsyncPrefabViewResolverSystem(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator, IResourceLoader resourceLoader) 
            : base(collectionManager, eventSystem, instantiator)
        {
            ResourceLoader = resourceLoader;
        }

        protected abstract string AssetBundleTemplate(IEntity entity);
        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            assetBundleInfo.Require(view);
            entity.GetComponent<DummyViewComponent>().AsyncView.Value = view;
            entity.AddComponent<AsyncComponent>();
        }

        public override async void Setup(IEntity entity)
        {
            entity.AddComponent<DummyViewComponent>();
            prefab = await GetPrefab(entity) as GameObject;
            base.Setup(entity);
        }

        protected async Task<Object> GetPrefab(IEntity entity)
        {
            assetBundleInfo = await ResourceLoader.LoadAsyn(AssetBundleTemplate(entity));
            return assetBundleInfo.mainObject;
        }
    }
}
