using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            this.BindingContext = this;
            this.IsBusy = false;
        }

        public async void OnClickPublish(object sender, EventArgs e)
        {
            ToggleBusy();
            title = TitleEntry.Text;
            description = DescriptionEditor.Text;

            // Validate input
            if (title == null)
            {
                await DisplayAlert("No Title", "Please enter a title.", "OK");
                ToggleBusy();
                return;
            }
            if (description == null)
            {
                await DisplayAlert("No Description", "Please enter a description.", "OK");
                ToggleBusy();
                return;
            }
            if (imageStreamBase64 == null)
            {
                await DisplayAlert("No Image", "Please upload an image.", "OK");
                ToggleBusy();
                return;
            }

            // Insert post on database
            if (!Common.InsertPostOnDatabase(title, description, imageStreamBase64))
            {
                await DisplayAlert("Oops", "Something went wrong. Try again later.", "OK");
                ToggleBusy();
                return;
            }
            ToggleBusy();
            await Navigation.PopAsync();
        }

        void ToggleBusy()
        {
            IsBusy = !IsBusy;
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

            // Store as string 
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

            // Store as string 
            imageStreamBase64 = Convert.ToBase64String(byteStream);
            PostImage.Source = ImageSource.FromStream(() => new MemoryStream(byteStream));
        }
    }
}