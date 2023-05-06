namespace DeviceService.Core.Models
{
    public readonly record struct DeviceId(Guid Value)
    {
        public static DeviceId From(Guid? deviceId) =>
            new(deviceId.AssertNotEmpty());
    }
}
