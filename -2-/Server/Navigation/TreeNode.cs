using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Navigation
{
    public class TreeNode
    {
        public string Name { get; set; }
        public Dictionary<string, TreeNode> SubNodes { get; } = new Dictionary<string, TreeNode>();
        public Func<string> Action { get; set; }

        public bool HasSubNodes => SubNodes.Count > 0;

        public string Execute()
        {
            if (HasSubNodes)
            {
                string result = DisplayMenu();
                if (SubNodes.TryGetValue(result, out var selectedNode))
                {
                    // Если выбран узел поддерева, рекурсивно выполняем его
                    return selectedNode.Execute();
                }
                else
                {
                    // Если выбрано действие, выполняем его и возвращаем результат
                    return Action != null ? ExecuteAction() : "Действие не определено.";
                }
            }
            else
            {
                return Action != null ? ExecuteAction() : "Действие не определено.";
            }
        }

        private string DisplayMenu()
        {
            string menu = "";
            menu += $"{Name}:";
            foreach (var key in SubNodes.Keys)
            {
                menu += $"{key}|";
            }
            return menu;
        }

        private string ExecuteAction()
        {
            string result = "результат";
            if (Action != null)
            {
                Action();
            }
            return result;

        }
    }
}
