using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model
{
    public static class Validator
    {
        public static object ConvertToType(Type type, string str)
        {
            if (type == typeof(int))
            {
                if (int.TryParse(str, out var value))
                    return value;
                else throw new FormatException();
            }
            else if (type == typeof(bool))
            {
                if (double.TryParse(str, out var value))
                    return value;
                else throw new FormatException();
            }
            else if (type == typeof(double))
            {
                if ((double.TryParse(str, out var value)))
                    return value;
                else throw new FormatException();
            }
            else return str;
        }
    }
}
