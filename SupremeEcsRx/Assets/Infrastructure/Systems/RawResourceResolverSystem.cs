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
using EcsRx.Unity.Components;
using EcsRx.Unity.Loader;
using EcsRx.Unity.MonoBehaviours;
using NO1Software.ABSystem;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class RawResourceResolverSystem : ISetupSystem, IDisposable
    {
        private readonly IDictionary<Guid, AssetBundleInfo> rawResourcesCache;
        private readonly List<GameObject> hostCache;
        private readonly IDisposable entitySubscription;
        public virtual Group TargetGroup { get{return new Group(typeof(RawResourceComponent));} }
        public IEntityCollectionManager CollectionManager { get; private set; }
        public IEventSystem EventSystem { get; private set; }
        public IResourceLoader ResourceLoader { get; private set; }


        protected abstract void ResolveResource(IEntity entity, Action<AssetBundleInfo> callback);

        protected RawResourceResolverSystem(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IResourceLoader resourceLoader)
        {
            CollectionManager = collectionManager;
            EventSystem = eventSystem;
            ResourceLoader = resourceLoader;

            rawResourcesCache = new Dictionary<Guid, AssetBundleInfo>();
            hostCache = new List<GameObject>();

            entitySubscription = EventSystem
                .Receive<ComponentRemovedEvent>()
                .Where(x => x.Component is RawResourceComponent && rawResourcesCache.ContainsKey(x.Entity.Id))
                .Subscribe(x =>
                {
                    var rawResource = rawResourcesCache[x.Entity.Id];
                    rawResource.Release();
                    rawResourcesCache.Remove(x.Entity.Id);
                });
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

        public void Dispose()
        { entitySubscription.Dispose(); }
    }

}

