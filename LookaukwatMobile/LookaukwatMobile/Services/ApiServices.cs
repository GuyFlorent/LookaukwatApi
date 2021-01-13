using LookaukwatMobile.Models;
using LookaukwatMobile.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LookaukwatMobile.Services
{
    public class ApiServices
    {
        public async Task<bool> RegisterAsync(string email, string firstName, string phone, string password, string confirmPassword)
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);


            var model = new RegisterBindingModel()
            {
                Email = email,
                FirstName = firstName,
                Phone = phone,
                Password = password,
                ConfirmPassword = confirmPassword

            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync("https://192.168.0.130:45455/api/Account/Register", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LogoutASync(string accessToken)
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);
            HttpContent content = new StringContent(null);
            var response = await client.PostAsync("https://192.168.0.130:45455/api/Account/Logout", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> LoginASync(string email, string password)
        {
            string body = "grant_type=password&username=" + email + "&password=" + password;
            //var keyValues = new List<KeyValuePair<string, string>>()
            //{
            //    new KeyValuePair<string, string>("username",email),
            //    new KeyValuePair<string, string>("password",password),
            //    new KeyValuePair<string, string>("grand_type","password")
            //};
            var request = new HttpRequestMessage(HttpMethod.Post, "https://192.168.0.130:45455/Token");
            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            //request.Content = new FormUrlEncodedContent(keyValues);

            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);

            var response = await client.SendAsync(request);
            var jwt = await response.Content.ReadAsStringAsync();
            var joo = JObject.Parse(jwt);
            var accessToken = (string)joo["access_token"];

            //var accessToken = jwtDynamic.Value<string>("access_token");

           Debug.WriteLine(jwt);

            return accessToken;
        }

        public async Task<int> JobPostAsync(string accessToken, string titleJob, string description, string town, string street, int price, 
            string searchOrAskJob, string typeContract, string activitySector)
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);
            CategoryModel categorie = new CategoryModel() { CategoryName = "Emploi" };

            var model = new JobModel()
            {
                Title = titleJob,
                Description = description,
                Town = town,
                Street = street,
                Price = price,
                SearchOrAskJob = searchOrAskJob,
                TypeContract = typeContract,
                ActivitySector = activitySector,
                Category = categorie,
                DateAdd = DateTime.Now
                
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);
            var response = await client.PostAsync("https://192.168.0.130:45455/api/JobModels", content);
            Debug.WriteLine(response);
            var jwt = await response.Content.ReadAsStringAsync();
            var joo = JObject.Parse(jwt);
            var id = (int)joo["id"];

            //var accessToken = jwtDynamic.Value<string>("access_token");

            Debug.WriteLine(jwt);

            return id;
        }

        public async Task<List<ProductForMobileViewModel>> GetProductsAsync( int pageIndex, int pageSize)
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);

           // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);

            var json = await client.GetStringAsync("https://192.168.0.130:45455/api/Product");
           
            var List = JsonConvert.DeserializeObject<List<ProductForMobileViewModel>>(json);
            var liste = List.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            return liste;

        }

        public async Task<int> Get_AllNumber_ProductsAsync()
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);

            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);

            var json = await client.GetStringAsync("https://192.168.1.66:45455/api/Product");

            var List = JsonConvert.DeserializeObject<List<ProductModel>>(json);
            
            return List.Count;

        }


        public async Task<JobModel> GetUniqueJobAsync(int id)
        {
            HttpClient client;

            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            client = new HttpClient(httpClientHandler);

          
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", accessToken);

            var json = await client.GetStringAsync("https://192.168.0.130:45455/api/Product/?id=" + id);

            var Job = JsonConvert.DeserializeObject<JobModel>(json);
           
            return Job;

        }

    }
}
