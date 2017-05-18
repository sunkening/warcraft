using System.Net.Sockets;
using System.Net;

namespace skntcp
{
    public class TCPSession
    {
        public TCPSession(IService service)
        {
            this.service = service;
        }
        public int id;
        public Socket socket;
        private IService service;
        public void Write(object message)
        {
            socket.Send(service.getProtocalCodecFilter().Encode(this,message));
        }
    }
}
