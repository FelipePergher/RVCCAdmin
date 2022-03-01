// <copyright file="Startup.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Business.Services;
using RVCC.Data;
using RVCC.Data.Models;
using RVCC.Data.Models.RelationModels;
using RVCC.Data.Repositories;
using RVCC.Services;
using System;
using System.Globalization;

namespace RVCC
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
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

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

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            IMvcBuilder mvcBuilder = services.AddControllersWithViews();

#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif

            services.AddRazorPages();

            services.AddTransient<IEmailSender, EmailSender>(i =>
               new EmailSender(
                   Configuration["EmailSender:Host"],
                   Configuration.GetValue<int>("EmailSender:Port"),
                   Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                   Configuration["EmailSender:UserName"],
                   Configuration["EmailSender:Password"],
                   Configuration["EmailSender:EmailFrom"]));

            services.AddLogging();

            services.AddAntiforgery();

            // Application Services
            services.AddTransient<IDataRepository<Doctor>, DoctorRepository>();
            services.AddTransient<IDataRepository<TreatmentPlace>, TreatmentPlaceRepository>();
            services.AddTransient<IDataRepository<CancerType>, CancerTypeRepository>();
            services.AddTransient<IDataRepository<Medicine>, MedicineRepository>();
            services.AddTransient<IDataRepository<Patient>, PatientRepository>();
            services.AddTransient<IDataRepository<Phone>, PhoneRepository>();
            services.AddTransient<IDataRepository<Address>, AddressRepository>();
            services.AddTransient<IDataRepository<FamilyMember>, FamilyMemberRepository>();
            services.AddTransient<IDataRepository<FileAttachment>, FileAttachmentRepository>();
            services.AddTransient<IDataRepository<Presence>, PresenceRepository>();
            services.AddTransient<IDataRepository<Naturality>, NaturalityRepository>();
            services.AddTransient<IDataRepository<PatientInformation>, PatientInformationRepository>();
            services.AddTransient<IDataRepository<Benefit>, BenefitRepository>();
            services.AddTransient<IDataRepository<PatientBenefit>, PatientBenefitRepository>();
            services.AddTransient<IDataRepository<Stay>, StayRepository>();
            services.AddTransient<IDataRepository<AdminInfo>, AdminInfoRepository>();
            services.AddTransient<IDataRepository<SaleShirt2020>, SaleShirt2020Repository>();
            services.AddTransient<UserResolverService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error/500");
                app.UseHsts();
            }

            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    // Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/error/404";
                    await next();
                }
            });

            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo("pt-BR")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context => context.Context.Response.Headers.Add("Cache-Control", "public, max-age=31536000")
            });
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            loggerFactory.AddLog4Net();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            SeedData.ApplyMigrations(app.ApplicationServices);

            IServiceScopeFactory scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            SeedData.SeedRoles(scopeFactory).Wait();
            SeedData.SeedAdmin(scopeFactory, Configuration["Admin:Email"], Configuration["Admin:Password"]).Wait();
        }
    }
}
