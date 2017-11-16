using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDSApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        //Post post;

        public DetailsPage()
        {
            InitializeComponent();
            this.FindByName<ContentPage>("Details").Title = "Title";
            this.FindByName<Label>("Description").Text = "Sample text";
            this.FindByName<Image>("Image").Source = "icon.png";
            this.FindByName<Label>("Location").Text = "Location";
        }

        public DetailsPage(Post post) 
        {
            InitializeComponent();
            this.FindByName<ContentPage>("Details").Title = post.Title;
            Debug.WriteLine("title:" + post.Title);
            this.FindByName<Label>("Description").Text = post.Description;
            this.FindByName<Image>("Image").Source = post.Picture.Source;
            this.FindByName<Label>("Location").Text = post.Location;
        }

        private void OnClickOpenLocation(object sender, EventArgs e)
        {
            Debug.WriteLine("Opening location");
        }
    }
}