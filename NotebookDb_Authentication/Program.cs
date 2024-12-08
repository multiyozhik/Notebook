using _21_NotebookDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using _21_NotebookDb;
using Microsoft.AspNetCore;
using System.Data;
using System;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


var host = BuildWebHost(args);

using (var scope = host.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    Task.Run(() => AddAdminAsync(userManager)).GetAwaiter().GetResult();
}

host.Run();

static IWebHost BuildWebHost(string[] args)
{
    return WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .Build();
}


static async Task AddAdminAsync(UserManager<ApplicationUser> userManager)
{
    var existedAdmin = await userManager.FindByNameAsync("admin");
    if (existedAdmin is not null)
        return;

    var admin = new ApplicationUser() { UserName = "admin", Id = "76a0b401-e1b9-4767-bdab-abfd4d148bd2" };
    var res = await userManager.CreateAsync(admin, "Admin_123");
    await userManager.AddToRoleAsync(admin, "Admin");
}


