using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FilipBlog.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser {

        [Required]
		[DisplayName("First Name")]
		public string FirstName { get; set; }
		[DisplayName("Last Name")]
		[Required] public string LastName { get; set; }
        [Required]
        [DisplayName("Date of Birth")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; }
        public string Biography { get; set; }
        public string ProfilePictureURL { get; set; }

        public virtual ICollection<Post> PostsAuthored { get; set; }
        public virtual ICollection<Post> PostsLiked { get; set; }
        public virtual ICollection<Post> PostsDisliked { get; set; }
        public virtual ICollection<Post> PostsCommentedOn { get; set; }
        public virtual ICollection<Post> PostsReported { get; set; }
        public virtual ICollection<Comment> CommentsCommentedOn { get; set; }

        public String FullName ()
        {
            return String.Format("{0} {1}", FirstName, LastName);
        }


        public ApplicationUser() {
            this.PostsAuthored = new List<Post>();
            this.PostsLiked = new List<Post>();
            this.PostsDisliked = new List<Post>();
            this.PostsCommentedOn = new List<Post>();
            this.PostsReported = new List<Post>();
            this.CommentsCommentedOn = new List<Comment>();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


    }

}