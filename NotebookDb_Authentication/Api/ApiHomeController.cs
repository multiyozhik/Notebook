using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace _21_NotebookDb.Api
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ApiHomeController : ControllerBase
    {
        HomeModel HomeModel { get; }
        public ApiHomeController(HomeModel homeModel)
        {
            HomeModel = homeModel;
        }

        [Route("api/contacts")]
        [AllowAnonymous]
        [HttpGet] //возвращает коллекцию Contacts
        public async Task<IEnumerable<Contact>> GetContacts()
        {
            await HomeModel.UpdateContactsAsync();
            return HomeModel.Contacts;
        }

        [Route("api/contacts/{id}")]
        [AllowAnonymous]
        [HttpGet] //возвращает Contact по id
        public async Task<Contact> GetContactsById(Guid id)
        {
            return await HomeModel.GetContactByIdAsync(id);
        }

        [Route("api/add")]
        [AllowAnonymous]
        [HttpPost] //добавляет новый Contact
        public async Task AddContact([FromBody] Contact contact)
        {
            await HomeModel.AddAsync(contact);
        }

        [Route("api/change")]
        [AllowAnonymous]
        [HttpPut] //изменяет выделенный Contact
        public async Task ChangeContact([FromBody] Contact contact)
        {
            await HomeModel.ChangeAsync(contact);
        }

        [Route("api/delete/{id}")]
        [AllowAnonymous]
        [HttpDelete] //удаляет выделенный Contact
        public async Task Delete(Guid id)
        {
            await HomeModel.DeleteAsync(id);
        }
    }
}
