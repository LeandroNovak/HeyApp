using System;
using System.Collections.Generic;
using Xamarin.Forms;

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
