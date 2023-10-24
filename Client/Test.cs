using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Test
    {
        public static UdpClient udpClient;

        public static void Main()
        {
            udpClient = new UdpClient(8002);
            Console.WriteLine("Клиент работает");
            while (true)
            {
                try
                {
                    SendMessage();
                    ReceiveMessage();
                    Console.ReadKey();
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); Console.ReadKey(); }
            }
        }

        private static void ReceiveMessage()
        { 
            IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIp);
                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine("Ответ от сервера: {0}", message);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private static void SendMessage()
        {                
            try
            {
                Console.Write("Введите сообщение: ");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, "127.0.0.1", 8001);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
    }
}
