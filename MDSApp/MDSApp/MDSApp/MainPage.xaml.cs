using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MDSApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Post post = new Post
            {
                Title = "Lorem Ipsum",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam diam risus, malesuada non leo nec, volutpat tempus diam. Integer euismod pretium arcu sit amet malesuada. Sed id lacus massa. Curabitur aliquet justo in lectus hendrerit, sit amet rutrum diam sodales. Vivamus eget bibendum neque. Nulla facilisis, mi et pulvinar euismod, massa turpis congue neque, ut pretium est risus nec ipsum. In turpis risus, pretium ut est quis, fringilla dapibus enim. Nulla dapibus tristique ex nec placerat. Pellentesque eleifend magna quis tortor ultricies, eget mollis ligula pharetra. Aenean varius vehicula turpis sed porta. Sed sollicitudin tellus cursus vulputate cursus. ",
                Picture = new Image() { Source = ImageSource.FromUri(new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Hausziege_04.jpg/1200px-Hausziege_04.jpg")) },
                Location = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."
            };
            Navigation.PushAsync(new DetailsPage(post));
        }

        private void OnClickNewPost(object sender, EventArgs e)
        {

            Navigation.PushAsync(new NewPost());
        }
    }
}