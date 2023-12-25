using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Other
{
    public class FunctionInfo
    {
        public string Name { get; private set; }
        public FunctionDelegate Function { get; private set; }
        public Type[] ParameterTypes { get; private set; }

        public delegate void FunctionDelegate(params object[] parameters);

        public FunctionInfo(string name, FunctionDelegate function, params Type[] parameterTypes)
        {
            Name = name;
            Function = function;
            ParameterTypes = parameterTypes;
        }
    }

    public class FuncList
    {
        public string Name { get; private set; }
        private List<FunctionInfo> Functions;

        public FuncList(string name)
        {
            Name = name;
            Functions = new List<FunctionInfo>();
        }

        public void AddFunction(string functionName, FunctionInfo.FunctionDelegate function, params Type[] parameterTypes)
        {
            Functions.Add(new FunctionInfo(functionName, function, parameterTypes));
        }

        public void RemoveFunction(string functionName)
        {
            Functions.RemoveAll(x => x.Name == functionName);
        }

        public void RemoveFunction(int functionIndex)
        {
            Functions.RemoveAt(functionIndex);
        }

        public void ExecuteFunction(int index, params object[] parameters)
        {
            if (index >= 0 && index < Functions.Count)
            {
                var functionInfo = Functions[index];
                ValidateParameters(index, parameters);
                Functions[index].Function.Invoke();
            }
            else { throw new Exception("Недопустимый номер функции."); }
        }

        private void ValidateParameters(int index, object[] parameters)
        {
            var functionInfo = Functions[index];

            if (parameters.Length != functionInfo.ParameterTypes.Length)
            {
                throw new Exception("Неверное количество параметров для функции.");
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] != null && parameters[i].GetType() != functionInfo.ParameterTypes[i])
                {
                    throw new Exception($"Неверный тип параметра {i} для функции.");
                }
            }
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
}
