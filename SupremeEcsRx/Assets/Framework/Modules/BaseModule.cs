using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;
using EcsRx.UI;
using EcsRx.Unity.Audio;
using EcsRx.Unity.Managers;

namespace EcsRx.Unity.Modules
{
    public class BaseModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<SceneManager>();
            container.Bind<AudioManager>();
            container.Bind<UIManager>();
        }
    }
}
