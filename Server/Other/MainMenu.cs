using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Server.MVC.Model.CSVModel;

namespace Server.Other
{
    //public class MainMenuCSVModel <T> where T : class, new()
    //{
    //    public FuncList functions {  get; private set; }
    //    CSVModel<T> model;

    //    public MainMenuCSVModel(string filePath)
    //    {
    //        try
    //        {
    //            model = new CSVModel<T>(filePath);
    //        }
    //        catch (Exception e)
    //        {
    //            throw e;
    //        }

    //        functions = new FuncList("Главное меню");

    //        functions.AddFunction("Вывести все записи", OutputAll);
    //        functions.AddFunction("Вывести запись по номеру", OutputByNumber);
    //        functions.AddFunction("Записать данные в файл", WriteData);
    //        functions.AddFunction("Удалить запись из файла", DeleteRecordFromFile);
    //        functions.AddFunction("Добавить запись в файл", AddAnEntry);
    //        functions.AddFunction("Выход", Exit);

    //        SelectFromTheMenu(functions);
    //    }

    //    // Вывести все записи
    //    private void OutputAll()
    //    {
    //        view.Table(model.GetValues());
    //        ExitToTheMenu();
    //    }


    //    // Вывести запись по номеру
    //    private void OutputByNumber()
    //    {
    //        List<T> records = model.GetValues();

    //        if (records.Count == 0)
    //        {
    //            view.Massage("Нет записей.");
    //            ExitToTheMenu();
    //            return;
    //        }

    //        while (true)
    //        {
    //            view.Table(records);
    //            int x = view.InputInt("Введите номер строки");

    //            if (x >= 0 && x <= records.Count - 1)
    //            {
    //                view.PrintObjectProperties(records[x]);
    //                break;
    //            }
    //            else
    //            {
    //                view.Error("Недопустимый номер записи или некорректный ввод. Попробуйте снова.");
    //            }

    //        };

    //        ExitToTheMenu();
    //    }

    //    //Перезапись данных в файл !!!!!!!!!!!!!
    //    private void WriteData()
    //    {
    //        List<T> records = new();
    //        PropertyInfo[] properties = typeof(T).GetProperties();
    //        List<string> menu = new List<string>() { "Число - заменить запись", "+ - добавить новую запись", "- - удалить запись", "save - сохранить записи", "cancel - отмена" };

    //        string menuInput;
    //        bool cycle = true;

    //        while (cycle)
    //        {
    //            view.Table(records);
    //            view.Menu(menu, "Меню редактирования файлов", false);
    //            menuInput = view.InputString();

    //            if (menuInput == "+")
    //            {
    //                T? record = CreateNewRecord();
    //                if (record != null)
    //                {
    //                    records.Add(record);
    //                }
    //            }
    //            else if (menuInput == "-")
    //            {
    //                records = DeleteRecord(records);
    //            }
    //            else if (menuInput == "save")
    //            {
    //                view.Massage("Вы точно хотите сохранить файл?");
    //                if (view.InputBool())
    //                {
    //                    model.OverwritingTable(records);
    //                    cycle = false;

    //                    break;
    //                }
    //            }
    //            else if (menuInput == "cancel")
    //            {
    //                cycle = false;
    //                break;
    //            }
    //            else if (Int32.TryParse(menuInput, out int x) && x >= 0 && x <= records.Count - 1)
    //            {
    //                T? record = CreateNewRecord(records[x]);
    //                if (record != null)
    //                {
    //                    records[x] = record;
    //                }
    //            }
    //        }
    //        ExitToTheMenu();
    //    }

    //    public void DeleteRecordFromFile()
    //    {
    //        List<T> records = model.GetValues();
    //        if (records.Count == 0)
    //        {
    //            view.Massage("Нет записей для удаления.");
    //            ExitToTheMenu();
    //            return;
    //        }

    //        while (true)
    //        {
    //            view.Table(records);
    //            int indexToDelete = -1;

    //            view.TextOutput("Нажмите на любую клавишу чтобы продолжить (ESC для отмены):");
    //            var key = Console.ReadKey();
    //            Console.Out.Flush();

    //            if (key.Key == ConsoleKey.Escape)
    //            {
    //                view.Massage("Отменено.");
    //                break;
    //            }

    //            indexToDelete = view.InputInt();
    //            if (indexToDelete >= 0 && indexToDelete < records.Count)
    //            {
    //                model.RemoveEntry(indexToDelete);
    //                view.Massage("Запись удалена.");
    //                break;
    //            }
    //            else
    //            {
    //                view.Error("Недопустимый номер записи.");
    //            }
    //        }
    //        ExitToTheMenu();
    //    }

