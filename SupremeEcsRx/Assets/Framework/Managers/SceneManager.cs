using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Unity.Components;
using EcsRx.Extensions;
using EcsRx.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Zenject;

namespace EcsRx.Unity.Managers
{
    public class SceneManager : IInitializable, IDisposable
    {
        public class SceneEventArg
        {
            public Scene Scene;
            public LoadSceneMode SceneMode;
        }

        private CompositeDisposable disposables = new CompositeDisposable();
        private IList<string> scenes;
        private IDictionary<string, IEntity> sceneEntities;

        public SceneManager(IEntityCollectionManager collectionManager, UIManager uiManager)
        {
            sceneEntities = new Dictionary<string, IEntity>();
            scenes = new List<string>();

            var defaultCollection = collectionManager.GetCollection();
            SceneLoadedAsObservable().Subscribe(arg =>
            {
                scenes.Add(arg.Scene.name);
                var entity = defaultCollection.CreateEntity();
                var sceneComponent = new SceneComponent { Scene = arg.Scene };
                entity.AddComponents(sceneComponent);
                sceneEntities.Add(new KeyValuePair<string, IEntity>(arg.Scene.name, entity));
            }).AddTo(disposables);

            SceneUnloadedAsObservable().Subscribe(scene =>
            {
                if (sceneEntities.ContainsKey(scene.name))
                {
                    scenes.Remove(scene.name);
                    defaultCollection.RemoveEntity(sceneEntities[scene.name].Id);
                    sceneEntities.Remove(scene.name);
                    uiManager.CurrentScreen.Value = null;
                }
            }).AddTo(disposables);
        }

        public void Initialize()
        {
           
        }

        public void Dispose()
        {
            disposables.Dispose();
            sceneEntities.Clear();
        }

        public IObservable<SceneEventArg> SceneLoadedAsObservable()
        {
            return Observable.FromEvent<UnityAction<Scene, LoadSceneMode>, SceneEventArg>(h => (scene, sceneMode) => h(new SceneEventArg { Scene = scene, SceneMode = sceneMode }),
               h => UnityEngine.SceneManagement.SceneManager.sceneLoaded += h,
               h => UnityEngine.SceneManagement.SceneManager.sceneLoaded -= h);
        }

        public IObservable<Scene> SceneUnloadedAsObservable()
        {
            return Observable.FromEvent<UnityAction<Scene>, Scene>(h => scene => h(scene),
                h => UnityEngine.SceneManagement.SceneManager.sceneUnloaded += h,
                h => UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= h);
        }

        public void AddScene(string sceneName)
        {
            LoadScene(sceneName);
        }

        public Scene PopScene()
        {
            string sceneName = scenes.Last();
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            UnloadScene(sceneName);
            scenes.RemoveAt(scenes.Count-1);
            return scene;
        }

        public Scene RemoveScene(string sceneName)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
            UnloadScene(sceneName);
            scenes.Remove(sceneName);
            return scene;
        }

        private void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        private void UnloadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }

      
    }

}

