using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class TCPServer
    {
        private TcpListener listener;
        //private List<TcpClient> clients = new();
        private List<ConnectedClient> ConnectedClients = new();

        public TCPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                StreamReader Reader  = new(client.GetStream());
                StreamWriter Writer = new StreamWriter(client.GetStream());
                Writer.AutoFlush = true;

                while (client.Connected)
                {
                    var line = Reader.ReadLine();

                    if (line.Contains("Login: ") && !string.IsNullOrWhiteSpace(line.Replace("Login: ", "")))
                    {
                        var Nick = line.Replace("Login: ", "");

                        if (ConnectedClients.FirstOrDefault(s => s.Nick == Nick) == null)
                        {
                            ConnectedClients.Add(new ConnectedClient(client, Nick));
                            Console.WriteLine($"Connected client: {Nick}");
                            Writer.WriteLine("True");
                            
                            break;
                        }
                        else
                        {
                            Writer.WriteLine("False");
                            client.Client.Disconnect(false);
                        }
                    }

                }
                //поток для обработки клиента
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(ConnectedClients.First(x => x.Client == client));
            }
        }

        private void HandleClient(object clientObject)
        {
            ConnectedClient connectedClient = (ConnectedClient)clientObject;

            NetworkStream stream = connectedClient.Client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

            string clientMessage;

            try
            {
                while (true)
                {
                    clientMessage = reader.ReadLine();

                    if (string.IsNullOrEmpty(clientMessage)) break;


                    Console.WriteLine($"{connectedClient.Nick} : {clientMessage}");
                    string reponse = ProcessClientRequest(clientMessage);
                    writer.WriteLine(reponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RemoveClient(TcpClient client)
        {
            try
            {
                ConnectedClients.Remove(ConnectedClients.Find(x => x.Client == client));
            }
            catch (Exception ex) { }
            client.Close();
            Console.WriteLine("Клиент отключился");
        }

        private string ProcessClientRequest(string request)
        {
            if (request == "get_data")
            {
                string Data = "Данные";
                return Data;
            }
            else
            {
                return "Неизваестная команда";
            }
        }
    }
}
