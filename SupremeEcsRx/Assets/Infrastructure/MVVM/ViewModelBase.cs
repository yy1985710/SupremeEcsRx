using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.UI;
using EcsRx.Unity.Components;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace EcsRx.MVVM
{
    public abstract class ViewModelBase<T, U> : ISetupSystem where T : ModelBase, new() where U : ViewBase, new()
    {
        protected string UIName { get; set; }
        protected CompositeDisposable disposables;
        [Inject]
        protected UIManager uiManager;

        public virtual T CreateModel()
        {
            return new T();
        }

        public virtual U CreateView(IEntity entity)
        {
            return entity.AddComponent<U>();
        }

        public virtual void Initialize(U view, T model)
        {
            view.Model = model;
            SetupModel(view, model);
            Reinit(view, model);
            view.View.OnEnableAsObservable().Subscribe(_ =>
            {
                view.Unbind();
                Reinit(view, model);
            }).AddTo(view.View);
            view.View.OnDisableAsObservable().Subscribe(_ =>
            {
                view.Unbind();
            }).AddTo(view.View);
        }

        protected void Reinit(U view, T model)
        {
            view.Bind();
            SetupEvents(view);
        }

        protected virtual void SetupEvents(U view)
        {
            
        }

        protected virtual void SetupModel(U view, T model)
        {
            
        }

        public virtual IGroup Group {
            get { return new Group(entity => entity.GetComponent<UIComponent>().UIName == UIName, typeof (UIComponent), typeof(ViewComponent), typeof(AsyncComponent)); }
        }

        public void Setup(IEntity entity)
        {
            
            var viewComponet = entity.GetComponent<ViewComponent>();
           // viewComponet.View.SetActive(false);
            var model = CreateModel();
            var view = CreateView(entity);
            view.InitWithView(viewComponet.View);
            Initialize(view, model);

            //viewComponet.View.SetActive(true);
        }
    }
}
