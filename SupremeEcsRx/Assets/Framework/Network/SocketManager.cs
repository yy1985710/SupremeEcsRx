using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace EcsRx.Unity.Network
{
    public class SocketManager
    {
        private List<SocketChannel> channels;
        [Inject]
        private SocketChannel.Factory factory;
        public SocketChannel DefaultChannel => channels.FirstOrDefault();

        public SocketManager()
        {
            channels = new List<SocketChannel>();
        }

        public SocketChannel Create(string ip, int port)
        {
            var channel = factory.Create(ip, port);
            channels.Add(channel);
            return channel;
        }

        public SocketChannel Get(int index)
        {
            return channels[index];
        }
    }
}
