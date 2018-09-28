using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LigaCancer.Data.Models;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Application Services
            services.AddTransient<IDataStore<Doctor>, DoctorStore>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.ApplyMigrations(app.ApplicationServices);

            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            SeedData.SeedRoles(scopeFactory).Wait();
            SeedData.SeedAdminUser(scopeFactory).Wait();
        }
    }

    public static class SeedData
    {
        private static readonly string[] Roles = new string[] { "Admin", "User" };

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public static async Task SeedRoles(IServiceScopeFactory scopeFactory)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!dbContext.UserRoles.Any())
                {
                    RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole
                            {
                                Name = role

                            });
                        }
                    }
                }
            }
        }

        public static async Task SeedAdminUser(IServiceScopeFactory scopeFactory)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!userManager.GetUsersInRoleAsync("Admin").Result.Any())
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        RegisterDate = DateTime.Now,
                        CreatedBy = "System"
                    };
                    IdentityResult result = await userManager.CreateAsync(user, "Admin123!");
                    if (result.Succeeded)
                    {
                        IdentityRole applicationRole = await roleManager.FindByNameAsync("Admin");
                        if (applicationRole != null)
                        {
                            IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        }
                    }
                }
            }
        }
    }

}
