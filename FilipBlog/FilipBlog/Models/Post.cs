using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class Post {

        public Post() {
            this.ImageURLs = new List<ImageLink>();
            this.VideoURLs = new List<VideoLink>();
            this.Likes = new List<Like>();
            this.Dislikes = new List<Dislike>();
            this.Reports = new List<Report>();
            this.Comments = new List<Comment>();
            this.Categories = new List<Category>();
            this.DateOfModification = DateTime.Now;
            this.DateOfCreation = DateTime.Now;
        }

        [Key]
        public int PostId { get; set; }
        [Required] [StringLength(160)] public String Title { get; set; }
        [StringLength(160)] public String Subtitle { get; set; }
        [Required] [DataType(DataType.MultilineText)] public String Content { get; set; }

        // one to one
        [Required]
        [ForeignKey("Author")]
        public String AuthorRefId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        // One post can have Multiple images [1-to-M] 
        public virtual ICollection<ImageLink> ImageURLs { get; set; }
        public virtual ICollection<VideoLink> VideoURLs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } 

        // a category can cover multiple articles and an article can be categorized under by multiple categories. [m2m]
        public virtual ICollection<Category> Categories { get; set; } 

        // a user can like multiple articles and an article can be liked by multiple users. [m2m]
        public virtual ICollection<Like> Likes { get; set; } 
        public virtual ICollection<Dislike> Dislikes { get; set; } 
        public virtual ICollection<Report> Reports { get; set; }

        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfModification { get; set; }
        public Boolean IsFlagged { get; set; }

        public override string ToString() {
            return String.Format("POST ID: {0,10}\n", this.PostId) +
                String.Format("Title: {0,10}\n", this.Title) +
                String.Format("Subtitle: {0,10}\n", this.Subtitle) +
                String.Format("Content: {0,10}\n", this.Content)
                +
                String.Format("AuthorRefId: {0,30}\n", this.AuthorRefId)
                //+
                //String.Format("Author.UserName: {0,30}\n", this.Author.UserName)
                // +
                //String.Join(Environment.NewLine, this.ImageURLs) +
                //String.Join(Environment.NewLine, this.VideoURLs)
                ;
        }


    }

}