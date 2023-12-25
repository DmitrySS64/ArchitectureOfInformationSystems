using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NetController;

namespace Client
{
    public class Test
    {
        public static UdpClient udpClient;

        public static void Main()
        {
            int port = 8002;
            udpClient = null;

            TransmittedData transmittedData = new TransmittedData();

            ConsoleKey input;
            string func;
            string args;

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
                    OutputError("Не удалось найти свободный порт.");
                    return;
                }
            }
            OutputMassege("Клиент работает");



            while (true)
            {
                try
                {
                    
                    //SendMessage();
                    ReceiveMessage();
                    Console.ReadKey();
                }
                catch (Exception ex) { OutputError(ex.ToString()); Console.ReadKey(); }
            }
        }

        //private static void ReceiveMessage()
        //{ 
        //    IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint;
        //    try
        //    {
        //        byte[] data = udpClient.Receive(ref remoteIp);
        //        string message = Encoding.Unicode.GetString(data);

        //        try
        //        {
        //            dynamic dynamicObject = DeserializeDynamic(message);

        //            if (dynamicObject is ExpandoObject) PrintTable(dynamicObject);
        //        }
        //        catch {
        //            OutputMassege($"Ответ от сервера: {message}");
        //        }
        //    }
        //    catch (Exception e) { OutputError(e.Message); }
        //}

        private static void SendMessage(string message)
        {
            try
            {
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

        static dynamic DeserializeDynamic(string json) => JsonSerializer.Deserialize<ExpandoObject>(json);

        static void PrintTable(ExpandoObject expando)
        {
            foreach (var property in expando)
            {
                Console.WriteLine($"{property.Key}: {property.Value}");
            }
        }

        private static TransmittedData ReceiveMessage()
        {
            IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint;
            TransmittedData status = new();
            // allDone.Set();
            try
            {
                byte[] data = udpClient.Receive(ref remoteIp);
                string decoded = Encoding.UTF8.GetString(data);
                status = JsonSerializer.Deserialize<TransmittedData>(decoded);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return status;
        }

        private void GetStructurs()
        {

        }
    }
}
