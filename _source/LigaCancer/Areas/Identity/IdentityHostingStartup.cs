using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LigaCancer.Areas.Identity.IdentityHostingStartup))]
namespace LigaCancer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}