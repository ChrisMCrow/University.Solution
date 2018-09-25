using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using University;

namespace University.Models
{
    public class Course
    {
        public int Id {get;set;}
        public string Name {get; set;}
        public string CourseNumber {get; set;}

        public Course(string newName, string courseNumber, int id =0)
        {
            Name =newName;
            CourseNumber = courseNumber;
            Id = id;
        }
        public override bool Equals(System.Object otherCourse)
        {
            if(!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = (this.Id == newCourse.Id);
                bool nameEquality = (this.Name == newCourse.Name);
                bool courseNumberEquality = (this.CourseNumber == newCourse.CourseNumber);
                return (nameEquality && courseNumberEquality && idEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public static List<Course> GetAllCourses()
        {
            List<Course> allCourses = new List<Course>{};
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM courses;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string courseNumber = rdr.GetString(2);
                Course newCourse = new Course(name,courseNumber,id);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO courses (name,course_number) VALUES (@newName, @newNumber);";

            cmd.Parameters.AddWithValue("@newName", this.Name);
            cmd.Parameters.AddWithValue("@newNumber", this.CourseNumber);

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
            cmd.CommandText = @"DELETE FROM courses;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Course Find(int thisId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE id = @id;";

            cmd.Parameters.AddWithValue("@id", thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string name = "";
            string courseNumber = "";
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                name = rdr.GetString(1);
                courseNumber = rdr.GetString(2);
            }
            Course newCourse = new Course(name, courseNumber, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCourse;
        }

        public void Edit(string newName, string newNumber)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE courses SET name = @newName, course_number = @newNumber WHERE id = @searchId;";

            cmd.Parameters.AddWithValue("@newName", newName);
            cmd.Parameters.AddWithValue("@newNumber", newNumber);
            cmd.Parameters.AddWithValue("@searchId", this.Id);

            this.Name = newName;
            this.CourseNumber = newNumber;
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
            cmd.CommandText = @"DELETE FROM courses WHERE id = @searchid;";

            cmd.Parameters.AddWithValue("@searchid", this.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddStudent(int studentId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@studentId, @courseId);";

            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@courseId", this.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT students.* FROM courses
            JOIN students_courses ON (courses.id = students_courses.course_id)
            JOIN students ON (students_courses.student_id = students.id)
            WHERE courses.id = @courseId;";

            cmd.Parameters.AddWithValue("@courseId", this.Id);

            List<Student> allStudents = new List<Student> {};
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                DateTime enrollmentDate = rdr.GetDateTime(2);
                allStudents.Add(new Student(name, enrollmentDate, id));
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        
    }
}