    //    private List<T> DeleteRecord(List<T> records)
    //    {
    //        if (records.Count == 0)
    //        {
    //            view.Massage("Нет записей для удаления.");
    //            ExitToTheMenu();
    //            return records;
    //        }


    //        while (true)
    //        {
    //            view.Table(records);
    //            int indexToDelete = -1;

    //            view.TextOutput("Нажмите на любую клавишу чтобы продолжить (ESC для отмены):");
    //            var key = Console.ReadKey();
    //            Console.Out.Flush();

    //            if (key.Key == ConsoleKey.Escape)
    //            {
    //                view.Massage("Отменено.");
    //                return records;
    //            }

    //            indexToDelete = view.InputInt();
    //            if (indexToDelete >= 0 && indexToDelete < records.Count)
    //            {
    //                records.RemoveAt(indexToDelete);
    //                view.Massage("Запись удалена.");
    //                break;
    //            }
    //            else
    //            {
    //                view.Error("Недопустимый номер записи.");
    //            }

    //        }
    //        return records;
    //    }

    //    private void AddAnEntry()
    //    {
    //        T? record = CreateNewRecord();
    //        if (!EqualityComparer<T>.Default.Equals(record, default(T))) model.AddValues(new List<T>() { record });
    //        ExitToTheMenu();
    //    }

    //    private bool IsObjectFullyInitialized<T>(T obj)
    //    {
    //        Type type = typeof(T);
    //        PropertyInfo[] properties = type.GetProperties();

    //        foreach (var property in properties)
    //        {
    //            if (property.IsDefined(typeof(RequiredAttribute), false))
    //            {
    //                object? value = property.GetValue(obj);
    //                if (value == null || (value is string strValue && string.IsNullOrWhiteSpace(strValue)))
    //                {
    //                    return false;
    //                }
    //            }
    //        }

    //        return true;
    //    }

    //    private void ExitToTheMenu()
    //    {
    //        view.Massage("Нажмите на любую клавишу чтобы выйти в меню");
    //        Console.ReadKey();
    //        SelectFromTheMenu(functions);
    //    }

    //    private void Exit()
    //    {
    //        view.Massage("Нажмите на любую клавишу чтобы выйти");
    //        Console.ReadKey();
    //        Environment.Exit(0);
    //    }


    //    private void SelectFromTheMenu(FunctionList functionList)
    //    {
    //        while (true)
    //        {
    //            view.Menu(functions.GetFunNames());
    //            int input = view.InputInt("Введите номер операции");

    //            try
    //            {
    //                functionList.ExecuteFunction(input);
    //            }
    //            catch (Exception e)
    //            {
    //                view.Error(e.Message);
    //            }

    //        }
    //    }

    //    private T? CreateNewRecord(T? newRecord = default)
    //    {
    //        if (EqualityComparer<T>.Default.Equals(newRecord, default(T)))
    //        {
    //            newRecord = Activator.CreateInstance<T>();
    //        }

    //        PropertyInfo[] properties = typeof(T).GetProperties();

    //        //Dictionary<Type, Delegate> inputMethods = new()
    //        //{
    //        //    { typeof(string), new Input<string>(view.InputString) },
    //        //    { typeof(int), new Input<int>(view.InputInt) },
    //        //    { typeof(bool), new Input<bool>(view.InputBool) }
    //        //};

    //        List<string> menu = new List<string>(properties.Select(p => p.Name));

    //        menu.Add("Сохранить изменения");
    //        menu.Add("Отмена");

    //        bool cycle = true;

    //        while (cycle)
    //        {
    //            view.PrintObjectProperties(newRecord);

    //            view.Menu(menu, "Что вы хотите радактировать?", false);

    //            int input = view.InputInt();

    //            if (input >= 0 && input <= menu.Count - 3)
    //            {
    //                PropertyInfo property = properties[input];

    //                var value = Validator.ConvertToType(property.PropertyType, view.InputString($"Введите {property.Name}"));

    //                try
    //                {
    //                    property.SetValue(newRecord, value);
    //                }
    //                catch (Exception ex) { view.Error(ex.Message); }
    //            }
    //            else if (input == menu.Count - 2)
    //            {
    //                if (IsObjectFullyInitialized(newRecord))
    //                {
    //                    view.Massage("Сохранено");
    //                    cycle = false;
    //                    return newRecord;
    //                }
    //                else
    //                {
    //                    view.Error("Введены не все данные");
    //                }
    //            }
    //            else if (input == menu.Count - 1)
    //            {
    //                view.Massage("Отмена...");
    //                return default;
    //            }
    //            else
    //            {
    //                view.Error("Некорректный ввод, попробуйте снова");
    //            }
    //        }

    //        return newRecord;
    //    }
    //}
}
