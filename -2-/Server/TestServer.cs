using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class TestServer
    {
        static string message = "";
        public static UdpClient udpServer;
        public static void Main()
        {
            udpServer = new UdpClient(8001);
            try
            {
                Console.WriteLine("Сервер работает");
                ReceiveMessage();
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static void SendMessage()
        {
            try
            {
                Console.WriteLine("Введите ответ на сообщение: \"{0}\" ", TestServer.message);
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpServer.Send(data, data.Length, "127.0.0.1", 8002);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private static void ReceiveMessage()
        {
            IPEndPoint remoteId = (IPEndPoint)udpServer.Client.LocalEndPoint;
            try
            {
                byte[] data = udpServer.Receive(ref remoteId);
                message = Encoding.Unicode.GetString(data);
                Console.WriteLine("Сообщение от клиента {0}", message);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
