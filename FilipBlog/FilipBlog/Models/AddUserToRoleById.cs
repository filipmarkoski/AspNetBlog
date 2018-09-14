using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class AddUserToRoleById {

        public string UserId { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }

        public AddUserToRoleById() {
			this.Roles = new List<string>() { "Admin", "Editor", "Moderator", "User","Banned" };
			
		}
    }
}