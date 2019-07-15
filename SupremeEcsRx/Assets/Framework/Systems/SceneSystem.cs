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
        protected CompositeDisposable disposables;
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
            disposables = new CompositeDisposable();
            var sceneComponent = entity.GetComponent<SceneComponent>();
            scene = sceneComponent.Scene;
            SceneManager.SetActiveScene(scene);
            InitScene(sceneComponent.Scene);
        }

        public virtual void InitScene(Scene scene)
        {
            SceneContext sceneContext = null;
            ;
            foreach (var root in scene.GetRootGameObjects())
            {
                sceneContext = root.GetComponent<SceneContext>();
                if (sceneContext != null)
                {
                    break;
                }
            }
            DependencyContainer = new ZenjectDependencyContainer(sceneContext.Container);
            SystemExecutor = DependencyContainer.Resolve<ISystemExecutor>();
        }

        public virtual void ExitScene(Scene scene)
        {
            disposables.Dispose();
        }

        protected virtual ISystem RegisterBoundSystem<T>() where T : ISystem
        {
            var system = DependencyContainer.Resolve<T>();
            SystemExecutor.AddSystem(system);
            return system;
        }
    }
}
