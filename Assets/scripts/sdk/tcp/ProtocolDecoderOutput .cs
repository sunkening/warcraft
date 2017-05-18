using System.Collections;

namespace skntcp
{
   public  class ProtocolDecoderOutput
    {
       public ArrayList messageArrayList=new ArrayList();
       public void Write(object message)
       {
           messageArrayList.Add(message);
       }
    }
}
