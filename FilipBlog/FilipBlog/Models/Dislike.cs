using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class Dislike {

        public Dislike() {
            this.Posts = new List<Post>();
        }

        [Key] public int DislikeId { get; set; }

        public int Value { get; set; }

        public DateTime DateOfCreation { get; set; }

        [ForeignKey("Disliker")]
        public string DislikerRefId { get; set; }
        public virtual ApplicationUser Disliker { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}