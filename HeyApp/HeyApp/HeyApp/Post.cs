using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/*
stream = file.GetStream();
var bytes = new byte [stream.Length];
await stream.ReadAsync(bytes, 0, (int)stream.Length);
string base64 = System.Convert.ToBase64String(bytes);
*/

namespace HeyApp
{
    public class Post
    {
		public string Title { get; set; }

        public string Description { get; set; }

        public ImageSource Image { get; set; }

        private string _imageBase64;
        public string ImageBase64
        {
            get
            {
                return _imageBase64;
            }
            set
            {
                this._imageBase64 = value;
                byte[] bytes = Convert.FromBase64String(_imageBase64);
                MemoryStream memoryStream = new MemoryStream(bytes);
                Image = ImageSource.FromStream(() => memoryStream);
            }
        }

        public Post()
        {

        }

        //public List<Post> GetPosts()
        //{
        //    List<Post> postList = Common.GetPostsFromDatabase();
        //    return postList;
        //}

        //public ImageSource Base64ToImageSource()
        //{
        //	ImageSource imageSource = ImageSource.FromFile("");
        //	return imageSource;
        //}

        //public string ToBase64()
        //{
        //	string base64string = "";
        //	return base64string;
        //}
    }
}
