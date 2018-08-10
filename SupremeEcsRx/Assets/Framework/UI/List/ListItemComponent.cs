using UnityEngine;
using System.Collections;
using EcsRx.Components;

namespace EcsRx.UI
{
    public class ListItemComponent : IComponent
    {
        public string ItemName;
        public GameObject List;
        public int ID;
    }

}

