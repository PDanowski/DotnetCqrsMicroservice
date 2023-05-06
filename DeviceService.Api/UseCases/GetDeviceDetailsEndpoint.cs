using DeviceService.Core.Models;
using DeviceService.Queries;
using static Microsoft.AspNetCore.Http.Results;

namespace DeviceService.Api.UseCases
{
    internal static class GetDeviceDetailsEndpoint
    {
        internal static IEndpointRouteBuilder UseGetDeviceDetailsEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(
                    "/api/devices/{id:guid}",
                    async (
                        IQueryable<Device> queryable,
                        Guid id,
                        CancellationToken ct
                    ) =>
                    {
                        var query = GetDeviceDetails.From(id);
                        var result = await queryable.Query(query, ct);
                        return result != null ? Ok(result) : NotFound();
                    })
                .Produces<Device?>()
                .Produces(StatusCodes.Status400BadRequest);

            return endpoints;
        }
    }
}
