using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.MVVM;
using EcsRx.Framework.Components;
using EcsRx.Framework.Extensions;
using EcsRx.Unity.Events;
using EcsRx.Unity.UI;
using EcsRx.Views.Components;
using UIWidgets;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using EventSystem = EcsRx.Events.EventSystem;

namespace EcsRx.UI
{
    public class UIManager
    {
        public static string AssetBundlePath = "Assets.AssetBundleResources.Prefabs.UI.";
        public static string ResourcePath = "Prefabs/UI/";
        public static string UIRoot = "Canvas";
        public ReactiveProperty<IEntity> CurrentScreen { get; set; }

        private IEntityCollection defaultConllection;
        private IEventSystem eventSystem;
        private IObservableGroup uiGroup;


        public UIManager(IEntityCollectionManager collectionManager, IEventSystem eventSystem)
        {
            CurrentScreen = new ReactiveProperty<IEntity>();
            defaultConllection = collectionManager.GetCollection();
            this.eventSystem = eventSystem;
            uiGroup = collectionManager.GetObservableGroup(new Group(typeof(UIComponent)));
        }

        public IEntity GetUI(string ui)
        {
           return uiGroup.SingleOrDefault(
                   entity => entity.GetComponent<UIComponent>().UIName == ui);
        }


        public async Task<IEntity> ShowUI(string ui, UIType type)
        {
            var uiEntity = CreateUI(new DefaultUIBlueprint(ui , type));
            await uiEntity.GetView();
            CreateUI(uiEntity);
            return uiEntity;
        }

        public async Task<IEntity> ShowPopup(string ui, string title = null, string message = null, bool model = false, Color? modelColor = null)
        {
            var uiEntity = CreateUI(new PopupUIBlueprint(ui, title, message, model, modelColor??new Color(0.0f, 0.0f, 0.0f, 0.8f)));
            await uiEntity.GetView();
            CreateUI(uiEntity);
            return uiEntity;
        }


        public async Task<IEntity> ShowDialog(string ui, string title = null, string message = null, DialogActions butttons = null,
            bool model = false, Color? modelColor = null)
        {
            var uiEntity = CreateUI(new DialogUIBlueprint(ui, title, message, butttons, model, modelColor ?? new Color(0.0f, 0.0f, 0.0f, 0.8f)));
            await uiEntity.GetView();
            CreateUI(uiEntity);
            return uiEntity;
        }

        public async Task<IEntity> ShowNotify(string ui,
            string message = null,
            float? customHideDelay = null,
            Transform container = null,
            Func<Notify, IEnumerator> showAnimation = null,
            Func<Notify, IEnumerator> hideAnimation = null,
            bool? slideUpOnHide = null,
            NotifySequence sequenceType = NotifySequence.None,
            float sequenceDelay = 0.3f,
            bool clearSequence = false,
            bool? newUnscaledTime = null)
        {
            var uiEntity = CreateUI(new NotifyUIBlueprint(ui, message, customHideDelay, container, showAnimation, 
                hideAnimation, slideUpOnHide, sequenceType, sequenceDelay, clearSequence, newUnscaledTime));
            await uiEntity.GetView();
            CreateUI(uiEntity);
            return uiEntity;
        }

        public IEntity CreateUI(IEntity ui)
        {
            var uiComponent = ui.GetComponent<UIComponent>();
            if (uiComponent.UIType == UIType.UI_SCREEN)
            {
                if (CurrentScreen.Value != null)
                {           
                    RemoveUI(CurrentScreen.Value);
                }
                CurrentScreen.Value = ui;
            }

            //var viewComponent = ui.GetComponent<ViewComponent>();
            //viewComponent.View.SetActive(true);
            eventSystem.Publish(new UIShowedEvent(ui));
            return ui;
        }

        public void ShowUI(IEntity ui)
        {
            var viewComponent = ui.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            view.SetActive(true);
            eventSystem.Publish(new UIShowedEvent(ui));
        }

        public void HideUI(IEntity ui)
        {
            var viewComponent = ui.GetComponent<ViewComponent>();
            var view = viewComponent.View as GameObject;
            view.SetActive(false);
            eventSystem.Publish(new UIHidedEvent(ui));
        }

        public void RemoveUI(IEntity ui)
        {
            if (ui != null)
            {
                defaultConllection.RemoveEntity(ui.Id);
                eventSystem.Publish(new UIRemovedEvent(ui));
            }
           }

        public IEntity CreateUI<T>(T blueprint) where T : IBlueprint
        {
            IEntity entity = defaultConllection.CreateEntity(blueprint);
            return entity;
        }

        public bool IsPointerOverUIObject()
        {//判断是否点击的是UI，有效应对安卓没有反应的情况，true为UI  
            PointerEventData eventDataCurrentPosition = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
