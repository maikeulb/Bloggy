using System;
using Bloggy.API.Data;
using Bloggy.API.Data.Seed;
using Bloggy.API.Services.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bloggy.API
{
    public class Program
    {
        public static void Main (string[] args)
        {
            var host = BuildWebHost (args);
            using (var scope = host.Services.CreateScope ())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var bloggyContext = services.GetRequiredService<BloggyContext> ();
                    var passwordHasher = services.GetRequiredService<IPasswordHasher> ();
                    var bloggyContextInitializerLogger = services.GetRequiredService<ILogger<BloggyContextInitializer>> ();
                    BloggyContextInitializer.Initialize (bloggyContext, passwordHasher, bloggyContextInitializerLogger).Wait ();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>> ();
                    logger.LogError (ex, "An error occurred while seeding the database.");
                }
            }
            host.Run ();
        }

        public static IWebHost BuildWebHost (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseKestrel()
            .UseUrls($"http://+:5000")
            .UseStartup<Startup> ()
            .Build ();
    }
}
