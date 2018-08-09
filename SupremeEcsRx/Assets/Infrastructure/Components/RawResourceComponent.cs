using UnityEngine;
using System.Collections;
using EcsRx.Components;
using NO1Software.ABSystem;

namespace EcsRx.Unity.Components
{

    public class RawResourceComponent : IComponent
    {
        public AssetBundleInfo AssetBundle;
        public GameObject Host;
    }
}

