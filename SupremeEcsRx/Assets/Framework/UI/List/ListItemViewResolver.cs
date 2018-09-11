using System;
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Loader;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.UI
{
    public class ListItemViewResolver : AssetBundlePrefabViewResolverSystem
    {
        public override IGroup Group
        {
            get { return new Group(typeof(ListItemComponent), typeof(ViewComponent)); }
        }

        public override string ResourcePath { get; set; }

        public ListItemViewResolver(IEntityCollectionManager collectionManager, IUnityInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader) : base(collectionManager, instantiator, resourceLoader)
        {
           
        }

        public override async Task<Object> CreateView(IEntity entity)
        {
            ListItemComponent listItemComponet = entity.GetComponent<ListItemComponent>();
            ResourcePath = UIManager.AssetBundlePath + listItemComponet.ItemName + ".prefab";
            return await base.CreateView(entity);
        }
    }

}

