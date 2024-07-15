using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetController
{
    internal class JsonConversion
    {
        dynamic JsonToDynamic()
        {
            dynamic obj = new ExpandoObject();
            return obj;
        }
    }
}
