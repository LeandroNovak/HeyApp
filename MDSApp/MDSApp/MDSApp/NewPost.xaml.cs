using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDSApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPost : ContentPage
    {
        private Post newPost;

        public NewPost()
        {
            newPost = new Post();
            InitializeComponent();
        }

        private void OnClickSendPost(object sender, EventArgs e)
        {

        }

        private async Task OnClickAddPhotoAsync(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Cancel", null, "Select from Gallery", "Take a Photo");
            Debug.WriteLine("Action: " + action);

        }
    }
}