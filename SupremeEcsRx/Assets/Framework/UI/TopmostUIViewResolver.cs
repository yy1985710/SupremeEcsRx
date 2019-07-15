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
using EcsRx.UI;
using EcsRx.Unity.Components;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Loader;
using EcsRx.Unity.Scenes;
using EcsRx.Unity.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.UI
{
    public class TopmostUIViewResolver : AssetBundlePrefabViewResolverSystem
    {
        public override IGroup Group
        {
            get { return new Group(entity => entity.GetComponent<UIComponent>().IsDynamic && (entity.HasComponent<DialogComponent>() || entity.HasComponent<PopupComponent>()), typeof(UIComponent), typeof(ViewComponent)); }
        }

        public override string ResourcePath { get; set; }

        public TopmostUIViewResolver(IEntityCollectionManager collectionManager, IUnityInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader) : base(collectionManager, instantiator, resourceLoader)
        {

        }

        public override async Task<Object> CreateView(IEntity entity)
        {
            UIComponent uiComponent = entity.GetComponent<UIComponent>();
            ResourcePath = UIManager.AssetBundlePath + uiComponent.UIName + ".prefab";
            return await base.CreateView(entity);
        }


        protected override void OnViewCreated(IEntity entity, ViewComponent viewComponent)
        {
            base.OnViewCreated(entity, viewComponent);
            var view = viewComponent.View as GameObject;
            UIComponent uiComponent = entity.GetComponent<UIComponent>();
            Scene scene = SceneManager.GetSceneByName(SceneConst.SceneName.RootScene);
            GameObject uiRoot = scene.GetRootGameObjects().Single(o => o.name == UIManager.UIRoot);
            Transform container = uiRoot.transform;
            if (uiComponent.Container != "")
            {
                container = uiRoot.transform.Find(uiComponent.Container);
                view.transform.SetParent(container, false);
            }
            
        }
    }
}
