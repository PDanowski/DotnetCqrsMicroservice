using DeviceService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Persistence
{
    public static class EntitiesExtensions
    {
        public static async ValueTask AddAndSave<T>(this DbContext dbContext, T entity, CancellationToken ct)
            where T : notnull
        {
            await dbContext.AddAsync(entity, ct);
            await dbContext.SaveChangesAsync(ct);
        }

        public static ValueTask<T?> Find<T, TId>(this DbContext dbContext, TId id, CancellationToken ct)
            where T : class where TId : notnull
            => dbContext.FindAsync<T>(new object[] { id }, ct);

        public static ValueTask<bool> DevicesWithGivenProfile(this DeviceDbContext dbContext, DeviceProfile deviceProfile, CancellationToken ct)
            => new(dbContext.Set<Device>().AnyAsync(product => product.DeviceProfile.Value == deviceProfile.Value, ct));
    }
}
