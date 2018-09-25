using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilipBlog.Models {
    public class VideoLink {
        [Key] public int VideoLinkId { get; set; }
        public string URL { get; set; }

        [ForeignKey("Post")]
        public int PostRefId { get; set; }
        public virtual Post Post { get; set; }

        public override string ToString() {
            return this.URL;
        }

		public string YoutubeId()
		{
			if (URL.Contains("watch"))
			{
				return URL.Split('=').Last();
			}
			return URL.Split('/').Last();
		}
	}
}