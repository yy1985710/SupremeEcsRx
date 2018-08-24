using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Components;
using NO1Software.ABSystem;
using UniRx;
using UnityEngine;
using Object = System.Object;

namespace EcsRx.Framework.Components
{
    public class DummyViewComponent : IComponent
    {
        public ReactiveProperty<GameObject> AsyncView;
        public async Task<UnityEngine.Object> GetView()
        {
            return await AsyncView;
        }
    }
}
