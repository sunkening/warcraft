using System;

namespace skntcp
{
    public interface IHandler
    {
        void sessionCreated(TCPSession session);
        void messageReceived(TCPSession session, Object message);
    }
}
