using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace skntcp
{
    public class TCPClient : IService
    {
        public TCPSession session;
        private PackageBuffer messageBuffer = new PackageBuffer();
        ProtocolCodecFilter protocolCodecFilter;
        IHandler handler;

        protected ProtocolDecoderOutput output = new ProtocolDecoderOutput();
        public void setProtocolCodecFilter(ProtocolCodecFilter protocolCodecFilter)
        {
            this.protocolCodecFilter = protocolCodecFilter;
        }
        public void setHandler(IHandler handler)
        {
            this.handler = handler;
        }
        public bool Connect(string ip, int port)
        {
            //IPAddress address = IPAddress.Parse(ip);
            //IPEndPoint endP = new IPEndPoint(address, port);
            try
            {
                session = new TCPSession(this);
                session.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //异步方法，连接上后调用ConnectionCallback
                session.socket.Connect(ip, port);

                handler.sessionCreated(session);
                beginReceive();
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public void beginReceive()
        {
            session.socket.BeginReceive(messageBuffer.getBytes(), messageBuffer.getCurIndex(), PackageBuffer.MAX_LEN - messageBuffer.getLength(), SocketFlags.None, receive, null);
        }


        private void receive(System.IAsyncResult result)
        {
            int read;
            try
            {
                read = session.socket.EndReceive(result);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                System.Console.WriteLine("失去连接");
                return;
            }

            if (read < 1)
            {
                System.Console.WriteLine("失去连接");
                return;
            }
            //messageBuffer.length = read;
            messageBuffer.addLength(read);
            while (protocolCodecFilter.Decode(session, messageBuffer, output))
            {
                messageBuffer.read();
            }
            foreach (object message in output.messageArrayList)
            {
                handler.messageReceived(session, message);
            }
            output.messageArrayList.Clear();
            session.socket.BeginReceive(
                messageBuffer.getBytes(),
                0,
                PackageBuffer.MAX_LEN,
                SocketFlags.None,
                new System.AsyncCallback(receive),
                null
                );
        }

        public ProtocolCodecFilter getProtocalCodecFilter()
        {
            return protocolCodecFilter;
        }
        /*
        //以下使用ReceiveHeader和ReceiveBody连个方法循环读取流，
        //在TCP中，客户端和监听端的发送和接收不一定对应，TCP的传输是流的概念，不是发送次数，所以尽管读取流
        private void ReceiveHeader(System.IAsyncResult result)
        {
            try
            {
                int read = _socket.EndReceive(result);
                if (read < 1)
                {
                    System.Console.WriteLine("失去连接");
                    return;
                }

                //根据header 描述的长度读取body
                _socket.BeginReceive(
                    messageBuffer.getBodyBytes(),
                    MessageBuffer.HEAD_LEN,
                    messageBuffer.readHead(),
                    SocketFlags.None,
                    new System.AsyncCallback(ReceviBody),
                    null
                    );

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

        }

        private void ReceviBody(System.IAsyncResult result)
        {
            try
            {
                int read = _socket.EndReceive(result);
                if (read < 1)
                {
                    System.Console.WriteLine("失去连接");
                    return;
                }
                string msg = messageBuffer.ReadString();
                System.Console.WriteLine("收到消息--------->>" + msg);


                //接收完一对header和body后，继续重新等待下一个流的header
                _socket.BeginReceive(
                    messageBuffer.getHeadBytes(),
                    0,
                    MessageBuffer.HEAD_LEN,
                    SocketFlags.None,
                    new System.AsyncCallback(ReceiveHeader),
                    null
                    );

            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

        }
    */
    }
}