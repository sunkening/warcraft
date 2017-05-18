using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using msg;
using ProtoBuf;

namespace skntcp
{
    public interface IAction
    {
        void doAction(Object message);
        object parse(byte[] bytes);
        byte[] toBytesArray(Object message);
    }
   public class BaseAction<T> :IAction
    {
        virtual  public void doAction(Object message)
        {

        }
        public Object parse(byte[] bytes)
        {
            MemoryStream msgHeadStream = new MemoryStream(bytes);
            return Serializer.Deserialize<T>(msgHeadStream);
             
        }
        public byte[] toBytesArray(Object message)
        {
            T t=(T)message;
            MemoryStream memStream  = new MemoryStream();
            Serializer.Serialize<T>(memStream, t);
            byte[] bytes = new byte[memStream.Length];
            Buffer.BlockCopy(memStream.GetBuffer(), 0, bytes, 0, (int)memStream.Length);
            return bytes;
        }
    }
}
