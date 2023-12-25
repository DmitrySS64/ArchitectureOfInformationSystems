using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        private UdpClient server;
        private Thread listenThread;

        public Server()
        {
            server = new UdpClient(8001); // Порт сервера
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
        }

        private void ListenForClients()
        {
            try
            {
                while (true)
                {
                    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] clientData = server.Receive(ref clientEndPoint);

                    // Создаем новый поток для обработки клиента
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.Start(new UdpClientState(clientEndPoint, clientData));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void HandleClient(object clientObj)
        {
            UdpClientState clientState = (UdpClientState)clientObj;
            UdpClient client = new UdpClient();

            try
            {
                Console.WriteLine("Client connected: " + clientState.EndPoint.ToString());

                while (true)
                {
                    // Отправка данных клиенту
                    string messageToSend = "Hello, client!";
                    byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);
                    client.Send(dataToSend, dataToSend.Length, clientState.EndPoint);

                    // Получение данных от клиента
                    //byte[] receivedData = server.Receive(ref clientState.EndPoint);
                    //string receivedMessage = Encoding.UTF8.GetString(receivedData);
                    //Console.WriteLine("Received from client: " + receivedMessage);

                    // Можно добавить обработку полученных данных и отправить новые данные клиенту
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Close();
            }
        }

        // Класс для хранения состояния клиента
        private class UdpClientState
        {
            public IPEndPoint EndPoint { get; }
            public byte[] Data { get; set; }

            public UdpClientState(IPEndPoint endPoint, byte[] data)
            {
                EndPoint = endPoint;
                Data = data;
            }
        }
    }
}
