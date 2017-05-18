using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using msg;
using msg.login;

namespace skntcp
{
    public class ProtobufHandler : IHandler
    {
        public void messageReceived(TCPSession session, object message)
        {
            MessageContext messageContext=(MessageContext)message;
            messageContext.action.doAction(messageContext.message);
        }

        public void sessionCreated(TCPSession session)
        {
            CAccount a = new CAccount();
            a.name = "孙克宁";
            a.password = "123456";
            TCPUtil.sendMessage(session,a);
            Debug.Log("和服务器建立连接");
        }
    }
}
