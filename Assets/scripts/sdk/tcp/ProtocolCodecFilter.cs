

namespace skntcp
{
    public class ProtocolCodecFilter
    {
         
        public virtual  byte[] Encode(TCPSession session, object message)
        {
            return null;
        }

        public virtual bool Decode(TCPSession session, PackageBuffer buffer ,ProtocolDecoderOutput output)
        {
            return false;
        }

        
    }
}

 
 
