using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignupPage : ContentPage
	{

		public SignupPage ()
		{
			InitializeComponent ();

            // Adds a command to "already registered..." label
            var signinTapGestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Navigation.InsertPageBefore(new LoginPage(), this);
                    Navigation.PopAsync();
                })
            };

            signinTapGestureRecognizer.NumberOfTapsRequired = 1;
            SigninLabel.GestureRecognizers.Add(signinTapGestureRecognizer);
        }

        public void OnSignupClicked(object sender, EventArgs e)
        {
            IsRunningIndicator.IsRunning = true;
            var email = MailEntry.Text.TrimEnd(' ');
            var name = NameEntry.Text.TrimEnd(' ');

            if (Common.IsValidEmail(email))
            {
                if (name != null)
                {
                    // confirm email
                    if (Common.UserExists(email))
                    {
                        // Error: User already exists. Sign in.
                        IsRunningIndicator.IsRunning = false;
                        DisplayAlert("Email already used", "Sign in to use the app", "OK");
                    }
                    else
                    {
                        Application.Current.Properties["name"] = name;
                        Application.Current.Properties["email"] = email;

                        //Navigation.PushAsync(new EmailConfirmationPage(true));
                        IsRunningIndicator.IsRunning = false;
                        Navigation.InsertPageBefore(new EmailConfirmationPage(true), this);
                        Navigation.PopAsync();
                    }
                }
                else
                {
                    // display error for null name
                    IsRunningIndicator.IsRunning = false;
                    DisplayAlert("Name field empty", "You must enter your name!", "OK");
                }
            }
            else
            {
                // display error for invalid email
                IsRunningIndicator.IsRunning = false;
                DisplayAlert("Invalid email!", "Use a valid email address", "OK");
            }
        }
    }
}