using System;
using UnityEngine;
using System.Collections;
using EcsRx.Components;
using NO1Software.ABSystem;

namespace EcsRx.Unity.Audio
{

    public class AudioComponent : IComponent
    {
        public string Name;
        public AudioClip AudioClip;
    }
}

