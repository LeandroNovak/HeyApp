using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//<Editor x:Name="DescriptionEditor" HorizontalOptions="FillAndExpand" />

namespace HeyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPostPage : ContentPage
	{
        System.IO.Stream imageStream;

		public NewPostPage ()
		{
            InitializeComponent();
            CrossMedia.Current.Initialize();
        }

        public void OnClickPublish(object sender, EventArgs e)
        {

        }

        public async Task OnClickAddImageAsync(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("What do you want to do?", "Cancel", null, "Take a photo", "Upload from gallery");

            if (action.Equals("Take a photo"))
            {
                TakePhoto();
                return;
            }
            if (action.Equals("Upload from gallery"))
            {
                PickPhoto();
                return;
            }
        }

        public async void TakePhoto()
        {
            
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            PostImage.Source = ImageSource.FromStream(() =>
            {
                imageStream = file.GetStream();
                file.Dispose();
                return imageStream;
            });
        }

        public async void PickPhoto()
        {
            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", "Permission not granted to photos.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full
            });

            if (file == null)
                return;

            PostImage.Source = ImageSource.FromStream(() =>
            {
                imageStream = file.GetStream();
                file.Dispose();
                return imageStream;
            });
        }
        
        public void OnClickAddLocation()
        {

        }


    }
}