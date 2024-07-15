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

        string? FileName = null;

        private FunctionList Functions = new FunctionList("Главное меню");

        List<FunctionInfo> functionList = new List<FunctionInfo>()
        {
            new FunctionInfo("Определить файл", IdentifyTheFile),
            new FunctionInfo("Вывод всех записей на экран", DisplayingAllEntriesOnTheScreen),
            new FunctionInfo("Вывод записи по номеру", OutputOfTheRecordByNumber),
            new FunctionInfo("Запись данных в файл", WritingDataToAFile),
            new FunctionInfo("Удаление записи (записей) из файла", DeletingARecordsFromAFile),
            new FunctionInfo("Добавление записи в файл", AddingAnEntryToAFile),

        };

        public static void Main()
        {
            int port = 8002;
            udpClient = null;

            //TransmittedData transmittedData = new TransmittedData();

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

            MainMenu();
        }

        public static void MainMenu()
        {
            View.MainMenu(FileName, );
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

        private static void IdentifyTheFile() {
            
        }
        private static void DisplayingAllEntriesOnTheScreen() {


        }
        private static void OutputOfTheRecordByNumber() {


        }
        private static void WritingDataToAFile() {


        }
        private static void DeletingARecordsFromAFile () {


        }
        private static void AddingAnEntryToAFile () {


        }

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

        private void GetStructurs(string jsonText)
        {
            dynamic json = JsonSerializer.Deserialize<ExpandoObject>(jsonText);
        }
    }

    #region Function List

    public class FunctionInfo
    {
        public string Name { get; private set; }
        public FunctionDelegate Function { get; private set; }

        public delegate void FunctionDelegate();

        public FunctionInfo(string name, FunctionDelegate function)
        {
            Name = name;
            Function = function;
        }
    }

    public class FunctionList
    {
        public FunctionList(string name)
        {
            Name = name;
            Functions = new List<FunctionInfo>();
        }

        private List<FunctionInfo> Functions;

        public string Name
        {
            get;
            private set;
        }

        public void AddFunction(string functionName, FunctionInfo.FunctionDelegate function)
        {
            Functions.Add(new FunctionInfo(functionName, function));
        }

        public void RemoveFunction(string functionName)
        {
            Functions.RemoveAll(x => x.Name == functionName);
        }
        public void RemoveFunction(int functionIndex)
        {
            Functions.RemoveAt(functionIndex);
        }

        public void ExecuteFunction(int index)
        {
            if (index >= 0 && index < Functions.Count)
            {
                Functions[index].Function.Invoke();
            }
            else { throw new Exception("Недопустимый номер функции."); }
        }

        public List<string> GetFunNames()
        {
            List<string> names = new();
            foreach (FunctionInfo f in Functions)
            {
                names.Add(f.Name);
            }
            return names;
        }
    }
    #endregion Function List
}
