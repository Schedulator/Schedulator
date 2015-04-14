using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Schedulator.Models;

namespace Schedulator.Controllers
{
    [Authorize(Roles="Program Director")]
    public class ProgramDirectorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgramDirector
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

        // GET: ProgramDirector/Details/5
        public ActionResult Details(int? id)
        {
            ProgramDirector pd = new ProgramDirector();
            Course course = db.Courses.Find(id);
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                //Grab Lectures from the Course
                //course.LoadLecturesForCourse(pd.getLecturesFromCourse(course));
            }
 
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: ProgramDirector/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgramDirector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,CourseLetters,CourseNumber,Credit,Prerequisites,SpecialNote,CourseId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("CreateLecture", new { courseid = course.CourseID });
            }
            return RedirectToAction("CreateLecture", new { courseid = course.CourseID });

        }
        

        //Create Lecture
        public ActionResult CreateLecture(int? courseid)
        
        {
            if (courseid != null)
            {
                TempData["courseId"] = courseid;
            }
            else
            {
                return HttpNotFound();
            }
           return View();
        }

        // POST: ProgramDirector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLecture([Bind(Include = "LectureLetter,Teacher,ClassRoomNumber,StartTime,EndTime,FirstDay,SecondDay,Semester,LectureID")] Lecture lecture)
        {
            Course course = db.Courses.Find(TempData["courseId"]);
            
            //Add Lecture for the course
            course.Lectures.Add(lecture);

            if (ModelState.IsValid)
            {
                db.Lectures.Add(lecture);
                db.SaveChanges();
                return RedirectToAction("CreateTutorial", new { lectureid = lecture.LectureID });
            }
            return RedirectToAction("CreateTutorial", new { lectureid = lecture.LectureID });
        }

        //Create Tutorial
        public ActionResult CreateTutorial(int? lectureId)
        {
            if (lectureId != null)
            {
                TempData["lectureId"] = lectureId;
            }
            else
            {
                HttpNotFound();
            }
            return View();
        }

        // POST: ProgramDirector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTutorial([Bind(Include = "TutorialLetter,Teacher,ClassRoomNumber,StartTime,EndTime,FirstDay,SecondDay,TutorialID")] Tutorial tutorial)
        {

            Course course = db.Courses.Find(TempData["courseId"]);

            var lectures = course.Lectures.Where(n => n.LectureID == (int)TempData["lectureId"]);

            if (lectures != null)
            {
                foreach (var lec in lectures)
                {
                    lec.Tutorials.Add(tutorial);
                }
            }
           
          
            
            if (ModelState.IsValid)
            {
                db.Tutorials.Add(tutorial);
                db.SaveChanges();
                return RedirectToAction("CreateLab");
            }
            return RedirectToAction("CreateLab");
        }

        //Create Tutorial
        public ActionResult CreateLab()
        {
            return View();
        }

        // POST: ProgramDirector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateLab([Bind(Include = "LabLetter,Teacher,ClassRoomNumber,StartTime,EndTime,FirstDay,SecondDay", Exclude = "TutorialID")] Lab lab) //int? id
        {
            Course course = db.Courses.Find(TempData["courseId"]);

            var lectures = course.Lectures.Where(n => n.LectureID == (int)TempData["lectureId"]);

            if (lectures != null)
            {
                foreach (var lec in lectures)
                {
                    lec.Labs.Add(lab);
                }
            }
            
            if (ModelState.IsValid)
            {
                db.Labs.Add(lab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        // GET: ProgramDirector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: ProgramDirector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,CourseLetters,CourseNumber,SpecialNote")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: ProgramDirector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: ProgramDirector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
