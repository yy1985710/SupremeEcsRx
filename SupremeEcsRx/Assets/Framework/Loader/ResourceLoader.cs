using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NO1Software.ABSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Loader
{
    public class ResourceLoader : IResourceLoader
    {
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public IObservable<AssetBundleInfo> LoadAsyn(string path)
        {
            throw new NotImplementedException();
        }
    }
}
