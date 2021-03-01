using LookaukwatApi.Models;
using LookaukwatApi.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace LookaukwatApi.Services
{
    public class CoordonateService
    {
        public static async Task<ProductCoordinateModel> GetCoodinateAsync(string Town, string Street)
        {
            HttpClient client;

            //var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };

            client = new HttpClient();

            var fullAddress = $"{ Street + Town + ",Cameroun"}";

            var response = await client.GetAsync("https://api.opencagedata.com/geocode/v1/json?q=" + fullAddress + "&key=a196040df44a4a41a471173aed07635c"); ;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var jsonn = await response.Content.ReadAsStringAsync();
            var joo = JObject.Parse(jsonn);
            var latt = (string)joo["results"][0]["geometry"]["lat"];
            var lonn = (string)joo["results"][0]["geometry"]["lng"];


            ProductCoordinateModel coor = new ProductCoordinateModel() { Lat = latt, Lon = lonn };
            return await Task.FromResult(coor);

        }


        public static async Task<TrackingCommandViewModel> GetLocationAsync(string Lat, string Lon)
        {
            HttpClient client;

            //var httpClientHandler = new HttpClientHandler();

            //httpClientHandler.ServerCertificateCustomValidationCallback =
            //(message, cert, chain, errors) => { return true; };

            client = new HttpClient();
            Lat = Lat.Replace(",", ".");
            Lon = Lon.Replace(",", ".");
            var response = await client.GetAsync("https://api.opencagedata.com/geocode/v1/json?q=" + Lat+"+"+Lon + "&key=a196040df44a4a41a471173aed07635c"); ;
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var jsonn = await response.Content.ReadAsStringAsync();
            var joo = JObject.Parse(jsonn);
            var Street = (string)joo["results"][0]["formatted"];
            var Town = (string)joo["results"][0]["components"]["city"];
            var Road = (string)joo["results"][0]["components"]["road"];
            var suburb = (string)joo["results"][0]["components"]["suburb"];


            TrackingCommandViewModel coor = new TrackingCommandViewModel()
            {
                Town = Town,
                Street = Street,
                Road = Road + suburb
            };
            return coor;

        }
    }
}