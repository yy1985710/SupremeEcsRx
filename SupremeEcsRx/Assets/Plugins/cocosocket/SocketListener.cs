using System;

namespace cocosocket4unity
{
	public abstract  class SocketListener
	{
	    public class SocketCloseEventArg : EventArgs
        {
	        public USocket Socket { get; set; }
            public bool FromRemote { get; set; }
	    }
	    public class SocketErrorEventArg : EventArgs
	    {
	        public USocket Socket { get; set; }
	        public string Error { get; set; }
        }

	    public event Action<USocket> SocketOpenedEvent;
	    public event EventHandler<SocketCloseEventArg> SocketClosedEvent;
	    public event EventHandler<SocketErrorEventArg> SocketErrorEvent;

	    protected virtual void OnSocketOpened(USocket us)
	    {
	        SocketOpenedEvent?.Invoke(us);
        }

	    protected virtual void OnSocketClosed(USocket us, bool fromRemote)
	    {
	        SocketClosedEvent?.Invoke(this, new SocketCloseEventArg { Socket = us, FromRemote = fromRemote });
	    }

	    protected virtual void OnSocketError(USocket us, string err)
	    {
	        SocketErrorEvent?.Invoke(this, new SocketErrorEventArg { Socket = us, Error = err });
        }

        public abstract void OnMessage(USocket us,ByteBuf bb);
		public abstract void OnClose(USocket us,bool fromRemote);
		public abstract void OnIdle(USocket us);
		public abstract void OnOpen(USocket us);
		public abstract void OnError(USocket us,string err);

	  
	}
}

