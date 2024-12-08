using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfClientApp.Models;

namespace WpfClientApp.Services
{
    public class ContactsApi
    {
        private readonly HttpClient httpClient;
        private static Uri baseAddress;

        public ContactsApi(Uri baseRoute) 
        {
            httpClient = new HttpClient();
            baseAddress = baseRoute;
        }

        public async Task<List<Contact>?> GetContacts()
        {
            var uri = new Uri(baseAddress, "/api/contacts");
            var httpResponseMsg = await httpClient.GetAsync(uri);
            return await httpResponseMsg.Content.ReadFromJsonAsync<List<Contact>?>();            
        }

        public async Task<Contact?> GetContactsById(Guid id)
        {
            var uri = new Uri(baseAddress, "/api/contacts/id");
            var httpResponseMsg = await httpClient.GetAsync(uri);
            return await httpResponseMsg.Content.ReadFromJsonAsync<Contact?>();
        }

        public async Task AddContact(Contact contact)
        {
            var uri = new Uri(baseAddress, "/api/add");

            // !!!обязательно надо указывать кодировку и тип контента, иначе 415 ошибка

            await httpClient.PostAsync(
                requestUri: uri,               
                content: new StringContent(
                    JsonSerializer.Serialize(contact), 
                    Encoding.UTF8, 
                    "application/json"));
        }

        public async Task ChangeContact(Contact contact)
        {
            var uri = new Uri(baseAddress, "/api/change");
            await httpClient.PutAsync(
                requestUri: uri,
                content: new StringContent(
                    JsonSerializer.Serialize(contact),
                    Encoding.UTF8,
                    "application/json"));         
        }

        public async Task DeleteContact(Guid id)
        {
            var uri = new Uri(baseAddress, $"/api/delete/{id}");
            await httpClient.DeleteAsync(uri);           
        }        
    }
}
