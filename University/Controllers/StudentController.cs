using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet("/students")]
        public ActionResult Index()
        {
            return View(Student.GetAllStudents());
        }

        [HttpPost("/students")]
        public ActionResult Create(string studentName, string enrollmentString)
        {
            DateTime enrollmentDate = Convert.ToDateTime(enrollmentString);
            Student newStudent = new Student(studentName, enrollmentDate);
            newStudent.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/students/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            return View(Student.Find(id));
        }

        [HttpPost("/students/{id}")]
        public ActionResult Update(string newName, string newDateString, int id)
        {
            DateTime newDate = Convert.ToDateTime(newDateString);
            Student newStudent = Student.Find(id);
            newStudent.Edit(newName,newDate);
            return RedirectToAction("Index");
        }

        [HttpGet("/students/{id}/delete")]
        public ActionResult Remove(int id)
        {
            Student.Find(id).Delete();
            return RedirectToAction("Index");
        }

        [HttpGet("/students/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Student thisStudent = Student.Find(id);
            List<Course> allCourses = Course.GetAllCourses();
            model.Add("student", thisStudent);
            model.Add("courses", allCourses);
            return View(model);
        }

        [HttpPost("/students/{id}/courses/new")]
        public ActionResult Add(int id, string courseId)
        {
            int courseInt = int.Parse(courseId);
            Student foundStudent = Student.Find(id);
            foundStudent.AddCourse(courseInt);
            return RedirectToAction("Details", new {id = foundStudent.Id});
        }
    }
}
