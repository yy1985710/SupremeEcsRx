using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace EcsRx.UI
{
    public class UIComponent : IComponent
    {
        public string UIName { get; set; }
        public UIType UIType { get; set; }
        public bool IsDynamic { get; set; }

        public string Container { get; set; }
    }
}
