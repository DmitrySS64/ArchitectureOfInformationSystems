using ArchitectureOfInformationSystems.MVC.Model;
using ArchitectureOfInformationSystems.MVC.Model.Entity;
using ArchitectureOfInformationSystems.MVC.View;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArchitectureOfInformationSystems.MVC.Core
{
    public class Main
    {
        string filePath = @"D:\VS\ArchitectureOfInformationSystems\Data\data.CSV";
        
        public Main() {
            MenuHander<Student> menuHander = new(filePath);
        }
    }

    public class MenuHander<T>
    {
        private FileManagement<T> fileManagement;
        private View.View view;
        private FunctionList functions;

        public MenuHander(string filePath)
        {
            fileManagement = new FileManagement<T>(filePath);
            view = new View.View();
            functions= new FunctionList();

            functions.AddFunction("Главное меню", MainMenu);//1
            functions.AddFunction("Вывести все записи", OutputAll);
            functions.AddFunction("Вывести запись по номеру", OutputByNumber);
            functions.AddFunction("Записать данные в файл", WriteData);
            functions.AddFunction("Удалить запись из файла", DeleteRecordFromFile);
            functions.AddFunction("Добавить запись в файл", AddAnEntry);
            functions.AddFunction("Выход", Exit);

            functions.ExecuteFunction(0);
        }

        #region main menu
        //Главное меню
        private void MainMenu()
        {
            view.Menu(functions.GetFunNames());
            SelectFromTheMenu(functions);
        }

        //Вывести все записи
        private void OutputAll()
        {
            view.Table(fileManagement.ReadAllRecords());
            ExitToTheMenu();
        }

        //Вывести запись по номеру
        private void OutputByNumber()
        {
            List<T> records = fileManagement.ReadAllRecords();

            if (records.Count == 0)
            {
                view.Massage("Нет записей.");
                ExitToTheMenu();
                return;
            }
            else view.Table(records);


            int x;
            bool inputValid = false;

            do
            {
                view.Massage("Введите номер строки (нажмите ESC для отмены)");
                var key = Console.ReadKey();
                Console.Out.Flush();

                if (key.Key == ConsoleKey.Escape)
                {
                    view.Massage("Выход из операции.");
                    break;
                }

                string input = key.KeyChar.ToString();

                if (Int32.TryParse(input, out x) && x >= 0 && x <= records.Count - 1)
                {
                    inputValid = true;
                    T record = records[x];
                    view.PrintObjectProperties(record);
                }
                else
                {
                    view.Error("Недопустимый номер записи или некорректный ввод. Попробуйте снова.");
                }

            } while (!inputValid);

            ExitToTheMenu();
        }

        //Записать данные в файл
        private void WriteData()
        {
            view.Massage("Введите ");
            ExitToTheMenu();
        }

        //Удалить запись из файла
        public void DeleteRecordFromFile()
        {
            List<T> records = fileManagement.ReadAllRecords();
            if (records.Count == 0)
            {
                view.Massage("Нет записей для удаления.");
                ExitToTheMenu();
                return;
            }

            bool cycle = true;

            while (cycle)
            {
                view.Table(records);
                int indexToDelete = -1;

                view.TextOutput("Введите номер записи для удаления (нажмите ESC для отмены):");
                var key = Console.ReadKey();
                Console.Out.Flush();

                if (key.Key == ConsoleKey.Escape)
                {
                    view.Massage("Отменено.");
                    ExitToTheMenu();
                    return;
                }

                if (int.TryParse(key.KeyChar.ToString(), out indexToDelete) && indexToDelete >= 0 && indexToDelete < records.Count)
                {
                    records.RemoveAt(indexToDelete);
                    view.Massage("Запись удалена.");
                }
                else
                {
                    view.Error("Недопустимый номер записи.");
                }

            }
            fileManagement.OverwriteTheFile(records);
            ExitToTheMenu();
        }


        //Добавить запись в файл
        private void AddAnEntry()
        {
            InputParameter inputParameter = new InputParameter(view);
            T record = Activator.CreateInstance<T>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            List<string> menu = new List<string>();

            foreach (var property in properties)
            {
                menu.Add(property.Name);
            }

            menu.Add("Сохранить изменения");

            Dictionary<Type, Action<PropertyInfo>> inputMethods = new Dictionary<Type, Action<PropertyInfo>>
            {
                { typeof(string), (property) =>
                {
                    try
                    {
                        property.SetValue(record,  inputParameter.String($"Введите {property.Name}"));
                    }
                    catch (Exception e) { view.Error(e.Message); }
                } },
                { typeof(int), (property) =>{
                    try
                    {
                        property.SetValue(record, inputParameter.Int($"Введите {property.Name}"));
                    }
                    catch (Exception e) { view.Error(e.Message); }
                } },
                { typeof(bool), (property) => property.SetValue(record, inputParameter.Bool())}

            };

            bool cycle = true;

            while (cycle)
            {
                view.PrintObjectProperties(record);

                view.TextOutput("Что вы хотите радактировать?");
                view.Menu(menu, false);

                int input = inputParameter.Int();

                if (input >= 0 && input < properties.Length)
                {
                    PropertyInfo property = properties[input];

                    if (inputMethods.ContainsKey(property.PropertyType))
                    {
                        inputMethods[property.PropertyType](property);
                    }
                    else
                    {
                        view.Error("Тип данных не поддерживается.");
                    }
                }
                else if (input == properties.Length)
                {
                    if (IsObjectFullyInitialized(record))
                    {
                        cycle = false;
                    }
                    else
                    {
                        view.Error("Введены не все данные");
                    }
                }
                else
                {
                    view.Error("Некорректный ввод, попробуйте снова");
                }
            }
            fileManagement.AddRecordToFile(record);
            ExitToTheMenu();
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private bool IsObjectFullyInitialized<T>(T obj)
        {
            Type type = typeof(T);
            PropertyInfo nameProperty = type.GetProperty("Name");
            PropertyInfo lastNameProperty = type.GetProperty("LastName");

            if (nameProperty != null && lastNameProperty != null)
            {
                string name = (string)nameProperty.GetValue(obj);
                string lastName = (string)lastNameProperty.GetValue(obj);

                return !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(lastName);
            }

            return false;
        }


        private void ExitToTheMenu()
        {
            view.Massage("Нажмите на любую клавишу чтобы выйти в меню");
            Console.ReadKey();
            functions.ExecuteFunction(0);
        }

        //Выход
        private void Exit()
        {
            view.Massage("Нажмите на любую клавишу чтобы выйти");
            Console.ReadKey();
            Environment.Exit(0);
        }
        #endregion main menu

        private void SelectFromTheMenu(FunctionList functionList)
        {
            int x;
            bool inputValid = false;

            do
            {
                view.Massage("Введите номер операции (нажмите ESC для отмены)");
                var key = Console.ReadKey();
                Console.Out.Flush();

                if (key.Key == ConsoleKey.Escape)
                {
                    Exit();
                    break;
                }

                string input = key.KeyChar.ToString();

                if (Int32.TryParse(input, out x))
                {
                    try
                    {
                        inputValid = true;
                        functionList.ExecuteFunction(x);
                    }
                    catch (Exception e)
                    {
                        view.Error($"Ошибка: {e.Message}");
                    }
                }
                else
                {
                    view.Error("Некорректный ввод, попробуйте снова");
                    SelectFromTheMenu(functionList);
                }

            } while (!inputValid);
        }
    }

    public class InputParameter
    {
        private View.View view;

        public InputParameter (View.View view)
        {
            this.view = view;
        }

        public string String(string massage = "Введите текст")
        {
            string? input;

            while (true)
            {
                view.Massage(massage);
                input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input)) return input;
                view.Error("Некорректный ввод, попробуйте снова");
            }
        }

        public bool Bool(string massage = "0 - False, 1 - True")
        {
            while (true)
            {
                view.Massage(massage);

                if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int x) && (x == 0 || x == 1))
                {
                    return x == 1;
                }

                view.Error("Некорректный ввод, попробуйте снова");
            }
        }

        public int Int(string massage = "Введите число")
        {
            while (true)
            {
                view.Massage(massage);

                if (Int32.TryParse(Console.ReadLine(), out int x))
                {
                    return x;
                }

                view.Error("Некорректный ввод, попробуйте снова");
            }
        }
        //0..9
        public int Digit(string massage = "Введите цифру")
        {
            while (true)
            {
                view.Massage(massage);

                if (Int32.TryParse(Console.ReadKey().KeyChar.ToString(), out int x))
                {
                    return x;
                }

                view.Error("Некорректный ввод, попробуйте снова");
            }
        }
    }

    #region Function List
    public class FunctionInfo
    {
        public string Name { get; set; }
        public FunctionDelegate Function { get; set; }

        public delegate void FunctionDelegate();

        public FunctionInfo(string name, FunctionDelegate function)
        {
            Name = name;
            Function = function;
        }
    }

    public class FunctionList
    {
        private List<FunctionInfo> Functions = new List<FunctionInfo>();

        public void AddFunction(string functionName, FunctionInfo.FunctionDelegate function)
        {
            Functions.Add(new FunctionInfo(functionName, function));
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
            List<string> names = new List<string>();
            foreach (FunctionInfo f in Functions) {
                names.Add(f.Name);
            }
            return names;
        }
    }
    #endregion Function List
}
