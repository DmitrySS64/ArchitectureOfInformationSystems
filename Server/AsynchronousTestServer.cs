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

namespace Server
{
    class AsynchronousTestServer
    {
         //Семафор
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpServer;
        private int port;
        //private List<client> clients;

        //List<ClientManager> clients;

        CommandRepository commandRepository;
        Dictionary<string, Delegate> functions = new Dictionary<string, Delegate>();

        //Логирование
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AsynchronousTestServer(int _port)
        {
            //clients = new List<client>();
            commandRepository = new CommandRepository();
            udpServer = new UdpClient(this.port = _port);
            OutputInfo("Асинхронный сервер работает");

            functions["DisplayingAllEntries"] = commandRepository.DisplayingAllEntries;
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

            if(functions.ContainsKey(func_str))
            {
                if(data.Length == 1) {
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
            foreach(var func in functions.Keys ) {
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

        //private class client
        //{
        //    public IPEndPoint endPoint;
        //    public delegate string FuncDelegate(params object[] parametrs);
        //    public FuncDelegate func;
        //    public Type[] ParameterTypes { get; set; }
        //}

    }
}
