using System;
using System.IO;
using msg;
using msg.login;
using ProtoBuf;

namespace skntcp
{
   public  class ProtobufProtocolFilter : ProtocolCodecFilter
    {
        public override bool Decode(TCPSession session, PackageBuffer buffer, ProtocolDecoderOutput output )
        {
            // 如果没有接收完Header部分（4字节），直接返回false  
            if (buffer.remaining() < MessageManager.MsgHeadLength) {
                return false;
            } else {
                // 标记开始位置，如果一条消息没传输完成则返回到这个位置  
                buffer.mark();

                byte[] bytes = new byte[MessageManager.MsgHeadLength];
                // 读取消息头 
                buffer.get(bytes);
                MemoryStream msgHeadStream = new MemoryStream(bytes);
                MsgHead msgHead = Serializer.Deserialize<MsgHead>(msgHeadStream);
                msgHeadStream.Close();
                // 读取消息体的长度， 
                uint bodyLength =  msgHead.msgLength;

                // 如果消息体没有接收完整，直接返回false  
                if (buffer.remaining() < bodyLength) {
                    buffer.reset(); // IoBuffer position回到原来标记的地方  
                    return false;
                } else
                {
                    //获取消息类型
                    uint messageType = msgHead.msgType;
                    bytes = new byte[bodyLength];
                    // 读取消息体
                    buffer.get(bytes);
                    //MemoryStream msgbodyStream = new MemoryStream(bytes);
                    MessageContext messageContext=MessageManager.instance.getMessageContext((int)messageType);
                    messageContext.message=messageContext.action.parse(bytes);
                    
                    output.Write(messageContext);   
                    return true;
                }
            }
        }

      
       

        public override byte[] Encode(TCPSession session, object message)
        {
            // MessageContext messsageContext=(MessageContext)message;
           // byte[] bodyBytes = (byte[])message;
           // //MemoryStream memStreamBody = new MemoryStream();
           // MsgHead msghead = new MsgHead();
           //// msghead.msgType = (uint)messsageContext.msgtype;
           // //byte[] messagebody=messsageContext.action.toBytesArray(messsageContext.message);
           // msghead.msgLength = (uint)bodyBytes.Length;
           // MemoryStream memStreamHead = new MemoryStream();
           // Serializer.Serialize<MsgHead>(memStreamHead, msghead);
           // byte[] package = new byte[MessageManager.MsgHeadLength + msghead.msgLength];
           // Buffer.BlockCopy(memStreamHead.GetBuffer(), 0, package, 0, MessageManager.MsgHeadLength);
           // Buffer.BlockCopy(bodyBytes, 0, package, MessageManager.MsgHeadLength, bodyBytes.Length);
            return (byte[])message;
        }
    }
}
