using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelExpenses.Data;

namespace TravelExpenses
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //dbContext.Database.Migrate();

                var adminRole = new IdentityRole("Admin");
                var SolRole = new IdentityRole("Solicitante");
                var AproRole = new IdentityRole("Aprobador");
                var ProcRole = new IdentityRole("Procesador");


                //IdentityRole role =await roleMgr.FindByNameAsync("TEST");
                //var result = roleMgr.DeleteAsync(role);

                //roleMgr.DeleteAsync(new IdentityRole("TEST"));
                //roleMgr.DeleteAsync(new IdentityRole("Revisor"));
                //roleMgr.DeleteAsync(new IdentityRole("Usuario"));


                if (!dbContext.Roles.Any())
                {
                    roleMgr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(SolRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(AproRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(ProcRole).GetAwaiter().GetResult();
                   
                }

                if (!dbContext.Users.Any(u => u.UserName == "Administrador"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "Administrador@ozono.com",
                        Email = "Administrador@ozono.com"
                    };
                    var result = userMgr.CreateAsync(adminUser, "Admin123").GetAwaiter().GetResult();
                    userMgr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
