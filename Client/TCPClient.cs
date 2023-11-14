using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class TCPClient
    {
        private string _IP;
        private int _Port;
        private string _Nick;
        private string _Chat;
        private string _Message;

        TcpClient client;
        StreamReader streamReader;
        StreamWriter streamWriter;

        public TCPClient(int Port)
        {
            _IP = "127.0.0.1";
            _Port = Port;
            _Nick = "NickName";
            ConnectCommand();
        }

        public void ConnectCommand()
        {
            if (client?.Connected == true)
            {
                Console.WriteLine("Уже подключен");
                return;
            }
            try
            {
                client = new TcpClient();
                client.Connect(_IP, _Port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
            Registration();
        }

        private void Registration()
        {
            streamReader = new StreamReader(client.GetStream(), Encoding.UTF8);
            streamWriter = new StreamWriter(client.GetStream(), Encoding.UTF8);
            streamWriter.AutoFlush = true; //автоотправка

            while (true)
            {
                Console.WriteLine($"Введите ник");
                _Nick = Console.ReadLine();
                streamWriter.WriteLine($"Login: {_Nick}");
                var line = streamReader.ReadLine();
                bool value = false;
                try
                {
                    value = (bool)Validator.ConvertToType(typeof(bool), line);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                if (value)
                {
                    Console.WriteLine("Подключение успешно");
                    break;
                }
                else
                {
                    Console.WriteLine("Пользователь с таким ником уже подключен");
                }
            }

            SendCommand();
        }

        public void SendCommand()
        {
            if (!client.Connected)
            {
                Console.WriteLine("Отсутствует подключение");
                return;
            }
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        _Message = Console.ReadLine();
                        streamWriter.WriteLine($"{_Message}");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }

            });

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        if (client?.Connected == true)
                        {
                            var line = streamReader.ReadLine();

                            if (line != null)
                            {
                                Console.WriteLine($"{line}");
                                _Chat += line + "\n";
                            }
                            else
                            {
                                client.Close();
                                Console.WriteLine("ConnectedError");
                                _Chat += "ConnectedError\n";
                            }
                        }
                        Task.Delay(10).Wait();
                    }
                    catch (Exception) { }
                }
            });

            while(true);
        }


    }
}
