﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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


        public Student()
        {
            Name = string.Empty;
            LastName = string.Empty;
            Age = 0;
            IsStudent = false;
        }

        public Student(string name, string lastName, int age, bool isStudent)
        {
            Name = name;
            LastName = lastName;
            Age = age;
            IsStudent = isStudent;
        }
    }
}
