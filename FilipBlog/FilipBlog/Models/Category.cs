using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilipBlog.Models {
    public class Category {

        public Category() {
            this.Posts = new List<Post>();
        }

        [Key] public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfModification { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}