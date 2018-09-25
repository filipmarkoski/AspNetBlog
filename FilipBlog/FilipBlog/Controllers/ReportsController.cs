using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilipBlog.Models;
using Microsoft.AspNet.Identity;
namespace FilipBlog.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        public ActionResult Index()
        {
            var reports = db.Reports.Include(r => r.Post).Include(r => r.Reporter);
            return View(reports.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create(int postId)
        {
            //ViewData["postId"] = postId;
            Report model = new Report {
                Post_PostId = postId,
                DateOfCreation = DateTime.Now,
                DateOfModification = DateTime.Now,
                ReporterRefId = User.Identity.GetUserId()
            };

            return View(model);
        }


        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportId,Content,DateOfCreation,DateOfModification,ReporterRefId,Post_PostId")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
				var postReported = db.Posts.Find(report.Post_PostId);
				var userId = User.Identity.GetUserId();
				db.Users.Find(userId)
					.PostsReported.Add(postReported);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.Post_PostId = new SelectList(db.Posts, "PostId", "Title", report.Post_PostId);
            ViewBag.ReporterRefId = new SelectList(db.Users, "Id", "FirstName", report.ReporterRefId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReportId,Content,DateOfCreation,DateOfModification,ReporterRefId,Post_PostId")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Post_PostId = new SelectList(db.Posts, "PostId", "Title", report.Post_PostId);
            ViewBag.ReporterRefId = new SelectList(db.Users, "Id", "FirstName", report.ReporterRefId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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

		public ActionResult ViewReports (int postId)
		{
			return View("_ViewReports", db.Reports.ToList().Where(x => x.Post_PostId == postId));
		}
    }
}
