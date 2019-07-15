﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;

namespace EcsRx.UI
{
    public class DefaultUIBlueprint : IBlueprint
    {
        private readonly string uiName;
        private UIType type;
  
        public DefaultUIBlueprint(string ui, UIType type)
        {
            uiName = ui;
            this.type = type;
        }
        public void Apply(IEntity entity)
        {
            var uiComponent = new UIComponent {UIName = uiName, IsDynamic = true, UIType = type};
            entity.AddComponents(uiComponent);
            entity.AddComponent<ViewComponent>();
            
        }
    }
}
