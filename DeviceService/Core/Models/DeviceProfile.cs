
namespace DeviceService.Core.Models
{
    public record DeviceProfile(string Value)
    {
        public static DeviceProfile From(string FirmwareVersion, string Name) =>
            new($"{Name.AssertNotEmpty()}_{FirmwareVersion.AssertNotEmpty()}");
    }
}
