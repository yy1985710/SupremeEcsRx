using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcsRx.Unity.Loader;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsRx.Framework.Handlers
{
    public class GameObjectViewHandlerEx : IViewHandlerEx
    {
        public IResourceLoader ResourceLoader { get; private set; }

        protected string PrefabTemplate { get; }
        public GameObjectViewHandlerEx(IResourceLoader resourceLoader, string prefabTemplate)
        {
            ResourceLoader = resourceLoader;
            PrefabTemplate = prefabTemplate;
        }


        public void DestroyView(object view)
        {
            Object.Destroy(view as GameObject);
        }

        public void SetActiveState(object view, bool isActive)
        {
            throw new NotImplementedException();
        }

        public object CreateView()
        {
            throw new NotImplementedException();
        }

        public async Task<object> CreateViewEx()
        {
            return await ResourceLoader.LoadAsyn(PrefabTemplate);
        }
    }

}

