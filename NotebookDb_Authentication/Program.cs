using _21_NotebookDb;
using Microsoft.AspNetCore;


CreateWebHostBuilder(args).Build().Run();

static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
	WebHost.CreateDefaultBuilder(args)
		.UseStartup<Startup>()
		.UseDefaultServiceProvider(options => options.ValidateScopes = false);


