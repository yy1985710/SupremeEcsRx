using UnityEngine;
using System.Collections;
using EcsRx.Components;

namespace EcsRx.Unity.Audio
{

    public class SoundComponent : IComponent
    {
        public string Name;
        public GameObject Mount;
    }
}

