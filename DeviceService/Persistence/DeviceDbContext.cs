using Microsoft.EntityFrameworkCore;

namespace DeviceService.Persistence;
public class DeviceDbContext : DbContext
{
    public DeviceDbContext(DbContextOptions<DeviceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetupDevicesModel();
    }
}