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
    public partial class PublishAnnouncePage : ContentPage
    {
        public PublishAnnouncePage()
        {
            InitializeComponent();
            //picker.ItemsSource = StaticListViewModel.OfferOSearchList;
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var selectedItem = picker.SelectedItem; // This is the model selected in the picker
        }
    }
}