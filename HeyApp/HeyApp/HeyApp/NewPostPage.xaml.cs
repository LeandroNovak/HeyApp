using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        string title;
        string description;
        System.IO.Stream imageStream;
        string imageStreamBase64;
        byte[] byteStream;

		public NewPostPage ()
		{
            InitializeComponent();
            CrossMedia.Current.Initialize();
        }

        public void OnClickPublish(object sender, EventArgs e)
        {
            title = TitleEntry.Text;
            description = DescriptionEditor.Text;

            // Validate input
            if (title == null)
            {
                DisplayAlert("No Title", "Please enter a title.", "OK");
                return;
            }
            if (description == null)
            {
                DisplayAlert("No Description", "Please enter a description.", "OK");
                return;
            }
            if (imageStreamBase64 == null)
            {
                DisplayAlert("No Image", "Please upload an image.", "OK");
                return;
            }

            // Insert post on database
            if (!Common.InsertPostOnDatabase(title, description, imageStreamBase64))
            {
                DisplayAlert("Oops", "Something went wrong. Try again later.", "OK");
                return;
            }

            Navigation.PopAsync();
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
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 95
            });

            if (file == null)
                return;

            imageStream = file.GetStream();
            byteStream = new byte[imageStream.Length];
            await imageStream.ReadAsync(byteStream, 0, (int)imageStream.Length);
            imageStreamBase64 = Convert.ToBase64String(byteStream);
            PostImage.Source = ImageSource.FromStream(() => new MemoryStream(byteStream));
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
                PhotoSize = PhotoSize.Medium,
                CompressionQuality = 95
            });

            if (file == null)
                return;

            imageStream = file.GetStream();
            byteStream = new byte[imageStream.Length];
            await imageStream.ReadAsync(byteStream, 0, (int)imageStream.Length);
            imageStreamBase64 = Convert.ToBase64String(byteStream);
            PostImage.Source = ImageSource.FromStream(() => new MemoryStream(byteStream));
        }
        
        //public void OnClickAddLocation()
        //{
        //        < Button x: Name = "AddLocationButton" Text = "Add location" Clicked = "OnClickAddLocation" HorizontalOptions = "FillAndExpand" HeightRequest = "50" BackgroundColor = "#7635EB" TextColor = "White" />
        //}


    }
}