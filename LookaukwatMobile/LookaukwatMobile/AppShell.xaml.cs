using System;
using System.Collections.Generic;
using LookaukwatMobile.ViewModels;
using LookaukwatMobile.Views;
using Xamarin.Forms;

namespace LookaukwatMobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent(); 
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(PublishProductPage), typeof(PublishProductPage));
            Routing.RegisterRoute(nameof(PublishAnnouncePage), typeof(PublishAnnouncePage));
            Routing.RegisterRoute(nameof(UserProfilePage), typeof(UserProfilePage));
            Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(JobAddPage), typeof(JobAddPage));
            Routing.RegisterRoute(nameof(LoginRedirectUserAccountPage), typeof(LoginRedirectUserAccountPage));
            Routing.RegisterRoute(nameof(RegisterRedirectLoginUserAccountPage), typeof(RegisterRedirectLoginUserAccountPage));
            
        }

    }
}
