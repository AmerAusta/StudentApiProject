using System.Data;
using StudentDataAccessLayer;

namespace StudentBusinessLayer
{
    public class Student
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public StudentDTO SDTO
        {
            get { return new StudentDTO(this.ID, this.Name, this.Age, this.Grade); }
        }

        public Student(StudentDTO SDTO, enMode cMode = enMode.AddNew)
        {
            this.ID = SDTO.Id;
            this.Name = SDTO.Name;
            this.Age = SDTO.Age;
            this.Grade = SDTO.Grade;

            Mode = cMode;
        }

        public static List<StudentDTO> GetAllStuddent()
        {
            return StudentData.GetAllStudent();
        }

        public static List<StudentDTO> GetPassStudent()
        {
            return StudentData.GetPassStudent();
        }

        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public static Student Find(int ID)
        {
            StudentDTO SDTO = StudentData.GetStudentByID(ID);

            if (SDTO != null)
            {
                return new Student(SDTO, enMode.Update);
            }
            else
                return null;
        }

        private bool _AddNewStudent()
        {
            this.ID = StudentData.AddNewStudent(SDTO);
            return (this.ID != -1);
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }

        public bool Save()
        {
            switch (Mode)

            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateStudent();
            }
            return false;
        }

        public static bool DeleteStudent(int StudentID)
        {
            return StudentData.DeleteStudent(StudentID);
        }
    }
}
