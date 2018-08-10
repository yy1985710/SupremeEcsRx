using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.Unity.Loader
{
    public class RemoteFileLoader : IFileLoader
    {
     
        public virtual IObservable<WWW> Load(string path)
        {
            return ObservableWWW.GetWWW(path);
        }
    }
}
