using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Schedulator.Controllers
{
    public class ProgressionController : Controller
    {
        // GET: Progression
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            List<Enrollment> studentEnrollments = db.Enrollment.Where(n => n.Schedule.ApplicationUser.Id == userId).ToList();
            Program program = db.Users.Find(User.Identity.GetUserId()).Program;
            List<CourseSequence> courseSequences = db.CourseSequence.Where(n => n.Program.ProgramId == program.ProgramId).ToList();

            Progression studentsProgression = new Progression(){ CompletedCourse = new List<CourseSequence>(), IncompleteCourse  = new List<CourseSequence>(), InProgressCourse = new List<CourseSequence>()};
            studentsProgression.StudentsProgression(studentEnrollments, courseSequences);

            return View();
        }
    }
}