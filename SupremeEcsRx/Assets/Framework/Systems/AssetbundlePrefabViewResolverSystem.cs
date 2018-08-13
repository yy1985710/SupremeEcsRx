using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.Loader;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class AssetBundlePrefabViewResolverSystem : ViewResolverSystem
    {
        public IEntityCollectionManager CollectionManager { get; }
        public IInstantiator Instantiator { get; }

        public IResourceLoader ResourceLoader { get; }

        protected abstract string AssetBundleTemplate { get; }

        protected AssetBundlePrefabViewResolverSystem(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator, IResourceLoader resourceLoader) : base(eventSystem)
        {
            CollectionManager = collectionManager;
            Instantiator = instantiator;
            ResourceLoader = resourceLoader;
            ViewHandler = CreateViewHandler();
        }

        protected IViewHandler CreateViewHandler()
        { return new AssetBundleViewHandler(Instantiator, ResourceLoader, AssetBundleTemplate); }

        public override IViewHandler ViewHandler { get; }

        protected override void OnViewCreated(IEntity entity, ViewComponent viewComponent)
        {
            throw new NotImplementedException();
        }

        protected abstract void OnViewCreated(IEntity entity, GameObject view);

        public override void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var assetBundleViewHandler = ViewHandler as AssetBundleViewHandler;
            viewComponent.View = assetBundleViewHandler.CreateView();
            OnViewCreated(entity, viewComponent);
        }
    }
}
