using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace EcsRx.MVVM
{
    public abstract class ModelBase : IDisposable
    {
        protected CompositeDisposable disposables = new CompositeDisposable();

        public virtual void Clear()
        {
            disposables.Clear();
        }
        public virtual void Dispose()
        {
            disposables.Dispose();
        }
    }
}