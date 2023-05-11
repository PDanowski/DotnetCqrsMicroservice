using Microsoft.VisualStudio.TestPlatform.TestHost;
using static Ogooreck.API.ApiSpecification;
using Ogooreck.API;
using DeviceService.Queries;
using DeviceService.Api.UseCases;

namespace DeviceService.Api.Tests.UseCases
{
    public class GetDeviceDetailsTests : IClassFixture<GetDeviceDetailsFixture>
    {
        private readonly GetDeviceDetailsFixture API;

        public GetDeviceDetailsTests(GetDeviceDetailsFixture api) => API = api;

        [Fact]
        public Task ValidRequest_With_NoParams_ShouldReturn_200() =>
            API.Given(URI($"/api/Devices/{API.ExistingDevice.Id}"))
                .When(GET)
                .Then(OK, RESPONSE_BODY(API.ExistingDevice));

        [Theory]
        [InlineData(12)]
        [InlineData("not-a-guid")]
        public Task InvalidGuidId_ShouldReturn_404(object invalidId) =>
            API.Given(URI($"/api/Devices/{invalidId}"))
                .When(GET)
                .Then(NOT_FOUND);

        [Fact]
        public Task NotExistingId_ShouldReturn_404() =>
            API.Given(URI($"/api/Devices/{Guid.NewGuid()}"))
                .When(GET)
                .Then(NOT_FOUND);
    }

    public class GetDeviceDetailsFixture : ApiSpecification<Program>, IAsyncLifetime
    {
        public DeviceDetails ExistingDevice = default!;

        public GetDeviceDetailsFixture() : base(new WarehouseTestWebApplicationFactory()) { }

        public async Task InitializeAsync()
        {
            var registerDevice = new RegisterDeviceRequest("ABC123", "Description", "1.0.0", "ProfileName");
            var registerResponse = await Send(
                new ApiRequest(POST, URI("/api/Devices"), BODY(registerDevice))
            );

            await CREATED(registerResponse);

            var (name, description, version, profileName) = registerDevice;
            ExistingDevice = new DeviceDetails(registerResponse.GetCreatedId<Guid>(), profileName + version, name!, description);
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
