using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelExpenses.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TravelExpenses
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContextPool<TravelExpensesContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TravelExDb"));
            });
            services.AddScoped<IEmpresa, EmpresaDA>();
            services.AddScoped<IEstado, SqlEstadoData>();
            services.AddScoped<IUbicacion, UbicacionDA>();
            services.AddScoped<IDepartamento, DepartamentoDA>();
            services.AddScoped<ISolicitudes, SolicitudesDA>();
            services.AddScoped<IGasto, GastoDA>();
            services.AddScoped<IDestinos, DestinosDA>();
            services.AddScoped<ICentroCosto, CentroCostoDA>();
            services.AddScoped<IMoneda, MonedaDA>();
            services.AddScoped<ICentroCostoEmpresa, CentroCostoEmpresaDA>();
            services.AddScoped<IRembolso, RembolsoDA>();
            services.AddScoped<IComprobante, ComprobanteDA>();
            services.AddScoped<IPolitica, PoliticaDA>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.                
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("TravelExDb")));

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
             {
                // Password settings.
                options.Password.RequireDigit = true;
                 options.Password.RequireLowercase = false;
                 options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireUppercase = false;
                 options.Password.RequiredLength = 6;
                 options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                 options.Lockout.MaxFailedAccessAttempts = 100;
                 options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                 options.User.RequireUniqueEmail = false;
                 //options.SignIn.RequireConfirmedEmail = true;
             })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAccess", policy => policy.RequireRole("Admin"));

                options.AddPolicy("ManagerAccess", policy =>
                    policy.RequireAssertion(context =>
                                context.User.IsInRole("Admin")
                                || context.User.IsInRole("Manager")));

                options.AddPolicy("UserAccess", policy =>
                    policy.RequireAssertion(context =>
                                context.User.IsInRole("Admin")
                                || context.User.IsInRole("Manager")
                                || context.User.IsInRole("User")));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseSession();
             
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=ListarSolicitudes}/{id?}");
            });
        }
    }
}
