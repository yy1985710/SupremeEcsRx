using Google.Protobuf;
using System;
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
            throw new NotImplementedException();
        }

        public object Deserialize(object type, MemoryStream source)
        {
            var message = type as IMessage;
            if (message != null)
            {
                message.MergeFrom(source);
                return message;
            }
            else
            {
                throw new InvalidCastException("the type don't derived from IMessage!");
            }
        }

        public object Deserialize(object type, byte[] source)
        {
            var message = type as IMessage;
            if (message != null)
            {
                message.MergeFrom(source);
                return message;
            }
            else
            {
                throw new InvalidCastException("the type don't derived from IMessage!");
            }
        }
    }
}
