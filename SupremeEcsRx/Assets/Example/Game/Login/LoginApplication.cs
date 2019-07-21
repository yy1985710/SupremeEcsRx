using System.Collections;
using System.Collections.Generic;
using EcsRx.Infrastructure.Extensions;
using EcsRx.Zenject;
using UnityEngine;

namespace HardMan.Game.Login
{
    public class LoginApplication : EcsRxApplicationBehaviour
    {
        public override void StartApplication()
        {
            BindSystems();
            StartSystems();
            ApplicationStarted();
        }

        protected override void ApplicationStarted()
        {
            throw new System.NotImplementedException();
        }
    }
}
