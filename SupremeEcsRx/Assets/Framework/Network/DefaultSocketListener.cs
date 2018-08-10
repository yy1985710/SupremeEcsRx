using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cocosocket4unity;
using EcsRx.ErrorHandle;
using EcsRx.Events;
using EcsRx.Serialize;
using UnityEngine;

namespace EcsRx.Network
{
    public class DefaultSocketListener : SocketListener
    {
        private ISocketProtocol protocol;

        public DefaultSocketListener(ISocketProtocol protocol)
        {
            this.protocol = protocol;
        }

        public override void OnMessage(USocket us, ByteBuf bb)
        {
            bb.ReaderIndex(us.getProtocol().HeaderLen());
            int cmd = bb.ReadInt();
            Debug.Log("Socket Recive Message ID: " + cmd);
            byte[] bs = bb.GetRaw();
            using (var stream = new MemoryStream(bs, bb.ReaderIndex(), bb.ReadableBytes()))
            {
                if (MessageQueueHandler.GetProtocolType(cmd) != null)
                {
                    object obj = protocol.DecodeMessage(MessageQueueHandler.GetProtocolType(cmd), stream);
                    MessageQueueHandler.PushQueue((short)cmd, obj);
                }
            }
        }

        public override void OnClose(USocket us, bool fromRemote)
        {
            Debug.LogWarning(fromRemote ? "与服务器连接已断开" : "关闭连接");
            OnSocketClosed(us, fromRemote);
        }

        public override void OnIdle(USocket us)
        {
            Debug.LogWarning("连接超时");
        }

        public override void OnOpen(USocket us)
        {
            OnSocketOpened(us);
        }

        public override void OnError(USocket us, string err)
        {
            Debug.LogWarning("异常:" + err);
            OnSocketError(us, err);
        }
    }
}
