using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmailConfirmationPage : ContentPage
	{
        string name;
        string email;
        string code;
        bool isNewUser;

        public EmailConfirmationPage (bool isNewUser)
		{
			InitializeComponent ();

            this.BindingContext = this;
            this.IsBusy = false;
            this.VerfifyButton.Clicked += OnVerifyClickedAsync;

            // Adds a command to "already registered..." label
            var signinTapGestureRecognizer = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    // Re-send email
                    code = Common.GenerateCode();
                    Common.SendConfirmationEmail(name, email, code);
                    DisplayAlert("Verification code sent", "Check your email box", "OK");
                })
            };

            signinTapGestureRecognizer.NumberOfTapsRequired = 1;
            ReSendLabel.GestureRecognizers.Add(signinTapGestureRecognizer);

            // Retrieves user data
            if (Application.Current.Properties.ContainsKey("name"))
            {
                name = Application.Current.Properties["name"].ToString().TrimEnd(' ');
            }

            if (Application.Current.Properties.ContainsKey("email"))
            {
                email = Application.Current.Properties["email"].ToString().TrimEnd(' ');
            }

            this.isNewUser = isNewUser;
            code = Common.GenerateCode();

            Common.SendConfirmationEmail(name, email, code);
        }

        public async void OnVerifyClickedAsync(object sender, EventArgs e)
        {
            IsBusy = true;
            if (code.Equals(VerificationCodeEntry.Text.TrimEnd(' ').ToLower()))
            {
                // Verified!
                if (isNewUser)
                {
                    // Add user to database and get id
                    Common.InsertUserOnDatabase(name, email);
                    Common.GetUserInformations(email);
                }

                Application.Current.Properties["LoginStatus"] = "true";

                //IsRunningIndicator.IsRunning = false;
                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
                //await Navigation.PushAsync(new MainPage());
            }
            else
            {
                //IsRunningIndicator.IsRunning = false;
                var answer = await DisplayAlert("Invalid code", "Would you like to receive another verification code?", "Yes", "No");

                if (answer)
                {
                    //IsRunningIndicator.IsRunning = true;
                    // Sending another code
                    code = Common.GenerateCode();
                    Common.SendConfirmationEmail(name, email, code);
                }
            }
        }
    }
}