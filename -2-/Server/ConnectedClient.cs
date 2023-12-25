using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ConnectedClient
    {
        public TcpClient Client { get; set; }
        public string Nick { get; set; }
        public ConnectedClient(TcpClient client, string nick)
        {
            Client = client;
            Nick = nick;
        }
    }
}
