using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _21_NotebookDb.Api
{
    //[Route("api/[controller]")]
    [Authorize(Policy = "OnlyForAdminRole")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        public UsersModel UserModel { get; }

        public ApiUsersController(UsersModel model)
        {
            UserModel = model;
        }

        [Route("api/users")]    //api users метод
        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            UserModel.UpdateUsers();
            return UserModel.Users;
        }
        
        [Route("api/delete/{id}")]  //!!! тут очень важно параметр id передать
        [HttpPost]                      //  api delete метод
        public async Task Delete(string id)
        {
            await UserModel.DeleteUser(id);
        }
    }
}
