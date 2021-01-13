using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LookaukwatMobile.Models;
using LookaukwatMobile.Services;
using Xamarin.Forms;

namespace LookaukwatMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        ApiServices _apiServices = new ApiServices();
        private string itemId;
        private int price;
        private string title;
        private string description;
        ObservableCollection<string> images = new ObservableCollection<string>();
        public ObservableCollection<string> Images { get => images; set => SetProperty(ref images, value); }
        public int Id { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var id = Convert.ToInt32(itemId);
                var item = await _apiServices.GetUniqueJobAsync(id);
                Id = item.id;
                Title = item.Title;
                Description = item.Description;
                var img = item.Images.Select(s => s.ImageMobile);
                foreach(var im in img)
                {
                    images.Add(im);
                }
                price = item.Price;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
