using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace _21_NotebookDb.Models
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
           
        }

		public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
		{
			UserManager<ApplicationUser> userManager =
				serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			RoleManager<IdentityRole> roleManager =
				serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			string? username = configuration["AdminUser:Name"];
			string? password = configuration["AdminUser:Password"];
			string? role = configuration["AdminUser:Role"];

			if (username is null || password is null || role is null)
				throw new Exception("Ошибка конфигурации администратора");

			//если пользов. с таким именем не было, то создаем вначале роль админ.,
			//затем созд. user и его учетную запись и доб. роль админа 
			if (await userManager.FindByNameAsync(username) is null)
			{
				if (await roleManager.FindByNameAsync(role) == null)
				{ 
					await roleManager.CreateAsync(new IdentityRole(role)); 
				}
				var user = new ApplicationUser { UserName = username };
				var result = await userManager.CreateAsync(user, password);
				if (result.Succeeded) 
				{ 
					await userManager.AddToRoleAsync(user, role); 
				}
			}
		}
	}
}
