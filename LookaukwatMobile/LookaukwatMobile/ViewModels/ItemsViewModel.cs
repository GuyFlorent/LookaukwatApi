using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using LookaukwatMobile.Models;
using LookaukwatMobile.Views;
using LookaukwatMobile.Services;
using LookaukwatMobile.Helpers;
using Xamarin.Forms.Extended;
using System.Collections.Generic;

namespace LookaukwatMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
         private ProductForMobileViewModel _selectedItem;
      
        
        ApiServices _apiServices = new ApiServices();
       
        public Command LoadItemsCommand { get; }
        // public Command AddItemCommand { get; }
         public Command<ProductForMobileViewModel> ItemTapped { get; }

        // public ItemsViewModel()
        // {

        //     var accessToken = Settings.AccessToken;
        //     TitlePage = "Accueil de lookaukwat";
        //     //Items = new ObservableCollection<ProductModel>();
        //     LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        //     ItemTapped = new Command<Item>(OnItemSelected);
        
        //     AddItemCommand = new Command(OnAddItem);

        //     Items = new InfiniteScrollCollection<ProductModel>
        //     {
        //         OnLoadMore = async () =>
        //         {
        //             IsRefressing = true;
        //             //Load next page
        //             var page = Items.Count / PageSize;
        //             var items = await _apiServices.GetProductsAsync(accessToken, page, PageSize, true);
        //             IsRefressing = false;
        //             return items;
        //         },
        //         OnCanLoadMore = () =>
        //         {
        //             return Items.Count < _apiServices.Get_AllNumber_ProductsAsync(accessToken).Result;
        //         }
        //     };

        // }


        async Task ExecuteLoadItemsCommand()
        {
            IsRefressing = true;

            try
            {
                Items.Clear();
               
                var items = await _apiServices.GetProductsAsync( pageIndex: 0, pageSize: PageSize);
                Items.AddRange(items);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefressing = false;
            }
        }

        //public void OnAppearing()
        //{
        //    IsRefressing = true;
        //    SelectedItem = null;
        //}

        public ProductForMobileViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        // private async void OnAddItem(object obj)
        // {
        //     await Shell.Current.GoToAsync(nameof(NewItemPage));
        // }

        async void OnItemSelected(ProductForMobileViewModel item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.id}");
        }



        private const int PageSize = 10;
      

        public InfiniteScrollCollection<ProductForMobileViewModel> Items { get; }

        public ItemsViewModel()
        {
            TitlePage = "Lookaukwat";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<ProductForMobileViewModel>(OnItemSelected);
            Items = new InfiniteScrollCollection<ProductForMobileViewModel>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // load the next page
                    var page = Items.Count / PageSize;

                    var items = await _apiServices.GetProductsAsync(page, PageSize);

                    IsBusy = false;

                    // return the items that need to be added
                    return items;
                },
                OnCanLoadMore = () =>
                {
                    return Items.Count < 350;
                }
            };

            DownloadDataAsync();
        }

        private async Task DownloadDataAsync()
        {
            var items = await _apiServices.GetProductsAsync(pageIndex: 0, pageSize: PageSize);

            Items.AddRange(items);
        }

    }
}