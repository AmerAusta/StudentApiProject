using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentDataAccessLayer
{
    public class StudentDTO
    {
        public StudentDTO(int Id,string Name,int Age, int Grade) 
        {
            this.Id = Id;
            this.Name = Name;
            this.Age = Age;
            this.Grade = Grade;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int  Age { get; set; }
        public int Grade {  get; set; }
    }

    public class StudentData
    {
        static string _connectionString = "Server=localhost;Database=StudentsDB;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<StudentDTO> GetAllStudent()
        {
            var StudentList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentList.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
                return StudentList;
            }
        }

        public static List<StudentDTO> GetPassStudent()
        {
            var ListStudent=new List<StudentDTO>();

            using(SqlConnection conn= new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListStudent.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                ));
                        }
                    }
                }
                return ListStudent;
            }
        }

        public static double GetAverageGrade()
        {
            double Average = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    object Resulte=cmd.ExecuteScalar();
                    if (Resulte != DBNull.Value)
                    {
                        Average = Convert.ToDouble(Resulte);
                    }
                    else
                        Average = 0;
                }
                
            }
            return Average;
        }

        public static StudentDTO GetStudentByID(int ID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", ID);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))
                                );

                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public static int AddNewStudent(StudentDTO studentDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("SP_AddStudent", conn))
                {
                    cmd.CommandType= CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", studentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", studentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", studentDTO.Grade);
                    var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outputIdParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    return (int)outputIdParam.Value;
                }
            }
        }

        public static bool UpdateStudent(StudentDTO studentDTO)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_UpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentId", studentDTO.Id);
                    cmd.Parameters.AddWithValue("@Name", studentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", studentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", studentDTO.Grade);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public static bool DeleteStudent(int StudentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DeleteStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", StudentId);
                    conn.Open();
                    int rowAffected = (int)cmd.ExecuteScalar();
                    return rowAffected == 1;
                }
            }
        }
    }
}
