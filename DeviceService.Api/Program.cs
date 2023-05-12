using DeviceService.Api;
using DeviceService.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRouting()
    .AddDeviceServices()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandlingMiddleware()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.UseDevicesServiceEndpoints();
    })
    .ConfigureDevicesService(app.Configuration)
    .UseSwagger()
    .UseSwaggerUI();

app.Run();

namespace DeviceService.Api
{
    public partial class Program
    {
    }
}