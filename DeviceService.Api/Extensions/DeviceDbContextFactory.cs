using DeviceService.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Api.Extensions
{
    public class DeviceDbContextFactory : IDesignTimeDbContextFactory<DeviceDbContext>
    {
        public DeviceDbContext CreateDbContext(params string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeviceDbContext>();

            if (optionsBuilder.IsConfigured)
                return new DeviceDbContext(optionsBuilder.Options);

            //Called by parameterless ctor Usually Migrations
            var environmentName = Environment.GetEnvironmentVariable("EnvironmentName") ?? "Development";

            var connectionString =
                new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false)
                    .AddEnvironmentVariables()
                    .Build()
                    .GetConnectionString("WarehouseDB");

            optionsBuilder.UseSqlServer(connectionString);

            return new DeviceDbContext(optionsBuilder.Options);
        }
    }
}
