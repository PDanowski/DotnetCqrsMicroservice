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
                    options => options.UseSqlServer("name=ConnectionStrings:DeviceDb"))
                .ConfigureDbContext();

        public static IEndpointRouteBuilder UseDevicesServiceEndpoints(this IEndpointRouteBuilder endpoints)
            => endpoints.UseDevicesEndpoints();

        public static IApplicationBuilder ConfigureDevicesService(this IApplicationBuilder app, IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var isTest = configuration.GetValue<bool>("UseInMemoryDb");

            if (environment == "Development" && !isTest)
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
