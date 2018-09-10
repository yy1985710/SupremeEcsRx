using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcsRx.Unity.Loader;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;
using UniRx;
using UnityEngine;

namespace EcsRx.Framework.Handlers
{
    public class RawResourceViewHandler : IRawResourceHandler
    {
        public IResourceLoader ResourceLoader { get; private set; }

        protected string RawResourceTemplate { get; }
        public RawResourceViewHandler(IResourceLoader resourceLoader)
        {
            ResourceLoader = resourceLoader;
        }

        public void DestroyRawResource(AssetBundleInfo rawResource)
        {
            rawResource.Release();
        }

        public async Task<AssetBundleInfo> CreateRawResource()
        {
            return await ResourceLoader.LoadAsyn(RawResourceTemplate);
        }
    }

}

