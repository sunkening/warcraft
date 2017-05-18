using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using msg;
using skntcp;


public class TCPUtil
{
    public static void sendMessage<T>(TCPSession session, T message)
    {
        byte[] bytes = toByteArray<T>(message);
        session.Write(bytes);
    }
    public static byte[] toByteArray<T>(T message)
    {
        //生成消息体
        int messageType=MessageManager.instance.getMessageType(message.GetType());
        MemoryStream memStreamBody = new MemoryStream();
        Serializer.Serialize<T>(memStreamBody, message);
        //生成消息头
        MsgHead msghead = new MsgHead();
        msghead.msgType = (uint)messageType;
        msghead.msgLength = (uint)memStreamBody.Length;
        MemoryStream memStreamHead = new MemoryStream();
        Serializer.Serialize<MsgHead>(memStreamHead, msghead);
        //整合消息体和消息头
        byte[] package = new byte[MessageManager.MsgHeadLength + msghead.msgLength];
        Buffer.BlockCopy(memStreamHead.GetBuffer(), 0, package, 0, MessageManager.MsgHeadLength);
        Buffer.BlockCopy(memStreamBody.GetBuffer(), 0, package, MessageManager.MsgHeadLength, (int)memStreamBody.Length);
        return package;
    }
}

