using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.UI;
using EcsRx.Framework.Components;
using EcsRx.Unity.Loader;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using EcsRx.Views.Systems;
using EcsRx.Views.ViewHandlers;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


namespace EcsRx.UI
{
    public class DefaultUIViewResolver : AsyncPrefabViewResolverSystem
    {

        public override IGroup Group
        {
            get { return new Group(entity => entity.GetComponent<UIComponent>().IsDynamic && !entity.HasComponent<DialogComponent>() && !entity.HasComponent<PopupComponent>(), typeof(UIComponent), typeof(ViewComponent)); }
        }

        public DefaultUIViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator, [Inject(Id = "AssetBundle")]IResourceLoader resourceLoader) : base(collectionManager, eventSystem, instantiator, resourceLoader)
        {

        }

        protected override string AssetBundleTemplate(IEntity entity)
        {
            UIComponent uiComponent = entity.GetComponent<UIComponent>();
            return UIManager.AssetBundlePath + uiComponent.UIName + ".prefab";
        }

        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            base.OnViewCreated(entity, view);
            UIComponent uiComponent = entity.GetComponent<UIComponent>();
            Scene scene = SceneManager.GetActiveScene();
            GameObject uiRoot = scene.GetRootGameObjects().Single(o => o.name == UIManager.UIRoot);
            Transform container = uiRoot.transform;
            if (uiComponent.Container != "")
            {
                container = uiRoot.transform.Find(uiComponent.Container);
            }
            view.transform.SetParent(container, false);
        }
    }
}
