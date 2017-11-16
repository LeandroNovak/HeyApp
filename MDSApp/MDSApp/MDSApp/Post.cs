using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MDSApp
{
    public class Post
    {
        public String Title { get; set; }
        public Image Picture { get; set; }
        public String Description { get; set; }
        public String Location { get; set; }
        public Boolean Status { get; set; }
        public int Id { get; set; }

        public Post()
        {
            Title = "Title";
            Picture = new Image();
            Description = "Description";
            Location = "Not set";
            Status = false;
            Id = 0;
        }
    }
}
