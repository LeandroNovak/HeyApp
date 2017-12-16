using System;
using System.IO;
using Xamarin.Forms;

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
            // Do nothing
        }
    }
}
