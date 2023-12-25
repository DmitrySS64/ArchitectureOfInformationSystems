using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetController
{
    public class TransmittedData
    {
        public int errorСode = 0;
        public string message = "";
        public string[] data;

        public TransmittedData() { }

        public TransmittedData(List<string> data)
        {
            this.data = data.ToArray();
        }

        public TransmittedData(string[] data)
        {
            this.data = data;
        }

        public TransmittedData(string data)
        {
            this.data = new string[1];
            this.data[0] = data;
        }

        public TransmittedData(string error, int error_code)
        {
            errorСode = error_code;
            message = error;
        }
    }

    class Controller
    {

    }
}
