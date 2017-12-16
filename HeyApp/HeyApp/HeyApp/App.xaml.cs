using System;
using Xamarin.Forms;

namespace HeyApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
        }

        protected override void OnStart ()
		{
            // Handle when your app starts
            // Verifies if user logged in
            if (Application.Current.Properties.ContainsKey("LoginStatus") && Application.Current.Properties["LoginStatus"].Equals("true"))
            {
                MainPage = new NavigationPage(new MainPage());
                Console.WriteLine("Goes to MainPage");
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
                Console.WriteLine("Goes to LoginPage");
            }
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
        }
	}
}
