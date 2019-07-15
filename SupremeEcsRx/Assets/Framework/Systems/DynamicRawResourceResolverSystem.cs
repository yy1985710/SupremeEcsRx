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
using EcsRx.Plugins.Views.Components;
using EcsRx.Unity.Components;
using UniRx;
using UniRx.Triggers;


namespace EcsRx.Unity.Systems
{
    public abstract class DynamicRawResourceResolverSystem : DynamicViewResolverSystemEx
    {
        public override IGroup Group { get{return new Group(typeof(RawResourceComponent));} }

        protected DynamicRawResourceResolverSystem(IEntityCollectionManager collectionManager) : base(collectionManager)
        {
        }

        public override async void Setup(IEntity entity)
        {
            var rawResourceComponent = entity.GetComponent<RawResourceComponent>();
            if (rawResourceComponent.RawResource != null) { return; }

            var viewObject = await CreateView(entity);
            OnViewCreated(entity, viewObject);

            var viewComponent = entity.GetComponent<ViewComponent>();
            var viewGameObject = viewComponent.View as GameObject;
            var entityCollection = CollectionManager.GetCollectionFor(entity);
            if (viewComponent.DestroyWithView)
            {
                viewGameObject.OnDestroyAsObservable()
                    .Subscribe(x => entityCollection.RemoveEntity(entity.Id))
                    .AddTo(viewGameObject);
            }

            entity.AddComponent<ViewReadyComponent>();
        }
    }

}

