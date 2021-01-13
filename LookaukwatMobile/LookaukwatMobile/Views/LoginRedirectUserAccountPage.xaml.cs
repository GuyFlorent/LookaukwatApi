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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginRedirectUserAccountPage : ContentPage
    {
        public LoginRedirectUserAccountPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel(Navigation);
        }
    }
}