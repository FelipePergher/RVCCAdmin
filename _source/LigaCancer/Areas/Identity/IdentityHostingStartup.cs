// <copyright file="IdentityHostingStartup.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Hosting;
using RVCC.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace RVCC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}