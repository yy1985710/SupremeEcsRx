using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.UI;
using EcsRx.Unity.Components;
using EcsRx.Unity.Loader;
using EcsRx.Unity.Systems;
using NO1Software.ChessAndCard.Scenes;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace EcsRx.UI
{
    public class TopmostUIViewResolver : ViewResolverSystem
    {
        private IInstantiator instantiator;
        private IResourceLoader resourceLoader;
        public override Group TargetGroup
        {
            get { return new Group(entity => entity.GetComponent<UIComponent>().IsDynamic && (entity.HasComponent<DialogComponent>() || entity.HasComponent<PopupComponent>()), typeof(UIComponent), typeof(ViewComponent)); }
        }

        public TopmostUIViewResolver(IViewHandler viewHandler, IInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader) : base(viewHandler)
        {
            this.instantiator = instantiator;
            this.resourceLoader = resourceLoader;
        }

        public override void ResolveView(IEntity entity, Action<GameObject> callback)
        {
            UIComponent uiComponent = entity.GetComponent<UIComponent>();
            resourceLoader.LoadAsyn(UIManager.AssetBundlePath + uiComponent.UIName + ".prefab").Subscribe(info =>
            {
                Scene scene = SceneManager.GetSceneByName(SceneConst.SceneName.RootScene);
                GameObject uiRoot = scene.GetRootGameObjects().Single(o => o.name == UIManager.UIRoot);
                Transform container = uiRoot.transform;
                if (uiComponent.Container != "")
                {
                    container = uiRoot.transform.Find(uiComponent.Container);
                }
                GameObject ui = instantiator.InstantiatePrefab(info.mainObject, container);
                info.Require(ui);
                callback(ui);
                entity.AddComponent<AsyncComponent>();
                entity.GetComponent<UIComponent>().IsReaday.Value = true;
            } );
        }
    }
}
