using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using EcsRx.Entities;
using EcsRx.Executor;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using EcsRx.Unity.Dependencies;
using EcsRx.Zenject.Dependencies;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace EcsRx.Unity.Systems
{
    public abstract class SceneSystem : ISetupSystem, ITeardownSystem
    {
        protected string sceneName;
        protected Scene scene;
        protected CompositeDisposable disposables = new CompositeDisposable();
        [Inject]
        protected Managers.SceneManager sceneManager;

        protected ISystemExecutor SystemExecutor { get; private set; }
        public IDependencyContainer DependencyContainer { get; private set; }

        public IGroup Group {
            get
            {
                return new Group(entity => entity.GetComponent<SceneComponent>().Scene.name == sceneName, typeof(SceneComponent));
            }
        }



        public void Teardown(IEntity entity)
        {
            ExitScene(scene);
        }

        public void Setup(IEntity entity)
        {
            var sceneComponent = entity.GetComponent<SceneComponent>();
            scene = sceneComponent.Scene;
            InitScene(sceneComponent.Scene);
        }

        public virtual void InitScene(Scene scene)
        {
           
        }

        public virtual void ExitScene(Scene scene)
        {
            disposables.Dispose();
        }
    }
}
