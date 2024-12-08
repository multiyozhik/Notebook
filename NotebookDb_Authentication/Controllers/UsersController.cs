using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _21_NotebookDb.Controllers
{
    [Authorize(Policy = "OnlyForAdminRole")]
    public class UsersController: Controller
    {
        public UsersModel UserModel { get; }        

        public UsersController(UsersModel model)
        { 
            UserModel = model;
        }
                
        [HttpGet] //вывод списка пользователей (UserName, IsAdmin, возможности DeleteUser)
        public IActionResult Index()
        {
            UserModel.UpdateUsers();
            return View(UserModel);
        }
                
        [HttpPost] //удаление пользователя администратором
        public async Task<IActionResult> DeleteUser(string id)
        {
            await UserModel.DeleteUser(id);            
            return RedirectToAction("Index");
        }
    }
}
