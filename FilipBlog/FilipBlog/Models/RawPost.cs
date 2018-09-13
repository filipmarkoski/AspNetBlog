using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models
{
    [NotMapped]
    public class RawPost:Post
    {
       // public int PostId { get; set; }

        //public string Title { get; set; }
       // public string Subtitle { get; set; }
        //public string Content { get; set; }
        //public string AuthorRefId { get; set; }
        public string RawImageURLs { get; set; }
        public string RawVideoURLs { get; set; }

        //public DateTime DateOfCreation { get; set; }
        //public DateTime DateOfModification { get; set; }
        //public bool IsFlagged { get; set; }

        public CategoryIntermediate[] RawCategories { get; set; }

        public RawPost():base()
        {
           
        }
    }
}