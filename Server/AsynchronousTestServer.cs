using NetController;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    class AsynchronousTestServer
    {
        //Семафор
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpServer;
        private int port;


        //Логирование
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AsynchronousTestServer(int _port)
        {
            udpServer = new UdpClient(this.port = _port);
            OutputInfo("Асинхронный сервер работает");
        }

        public void StartListenAsync()
        {
            while (true)
            {
                allDone.Reset();
                //принимает данные с узла в аснихронном режиме
                //(делегат, пользовательский объект)
                udpServer.BeginReceive(RequestCallback, udpServer);
                //ожидает .Set()
                allDone.WaitOne();
            }
        }

        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (UdpClient)ar.AsyncState;
            var ep = (IPEndPoint)udpServer.Client.LocalEndPoint;

            //завершение .BeginReceive() 
            var res = listener.EndReceive(ar, ref ep);
            string data = Encoding.Unicode.GetString(res);

            OutputInfo($"Сообщение клиента {ep.Port}: {data}");

            MessageHandler(ep, data);
        }

        private void MessageHandler(IPEndPoint socket, string receivedText)
        {
            var messageParts = receivedText.Split(';');
            var command = messageParts[0];

            var arguments = messageParts.Skip(1).ToArray();

            Console.WriteLine($"Received command: {command} with arguments: {string.Join(", ", arguments)}");

            switch (command)
            {
                case Commands.GetFileNames:
                    {

                        break;
                    }
                case Commands.Load:
                    {
                        //var data = LoadData(arguments[0]);
                        break;
                    }
                case Commands.View:
                    {
                        //var records = string.Join(Environment.NewLine, data);
                        //SendResponse(socket, records);
                        break;
                    }
                case Commands.Find:
                    {
                        var records = FindRecords(arguments[0], arguments[1]);
                        SendResponse(socket, records);
                        break;
                    }
                case Commands.Add:
                    {
                        AddRecord(arguments);
                        SendResponse(socket, "Record added successfully.");
                        break;
                    }
                case Commands.Edit:
                    {
                        EditRecord(arguments);
                        SendResponse(socket, "Record edited successfully.");
                        break;
                    }
                case Commands.Delete:
                    {
                        DeleteRecord(arguments);
                        SendResponse(socket, "Record deleted successfully.");
                        break;
                    }
                default:
                    {
                        SendResponse(socket, "Unknown command.");
                        break;
                    }
            }

        }

        private string LoadData(string data)
        {
            return "";
        }
        private string FindRecords(string data, string data2)
        {
            return "";
        }
        private void AddRecord(string[] data)
        {
            
        }
        private void EditRecord(string[] data)
        {
            
        }
        private void DeleteRecord(string[] data)
        {
            
        }



        private void OutputInfo(string str)
        {
            //Console.WriteLine(str);
            logger.Info(str);
        }

        private void OutputError(string str)
        {
            //Console.WriteLine($"Ошибка: {str}");
            logger.Error(str);
        }

        private void SendResponse(IPEndPoint endPoint, string result)
        {
            byte[] z = Encoding.Unicode.GetBytes(result);
            udpServer.SendAsync(z, z.Length, endPoint);
        }
        //private class client
        //{
        //    public IPEndPoint endPoint;
        //    public delegate string FuncDelegate(params object[] parametrs);
        //    public FuncDelegate func;
        //    public Type[] ParameterTypes { get; set; }
        //}

    }

    class miniUdpServer
    {
        async Task GetDataAsync()
        {
            UdpClient udpServer = new UdpClient(5555);
            Console.WriteLine("UDP-сервер запущен...");

            var result = await udpServer.ReceiveAsync();
            var message = Encoding.UTF8.GetString(result.Buffer);

            Console.WriteLine($"Получено {result.Buffer.Length} байт");
            Console.WriteLine($"Удаленный адрес: {result.RemoteEndPoint}");
            Console.WriteLine(message);
        }
    }
    class miniUdpClient
    {
        async Task SendDataAsync()
        {
            UdpClient udpClient = new UdpClient();

            string message = "Hello METANIT.COM";
            byte[] data = Encoding.UTF8.GetBytes(message);
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            int bytes = await udpClient.SendAsync(data, remotePoint);
            Console.WriteLine($"Отправлено {bytes} байт");
        }
    }
}
