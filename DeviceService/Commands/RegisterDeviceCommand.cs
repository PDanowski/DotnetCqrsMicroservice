using DeviceService.Core;
using DeviceService.Core.Models;

namespace DeviceService.Commands
{
    public static class RegisterDeviceHandler
    {
        public static async Task Handle(
            Func<Device, CancellationToken, ValueTask> addDevice,
            Func<DeviceProfile, CancellationToken, ValueTask<bool>> deviceWithGivenProfile,
            RegisterDevice command,
            CancellationToken ct
        )
        {
            var device = new Device(
                command.DeviceId,
                command.DeviceProfile,
                command.Name,
                command.Description
            );

            if (await deviceWithGivenProfile(command.DeviceProfile, ct))
                throw new InvalidOperationException(
                    $"Device with given profile`{command.DeviceProfile.Value} already exists.");

            await addDevice(device, ct);
        }
    }

    public record RegisterDevice(
        DeviceId DeviceId,
        DeviceProfile DeviceProfile,
        string Name,
        string? Description
    )
    {
        public static RegisterDevice From(Guid? id, string firmwareVersion, string profileName, string name, string? description) =>
            new(
                DeviceId.From(id),
                DeviceProfile.From(firmwareVersion, profileName),
                name.AssertNotEmpty(),
                description.AssertNullOrNotEmpty()
            );
    }
}
