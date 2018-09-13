using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class Like {
        public Like() {
            this.Posts = new List<Post>();
        }

        [Key] public int LikeId { get; set; }

        public int Value { get; set; }

        public DateTime DateOfCreation { get; set; }

        [ForeignKey("Liker")]
        public string LikerRefId { get; set; }
        public virtual ApplicationUser Liker { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}