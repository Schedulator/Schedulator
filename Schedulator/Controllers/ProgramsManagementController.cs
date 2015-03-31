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
            //Program management details view model instance
            ProgramManagementViewModel details = new ProgramManagementViewModel();

            //Program director instance
            ProgramDirector progD = new ProgramDirector();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get the program Id
            Program program = db.Program.Find(id);
             
            //Get the program Course IDs from the specified program Id
            var detailedCourseList = progD.getProgramCourseIDs(program.ProgramId);


            //Copy elements from list to details  list
            details.Courses = detailedCourseList.ToList();

            //Copy program details into details program instance
            details.Program = program;
       

            if (program == null)
            {
                return HttpNotFound();
            }
            return View(details);
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

        // GET: ProgramsManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            //Program management details view model instance
            ProgramManagementViewModel edit = new ProgramManagementViewModel();

            //Program director instance
            ProgramDirector progD = new ProgramDirector();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Get the program Id
            Program program = db.Program.Find(id);

            //Get the program Course IDs from the specified program Id
            var detailedCourseList = progD.getProgramCourseIDs(program.ProgramId);


            //Copy elements from list to details  list
            edit.Courses = detailedCourseList.ToList();

            //Copy program details into details program instance
            edit.Program = program;

            if (program == null)
            {
                return HttpNotFound();
            }
            return View(edit);
        }

        // POST: ProgramsManagement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramId,ProgramName,ProgramOption,CreditsRequirement,Courses,Credit,Lectures,Prerequisites")] ProgramManagementViewModel program)
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
