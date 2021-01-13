using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LookaukwatMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadImagePage : ContentPage
    {
        private MediaFile _mediaFile;
        public UploadImagePage()
        {
            InitializeComponent();


        }

       private async void PickPhoto_Click(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No pick photo", ":(No Pick photo available.", "ok");
                return;
            }

            _mediaFile = await CrossMedia.Current.PickPhotoAsync();
            if (_mediaFile == null)
                return;
            LocalPathLabel.Text = _mediaFile.Path;
            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }

        private async void TakePhoto_Click(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No pick photo", ":(No Pick photo available.", "ok");
                return;
            }
       
            _mediaFile = await CrossMedia.Current.TakePhotoAsync( new StoreCameraMediaOptions 
            { 
                PhotoSize = PhotoSize.Medium,
                CompressionQuality =40,
                Directory = "Sample",
                Name = "MyImage.jpg"
            }
                );
            if (_mediaFile == null)
                return;
            LocalPathLabel.Text = _mediaFile.Path;
            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }

        private async void UploadPhoto_Click(object sender, EventArgs e)
        {
            var content = new MultipartFormDataContent();
           
            content.Add(new StreamContent(_mediaFile.GetStream()),
                "\"file\"",
                $"\"{_mediaFile.Path}\"");

            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient();

            var uploadServiceBaseAdress = "https://192.168.1.66:45455/api/Product/UploadImages";
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var httpResponseMessage = await client.PostAsync(uploadServiceBaseAdress, content);
            RemotePathLabel.Text = await httpResponseMessage.Content.ReadAsStringAsync();

        }
        
    }
}
