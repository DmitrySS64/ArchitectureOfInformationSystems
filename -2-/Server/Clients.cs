using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ClientManager
    {
        private Dictionary<IPEndPoint, UdpClient> clients = new();


        public void AddClient(IPEndPoint endPoint)
        {
            if (!clients.ContainsKey(endPoint))
            {
                clients[endPoint] = new UdpClient();
            }
        }

        public void AddClient(IPEndPoint endPoint, UdpClient udpClient)
        {
            if (!clients.ContainsKey(endPoint))
            {
                clients[endPoint] = udpClient;
            }
        }

        public void RemoveClient(IPEndPoint endPoint)
        {
            if (clients.ContainsKey(endPoint))
            {
                clients[endPoint].Close();
                clients.Remove(endPoint);
            }
        }

        public void SendData(IPEndPoint endPoint, string data)
        {
            if (clients.ContainsKey(endPoint))
            {
                byte[] bytes = Encoding.Unicode.GetBytes(data);
                clients[endPoint].Send(bytes, bytes.Length, endPoint);
            }
        }

        public string ReceiveData(IPEndPoint endPoint)
        {
            if (clients.ContainsKey(endPoint))
            {
                byte[] data = clients[endPoint].Receive(ref endPoint);
                return Encoding.Unicode.GetString(data);
            }
            return null;
        }
    }
}
