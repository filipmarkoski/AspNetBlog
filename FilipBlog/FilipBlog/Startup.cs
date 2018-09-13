using FilipBlog.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FilipBlog.Startup))]
namespace FilipBlog {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesAndUsers() {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Creating Editor Role
            if (!roleManager.RoleExists("Admin")) {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }

            // Creating Editor Role
            if (!roleManager.RoleExists("Editor")) {
                IdentityRole role = new IdentityRole {
                    Name = "Editor"
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Moderator")) {
                IdentityRole role = new IdentityRole {
                    Name = "Moderator"
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User")) {
                IdentityRole role = new IdentityRole {
                    Name = "User"
                };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Banned")) {
                IdentityRole role = new IdentityRole {
                    Name = "Banned"
                };
                roleManager.Create(role);
            }

        }
    }
}
