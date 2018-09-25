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
            ViewBag.CommenterRefId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.Post_PostId = new SelectList(db.Posts, "PostId", "Title");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts
                .Include(p => p.Comments)
                .Include(p => p.Author)
                .Single(p => p.PostId == id);


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
        public ActionResult Create([Bind(Include = "Post,RawImageURLs,RawCategories")] RawPost rawPost)
        {




            rawPost.Post.Author = db.Users.Find(rawPost.Post.AuthorRefId);

            if (ModelState.IsValid)
            {

                db.Posts.Add(rawPost.Post);
                db.Users.Find(rawPost.Post.AuthorRefId).PostsAuthored.Add(rawPost.Post);
                db.SaveChanges();

                var latestId = db.Posts.Where(x => x.Content == rawPost.Post.Content && x.Title == rawPost.Post.Title).Max(x => x.PostId);

                rawPost.updatePost(latestId, db.Categories.ToList());
                db.ImageLinks.AddRange(rawPost.Post.ImageURLs);
                db.VideoLinks.AddRange(rawPost.Post.VideoURLs);
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
            RawPost rawPost = new RawPost(post, db.Categories.ToList());




            return View(rawPost);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Post,RawImageURLs,RawCategories,RawVideoURLs")] RawPost rawPost)
        {

            if (ModelState.IsValid)
            {

                Post post = db.Posts.Find(rawPost.Post.PostId);
                List<VideoLink> updatedVideoLinks = new List<VideoLink>();
                if (rawPost.RawVideoURLs!=null)
                updatedVideoLinks = rawPost.RawVideoURLs
                       .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                       .ToList()
                       .Select(str => new VideoLink { URL = str, PostRefId = rawPost.Post.PostId })
                       .ToList();

                List<VideoLink> oldVideos = db.VideoLinks.Where(x => x.PostRefId == rawPost.Post.PostId).ToList();
                List<VideoLink> addedVideos = updatedVideoLinks.Except(oldVideos).ToList();
                List<VideoLink> deletedVideos = oldVideos.Except(updatedVideoLinks).ToList();
                addedVideos.ForEach(x => db.Entry(x).State = EntityState.Added);
                deletedVideos.ForEach(x => db.Entry(x).State = EntityState.Deleted);


                List<ImageLink> updatedImageLinks = new List<ImageLink>();
                if(rawPost.RawImageURLs!=null)
                    updatedImageLinks = rawPost.RawImageURLs
                     .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                     .ToList()
                     .Select(str => new ImageLink { URL = str, PostRefId = rawPost.Post.PostId })
                     .ToList();
                List<ImageLink> oldImages = db.ImageLinks.Where(x => x.PostRefId == rawPost.Post.PostId).ToList();
                List<ImageLink> addedImages = updatedImageLinks.Except(oldImages).ToList();
                List<ImageLink> deletedImages = oldImages.Except(updatedImageLinks).ToList();
                deletedImages.ForEach(x => db.Entry(x).State = EntityState.Deleted);
                addedImages.ForEach(x => db.Entry(x).State = EntityState.Added);


                List<String> selectedCategoriesNames = rawPost.RawCategories.Where(c => c.IsSelected).Select(c => c.CategoryName).ToList();
                List<Category> selectedCategories = db.Categories.Where(cDB =>
                selectedCategoriesNames.Contains(cDB.Name)).ToList();
                var deletedCategories = post.Categories.Except(selectedCategories).ToList<Category>();
                var addedCategories = selectedCategories.Except(post.Categories).ToList<Category>();
                deletedCategories.ForEach(c => post.Categories.Remove(c));
                foreach (Category c in addedCategories)
                {
                    if (db.Entry(c).State == EntityState.Detached)
                        db.Categories.Attach(c);

                    post.Categories.Add(c);
                }

                post.Title = rawPost.Post.Title;
                post.Subtitle = rawPost.Post.Subtitle;
                post.Content = rawPost.Post.Content;
                post.DateOfModification = DateTime.Now;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rawPost);
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
            post.Likers.ToList().ForEach(x => x.PostsLiked.Remove(post));
            post.Dislikers.ToList().ForEach(x => x.PostsDisliked.Remove(post));
            post.Comments.Select(c => c.Commenter).ToList().ForEach(x => x.PostsCommentedOn.Remove(post));
            post.Categories.ToList().ForEach(c => c.Posts.Remove(post));
            post.Author.PostsAuthored.Remove(post);
          /*  post.ImageURLs.ToList().ForEach(c => db.Entry(c).State = EntityState.Deleted);
            post.VideoURLs.ToList().ForEach(c => db.Entry(c).State = EntityState.Deleted);
            */
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




        public ActionResult LikePost(int postId)
        {
            Post post = db.Posts.Find(postId);
            ApplicationUser liker = db.Users.Find(User.Identity.GetUserId());

            if (post.Dislikers.Contains(liker))
                post.Dislikers.Remove(liker);

            if (liker.PostsDisliked.Contains(post))
                liker.PostsDisliked.Remove(post);

            if (!post.Likers.Contains(liker))
                post.Likers.Add(liker);

            if (!liker.PostsLiked.Contains(post))
                liker.PostsLiked.Add(post);

            db.SaveChanges();

            return PartialView("_LikeDislikeCount", post);
        }


        public ActionResult DislikePost(int postId)
        {
            Post post = db.Posts.Find(postId);
            ApplicationUser disliker = db.Users.Find(User.Identity.GetUserId());

            if (post.Likers.Contains(disliker))
                post.Likers.Remove(disliker);

            if (disliker.PostsLiked.Contains(post))
                disliker.PostsLiked.Remove(post);

            if (!post.Dislikers.Contains(disliker))
                post.Dislikers.Add(disliker);

            if (!disliker.PostsDisliked.Contains(post))
                disliker.PostsDisliked.Add(post);

            db.SaveChanges();

            return PartialView("_LikeDislikeCount", post);
        }

        public ActionResult ShowComments(int postId)
        {
            List<Comment> comms = db.Posts.Find(postId).Comments.ToList();
            return PartialView("_CommentList", comms);
        }
    }
}
