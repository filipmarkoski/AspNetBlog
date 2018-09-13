using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class AddUserToRole {

        public string UserName { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }

        public AddUserToRole() {
            this.Roles = new List<string>();
        }
    }
}