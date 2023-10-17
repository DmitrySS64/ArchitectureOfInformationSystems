using ArchitectureOfInformationSystems.MVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.View
{
    public class View
    {
        public View() {}

        public void Menu(List<string> menu, bool clearView = true) {
            if (clearView) Clear();
            Console.WriteLine("Это меню");
            int i = 0;
            foreach (var item in menu) {             
                Console.Write(i.ToString() + ") ");
                Console.WriteLine(item.ToString());
                i++;
            }
        }

        public void Table<T>(List<T> records)
        {
            Clear();

            if (records.Count == 0) 
            {
                TextOutput("Нет записей.");
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



        public void Error(string s) 
        {
            Console.Write($"\n!!!\t{s}\t!!!\n");
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Massage(string str)
        {
            Console.WriteLine($">>\t{str}");
        }

        public void TextOutput(string str) {
            Console.WriteLine(str);
        }

        public void PrintObjectProperties(object obj, bool clearView = true)
        {
            if (clearView) Clear();

            if (obj == null)
            {
                TextOutput("Объект пуст");
                return;
            }

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Console.WriteLine($"Object of type {type.Name}:");

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                Console.WriteLine($"{property.Name}:\t{value}");
            }
        }
    }
}
