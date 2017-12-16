using System;
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

                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                var answer = await DisplayAlert("Invalid code", "Would you like to receive another verification code?", "Yes", "No");

                if (answer)
                {
                    // Sending another code
                    code = Common.GenerateCode();
                    Common.SendConfirmationEmail(name, email, code);
                }
            }
        }
    }
}