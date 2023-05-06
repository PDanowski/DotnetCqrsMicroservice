using DeviceService.Core;
using DeviceService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Queries
{
    public static class GetDevicesQuery
    {
        public static async ValueTask<IReadOnlyList<DeviceListItem>> Query(
            this IQueryable<Device> products,
            GetDevices query,
            CancellationToken ct
        )
        {
            var (filter, page, pageSize) = query;

            var filteredDevices = string.IsNullOrEmpty(filter)
                ? products
                : products
                    .Where(p =>
                        p.DeviceProfile.Value.Contains(query.Filter!) ||
                        p.Name.Contains(query.Filter!) ||
                        p.Description!.Contains(query.Filter!)
                    );

            return await filteredDevices
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Select(p => new DeviceListItem(p.Id.Value, p.DeviceProfile.Value, p.Name))
                .ToListAsync(ct);
        }
    }

    public record GetDevices(string? Filter, int Page, int PageSize)
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;

        public static GetDevices From(string? filter, int? page, int? pageSize) =>
            new(
                filter,
                (page ?? DefaultPage).AssertPositive(),
                (pageSize ?? DefaultPageSize).AssertPositive()
            );
    }

    public record DeviceListItem(
        Guid Id,
        string Profile,
        string Name
    );
}
