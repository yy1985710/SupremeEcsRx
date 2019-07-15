using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Plugins.Views.Components;
using EcsRx.Systems;
using EcsRx.Unity.Handlers;
using EcsRx.Unity.Loader;
using EcsRx.Unity.MonoBehaviours;
using NO1Software.ABSystem;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public abstract class DynamicViewResolverSystemEx : ISetupSystem, ITeardownSystem
    {
        public IEntityCollectionManager CollectionManager { get; }

        public abstract IGroup Group { get; }
        public abstract string ResourcePath { get; set; }

        protected DynamicViewResolverSystemEx(IEntityCollectionManager collectionManager)
        {
            CollectionManager = collectionManager;
        }


        public abstract Task<Object> CreateView(IEntity entity);
        public abstract void DestroyView(IEntity entity);
        public abstract void OnViewCreated(IEntity entity, Object viewObject);

        public virtual async void Setup(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            if (viewComponent.View != null) { return; }

            var viewObject = await CreateView(entity);
            OnViewCreated(entity, viewObject);
        }

        public void Teardown(IEntity entity)
        {
            DestroyView(entity);
        }


    }
}
