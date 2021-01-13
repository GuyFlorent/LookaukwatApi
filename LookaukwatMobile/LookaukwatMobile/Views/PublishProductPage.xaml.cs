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
    public partial class PublishProductPage : ContentPage
    {
        public PublishProductPage()
        {
            InitializeComponent();
            
           // SetMainPage();
        }

        private async void SetMainPage()
        {
            var token = Settings.AccessToken;
            var username = Settings.Username;
            var password = Settings.Password;
            if (!string.IsNullOrWhiteSpace(token))
            {
                 await Navigation.PushModalAsync(new PublishProductPage());
            }
            else if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(RegisterPage));
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var token = Settings.AccessToken;
            var username = Settings.Username;
            var password = Settings.Password;
            if (!string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.GoToAsync(nameof(PublishAnnouncePage));
            }
            else if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(RegisterPage));
               // await Navigation.PushModalAsync(new RegisterPage());
               // Navigation.RemovePage(this);
            }
        }
    }
}