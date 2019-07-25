using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EcsRx.Serialize
{
    public class ProtobufSerialize : IProtobufSerialize
    {
        public byte[] Serialize<T>(T data) 
        {
            var message = data as IMessage;
            if (message != null)
            {
                using (var stream = new MemoryStream())
                {
                    message.WriteTo(stream);
                    return stream.ToArray();
                }
            }
            else
            {
                throw new InvalidCastException("the type don't derived from IMessage!");
            }
        }
    }
}
