using ArchitectureOfInformationSystems.MVC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using ArchitectureOfInformationSystems.MVC.Model.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    public static class ModelMenu
    {
        public static Main<Student> StudentMenu = new Main<Student>();
    }



    public class MethodData
    {
        public string Method { get; set; }
        public List<string> DataTypes { get; set; }
        public List<string> SentData { get; set; }

        public MethodData(string method, List<string> dataTypes, List<string> sentData)
        {
            Method = method;
            DataTypes = dataTypes;
            SentData = sentData;
        }

        public string ExecuteMethod()
        {
            string? result;
            //получить метод
            var methodInfo = typeof(Main<Student>).GetMethod(Method);

            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();
                var methodParameters = new List<object>();

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    var parameterValue = Convert.ChangeType(SentData[i], parameterType);
                    methodParameters.Add(parameterValue);
                }

                result = (string)methodInfo.Invoke(null, methodParameters.ToArray());
            }
            else
            {
                result = "Ошибка";
            }

            return result;
        }
    }

    public static class Parser
    {
        public static string ConvertStringToFunction(string str)
        {
            string result = "";

            string[] parts = str.Split('?');

            if (parts.Length == 2) {
                string methodName = parts[0];
                string[] args = parts[1].Split('|');
                string[] types = args[0].Split(',');
                string[] data = args[1].Split(',');

                MethodData method = new MethodData(methodName, types.ToList(), data.ToList());
                result = method.ExecuteMethod();
            }

            return result;
        }

    }
}
