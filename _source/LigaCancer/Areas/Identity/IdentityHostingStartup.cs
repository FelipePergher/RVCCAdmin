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