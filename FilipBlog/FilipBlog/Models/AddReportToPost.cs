using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class AddReportToPost {
        public int ReportId { get; set; }
        public int PostId { get; set; }

        public List<Post> Posts { get; set; }

        public AddReportToPost() {
            this.Posts = new List<Post>();
        }
    }
}