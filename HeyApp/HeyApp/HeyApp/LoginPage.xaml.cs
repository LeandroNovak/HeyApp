using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Setup activity indicator
            this.BindingContext = this;
            activityIndicator.IsVisible = false;
            this.IsBusy = true;
            this.LoginButton.Clicked += OnLoginClicked;

            // Adds a command to "not registered yet..." label
            var signupTapGestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Navigation.InsertPageBefore(new SignupPage(), this);
                    Navigation.PopAsync();
                    //Navigation.PushAsync(new SignupPage());
                })
            };

            signupTapGestureRecognizer.NumberOfTapsRequired = 1;
            SignupLabel.GestureRecognizers.Add(signupTapGestureRecognizer);
        }

        public void OnLoginClicked(object sender, EventArgs e)
        {
            //this.IsBusy = true;
            activityIndicator.IsVisible = true;
            var email = MailEntry.Text.TrimEnd(' ');

            if (Common.IsValidEmail(email))
            {
                // confirm email
                if (Common.UserExists(email))
                {
                    // Send email (Confirmation Page)
                    Common.GetUserInformations(email);
                    //IsRunningIndicator.IsRunning = false;
                    Navigation.InsertPageBefore(new EmailConfirmationPage(false), this);
                    Navigation.PopAsync();
                }
                else
                {
                    // User not found. Sign up.
                    //IsRunningIndicator.IsRunning = false;
                    DisplayAlert("Email not found!", "Sign up to use the app", "OK");
                }
            }
            else
            {
                // display error
                //IsRunningIndicator.IsRunning = false;
                DisplayAlert("Invalid email!", "Use a valid email address", "OK");
            }
            activityIndicator.IsVisible = false;
        }
    }
}