using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models
{
    [NotMapped]
    public class RawPost
    {
		[Display(Name = "Image URLs")]
		[DataType(DataType.MultilineText)]
		public string RawImageURLs { get; set; }
		[Display(Name = "Video URLs")]
		[DataType(DataType.MultilineText)]
		public string RawVideoURLs { get; set; }
        public Post Post { get; set; }
        public CategoryIntermediate[] RawCategories { get; set; }

        public RawPost()
        {
            Post = new Post();
        }

        public RawPost(Post Post, List<Category> categories)
        {
            this.Post = Post;
            this.RawCategories = categories.ConvertAll(
                c => new CategoryIntermediate(
                    c.Name, Post.Categories.Contains(c)
                )).ToArray();

           
            this.RawImageURLs = String.Join(Environment.NewLine, Post.ImageURLs.Select(image => image.URL).ToArray());
            this.RawVideoURLs = String.Join(Environment.NewLine, Post.VideoURLs.Select(video => video.URL).ToArray());
            if (RawImageURLs == null) RawImageURLs = "";
            if (RawVideoURLs == null) RawVideoURLs = "";
        }

       

        public void updatePost (int postId, List <Category> categoriesFromDB)
        {
       Post.ImageURLs=RawImageURLs
                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .ToList()
                    .Select(str => new ImageLink { URL = str, PostRefId = postId })
                    .ToList();
           Post.VideoURLs = RawImageURLs
                   .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .ToList()
                    .Select(str => new VideoLink { URL = str, PostRefId = postId })
                    .ToList();

            Post.Categories = categoriesFromDB.Where(cDB =>
            RawCategories.Where(c => c.IsSelected).Select(c => c.CategoryName).Contains(cDB.Name)).ToList();


            foreach (Category c in Post.Categories)
            {
                c.Posts.Add(Post);
            }
        }
    }
}