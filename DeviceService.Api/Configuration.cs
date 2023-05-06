using DeviceService.Api.UseCases;
using DeviceService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Api
{
    public static class Configuration
    {
        public static IServiceCollection AddDeviceServices(this IServiceCollection services)
            => services
                .AddDbContext<DeviceDbContext>(
                    options => options.UseSqlServer("name=ConnectionStrings:WarehouseDB"))
                .AddDeviceServices();

        public static IEndpointRouteBuilder UseWarehouseEndpoints(this IEndpointRouteBuilder endpoints)
            => endpoints.UseDevicesEndpoints();

        public static IApplicationBuilder ConfigureWarehouse(this IApplicationBuilder app)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            if (environment == "Development")
            {
                app.ApplicationServices.CreateScope().ServiceProvider
                    .GetRequiredService<DeviceDbContext>().Database.Migrate();
            }

            return app;
        }

        public static IEndpointRouteBuilder UseDevicesEndpoints(this IEndpointRouteBuilder endpoints) =>
            endpoints
                .UseRegisterDeviceEndpoint()
                .UseGetDevicesEndpoint()
                .UseGetDeviceDetailsEndpoint();
    }
}
