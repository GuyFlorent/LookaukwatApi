using LookaukwatMobile.Helpers;
using LookaukwatMobile.Services;
using LookaukwatMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LookaukwatMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; set; }
        public Command LoginUserAccountCommand { get; set; }
        public Command RegisterViewCommand { get; }
        public INavigation Navigation { get; set; }
        ApiServices _apiServices = new ApiServices();

        private string usermame;
        private string password;

        public LoginViewModel()
        {
            usermame = Settings.Username;
            password = Settings.Password;
            TitlePage = "Connexion par email";
            LoginCommand = new Command(OnLoginClicked,ValidateLoging);
            RegisterViewCommand = new Command(OnRegisterViewClicked);
            LoginUserAccountCommand = new Command(OnLoginReturnUserClicked, ValidateLoging);
            this.PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }
        public LoginViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            //LoginCommand = new Command(OnLoginClicked);
            usermame = Settings.Username;
            password = Settings.Password;
            TitlePage = "Connexion par email";
            LoginCommand = new Command(OnLoginClicked, ValidateLoging);
            RegisterViewCommand = new Command(OnRegisterViewClicked);
            LoginUserAccountCommand = new Command(OnLoginReturnUserClicked, ValidateLoging);
            this.PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();
        }

        private bool ValidateLoging()
        {
            return !String.IsNullOrWhiteSpace(Username)
                && Username.Contains("@")
                && !String.IsNullOrWhiteSpace(Password);
        }
        public string Username
        {
            get => usermame;
            set => SetProperty(ref usermame, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        private async void OnRegisterViewClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
        private async void OnLoginClicked()
        {
            var accesstonken = await _apiServices.LoginASync(Username, Password);

            Settings.AccessToken = accesstonken;
        // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
        if (accesstonken != null)
        {
                await Shell.Current.GoToAsync(nameof(PublishProductPage));
                //await Shell.Current.GoToAsync("//Shell_Item/ItemsPage");
                //await Navigation.PushAsync(new PublishProductPage());
        }
        }

        private async void OnLoginReturnUserClicked()
        {
            var accesstonken = await _apiServices.LoginASync(Username, Password);

            Settings.AccessToken = accesstonken;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (accesstonken != null)
            {
                await Shell.Current.GoToAsync(nameof(UserPage));
                //await Navigation.PushAsync(new PublishProductPage());
            }
        }
    }
}
