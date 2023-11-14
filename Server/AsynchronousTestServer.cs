using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class AsynchronousTestServer
    {
        //Семафор
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpServer;
        private int port;

        List<ClientManager> clients;

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

            string response = "данные получены";

            byte[] z = Encoding.Unicode.GetBytes(response);
            udpServer.SendAsync(z, z.Length, ep);
        }

        private string HandleRequest(string request)
        {
            // Ваш код для обработки запроса и вызова методов Main<T>
            // ...
            return null;
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
    }
}
