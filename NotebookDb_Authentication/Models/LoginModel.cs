using System.ComponentModel.DataAnnotations;

namespace _21_NotebookDb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/Home/Index";
    }
}
