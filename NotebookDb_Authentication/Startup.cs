using _21_NotebookDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _21_NotebookDb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContactsDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("ContactsConString")));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("AppConString")));
            services.AddTransient<HomeModel>();
            services.AddTransient<UsersModel>();

            //System.InvalidOperationException: "Endpoint Routing does not support
            //'IApplicationBuilder.UseMvc(...)'. To use 'IApplicationBuilder.UseMvc'
            //set 'MvcOptions.EnableEndpointRouting = false' inside 'ConfigureServices(...)."
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6; // минимальное количество знаков в пароле
                options.Lockout.MaxFailedAccessAttempts = 10; // количество попыток о блокировки
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // конфигурация Cookie с целью использования их для хранения авторизации
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                //options.Cookie.Expiration = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Authenticate/Login";
                options.LogoutPath = "/Authenticate/Logout";
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "OnlyForAdminRole", 
                    policy => policy.RequireRole("Admin"));
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");                    
            });

            ConfigureRoles(userManager, roleManager);
        }

        public void ConfigureRoles(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
