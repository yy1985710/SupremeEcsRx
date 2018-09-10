using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NO1Software.ABSystem;

namespace EcsRx.Framework.Handlers
{
    public interface IRawResourceHandler
    {
        void DestroyRawResource(AssetBundleInfo rawResource);

        Task<AssetBundleInfo> CreateRawResource();
    }
}
