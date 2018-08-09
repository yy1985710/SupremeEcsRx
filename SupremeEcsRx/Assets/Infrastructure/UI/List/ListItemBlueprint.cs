using UnityEngine;
using System.Collections;
using EcsRx.Blueprints;
using EcsRx.Entities;

namespace EcsRx.UI
{
    public class ListItemBlueprint : IBlueprint
    {
        private string itemName;
        private GameObject list;

        protected int id;

        public ListItemBlueprint(string itemName, GameObject list, int id)
        {
            this.itemName = itemName;
            this.list = list;
            this.id = id;
        }
        public virtual void Apply(IEntity entity)
        {
            var listItemComponent = new ListItemComponent{ItemName = itemName, List = list, ID = id};
            entity.AddComponent(listItemComponent);
        }
    }

}

