using UnityEngine;
using System.Collections;
using EcsRx.Components;

namespace EcsRx.Unity.Audio
{

    public class MusicComponent : IComponent
    {
        public string Name;
        public AudioClip AudioClip;
    }
}

