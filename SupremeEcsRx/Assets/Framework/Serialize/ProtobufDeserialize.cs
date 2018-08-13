﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcsRx.Serialize
{
    public class ProtobufDeserialize : IProtobufDeserialize
    {
        public T Deserialize<T>(string data)
        {
            throw new NotImplementedException();
        }

        public object Deserialize(Type type, MemoryStream source)
        {
             return ProtoBuf.Serializer.NonGeneric.Deserialize(type, source);
        }
    }
}