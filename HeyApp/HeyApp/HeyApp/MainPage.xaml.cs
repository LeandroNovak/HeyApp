using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Mail;
using System.IO;

namespace HeyApp
{
	public partial class MainPage : ContentPage
	{
        Stack<Post> Posts;
        uint LastIndex;

        public MainPage()
        {
            LastIndex = 0;
            Posts = new Stack<Post>();
            Posts = Common.GetPostsFromDatabase(LastIndex);
            LastIndex += 5;
            InitializeComponent();

            this.BindingContext = this;
            this.IsBusy = false;
            
            postList.ItemsSource = Posts;
        }

        public void OnClickNewPost(Object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewPostPage());
        }

        public void ButtonClicked(Object sender, EventArgs e)
        {
            this.IsBusy = !this.IsBusy;
            Stack<Post> newPosts = Common.GetPostsFromDatabase(LastIndex);
            LastIndex += 5;

            foreach (var post in newPosts)
            {
                Posts.Push(post);
            }
        }
    }
}
