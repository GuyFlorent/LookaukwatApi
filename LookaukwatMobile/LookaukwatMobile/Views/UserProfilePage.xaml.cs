using LookaukwatMobile.Helpers;
using LookaukwatMobile.Services;
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
    public partial class UserProfilePage : ContentPage
    {
        ApiServices _apiServices;

        public UserProfilePage()
        {
            InitializeComponent();
            _apiServices = new ApiServices();
        }

        async void LogOut_Click(object sender, EventArgs e)
        {
            string accessToken = Settings.AccessToken;
            var response = true;
            if (response)
            {
                Settings.AccessToken = "";

                await Shell.Current.GoToAsync(nameof(UserPage));
            }
        }
    }
}