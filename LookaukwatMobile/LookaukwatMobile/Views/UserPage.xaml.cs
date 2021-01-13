using LookaukwatMobile.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LookaukwatMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var token = Settings.AccessToken;
            var username = Settings.Username;
            var password = Settings.Password;
            if (!string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.GoToAsync(nameof(UserProfilePage));
            }
            else if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                await Navigation.PushAsync(new LoginRedirectUserAccountPage());
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(RegisterPage));
                
            }
        }
    }
}