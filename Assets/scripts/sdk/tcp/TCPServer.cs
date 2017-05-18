using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace skntcp
{
    public class TCPServer:IService
    {
        ProtocolDecoderOutput output = new ProtocolDecoderOutput();
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ProtocolCodecFilter protocolCodecFilter;
        IHandler handler;
        //private ConcurrentDictionary<int, TCPSession> sessions=new ConcurrentDictionary<int, TCPSession>();
        private  Dictionary<int, TCPSession> sessions = new  Dictionary<int, TCPSession>();
        private int idCounter;

        public void sendMessageBySessionId(int sessionId,byte[] message)
        {
            sessions[sessionId].Write(message);
        }
        public void setProtocolCodecFilter(ProtocolCodecFilter protocolCodecFilter)
        {
            this.protocolCodecFilter = protocolCodecFilter;
        }
        public ProtocolCodecFilter getProtocalCodecFilter()
        {
            return protocolCodecFilter;
        }
        public void setHandler(IHandler handler)
        {
            this.handler = handler;
        }
        public void Bind(int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, 4530));

            //启动监听，并且设置一个最大的队列长度
            //方法参考：http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.listen(v=VS.100).aspx
            socket.Listen(40);
        }

        public void Start()
        {
            socket.BeginAccept(new AsyncCallback(ClientAccepted), null);
        }

        public  void ClientAccepted(IAsyncResult ar)
        {
            //这就是客户端的Socket实例，我们后续可以将其保存起来
            Socket socketClient = socket.EndAccept(ar);
            TCPClient tcpClient=new TCPClient();
            TCPSession session=new TCPSession(tcpClient);
            tcpClient.session = session;
            tcpClient.setProtocolCodecFilter((ProtocolCodecFilter)Activator.CreateInstance( protocolCodecFilter.GetType()));
            tcpClient.setHandler((IHandler)Activator.CreateInstance(handler.GetType()));
            session.id = idCounter++;
            session.socket = socketClient;
            sessions[session.id] = session;
            handler.sessionCreated(session);
            tcpClient.beginReceive();
            //准备接受下一个客户端请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }
    }
}


