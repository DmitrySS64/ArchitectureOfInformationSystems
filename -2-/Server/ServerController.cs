using ArchitectureOfInformationSystems.MVC.Core;
using ArchitectureOfInformationSystems.MVC.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerController
    {
        private Main<Student> StudentMenu;

        public ServerController()
        {
            StudentMenu = new Main<Student>();

        }

        public string OutputAllData()
        {
            StudentMenu.OutputAllValuesInString();
            return "";
        }

        public string OutputByNumber() {
            StudentMenu.OutputByNumberInString(1);
            return "";
        }
    }
}
