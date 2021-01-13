
using LookaukwatMobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LookaukwatMobile.ViewModels
{
    public class PublishAnnounceViewModel : BaseViewModel
    {
        public Command GetCategoryCommand { get; set; }
        private string category;
        public IList<string> liste { get; } 
        public PublishAnnounceViewModel()
        {
            GetCategoryCommand = new Command(OnSelectCategory);
            liste = StaticListViewModel.GetCategoryList;
            
        }
        public string Categori
        {
            get { return category; }
            set
            {

                SetProperty(ref category, value);

            }
        }

        async void OnSelectCategory()
        {
            switch (Categori)
            {
                case "Emploi":
                    await Shell.Current.GoToAsync(nameof(JobAddPage));
                    break;
                case "Immobilier":
                    break;
                case "Multimédia":
                    break;
                case "Véhicule":
                    break;
                case "Mode":
                    break;
                case "Maison":
                    break;
               
            }
        }
    }
}
