
using Server.MVC.Model.CSVModel;
using Server.MVC.Model.Entity;

namespace Server.MVC.Model
{
    public interface IModel<T> where T : class, new()
    {
        public List<T> GetValues();

        public void AddEntry(IEnumerable<string> entryFields, bool NeedSave = true);
        public void EditEntry(int key, IEnumerable<string> entryFields);
        public void RemoveEntry(int key);
    }

    public static class ModelRegistry
    {
        private static Dictionary<string, Type> availableModels = new()
        {
            {"StudentModel", typeof(CSVModel<Student>)}
        };

        public static List<string> GetModelNames()
        {
            List<string> names = new List<string>();
            foreach (var model in availableModels)
            {
                names.Add(model.Key);
            }
            return names;
        }

        public static Type GetModelType(string modelName)
        {
            if (availableModels.ContainsKey(modelName))
            {
                return availableModels[modelName];
            }
            return null;
        }
    }
}
