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
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Unity.Systems;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Systems
{
    public abstract class DynamicPrefabViewResolverSystem : DynamicViewResolverSystemEx
    {

        private IUnityInstantiator instantiator;

        protected DynamicPrefabViewResolverSystem(IEntityCollectionManager collectionManager, IUnityInstantiator instantiator) : base(collectionManager)
        {
            this.instantiator = instantiator;
        }

        protected abstract void OnViewCreated(IEntity entity, ViewComponent viewComponent);
        public override void DestroyView(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var viewGameObject = viewComponent.View as GameObject;

            Object.Destroy(viewGameObject);
        }

        public override void OnViewCreated(IEntity entity, Object viewObject)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            var viewGameObject = instantiator.InstantiatePrefab(viewObject as GameObject);
            viewComponent.View = viewGameObject;


            var entityBinding = viewGameObject.GetComponent<EntityView>();
            if (entityBinding == null)
            {
                entityBinding = viewGameObject.AddComponent<EntityView>();
                entityBinding.Entity = entity;
                entityBinding.EntityCollection = CollectionManager.GetCollectionFor(entity);
            }

            if (viewComponent.DestroyWithView)
            {
                viewGameObject.OnDestroyAsObservable()
                    .Subscribe(x => entityBinding.EntityCollection.RemoveEntity(entity.Id))
                    .AddTo(viewGameObject);
            }

            OnViewCreated(entity, viewComponent);
            entity.AddComponent<ViewReadyComponent>();
        }
    }
}
