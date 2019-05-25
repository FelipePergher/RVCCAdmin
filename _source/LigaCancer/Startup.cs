using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddLogging();
            
            services.AddAntiforgery();

            //Application Services
            services.AddTransient(typeof(ISpecification<>), typeof(BaseSpecification<>));
            services.AddTransient<IDataStore<Doctor>, DoctorStore>();
            services.AddTransient<IDataStore<TreatmentPlace>, TreatmentPlaceStore>();
            services.AddTransient<IDataStore<CancerType>, CancerTypeStore>();
            services.AddTransient<IDataStore<Medicine>, MedicineStore>();
            services.AddTransient<IDataStore<Patient>, PatientStore>();
            services.AddTransient<IDataStore<Phone>, PhoneStore>();
            services.AddTransient<IDataStore<Address>, AddressStore>();
            services.AddTransient<IDataStore<FamilyMember>, FamilyMemberStore>();
            services.AddTransient<IDataStore<FileAttachment>, FileAttachmentStore>();
            services.AddTransient<IDataStore<Presence>, PresenceStore>();
            services.AddTransient<IDataStore<Naturality>, NaturalityStore>();
            services.AddTransient<IDataStore<PatientInformation>, PatientInformationStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            loggerFactory.AddLog4Net();

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

}
