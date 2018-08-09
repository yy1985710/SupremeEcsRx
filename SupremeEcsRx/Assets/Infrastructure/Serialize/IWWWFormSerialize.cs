using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcsRx.Serialize
{
    public interface IWWWFormSerialize : ISerialize
    {
        byte[] Serialize(object data);
    }
}
