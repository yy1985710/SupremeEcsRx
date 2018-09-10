using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcsRx.Views.ViewHandlers;
using NO1Software.ABSystem;

namespace EcsRx.Framework.Handlers
{
    public interface IViewHandlerEx : IViewHandler
    {
        Task<object> CreateViewEx();
    }
}
