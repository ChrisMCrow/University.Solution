using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using University.Models;

namespace University.Controllers
{
    public class CourseController : Controller
    {
        [HttpGet("/courses")]
        public ActionResult Index()
        {
            return View(Course.GetAllCourses());
        }

        [HttpPost("/courses")]
        public ActionResult Create(string courseName, string courseNumber)
        {
            Course newCourse = new Course(courseName, courseNumber);
            newCourse.Save();
            return RedirectToAction("Index");
        }

        [HttpGet("/courses/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            return View(Course.Find(id));
        }

        [HttpPost("/courses/{id}")]
        public ActionResult Update(string newName, string newNumber, int id)
        {
            Course newCourse = Course.Find(id);
            newCourse.Edit(newName,newNumber);
            return RedirectToAction("Index");
        }

        [HttpGet("/courses/{id}/delete")]
        public ActionResult Remove(int id)
        {
            Course.Find(id).Delete();
            return RedirectToAction("Index");
        }

        [HttpGet("/courses/{id}")]
        public ActionResult Details(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object> {};
            Course thisCourse = Course.Find(id);
            List<Student> allStudents = Student.GetAllStudents();
            model.Add("course", thisCourse);
            model.Add("students", allStudents);
            return View(model);
        }

        [HttpPost("/courses/{id}/students/new")]
        public ActionResult Add(int id, string studentId)
        {
            int studentInt = int.Parse(studentId);
            Course foundCourse = Course.Find(id);
            foundCourse.AddStudent(studentInt);
            return RedirectToAction("Details", new {id = foundCourse.Id});
        }
    }
}
