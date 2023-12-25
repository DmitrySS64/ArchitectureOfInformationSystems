using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchitectureOfInformationSystems.MVC.Model.CSVModel;
using ArchitectureOfInformationSystems.MVC.Model.Entity;

namespace Server
{
    class CommandRepository
    {
        //Вывод всех записей на экран
        //Вывод записи по номеру
        //Запись данных в файл
        //Удаление записи(записей) из файла
        //Добавление записи в файл

        CSVModel<Student> model;
        string FilePath = Directory.GetCurrentDirectory() + "data.csv";

        public CommandRepository() {
            try
            { 
                model = new CSVModel<Student>(FilePath);
            }
            catch { }
        }
        public string DisplayingAllEntries()
        {
            string jsonArr = System.Text.Json.JsonSerializer.Serialize(model.GetValues());
            return jsonArr;
        }

    }
}
