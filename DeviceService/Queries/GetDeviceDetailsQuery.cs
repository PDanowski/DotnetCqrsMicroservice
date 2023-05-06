using DeviceService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Queries
{
    public record GetDeviceDetails(DeviceId DeviceId)
    {
        public static GetDeviceDetails From(Guid deviceId) =>
            new(DeviceId.From(deviceId));
    }

    public record DeviceDetails(
        Guid Id,
        string Profile,
        string Name,
        string? Description
    );

    public static class GetDeviceDetailsQuery
    {
        public static async ValueTask<DeviceDetails?> Query(
            this IQueryable<Device> devices,
            GetDeviceDetails query,
            CancellationToken ct
        )
        {
            var device = await devices
                .SingleOrDefaultAsync(p => p.Id == query.DeviceId, ct);

            if (device == null)
                return null;

            return new DeviceDetails(
                device.Id.Value,
                device.DeviceProfile.Value,
                device.Name,
                device.Description
            );
        }
    }
}
