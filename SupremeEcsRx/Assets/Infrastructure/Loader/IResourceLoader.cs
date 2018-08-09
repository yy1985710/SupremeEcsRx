using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NO1Software.ABSystem;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Loader
{
    public interface IResourceLoader : ILoader 
    {
        T Load<T>(string path) where T : Object;
        IObservable<AssetBundleInfo> LoadAsyn(string path);
    }
}
