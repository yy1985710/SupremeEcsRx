using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using EcsRx.UI;
using EcsRx.Unity.Components;
using UnityEngine;

namespace EcsRx.Unity.UI
{
    public class PopupUIBlueprint : IBlueprint
    {
        private string uiName;
        private bool model;
        private Color? modelColor;
        private string title;
        private string message;

        public PopupUIBlueprint(string name, string title = null, string message = null, bool model = false, Color? modelColor = null)
        {
            uiName = name;
            this.title = title;
            this.message = message;
            this.model = model;
            this.modelColor = modelColor;
        }

        public void Apply(IEntity entity)
        {
            var uiComponent = new UIComponent { UIName = uiName, IsDynamic = true, UIType = UIType.UI_POPUP };
            var popupComponent = new PopupComponent { Title = title, Message = message, Model = model, ModalColor = modelColor };
            entity.AddComponents(uiComponent, popupComponent);
            entity.AddComponent<ViewComponent>();
        }
    }
}
