using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NO1Software.ABSystem;
using UniRx;
using Zenject;
using Object = UnityEngine.Object;

namespace EcsRx.Unity.Loader
{
    public class AssetBundleLoader : IResourceLoader
    {
        public T Load<T>(string path) where T : Object
        {
            throw new System.NotImplementedException();
        }

        public IObservable<AssetBundleInfo> LoadAsyn(string path)
        {
            var subject = new Subject<AssetBundleInfo>();
            AssetBundleManager.Instance.Load(path, info =>
            {
                subject.OnNext(info);
                subject.OnCompleted();
            } );
            return subject;
        }
    }
}
