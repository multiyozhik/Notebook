using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace _21_NotebookDb.Controllers
{
    //в HomeController в конструктор подкладываем HomeModel и
    //следим за соответствием названий методов контроллера и названий View соотв. папки контроллера
    [Authorize]
    public class HomeController : Controller
    {
        HomeModel HomeModel { get; }
        public HomeController(HomeModel homeModel)
        {
            HomeModel = homeModel;
        }       


        [AllowAnonymous]
        [HttpGet] //передача в главную страницу модели для отображения ее с-ва Contacts
        public async Task<IActionResult> Index()
        {
            await HomeModel.UpdateContactsAsync();
            return View(HomeModel);
        }

        
        [HttpGet] //передача id чз строку запроса Home/GetContactInfo/@person.Id        
        public async Task<IActionResult> GetContactInfo(Guid id)
            => View(await HomeModel.GetContactByIdAsync(id));

        
        [HttpGet]
        public IActionResult GetCreatingContactView() => View();

        
        [HttpPost] //передача объекта типа Contact чз форму        
        public async Task<IActionResult> Create(Contact contact)
        {
            if (IsValid(contact, out string errorMessage))
            {
                await HomeModel.AddAsync(contact);
                return RedirectToAction("index");
            }
            else
            {
                return Content(errorMessage);
            }
        }

        [Authorize(Policy = "OnlyForAdminRole")]
        [HttpGet] //передача id чз строку запроса Home/GetChangingContactView/@person.Id        
        public async Task<IActionResult> GetChangingContactView(Guid id)
            => View(await HomeModel.GetContactByIdAsync(id));

        //передача id из запроса и передача объекта типа Contact чз форму
        //здесь нужно обязательно вытаскивать id и передавать из запроса вручную в изменяемый контакт,
        //т.к. если параметром прокидывать только контакт, то Guid id нулевой и
        //соответственно дальше в HomeModel.Change не находит контакт с id 

        [Authorize(Policy = "OnlyForAdminRole")]
        [HttpPost]       
        public async Task<IActionResult> Change([FromQuery] Guid id, [FromForm] Contact newDataofChangingContact)
        {
            if (IsValid(newDataofChangingContact, out string errorMessage))
            {
                await HomeModel.ChangeAsync(newDataofChangingContact with { Id = id });
                return RedirectToAction("Index");
            }
            else
            {
                return Content(errorMessage);
            }
        }

        [Authorize(Policy = "OnlyForAdminRole")]
        [HttpPost] //передача id чз строку запроса Home/DeleteContact/@person.Id
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            await HomeModel.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [NonAction]
        public bool IsValid(Contact contact, out string errorMessages)
        {
            //здесь некоторые специфические проверки на стороне сервера приходящих данных с формы 
            if (contact.FamilyName?.Length < 3)
                ModelState.AddModelError(
                    "FamilyName",
                    "Фамилия должна содержать не менее 3 символов");
            if (contact.Adress is not null && contact.Adress.Length < 15)
                ModelState.AddModelError(
                    "Adress",
                    "Адрес должен содержать не менее 15 символов");
            string pattern = @"\+7\(\d{3}\)\d{3}-\d{4}";
            if (!Regex.IsMatch(contact.PhoneNumber, pattern))
                ModelState.AddModelError(
                    "PhoneNymber",
                    "Номер телефона должен соответствовать шаблону +7(ххх)ххх-хххх");

            errorMessages = "";
            if (!ModelState.IsValid) //при Invalid словаря ModelState пробегаем по всем его эл-там
            {
                foreach (var property in ModelState)
                {
                    if (property.Value.ValidationState == ModelValidationState.Invalid)
                    {
                        errorMessages = $"Ошибка в свойстве {property.Key}\n";
                        foreach (var error in property.Value.Errors)
                            errorMessages = $"{errorMessages} {error.ErrorMessage}\n";
                    }
                }
                return false;
            }
            else return true;
        }
    }
}
