using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ArchitectureOfInformationSystems.MVC.Model.Entity;
using System.Reflection;
using ArchitectureOfInformationSystems.MVC.View;

namespace ArchitectureOfInformationSystems.MVC.Model
{
    internal class FileManagement<T>
    {
        private string FileName;
        //Path.Combine(Environment.CurrentDirectory, @"Data\data.CSV");
        public FileManagement(string filePath)
        {
            FileName = filePath;
        }
    
        //Вывод всех записей на экран
        public List<T> ReadAllRecords()
        {
            List<T> records = new();

            using (StreamReader reader = new StreamReader(FileName))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');

                    if (parts.Length == typeof(T).GetProperties().Length)
                    {
                        T record = Activator.CreateInstance<T>();

                        for (int i = 0; i < parts.Length; i++)
                        {
                            PropertyInfo property = typeof(T).GetProperties()[i];
                            Type type = property.PropertyType;

                            if (type == typeof(string))
                            {
                                property.SetValue(record, parts[i]);
                            }
                            else 
                            {
                                object parsedValue = Convert.ChangeType(parts[i], type);

                                if (parsedValue != null)
                                {
                                    property.SetValue(record, parsedValue);
                                }
                            }
                        }

                        records.Add(record);
                    }
                }
            }

            return records;
        }


        //Запись списка данных в файл
        public void WriteRecordsToFile(List<T> records)
        {
            using (StreamWriter writer = new StreamWriter(FileName, true, System.Text.Encoding.Default))
            {
                foreach (T record in records)
                {
                    string line = GetRecordString(record);
                    writer.WriteLine(line);
                }
            }
        }

        private string GetRecordString(T record)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> values = new List<string>();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(record);
                string stringValue = value != null ? value.ToString() : string.Empty;
                values.Add(stringValue);
            }

            return string.Join(";", values);
        }

        //Перезаписать файл
        public void OverwriteTheFile(List<T> records)
        {
            using (StreamWriter writer = new StreamWriter(FileName, false, System.Text.Encoding.Default))
            {
                foreach (T record in records)
                {
                    string line = GetRecordString(record);
                    writer.WriteLine(line);
                }
            }
        }


        //Добавление записи в файл
        public void AddRecordToFile(T record)
        {
            using (StreamWriter writer = new StreamWriter(FileName, true, System.Text.Encoding.Default))//дозапись
            {
                Type type = typeof(T);
                PropertyInfo[] properties = type.GetProperties();

                string line = string.Join(";", properties.Select(property => property.GetValue(record)));

                writer.WriteLine(line);
            }
        }
    }
}
