using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public class LocalFileLoader : IFileLoader
    {
        public virtual IObservable<WWW> Load(string path)
        {
            return ObservableWWW.GetWWW(@"file:///" + path);
        }
    }
}
