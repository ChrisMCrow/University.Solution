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
        public DateTime EnrollmentDate {get; set;}

        public Student(string newName, DateTime enrollmentDate, int id = 0)
        {
            Name = newName;
            EnrollmentDate = enrollmentDate;
            Id = id;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if(!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = (this.Id == newStudent.Id);
                bool nameEquality = (this.Name == newStudent.Name);
                bool enrollmentDateEquality = (this.EnrollmentDate == newStudent.EnrollmentDate);
                return (nameEquality && enrollmentDateEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public static List<Student> GetAllStudents()
        {
            List<Student> allStudents = new List<Student>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM students;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                Student newStudent = new Student(name, enrollmentDate, id);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO students (name,enrollment_date) VALUES (@newName, @newDate);";

            cmd.Parameters.AddWithValue("@newName", this.Name);
            cmd.Parameters.AddWithValue("@newDate", this.EnrollmentDate);

            cmd.ExecuteNonQuery();
            Id= (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Student Find(int thisId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "";
            DateTime enrollmentDate = new DateTime(2001-01-01);
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                name = rdr.GetString(1);
                enrollmentDate = rdr.GetDateTime(2);
            }
            Student newStudent = new Student(name, enrollmentDate, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newStudent;
        }

        public void Edit(string newName, DateTime newDate)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE students SET name = @newName, enrollment_date = @newDate WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@newDate", newDate);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.Name = newName;
            this.EnrollmentDate = newDate;
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM students WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void AddCourse(int courseId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";

            cmd.Parameters.AddWithValue("@studentId", this.Id);
            cmd.Parameters.AddWithValue("@courseId", courseId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Course> GetCourses()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT courses.* FROM students   JOIN students_courses ON (students.id = students_courses.student_id)
            JOIN courses ON (students_courses.course_id = courses.id)
            WHERE students.id = @studentId;";

            cmd.Parameters.AddWithValue("@studentId" , this.Id);

            List<Course> allCourses = new List<Course>{};
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string courseName = rdr.GetString(2);
                Course newCourse = new Course(name, courseName,id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }
        public override string ToString()
        {
            return String.Format("{{id = {0}, name = {1}, date={2}}}", Id, Name, EnrollmentDate);
        }
    }
}
