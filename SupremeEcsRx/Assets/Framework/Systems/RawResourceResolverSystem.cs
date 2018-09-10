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
using EcsRx.Framework.Handlers;
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
        public abstract IViewHandlerEx ViewHandlerEx { get; }

        public IEntityCollectionManager CollectionManager { get; private set; }
        public IResourceLoader ResourceLoader { get; private set; }

        protected abstract string RawResourceTemplate { get; }
        protected RawResourceResolverSystem(IEntityCollectionManager collectionManager, IResourceLoader resourceLoader)
        {
            CollectionManager = collectionManager;
            ResourceLoader = resourceLoader;
        }

        protected abstract void OnViewCreated(IEntity entity, RawResourceComponent rawResourceComponent);

        public async void Setup(IEntity entity)
        {
            var rawResourceComponent = entity.GetComponent<RawResourceComponent>();
            if (rawResourceComponent.AssetBundle != null) { return; }

            rawResourceComponent.AssetBundle = await ViewHandlerEx.CreateRawResource();
            OnViewCreated(entity, rawResourceComponent);

        }

        protected virtual void OnViewRemoved(IEntity entity, RawResourceComponent rawResourceComponent)
        {
            ViewHandlerEx.DestroyRawResource(rawResourceComponent.AssetBundle);
        }

        public virtual void Teardown(IEntity entity)
        {
            RawResourceComponent component = entity.GetComponent<RawResourceComponent>();
            OnViewRemoved(entity, component);
        }
    }

}

