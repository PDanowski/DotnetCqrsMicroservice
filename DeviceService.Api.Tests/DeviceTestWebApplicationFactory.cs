using DeviceService.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DeviceService.Api.Tests
{
    public class WarehouseTestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services
                    .AddTransient(s =>
                    {
                        var options = new DbContextOptionsBuilder<DeviceDbContext>();
                        options.UseInMemoryDatabase($"DevicesDb");
                        return options.Options;
                    });
            });

            var host = base.CreateHost(builder);

            return host;
        }
    }
}
