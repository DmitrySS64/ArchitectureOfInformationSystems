using System.Net.Sockets;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using NLog;


namespace Server
{
    class AsynchronousServer
    {
        //Семафор
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpServer;
        private int port;

        //Логирование
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AsynchronousServer(int _port)
        {
            udpServer = new UdpClient(this.port = _port);
            OutputInfo("Асинхронный сервер работает");

            //functions["DisplayingAllEntries"] = commandRepository.DisplayingAllEntries;
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

            var result = MessageHandler(data);

            byte[] z = Encoding.Unicode.GetBytes(result);
            udpServer.SendAsync(z, z.Length, ep);
        }

        private string MessageHandler(string request)
        {
            string[] data = request.Split(' ');

            string func_str = data[0];

            string output = "";

            if (functions.ContainsKey(func_str))
            {
                if (data.Length == 1)
                {
                    output = (string)functions[func_str].DynamicInvoke();
                }
                else
                {
                    Type type = Type.GetType(data[1]);
                    var arg = JsonConvert.DeserializeObject(data[2], type);
                    output = (string)functions[func_str].DynamicInvoke(arg);
                }
                return output;
            }

            output = "Список функций\n";
            foreach (var func in functions.Keys)
            {
                output += func + "\n";
            }

            return output;
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
