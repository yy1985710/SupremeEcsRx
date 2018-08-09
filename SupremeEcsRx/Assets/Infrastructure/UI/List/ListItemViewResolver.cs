using System;
using UnityEngine;
using System.Collections;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Unity.Components;
using EcsRx.Unity.Loader;
using EcsRx.Unity.Systems;
using UniRx;
using Zenject;

namespace EcsRx.UI
{
    public class ListItemViewResolver : ViewResolverSystem
    {
        private IInstantiator instantiator;
        private IResourceLoader resourceLoader;

        public override Group TargetGroup
        {
            get { return new Group(typeof(ListItemComponent), typeof(ViewComponent)); }
        }

        public ListItemViewResolver(IViewHandler viewHandler, IInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader) : base(viewHandler)
        {
            this.instantiator = instantiator;
            this.resourceLoader = resourceLoader;
        }

        public override void ResolveView(IEntity entity, Action<GameObject> callback)
        {
            ListItemComponent listItemComponet = entity.GetComponent<ListItemComponent>();
            resourceLoader.LoadAsyn(UIManager.AssetBundlePath + listItemComponet.ItemName + ".prefab").Subscribe(info =>
            {
                Transform container = listItemComponet.List.transform;
                GameObject item = instantiator.InstantiatePrefab(info.mainObject, container);
                info.Require(item);
                callback(item);
                entity.AddComponent<AsyncComponent>();
            });
        }
    }

}

