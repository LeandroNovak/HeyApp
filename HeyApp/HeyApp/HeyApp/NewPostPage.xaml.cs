using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HeyApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPostPage : ContentPage
	{
		public NewPostPage ()
		{
            InitializeComponent();
        }

        public void OnClickPublish(object sender, EventArgs e)
        {

        }

        public void OnClickAddImage(object sender, EventArgs e)
        {

        }
        
        public void OnClickAddLocation(object sender, EventArgs e)
        {

        }
    }
}