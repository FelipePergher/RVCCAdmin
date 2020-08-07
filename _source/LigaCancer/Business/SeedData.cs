// <copyright file="SeedData.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RVCC.Data;
using RVCC.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Business
{
    public static class SeedData
    {
        private static readonly string[] Roles = { Business.Roles.Admin, Business.Roles.User, Business.Roles.SocialAssistance };

        public static void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using (IServiceScope serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public static async Task SeedRoles(IServiceScopeFactory scopeFactory)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                foreach (string role in Roles)
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

        public static async Task SeedAdminUser(IServiceScopeFactory scopeFactory, string email, string password)
        {
            using (IServiceScope serviceScope = scopeFactory.CreateScope())
            {
                UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                IList<ApplicationUser> applicationUsers = await userManager.GetUsersInRoleAsync(Business.Roles.Admin);
                if (!applicationUsers.Any())
                {
                    var user = new ApplicationUser
                    {
                        Name = "Felipe Pergher",
                        EmailConfirmed = true,
                        UserName = email,
                        Email = email,
                        RegisterTime = DateTime.Now,
                        CreatedBy = "System"
                    };

                    IdentityResult result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        IdentityRole applicationRole = await roleManager.FindByNameAsync(Business.Roles.Admin);
                        if (applicationRole != null)
                        {
                            await userManager.AddToRoleAsync(user, applicationRole.Name);
                        }
                    }
                }
            }
        }
    }
}
