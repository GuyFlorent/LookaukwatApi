using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LookaukwatMobile.ViewModels
{
    public class PublishProductViewModel : BaseViewModel
    {
        public PublishProductViewModel()
        {
            TitlePage = "Publier une annonce";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}