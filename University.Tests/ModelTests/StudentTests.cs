using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace University.Tests
{
    [TestClass]
    public class StudentTests : IDisposable
    {
        public void Dispose()
        {
            Student.DeleteAll();
        }
        public StudentTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
        }

        [TestMethod]
        public void GetAllStudents_ReturnsAllStudents_StudentList()
        {
            Student newStudent = new Student("Chris",Convert.ToDateTime("2018-01-01"));
            newStudent.Save();

            List<Student> allStudents = Student.GetAllStudents();

            Assert.AreEqual(newStudent,allStudents[0]);
        }

        [TestMethod]
        public void Find_ReturnsStudentById_StudentObject()
        {
            Student newStudent = new Student("Meria", Convert.ToDateTime("2018-09-01"));
            newStudent.Save();

            Student result = Student.Find(newStudent.Id);

            Assert.AreEqual(newStudent, result);
        }

        [TestMethod]
        public void Edit_UpdatesStudentById_StudentObject()
        {
            Student newStudent = new Student("Meria", Convert.ToDateTime("2018-09-01"));
            newStudent.Save();

            Student updatedStudent = new Student ("Chris", Convert.ToDateTime("2017-09-01"), newStudent.Id);

            newStudent.Edit(updatedStudent.Name, updatedStudent.EnrollmentDate);

            Assert.AreEqual(newStudent, updatedStudent);
        }
        [TestMethod]
        public void Delete_DeleteStudent()
        {
            Student newStudent = new Student("Meria", Convert.ToDateTime("2018-09-01"));
            newStudent.Save();

            newStudent.Delete();
            int count = Student.GetAllStudents().Count;

            Assert.AreEqual(0,count);
        }
    }
}
