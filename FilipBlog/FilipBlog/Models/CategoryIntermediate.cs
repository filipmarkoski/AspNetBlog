using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilipBlog.Models
{
    public class CategoryIntermediate
    {
        public String CategoryName { get; set; }
        public Boolean IsSelected { get; set; }
        public CategoryIntermediate()
        {
                
        }
        public CategoryIntermediate(String CategoryName, Boolean IsSelected)
        {
            this.CategoryName = CategoryName;
            this.IsSelected = IsSelected;

        }

       
    }
}