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
	public class ApplicationUsersController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: ApplicationUsers
		public ActionResult Index()
		{
			return View(db.Users.ToList());
		}

		// GET: ApplicationUsers/Details/5
		public ActionResult Details(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = db.Users.Find(id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// GET: ApplicationUsers/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ApplicationUsers/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,FirstName,LastName,BirthDay,Biography,ProfilePictureURL,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
		{
			if (ModelState.IsValid)
			{
				db.Users.Add(applicationUser);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(applicationUser);
		}

		// GET: ApplicationUsers/Edit/5
		public ActionResult Edit(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = db.Users.Find(id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// POST: ApplicationUsers/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,BirthDay,Biography,ProfilePictureURL,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
		{
			if (ModelState.IsValid)
			{
				db.Entry(applicationUser).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(applicationUser);
		}

		// GET: ApplicationUsers/Delete/5
		public ActionResult Delete(string id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			ApplicationUser applicationUser = db.Users.Find(id);
			if (applicationUser == null)
			{
				return HttpNotFound();
			}
			return View(applicationUser);
		}

		// POST: ApplicationUsers/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(string id)
		{
			ApplicationUser applicationUser = db.Users.Find(id);
			db.Users.Remove(applicationUser);
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


		public ActionResult Profile(string id)
		{
			return View("Profile", db.Users.Find(id));
		}

		public ActionResult ProfilePic()
		{
			return View("_ProfilePic", db.Users.Find(User.Identity.GetUserId()));
		}

		public ActionResult DisplayPhotoAndName()
		{
			return PartialView("_PhotoAndName", db.Users.Find(User.Identity.GetUserId()));
		}
		public ActionResult DisplayAuthorPhotoAndName(string userId)
		{
			return PartialView("_PhotoAndName", db.Users.Find(userId));
		}

		public ActionResult DisplayPhoto(string userId)
		{
			return PartialView("_DisplayPhoto", db.Users.Find(userId));
		}
        public ActionResult TopAuthors ()
        { List<ApplicationUser> top = db.Users.ToList().OrderByDescending
              (x => x.PostsAuthored.Count
              + x.PostsAuthored.Select(y => y.Likers.Count).Sum()
              - x.PostsAuthored.Select(y => y.Dislikers.Count).Sum()
              + x.PostsAuthored.Select(y => y.Comments.Count).Sum())
              .Take(10)
              .ToList();
            return PartialView("_TopAuthorsList", top);
        }


	}
}
