using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using cocosocket4unity;
using EcsRx.Crypto;
using EcsRx.Serialize;
using EcsRx.Utility;
using protocol;

namespace EcsRx.Network
{
    public class SocketProtocol : ISocketProtocol
    {
        public ISerialize Serialize { get; set; }
        public IDeserialize Deserialize { get; set; }
        public ICrypto Crypto { get; set; }

        public SocketProtocol(ISerialize serialize, IDeserialize deserialize, ICrypto crypto)
        {
            Serialize = serialize;
            Deserialize = deserialize;
            Crypto = crypto;
        }

        public Frame EncodeMessage<TIn>(SocketRequestMessage<TIn> message)
        {
            var type = message.GetType();
            var protoAttribute = Attribute.GetCustomAttribute(type, typeof(ProtoAttribute), false) as ProtoAttribute;
            Debug.Log($"SocketRequest Request: MessageID: {protoAttribute.value} {protoAttribute.description}, MessageData: {OutputUtility.PrintToString(message.Data)}");
            byte[] data = Serialize.Serialize(message.Data);
            
            Frame f = new Frame32(512);
            f.PutInt(MessageQueueHandler.GetProtocolCMD(message.GetType()));
            byte[] encryptedData = Crypto.Encryption(data);
            f.PutBytes(encryptedData);
            f.End();
            return f;
        }

        public object DecodeMessage(Type type, MemoryStream stream)
        {
            var poco = Activator.CreateInstance(type);
            
            PropertyInfo propertyInfo = type.GetProperty("Data");
            object obj = Deserialize.Deserialize(propertyInfo.PropertyType, stream);
            propertyInfo.SetValue(poco, obj, null);

            var protoAttribute = Attribute.GetCustomAttribute(type, typeof(ProtoAttribute), false) as ProtoAttribute;
            Debug.Log($"SocketRequest Response: MessageID: {protoAttribute.value} {protoAttribute.description}, MessageData: {OutputUtility.PrintToString(obj)}");
            return poco;
        }
    }
}