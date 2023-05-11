using DeviceService.Api.UseCases;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Ogooreck.API;
using static Ogooreck.API.ApiSpecification;

namespace DeviceService.Api.Tests.UseCases
{
    public class RegisterDeviceTests : IClassFixture<WarehouseTestWebApplicationFactory>
    {
        private readonly ApiSpecification<Program> API;

        public RegisterDeviceTests(WarehouseTestWebApplicationFactory webApplicationFactory) =>
            API = ApiSpecification<Program>.Setup(webApplicationFactory);

        [Theory]
        [MemberData(nameof(ValidRequests))]
        public Task ValidRequest_ShouldReturn_201(RegisterDeviceRequest validRequest) =>
            API.Given(URI("/api/Devices/"), BODY(validRequest))
                    .When(POST)
                    .Then(CREATED);

        [Theory]
        [MemberData(nameof(InvalidRequests))]
        public Task InvalidRequest_ShouldReturn_400(RegisterDeviceRequest invalidRequest) =>
            API.Given(URI("/api/Devices"), BODY(invalidRequest))
                    .When(POST)
                    .Then(BAD_REQUEST);

        [Fact]
        public async Task RequestForExistingProfileShouldFail_ShouldReturn_409()
        {
            // Given
            var request = new RegisterDeviceRequest(ValidName, ValidDescription, "0.0", "InvalidProfile");

            // first one should succeed
            await API.Given(URI("/api/Devices/"), BODY(request))
                .When(POST)
                .Then(CREATED);

            // second one will fail with conflict
            await API.Given(URI("/api/Devices/"), BODY(request))
                .When(POST)
                .Then(CONFLICT);
        }

        private const string ValidName = "Name1";
        private static string ValidVersion => $"1.0.0";
        private static string ValidProfileName => $"Profile1";

        private const string ValidDescription = "Description1";

        public static TheoryData<RegisterDeviceRequest> ValidRequests = new()
        {
            new RegisterDeviceRequest(ValidName, ValidDescription, ValidVersion, ValidProfileName),
            new RegisterDeviceRequest(ValidName, ValidDescription, ValidVersion,  null!)
        };

        public static TheoryData<RegisterDeviceRequest> InvalidRequests = new()
        {
            new RegisterDeviceRequest(null!, ValidDescription, ValidVersion, ValidProfileName),
            new RegisterDeviceRequest(ValidName, ValidDescription, null!, ValidProfileName),
            new RegisterDeviceRequest(ValidName, null!, ValidVersion, ValidProfileName),
            new RegisterDeviceRequest(ValidName, ValidDescription, ValidVersion, null!),
        };
    }
}
