using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EcsRx.Unity.Loader
{
    public abstract class Loader<T>
    {
        protected ILoadStrategy loadStrategy;
        protected LocalFileLoader localFileLoader;
        protected Loader(ILoadStrategy loadStrategy, LocalFileLoader localFileLoader)
        {
            this.loadStrategy = loadStrategy;
            this.localFileLoader = localFileLoader;
        }

        public abstract IObservable<T> LoadFromLocal(string url);
        public abstract IObservable<T> LoadFromRemote(string url);
    }
}
