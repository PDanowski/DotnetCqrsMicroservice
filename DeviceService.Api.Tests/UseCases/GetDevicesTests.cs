using DeviceService.Api.UseCases;
using DeviceService.Queries;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Ogooreck.API;
using static Ogooreck.API.ApiSpecification;

namespace DeviceService.Api.Tests.UseCases
{
    public class GetDevicesTests : IClassFixture<GetDevicesFixture>
    {
        private readonly GetDevicesFixture API;

        public GetDevicesTests(GetDevicesFixture api) =>
            API = api;

        [Fact]
        public Task ValidRequest_With_NoParams_ShouldReturn_200() =>
            API.Given(URI("/api/Devices/"))
                .When(GET)
                .Then(OK, RESPONSE_BODY(API.RegisteredDevices));

        [Fact]
        public Task ValidRequest_With_Filter_ShouldReturn_SubsetOfRecords()
        {
            var registeredDevice = API.RegisteredDevices.First();
            var filter = registeredDevice.Profile;

            return API.Given(URI($"/api/Devices/?filter={filter}"))
                .When(GET)
                .Then(OK, RESPONSE_BODY(new List<DeviceListItem> { registeredDevice }));
        }

        [Fact]
        public Task ValidRequest_With_Paging_ShouldReturn_PageOfRecords()
        {
            // Given
            const int page = 2;
            const int pageSize = 1;
            var pagedRecords = API.RegisteredDevices
                .Skip(page - 1)
                .Take(pageSize)
                .ToList();

            return API.Given(URI($"/api/Devices/?page={page}&pageSize={pageSize}"))
                .When(GET)
                .Then(OK, RESPONSE_BODY(pagedRecords));
        }

        [Fact]
        public Task NegativePage_ShouldReturn_400() =>
            API.Given(URI($"/api/Devices/?page={-20}"))
                .When(GET)
                .Then(BAD_REQUEST);

        [Theory]
        [InlineData(0)]
        [InlineData(-20)]
        public Task NegativeOrZeroPageSize_ShouldReturn_400(int pageSize) =>
            API.Given(URI($"/api/Devices/?pageSize={pageSize}"))
                .When(GET)
                .Then(BAD_REQUEST);
    }


    public class GetDevicesFixture : ApiSpecification<Program>, IAsyncLifetime
    {
        public List<DeviceListItem> RegisteredDevices { get; } = new();

        public GetDevicesFixture() : base(new WarehouseTestWebApplicationFactory()) { }

        public async Task InitializeAsync()
        {
            var DevicesToRegister = new[]
            {
                new RegisterDeviceRequest("ABC123", "Desc1", "1.0", "Profile1"),
                new RegisterDeviceRequest("DEF456", "Desc2", "2.0", "Profile2"),
                new RegisterDeviceRequest("xyz001", "Desc3", "1.0.0.11", "Profile3")
            };

            foreach (var registerDevice in DevicesToRegister)
            {
                var registerResponse = await Send(
                    new ApiRequest(POST, URI("/api/Devices"), BODY(registerDevice))
                );

                await CREATED(registerResponse);

                var createdId = registerResponse.GetCreatedId<Guid>();

                var (name, _, firmware, profileName) = registerDevice;
                RegisteredDevices.Add(new DeviceListItem(createdId, $"{profileName}_{firmware}", name!));
            }
        }

        public Task DisposeAsync() => Task.CompletedTask;
    }
}
