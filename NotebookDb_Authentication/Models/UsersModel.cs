using Microsoft.AspNetCore.Identity;
using System.Data;


namespace _21_NotebookDb.Models
{
    public record UsersModel(ApplicationDbContext Context, UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> RoleManager)
    {
        public IReadOnlyCollection<UserModel> Users { get; set; } = new List<UserModel>();

		public async Task ChangeRole(string id, bool isAdmin)
		{
			var updatingUser = UserManager.Users.FirstOrDefault(u => u.Id == id);
			if (isAdmin)
                await UserManager.AddToRoleAsync(updatingUser, "Admin");
			else
                await UserManager.RemoveFromRoleAsync(updatingUser, "Admin");
            await Context.SaveChangesAsync();
		}

		public void UpdateUsers()
        {
            var adminRoleId = Context.Roles
                .Where(u => u.Name == "Admin")
                .Select(role => role.Id)
                .First();

            var query =
                from user in Context.Users               
                select new UserModel() 
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    IsAdmin = Context.UserRoles.Any(
                        userRole => userRole.RoleId == adminRoleId && userRole.UserId == user.Id)                   
                };
            Users = query.ToList();            
        }           

        public async Task DeleteUser(string id)
        {
            var deletedUser = UserManager.Users.FirstOrDefault(user => user.Id == id);
            await UserManager.DeleteAsync(deletedUser);
            await Context.SaveChangesAsync();
        }
    }
}
