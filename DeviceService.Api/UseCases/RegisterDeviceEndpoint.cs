using DeviceService.Commands;
using DeviceService.Persistence;
using static Microsoft.AspNetCore.Http.Results;
using static DeviceService.Commands.RegisterDeviceHandler;

namespace DeviceService.Api.UseCases
{
    internal static class RegisterDeviceEndpoint
    {
        internal static IEndpointRouteBuilder UseRegisterDeviceEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(
                    "api/devices/",
                    async (
                        DeviceDbContext dbContext,
                        RegisterDeviceRequest request,
                        CancellationToken ct
                    ) =>
                    {
                        var (name, description, firmwareVersion, profileName) = request;
                        var deviceId = Guid.NewGuid();
                        var command = RegisterDevice.From(deviceId, firmwareVersion, profileName, name, description);
                        await Handle(
                            dbContext.AddAndSave,
                            dbContext.DevicesWithGivenProfile,
                            command,
                            ct
                        );
                        return Created($"/api/devices/{deviceId}", deviceId);
                    })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            return endpoints;
        }
    }

    public record RegisterDeviceRequest(
        string Name,
        string? Description,
        string FirmwareVersion,
        string ProfileName
    );
}
