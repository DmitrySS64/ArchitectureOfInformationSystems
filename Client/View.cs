using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class View
    {
        public static int MainMenu(string? fileName, List<string> menu, bool clearView = true)
        {
            if (clearView) Output.Clear();
            int countVar = menu.Count;
            if (fileName == null || fileName == "")
            {
                fileName = "Не указан";
                countVar = 1;
            }
            Output.Text($"FileName : {fileName}");
            for (int i = 0; i < countVar; i++)
            {
                Output.ListItem(i, menu[i]);
            }

            return 0;
        }

        public static void Menu(List<string> menu, string menuName = "Меню", bool clearView = true)
        {
            if (clearView) Output.Clear();
            Console.WriteLine($"{menuName}");
            int i = 0;
            foreach (var item in menu)
            {
                Output.ListItem(i, item.ToString());
                i++;
            }
        }

        public static void DisplayTable(dynamic instance)
        {
            PropertyInfo[] properties = instance.GetType().GetProperties();

            // Выводим заголовки
            Console.WriteLine("| " + string.Join(" | ", properties.Select(p => p.Name)) + " |");

            // Выводим разделительную строку
            Console.WriteLine("|" + new string('-', properties.Length * 4 - 1) + "|");

            // Выводим значения
            Console.WriteLine("| " + string.Join(" | ", properties.Select(p => p.GetValue(instance))) + " |");

        }

        public static void Table<T>(List<T> records)
        {
            Output.Clear();

            if (records.Count == 0)
            {
                Output.Text("Нет записей.");
                return;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            // Вывод заголовков столбцов
            foreach (var property in properties)
            {
                Console.Write($"{property.Name}\t");
            }
            Console.WriteLine();

            int i = -1;
            foreach (var record in records)
            {
                i++;
                Console.Write($"{i})\t");
                foreach (var property in properties)
                {
                    object value = property.GetValue(record);
                    Console.Write($"{value}\t");
                }
                Console.WriteLine();
            }
        }


        public static void PrintObjectProperties(object? obj, bool clearView = true)
        {
            if (clearView) Output.Clear();

            if (obj == null)
            {
                Output.Text("Объект пуст");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Output.Text($"Object of type {type.Name}:");

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                Output.Parameter(property.Name, value.ToString());
                //Console.WriteLine($"{property.Name}:\t{value}");
            }
        }
    }

    public static class Output
    {
        public const string InputPrefix = ">>> ",
            MassagePrefix = ">> ",
            RejectionPrefix = "Исключение: ",
            ErrorPrefix = "Ошибка: ",
            TextPrefix = "> ",
            NumeratorPrefix = ") ",
            ParameterPrefix = " : ";

        public static void Error(string s)
        {
            Console.Write($"{ErrorPrefix}{s}\n");
            Console.ReadKey();
        }

        public static void Rejection(string massege = "Некорректный ввод, попробуйте снова")
        {
            Console.WriteLine($"{RejectionPrefix}{massege}");
            Console.ReadKey();
        }

        public static void Clear() =>
            Console.Clear();

        public static void Massage(string str) =>
            Console.WriteLine($"{MassagePrefix}{str}\n");

        public static void Text(string str) =>
            Console.WriteLine($"{TextPrefix}{str}");

        public static void ListItem(int index, string value) =>
            Console.WriteLine(index + NumeratorPrefix + value);

        public static void Parameter(string name, string value) =>
            Console.WriteLine($"{name}{ParameterPrefix}{value}");
    }


    public static class Input
    {
        public static string InputString(string massage = "Введите текст")
        {
            string? input;

            while (true)
            {
                Output.Massage(massage);
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)) return input;
                Output.Rejection();
            }
        }

        public static bool InputBool(string massage = "Введите 0 - False или 1 - True")
        {
            while (true)
            {
                Output.Massage(massage);

                if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int x) && (x == 0 || x == 1))
                {
                    return x == 1;
                }

                Output.Rejection();
            }
        }

        public static int InputInt(string massage = "Введите число")
        {
            while (true)
            {
                Output.Massage(massage);

                if (Int32.TryParse(Console.ReadLine(), out int x))
                {
                    return x;
                }

                Output.Rejection();
            }
        }

        public static void Expect(string message)
        {

        }
    }

}
