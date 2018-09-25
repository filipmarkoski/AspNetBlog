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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Reply (int ParentComment_CommentId )
        { Comment reply = new Comment();
            db.Comments.Find(ParentComment_CommentId ).Replies.Add(reply);
            return PartialView("_AjaxReply", reply);


        }


        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Commenter).Include(c => c.Post);
            return View(comments);
        }

        public ActionResult CommentList() {
            var comments = db.Comments.Include(c => c.Commenter).Include(c => c.Post);
            return PartialView(comments);
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.CommenterRefId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.Post_PostId = new SelectList(db.Posts, "PostId", "Title");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentId,Content,DateOfCreation,DateOfModification,CommenterRefId,Post_PostId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Commenter=db.Users.Find(comment.CommenterRefId);
                comment.Post = db.Posts.Find(comment.Post_PostId);
                comment.Commenter.PostsCommentedOn.Add(comment.Post);
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommenterRefId = new SelectList(db.Users, "Id", "FirstName", comment.CommenterRefId);
            ViewBag.Post_PostId = new SelectList(db.Posts, "Post_PostId", "Title", comment.Post_PostId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommenterRefId = new SelectList(db.Users, "Id", "FirstName", comment.CommenterRefId);
            ViewBag.Post_PostId = new SelectList(db.Posts, "Post_PostId", "Title", comment.Post_PostId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentId,Content,DateOfCreation,DateOfModification,CommenterRefId,Post_PostId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CommenterRefId = new SelectList(db.Users, "Id", "FirstName", comment.CommenterRefId);
            ViewBag.Post_PostId = new SelectList(db.Posts, "Post_PostId", "Title", comment.Post_PostId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		public ActionResult _Create(int post)
		{

			return View();
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ShowComment(int id)
        {
            return PartialView("_SingleComment", db.Posts.Find(id).Comments.LastOrDefault());
        }
    }
}
