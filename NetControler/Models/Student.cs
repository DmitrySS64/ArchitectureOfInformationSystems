namespace NetController.Models
{
    class Student
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
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
