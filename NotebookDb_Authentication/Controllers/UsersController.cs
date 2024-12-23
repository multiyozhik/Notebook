using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _21_NotebookDb.Controllers
{
    [Authorize(Policy = "OnlyForAdminRole")]
    public class UsersController: Controller
    {
        public UsersModel UsersModel { get; }        

        public UsersController(UsersModel model)
        { 
            UsersModel = model;
        }
                
        [HttpGet] //вывод списка пользователей (UserName, IsAdmin, возможности DeleteUser)
        public IActionResult Index()
        {
            this.UsersModel.UpdateUsers();
            return View(UsersModel);
        }

		[HttpPost] //изменение роли администратом по checkbox
		public async Task<IActionResult> ChangeRole(string id, bool isAdmin)
		{
			await UsersModel.ChangeRole(id, isAdmin);
            return RedirectToAction("Index");
		}

		[HttpPost] //удаление пользователя администратором
        public async Task<IActionResult> DeleteUser(string id)
        {
            await UsersModel.DeleteUser(id);            
            return RedirectToAction("Index");
        }
    }
}
