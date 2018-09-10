using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Framework.Components;
using EcsRx.Unity.Loader;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class RawResourceResolverSystem : ISetupSystem, ITeardownSystem
    {
        public virtual IGroup Group { get{return new Group(typeof(RawResourceComponent));} }
        public abstract IViewHandler ViewHandler { get; }

        public IEntityCollectionManager CollectionManager { get; private set; }
        public IResourceLoader ResourceLoader { get; private set; }

        protected RawResourceResolverSystem(IEntityCollectionManager collectionManager, IResourceLoader resourceLoader)
        {
            CollectionManager = collectionManager;
            ResourceLoader = resourceLoader;
        }

        public void Setup(IEntity entity)
        {
            ResolveResource(entity, info =>
            {

                info.Retain();
                rawResourcesCache.Add(entity.Id, info);
                var rawResourceComponent = entity.GetComponent<RawResourceComponent>();
                var entityBinding = rawResourceComponent.Host.GetComponent<EntityView>();
                if (entityBinding == null)
                {
                    entityBinding = rawResourceComponent.Host.AddComponent<EntityView>();
                   
                }
               
                var substitedEntityView = rawResourceComponent.Host.GetComponent<SubstitedEntityView>();
                if (substitedEntityView == null)
                {
                    substitedEntityView = rawResourceComponent.Host.AddComponent<SubstitedEntityView>();
                }
                 
                substitedEntityView.Entity = entityBinding.Entity;
                substitedEntityView.Pool = entityBinding.Pool;

                entityBinding.Entity = entity;
                entityBinding.Pool = PoolManager.GetContainingPoolFor(entity);

                var find = hostCache.Find(o => o == rawResourceComponent.Host);
                if (find == null)
                {
                    rawResourceComponent.Host.OnDestroyAsObservable()
                        .Subscribe(x =>
                        {
                            entityBinding.Pool.RemoveEntity(entity);
                            hostCache.Remove(rawResourceComponent.Host);
                        })
                        .AddTo(rawResourceComponent.Host);
                    hostCache.Add(rawResourceComponent.Host);
                }
                

                entity.AddComponent<AsyncComponent>();
            } );
           
        }

        protected virtual void OnViewRemoved(IEntity entity, RawResourceComponent rawResourceComponent)
        {
            this.ViewHandler.DestroyView(rawResourceComponent.AssetBundle);
        }

        protected abstract void OnViewCreated(IEntity entity, RawResourceComponent rawResourceComponent);

        public virtual async void Setup(IEntity entity)
        {
            RawResourceComponent component = entity.GetComponent<RawResourceComponent>();
            if (component.AssetBundle != null)
                return;
            component.AssetBundle = this.ViewHandler.CreateView() as AssetBundleInfo;
            this.OnViewCreated(entity, component);
        }

        public virtual void Teardown(IEntity entity)
        {
            RawResourceComponent component = entity.GetComponent<RawResourceComponent>();
            this.OnViewRemoved(entity, component);
        }
    }

}

