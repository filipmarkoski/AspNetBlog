using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilipBlog.Models {
    public class Comment {

        public Comment() {
            this.Likes = new List<Like>();
            this.Dislikes = new List<Dislike>();
            this.Replies = new List<Comment>();
        }

        [Key] public int CommentId { get; set; }
		[Required(ErrorMessage = "Can't subit an empty Comment")] public string Content { get; set; }

		[Display(Name = "Date Created")]
		public DateTime DateOfCreation { get; set; }
		[Display(Name = "Date Modified")]
		public DateTime DateOfModification { get; set; }

		[ForeignKey("Commenter")]
        public string CommenterRefId { get; set; }
        public virtual ApplicationUser Commenter { get; set; }

     //  [ForeignKey("ParentComment")]
        public int ParentComment_CommentId { get; set; }
       // public virtual Comment ParentComment {get;set;}
        
        [ForeignKey("Post")]
        public int Post_PostId { get; set; }
        public virtual Post Post { get; set; }
        //not used
        public virtual ICollection<Like> Likes { get; set; } 
        //not used
        public virtual ICollection<Dislike> Dislikes { get; set; } 
        public virtual ICollection<Comment> Replies { get; set; }

        public String howLongAgo ()
        {
            int seconds = (int) DateTime.Now.Subtract(DateOfCreation).TotalSeconds;
          
            if (seconds < 60) return String.Format("{0} second{1} ago", seconds, seconds>1?"s":"");
            if (seconds > 60 && seconds < 3600) return String.Format("{0} minute{1} ago", (int) seconds/60, (int)seconds /60 > 1 ? "s":"");
            if (seconds > 3600 && seconds < 3600*24) return String.Format("{0} hour{1} ago", (int)seconds / 3600, (int) seconds/3600 > 1 ? "s" : "");
            return String.Format("{0} day{1} ago", (int)seconds / (3600*24), (int)seconds / (3600 * 24) > 1? "s" : "");
        }
    }
}