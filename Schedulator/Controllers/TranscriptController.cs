using Schedulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Schedulator.Controllers
{
    [Authorize]
    public class TranscriptController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            Course course = db.Courses.Where(n => n.CourseLetters == "COMP" && n.CourseNumber == 345).FirstOrDefault();
            List<Enrollment> studentEnrollments = db.Enrollment.Where(n => n.Schedule.ApplicationUser.Id == userId).ToList();
            Program program = db.Users.Find(User.Identity.GetUserId()).Program;
            List<CourseSequence> courseSequences = db.CourseSequence.Where(n => n.Program.ProgramId == program.ProgramId && n.ContainerSequence == null).ToList();
            Transcript studentTranscript = new Transcript { TranscriptYears = new List<Transcript.TranscriptYear>() };
            studentTranscript.StudentTranscript(studentEnrollments);

            return View(studentTranscript);
        }
    }
}