using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _21_NotebookDb.Api
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ApiAuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ApiAuthenticateController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }       

        [Route("api/login")] 
        [HttpPost]
        // !!![FromBody] важно, иначе null в свойствах модели, т.к. где-то в FromForm искал
        public async Task<StatusCodeResult> ApiLogin([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(model.Username,
                    model.Password,
                    false,
                    lockoutOnFailure: false);

                if (!loginResult.Succeeded)
                    return new StatusCodeResult(401);
            }
            return new StatusCodeResult(200);
        }        

        [Route("api/register")] 
        [HttpPost]
        public async Task<StatusCodeResult> ApiRegister([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (!createResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return new StatusCodeResult(401);
                }
            }
            return new StatusCodeResult(200);
        }

        [Route("api/logout")]
        [HttpGet, ValidateAntiForgeryToken]
        public async Task LogOut()  
        {
            await _signInManager.SignOutAsync();
        }
    }
}
