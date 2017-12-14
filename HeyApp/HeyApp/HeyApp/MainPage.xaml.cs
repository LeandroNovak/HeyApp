using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Mail;
using System.IO;
using System.Windows.Input;
using Npgsql;

namespace HeyApp
{
    public partial class MainPage : ContentPage
    {
        Stack<Post> Posts;

        public MainPage()
        {
            InitializeComponent();
            Posts = Common.GetPostsFromDatabase();
            this.BindingContext = this;
            postList.ItemsSource = Posts;
        }

        public void OnClickNewPost(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPostPage());
        }
    }
}   
