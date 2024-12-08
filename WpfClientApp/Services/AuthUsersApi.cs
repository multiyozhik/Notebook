using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientApp.ViewModels;

namespace WpfClientApp.Services
{
    class AuthUsersApi
    {
        private readonly HttpClient httpClient;
        private static Uri baseAddress;
        public AuthUsersApi(Uri baseRoute) 
        {
            httpClient = new HttpClient();
            baseAddress = baseRoute;
        }

        public async Task<List<AppUser>?> GetUsers()
        {
            var uri = new Uri(baseAddress, "/api/users");
            var httpResponseMsg = await httpClient.GetAsync(uri);

            if (!httpResponseMsg.IsSuccessStatusCode)
            {
                throw new Exception("Пользователь не является администратором");
            }                    

            return await httpResponseMsg.Content.ReadFromJsonAsync<List<AppUser>?>();
        }

        public async Task<bool> Login(LoginVM loginVM)
        {
            var uri = new Uri(baseAddress, "/api/login");

            var httpResponseMsg = await httpClient.PostAsync(
                requestUri: uri,
                content: new StringContent(
                    JsonSerializer.Serialize(loginVM),
                    Encoding.UTF8,
                    "application/json"));

            return httpResponseMsg.IsSuccessStatusCode == true;
        }

        public async Task<bool> Register(RegisterVM registerVM)
        {
            var uri = new Uri(baseAddress, "api/register");

            var httpResponseMsg = await httpClient.PostAsync(
                requestUri: uri,
                content: new StringContent(
                    JsonSerializer.Serialize(registerVM),
                    Encoding.UTF8,
                    "application/json"));

            return httpResponseMsg.IsSuccessStatusCode == true;
        }

        public async Task Logout()
        {
            var uri = new Uri(baseAddress, "api/logout");

            await httpClient.GetAsync(uri);
        }

        public async Task DeleteUser(string id)
        {
            var uri = new Uri(baseAddress, $"api/delete/{id}");
            await httpClient.PostAsync(
                requestUri: uri,
                content: new StringContent(id));
            return;
        }
    }
}

