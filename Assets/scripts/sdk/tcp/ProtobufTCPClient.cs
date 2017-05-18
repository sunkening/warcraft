using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Net;

namespace skntcp
{
    public class ProtobufTCPClient  
    {
        TCPClient client = new TCPClient();
        public void connect(string ip,int port,IHandler handler)
        {
            client.setProtocolCodecFilter(new ProtobufProtocolFilter());
            client.setHandler(handler);
            client.Connect(ip, port);
        }
       

    }
}