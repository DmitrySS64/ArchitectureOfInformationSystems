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
            int port = 8002;
            udpClient = null;

            try
            {
                udpClient = new UdpClient(port);
            }
            catch (SocketException)
            {
                for (int i = 1; i <= 10; i++) // Попробовать 10 соседних портов
                {
                    try
                    {
                        udpClient = new UdpClient(port + i);
                        port = port + i;
                        break; // Удалось найти свободный порт
                    }
                    catch (SocketException)
                    {
                        continue; // Этот порт тоже занят, пробуем следующий
                    }
                }
                if (udpClient == null)
                {
                    // Не удалось найти свободный порт
                    Console.WriteLine("Не удалось найти свободный порт.");
                    return;
                }
            }

            OutputMassege("Клиент работает");
            while (true)
            {
                try
                {
                    SendMessage();
                    ReceiveMessage();
                    Console.ReadKey();
                }
                catch (Exception ex) { OutputError(ex.ToString()); Console.ReadKey(); }
            }
        }

        private static void ReceiveMessage()
        { 
            IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIp);
                string message = Encoding.Unicode.GetString(data);
                OutputMassege($"Ответ от сервера: {message}");
            }
            catch (Exception e) { OutputError(e.Message); }
        }

        private static void SendMessage()
        {                
            try
            {
                OutputMassege("Введите сообщение: ");
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, "127.0.0.1", 8001);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        private static void OutputMassege(string massege)
        {
            Console.WriteLine($"{massege}");
        }

        private static void OutputError(string massege)
        {
            Console.WriteLine($"{massege}");
        }
    }
}
