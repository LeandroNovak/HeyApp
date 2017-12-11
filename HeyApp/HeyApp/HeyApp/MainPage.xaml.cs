using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Mail;

namespace HeyApp
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = this;
            this.IsBusy = false;
            this.ButtonEnable.Clicked += ButtonClicked;
        }

        public void OnClickNewPost(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPostPage());
        }

        public void ButtonClicked(Object sender, EventArgs e)
        {
            this.IsBusy = !this.IsBusy;
        }
    }
}
