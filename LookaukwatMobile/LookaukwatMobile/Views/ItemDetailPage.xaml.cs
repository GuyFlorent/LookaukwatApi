using System.ComponentModel;
using Xamarin.Forms;
using LookaukwatMobile.ViewModels;

namespace LookaukwatMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}