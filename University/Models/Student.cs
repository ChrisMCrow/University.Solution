using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
    public class Student
    {
      public int Id {get; set;}
      public string Name {get; set;}
      public DateTime EnrollmentDate {get; set}

      public Student(string newName, DateTime enrollmentDate, int id = 0)
      {
        Name = newName;
        EnrollmentDate = enrollmentDate;
        Id = id;
      }

      public static List<Student> GetAllStudents()
      {
        List<Student> allStudents = new List<Student>{};
        MySqlConnection conn = DB.CreateCommand();
        conn.Open();

        MySqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = @"SELECT * FROM students;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int id = rdr.GetInt32(0);
          string name = rdr.GetString(1);
          DateTime enrollmentDate = rdr.GetDateTime(2);
        }
      }
    }
}
