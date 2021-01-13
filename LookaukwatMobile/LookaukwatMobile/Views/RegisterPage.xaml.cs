using LookaukwatMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LookaukwatMobile.Views
{
    
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();
        }

        //async void OnClick_ButtonLogin(object sender, EventArgs e)
        //{
        //    await Navigation.PushAsync(new LoginPage());
        //}
    }
}