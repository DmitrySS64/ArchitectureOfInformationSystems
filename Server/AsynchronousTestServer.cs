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
        private UdpClient udpClient_S;
        private int port;

        //Логирование
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AsynchronousTestServer(int _port)
        {
            udpClient_S = new UdpClient(_port);
            this.port = _port;
            Console.WriteLine("Асинхронный сервер работает");
        }
        public void StartListenAsync()
        {
            while (true)
            {
                allDone.Reset();
                //принимает данные с узла в аснихронном режиме
                //(делегат, пользовательский объект)
                udpClient_S.BeginReceive(RequestCallback, udpClient_S);
                //ожидает .Set()
                allDone.WaitOne();
            }
        }

        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (UdpClient)ar.AsyncState;
            var ep = (IPEndPoint)udpClient_S.Client.LocalEndPoint;
            //завершение .BeginReceive() 
            var res = listener.EndReceive(ar, ref ep);
            string data = Encoding.Unicode.GetString(res);
            Console.WriteLine("Сообщение клиента: {0}", data);
            byte[] z = Encoding.Unicode.GetBytes("Ваше сообщение получено");
            udpClient_S.SendAsync(z, z.Length, ep);
        }

        private void OutputInfo(string str)
        {
            Console.WriteLine(str);
            logger.Info(str);
        }

        private void OutputError(string str)
        {
            Console.WriteLine($"Ошибка: {str}");
            logger.Error(str);
        }
    }
}
