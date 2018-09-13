using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FilipBlog.Models;
using Microsoft.AspNet.Identity;

namespace FilipBlog.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author).Include(p => p.ImageURLs);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Comments).Include(p => p.Author).Single(p => p.PostId == id);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //// GET: Posts/Create
        //public ActionResult Create()
        //{
        //    ViewBag.AuthorRefId = new SelectList(db.Users, "Id", "FirstName");
        //    ViewBag.Message = User.Identity.GetUserId();            
        //    return View();
        //}

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.Message = User.Identity.GetUserId();
            RawPost rawPost = new RawPost();

            rawPost.RawCategories = db.Categories
                .Select(c => new CategoryIntermediate
                {
                    CategoryName = c.Name,
                    IsSelected = false
                })
                .ToArray();


            return View(rawPost);
        }



        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Subtitle,Content,AuthorRefId,DateOfCreation,DateOfModification,IsFlagged,RawImageURLs,RawCategories")] RawPost rawPost)
        {

            Post post = new Post
            {
                PostId = rawPost.PostId,
                Title = rawPost.Title,
                Subtitle = rawPost.Subtitle,
                Content = rawPost.Content,
                AuthorRefId = rawPost.AuthorRefId,
                DateOfCreation = rawPost.DateOfCreation,
                DateOfModification = rawPost.DateOfModification,
                IsFlagged = rawPost.IsFlagged,
                /*ImageURLs = imageLinks,
                VideoURLs = videoLinks,
                Categories = categories*/
            };

            rawPost.RawVideoURLs = rawPost.RawImageURLs;
            rawPost.Author = db.Users.Find(rawPost.AuthorRefId);

            if (ModelState.IsValid)
            {

                db.Posts.Add(post);
                db.SaveChanges();

                var latestId = db.Posts.Where(x => x.Content == rawPost.Content && x.Title == rawPost.Title).Max(x => x.PostId);
                Post savedPost = db.Posts.Find(latestId);

                List<ImageLink> imageLinks = rawPost.RawImageURLs
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .ToList()
                    .Select(str => new ImageLink { URL = str, PostRefId = savedPost.PostId })
                    .ToList();
                List<VideoLink> videoLinks = rawPost.RawImageURLs
                       .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .ToList()
                        .Select(str => new VideoLink { URL = str, PostRefId = savedPost.PostId })
                        .ToList();


                List<String> categoryNames = rawPost.RawCategories
                    .Where(c => c.IsSelected)
                    .Select(c => c.CategoryName)
                    .ToList();

                List<Category> categories = db.Categories.Where(c => categoryNames.Contains(c.Name))
                    .AsEnumerable().ToList();

                savedPost.ImageURLs = imageLinks;
                savedPost.VideoURLs = videoLinks;
                savedPost.Categories = categories;

                foreach (Category c in categories)
                {
                    c.Posts.Add(savedPost);
                }



                db.ImageLinks.AddRange(imageLinks);
                db.VideoLinks.AddRange(videoLinks);
                db.SaveChanges();




                return RedirectToAction("Index");
            }




            return View(rawPost);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorRefId = new SelectList(db.Users, "Id", "FirstName", post.AuthorRefId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Subtitle,Content,AuthorRefId,DateOfCreation,DateOfModification,IsFlagged")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorRefId = new SelectList(db.Users, "Id", "FirstName", post.AuthorRefId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
