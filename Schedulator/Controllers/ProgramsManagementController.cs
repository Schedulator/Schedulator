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
    public class ProgramsManagementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProgramsManagement
        public ActionResult Index()
        {
            return View(db.Program.ToList());
        }

        // GET: ProgramsManagement/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get the program Id
            Program program = db.Program.Find(id);

            if (program == null)
            {
                return HttpNotFound();
            }

            program.CourseSequences = program.CourseSequences.OrderBy(r => r.Year).ThenBy(r => r.Season).ToList();

            return View(program);
        }

        // GET: ProgramsManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProgramsManagement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgramId,ProgramName,ProgramOption,CreditsRequirement")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Program.Add(program);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(program);
        }

        //Add course to program
        public ActionResult AddCourse(string searchTerm, int courseSequenceId)
        {
            var courseSequenceData = db.CourseSequence.Find(courseSequenceId);

            var program = courseSequenceData.Program;

            var course = db.Courses.FirstOrDefault(i => (i.CourseLetters + " " + i.CourseNumber) == searchTerm);

            if (course == null)
            {
                return RedirectToAction("EditProgram", new { programId = program.ProgramId, season = courseSequenceData.Season, year = courseSequenceData.Year, error = true});
            }

            var courseToAdd = new CourseSequence
            {
                Season = courseSequenceData.Season,
                Year = courseSequenceData.Year,
                Program = courseSequenceData.Program,
                Course = course
            };

            program.CourseSequences.Add(courseToAdd);
      
             if (ModelState.IsValid)
            {
                db.Entry(program).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditProgram", new { programId = program.ProgramId, season = courseSequenceData.Season, year = courseSequenceData.Year, error = false });
            }
            return View(program);
        

        }



        // Edit Program 
        public ActionResult EditProgram(int programId, Season season, int year, bool error = false)
        {
            if(error == true)
                ViewBag.Error = "The Course is invalid.";

            var courses  = db.CourseSequence.Where(i => i.Program.ProgramId == programId && i.Season == season && i.Year == year).ToList();
            
            return View(courses);
        }

        // Delete Course in Program
        public ActionResult DeleteCourse(int? id)
        {
            var data = db.CourseSequence.AsNoTracking().First(i => i.CourseSequenceId == id);
            var pid = data.CourseSequenceId;
            var season = data.Season;
            var year = data.Year;
            CourseSequence course = db.CourseSequence.Find(id);
            db.CourseSequence.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }

        // POST: ProgramsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourseConfirmed(int id)
        {
            CourseSequence course = db.CourseSequence.Find(id);
            db.CourseSequence.Remove(course);
            db.SaveChanges();
            return RedirectToAction("EditProgram", new { programId = course.Program.ProgramId, season = course.Season, year = course.Year, error = false });
        }
       
  

        // GET: ProgramsManagement/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get the program Id
            Program program = db.Program.Find(id);

            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: ProgramsManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramId,ProgramName,ProgramOption,CreditsRequirement")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(program).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(program);
        }

        // GET: ProgramsManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = db.Program.Find(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: ProgramsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Program program = db.Program.Find(id);
            db.Program.Remove(program);
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
