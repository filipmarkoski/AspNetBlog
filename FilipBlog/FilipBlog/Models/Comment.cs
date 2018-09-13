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
        [Required] public string Content { get; set; }

        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfModification { get; set; }

        [ForeignKey("Commenter")]
        public string CommenterRefId { get; set; }
        public virtual ApplicationUser Commenter { get; set; }

        [ForeignKey("Post")]
        public int Post_PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual ICollection<Like> Likes { get; set; } // LikePostDBContext.cs resolves this dilemma 
        public virtual ICollection<Dislike> Dislikes { get; set; } // DislikePostDBContext.cs resolves this dilemma 
        public virtual ICollection<Comment> Replies { get; set; }
    }
}