using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilipBlog.Models;

namespace FilipBlog.Controllers
{

	public class CategoriesController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		// GET: Categories

		public ActionResult Index()
		{
			return View(db.Categories.ToList());
		}

		public ActionResult List()
		{
			return PartialView("_CategoryList", db.Categories.ToList());
		}

		// GET: Categories/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = db.Categories.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// GET: Categories/Create
		[Authorize(Roles = "Admin, Editor")]
		public ActionResult Create()
		{
			return View();
		}

		// POST: Categories/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin, Editor")]
		public ActionResult Create([Bind(Include = "CategoryID,Name,DateOfCreation,DateOfModification")] Category category)
		{
			if (ModelState.IsValid)
			{
				db.Categories.Add(category);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(category);
		}

		// GET: Categories/Edit/5
		[Authorize(Roles = "Admin, Editor")]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = db.Categories.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "CategoryID,Name,DateOfCreation,DateOfModification")] Category category)
		{
			if (ModelState.IsValid)
			{
				db.Entry(category).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(category);
		}

		// GET: Categories/Delete/5
		[Authorize(Roles = "Admin, Editor")]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Category category = db.Categories.Find(id);
			if (category == null)
			{
				return HttpNotFound();
			}
			return View(category);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Category category = db.Categories.Find(id);
			db.Categories.Remove(category);
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
		public ActionResult AllPostsFrom(int id)
		{
			List<Post> posts;
			if(id==0) posts= db.Posts.ToList();
			else posts= db.Categories.Find(id).Posts.ToList();
			posts.Reverse();
			return PartialView("_PostList", posts);
		}

		public ActionResult HomeView()
		{
			return View(db.Categories.ToList());
		}

		public ActionResult TopPosts()
		{
			List<Post> top = db.Posts.OrderByDescending(c => c.Likers.Count - c.Dislikers.Count + c.Comments.Count).Take(10).ToList();
			return PartialView("_TopPostList", top);

		}

	}
}
