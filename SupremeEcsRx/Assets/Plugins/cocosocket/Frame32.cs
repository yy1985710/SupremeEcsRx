using System;
using System.Text;

namespace cocosocket4unity
{
	public class Frame32 : Frame
	{
		protected Frame32()
		{
		}

		public Frame32(int len)
		{
			this.payload = new ByteBuf (len);
			payload.WriteInt(0);
		}
		
		/**
		 * 写入一个字符串
		 **/ 
		public override Frame PutString(string s,byte[] ks)
		{
			if (!end)
			{
				byte[] content = Encoding.UTF8.GetBytes (s.ToCharArray ());
				this.payload.WriteInt (content.Length);
				this.payload.WriteBytes (content);
			}
			return this;
		}

		/**
		 * 封包
		 **/ 
		public override void End()
		{
			ByteBuf bb = payload;
			int reader = bb.ReaderIndex();
			int writer = bb.WriterIndex();
		    int l = writer - reader - 4; //数据长度
			bb.WriterIndex(reader);
			bb.WriteInt(l);
			bb.WriterIndex(writer);
			end = true;
		}
	}
}

