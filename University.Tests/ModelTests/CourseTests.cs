using Microsoft.VisualStudio.TestTools.UnitTesting;
using University.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace University.Tests
{
    [TestClass]
    public class CourseTests : IDisposable
    {
        public void Dispose()
        {
            Course.DeleteAll();
        }
        public CourseTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=university_test;";
        }

        [TestMethod]
        public void GetAllCourses_ReturnsAllCourses_CourseList()
        {
            Course newCourse = new Course("Art", "ART101");
            newCourse.Save();

            List<Course> allCourses = Course.GetAllCourses();

            Assert.AreEqual(newCourse,allCourses[0]);
        }

        [TestMethod]
        public void Find_ReturnsCourseById_CourseObject()
        {
            Course newCourse = new Course("Art", "ART101");
            newCourse.Save();

            Course result = Course.Find(newCourse.Id);

            Assert.AreEqual(newCourse, result);
        }

        [TestMethod]
        public void Edit_UpdatesCourseById_CourseObject()
        {
            Course newCourse = new Course("Art", "ART101");
            newCourse.Save();

            Course updatedCourse = new Course ("History", "HIST101", newCourse.Id);

            newCourse.Edit(updatedCourse.Name, updatedCourse.CourseNumber);

            Assert.AreEqual(newCourse, updatedCourse);
        }
        
        [TestMethod]
        public void Delete_DeleteCourse()
        {
            Course newCourse = new Course("Art", "ART101");
            newCourse.Save();

            newCourse.Delete();
            int count = Course.GetAllCourses().Count;

            Assert.AreEqual(0,count);
        }
    }
}
