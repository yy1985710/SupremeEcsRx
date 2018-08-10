using UnityEngine;
using System.Collections;
using EcsRx.Crypto;
using EcsRx.Network;
using EcsRx.Serialize;

namespace EcsRx.Network
{
    public class DefaultHttpProtocol : HttpProtocol
    {
        public DefaultHttpProtocol(ISerialize serialize, IDeserialize deserialize, ICrypto crypto) : base(serialize, deserialize, crypto)
        {
        }
    }

}

