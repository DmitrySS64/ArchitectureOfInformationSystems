using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Other
{
    interface IMenuItem
    {
        string Name { get; }
    }

    //public class MenuItem_Func : IMenuItem
    //{
    //    public string Name { get; private set; }
    //    public Action Action { get; private set; }

    //}

    //public class MenuItem_Menu : IMenuItem
    //{
    //    public string Name { get; private set; }
    //    public List<IMenuItem> Items { get; private set; }

    //    public MenuItem(string name, Action action)
    //    {
    //        Name = name;
    //        Action = action;
    //    }

    //    public MenuItem(string name, MenuManager subMenu)
    //    {
    //        Name = name;
    //        SubMenu = subMenu;
    //    }

    //    public void Execute()
    //    {
    //        if (Action != null)
    //        {
    //            Action.Invoke();
    //        }
    //        else if (SubMenu != null)
    //        {
    //            SubMenu.Show();
    //        }
    //    }
    //}
    ////-
    //public class MenuManager
    //{
    //    private List<MenuItem> items;

    //    public MenuManager()
    //    {
    //        items = new List<MenuItem>();
    //    }

    //    public void AddItem(string name, Action action)
    //    {
    //        items.Add(new MenuItem(name, action));
    //    }

    //    public void AddSubMenu(string name, MenuManager subMenu)
    //    {
    //        items.Add(new MenuItem(name, subMenu));
    //    }

    //    public void Show()
    //    {
    //        while (true)
    //        {
    //            Console.WriteLine("Меню:");
    //            for (int i = 0; i < items.Count; i++)
    //            {
    //                Console.WriteLine($"{i + 1}. {items[i].Name}");
    //            }

    //            Console.WriteLine("0. Выйти");

    //            int choice = GetUserChoice(items.Count);

    //            if (choice == 0)
    //            {
    //                break;
    //            }

    //            items[choice - 1].Execute();
    //        }
    //    }

    //    private int GetUserChoice(int maxChoice)
    //    {
    //        int choice;
    //        while (true)
    //        {
    //            Console.Write("Введите номер: ");
    //            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 0 && choice <= maxChoice)
    //            {
    //                break;
    //            }
    //            else
    //            {
    //                Console.WriteLine("Некорректный ввод. Попробуйте снова.");
    //            }
    //        }
    //        return choice;
    //    }
    //}
}
