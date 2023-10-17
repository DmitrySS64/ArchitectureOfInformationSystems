using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchitectureOfInformationSystems.MVC.Model.Entity
{
    public class Student
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        private int _age { get; set; }
        public int Age {
            get
            {
                return _age;
            }
            set
            {
                if (value < 0 || value > 120) {
                    _age = 0;
                    throw new Exception("Некорректный ввод возраста");
                }
                else
                {
                    _age = value;
                }
            }
        }
        public bool IsStudent { get; set; }

    }
}
