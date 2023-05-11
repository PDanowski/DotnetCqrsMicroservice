using DeviceService.Core.Models;
using DeviceService.Queries;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.Results;

namespace DeviceService.Api.UseCases
{
    internal static class GetDevicesEndpoint
    {
        internal static IEndpointRouteBuilder UseGetDevicesEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints
                .MapGet(
                    "/api/devices",
                    async (
                        IQueryable<Device> devices,
                        [FromQuery] string? filter,
                        [FromQuery] int? page,
                        [FromQuery] int? pageSize,
                        CancellationToken cancellationToken
                    ) =>
                    {
                        var query = GetDevices.From(filter, page, pageSize);
                        var result = await devices.Query(query, cancellationToken);
                        return Ok(result);
                    })
                .Produces<IReadOnlyList<Device>>()
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            return endpoints;
        }
    }
}
