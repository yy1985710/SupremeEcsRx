using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcsRx.Serialize
{
    public interface IProtobufSerialize : ISerialize
    {
        byte[] Serialize(object target);
    }
}
