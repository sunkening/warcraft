using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using msg;
using msg.login;

namespace skntcp
{
    public class MessageManager
    {
        public static MessageManager instance = new MessageManager();
        static Dictionary<int, IAction> message2ActionMap = new Dictionary<int, IAction>();
        static Dictionary<Type, int> message2Type = new Dictionary<Type, int>();
        public const  int MsgHeadLength = 10;
        public MessageContext getMessageContext(int msgtype)
        {
            if (!message2ActionMap.ContainsKey(msgtype))
            {
                Debug.LogError("没有注册此消息，msgtype=" + msgtype);
                return null;
            }
            IAction action = message2ActionMap[msgtype];
            MessageContext messageContext = new MessageContext();
            messageContext.action = action;
            messageContext.msgtype = msgtype;
            return messageContext;
        }

        public void registerServerMessage(int msgtype, IAction action)
        {
            message2ActionMap[msgtype] = action;
        }
        public void registerClientMessage(int msgtype, Type t)
        {
            message2Type[t] = msgtype;
        }

        public int getMessageType(Type t)
        {
            return message2Type[t];
        }
        public void init()
        {
            registerServerMessage((int) MsgType.SAccount, new LoginAction());
            registerClientMessage((int) MsgType.CAccount, typeof (CAccount));
        }
    }
}
