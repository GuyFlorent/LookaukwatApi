using LookaukwatMobile.Helpers;
using LookaukwatMobile.Services;
using LookaukwatMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LookaukwatMobile.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();
        public string Message { get; set; }
        private string firstname;
        private string phone;
        private string email;
        public string password;
        public string confirmPassword;

        public string FirstName
        {
            get => firstname;
            set => SetProperty(ref firstname, value);
        }
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public RegisterViewModel()
        {
            TitlePage = "Creation de compte";
            RegisterCommand = new Command(OnRegister, ValidateRegister);
            RegisterReturnUserAccountCommand = new Command(OnRegisterReturnUserAccount, ValidateRegister);
            LoginCommand = new Command(OnLoginClicked);
            LoginReturnForUserCommand = new Command(OnLoginReturnForUserClicked);
            this.PropertyChanged +=
                (_, __) => RegisterCommand.ChangeCanExecute();
        }



        public Command RegisterCommand { get; set; }
        public Command RegisterReturnUserAccountCommand { get; set; }
        public Command LoginCommand { get; }
        public Command LoginReturnForUserCommand { get; }

        private bool ValidateRegister()
        {
            return !String.IsNullOrWhiteSpace(Email)
                && email.Contains("@")
                && !String.IsNullOrWhiteSpace(FirstName)
                && !String.IsNullOrWhiteSpace(Phone)
                && !String.IsNullOrWhiteSpace(Password)
                && !String.IsNullOrWhiteSpace(ConfirmPassword);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        
             private async void OnLoginReturnForUserClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(LoginRedirectUserAccountPage));
        }
        private async void OnRegister()
        {
            var isSuccess = await _apiServices.RegisterAsync(Email,FirstName,Phone, Password, ConfirmPassword);

           
            if (isSuccess)
            {
                Settings.Username = Email;
                Settings.Password = password;
                Message = "C'est bon c'est ajouté";
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            else
            {
                Message = "C'est pas bon";
            }
            // This will pop the current page off the navigation stack
             
        }

        private async void OnRegisterReturnUserAccount()
        {
            var isSuccess = await _apiServices.RegisterAsync(Email, FirstName, Phone, Password, ConfirmPassword);


            if (isSuccess)
            {
                Settings.Username = Email;
                Settings.Password = password;
                Message = "C'est bon c'est ajouté";
                await Shell.Current.GoToAsync(nameof(LoginRedirectUserAccountPage));
            }
            else
            {
                Message = "C'est pas bon";
            }
            // This will pop the current page off the navigation stack

        }
    }
}
