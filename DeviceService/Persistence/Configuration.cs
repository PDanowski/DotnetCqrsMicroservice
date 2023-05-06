using DeviceService.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceService.Persistence
{
    public static class Configuration
    {
        public static IServiceCollection AddDeviceServices(this IServiceCollection services)
            => services
                .AddTransient(sp =>
                    sp.GetRequiredService<DeviceDbContext>().Set<Device>().AsNoTracking()
                );


        public static void SetupDevicesModel(this ModelBuilder modelBuilder)
        {
            var device = modelBuilder.Entity<Device>();

            device
                .Property(e => e.Id)
                .HasConversion(
                    typed => typed.Value,
                    plain => new DeviceId(plain)
                );

            device
                .OwnsOne(e => e.DeviceProfile);
        }
    }
}
