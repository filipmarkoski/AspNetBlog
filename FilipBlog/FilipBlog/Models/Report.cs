using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class Report {
        public Report() {
            // this.Posts = new List<Post>();

            DateOfCreation = DateTime.Now;
            DateOfModification = DateTime.Now;
        }

        [Key] public int ReportId { get; set; }
        [Required] public string Content { get; set; }

		[Display(Name = "Date Created")]
		public DateTime DateOfCreation { get; set; }
		[Display(Name = "Date Modified")]
		public DateTime DateOfModification { get; set; }

		[ForeignKey("Reporter")]
        public string ReporterRefId { get; set; }
        public virtual ApplicationUser Reporter { get; set; }

        [ForeignKey("Post")]
        public int Post_PostId { get; set; }
        public virtual Post Post { get; set; }

        //  public virtual ICollection<Post> Posts { get; set; }
    }
}