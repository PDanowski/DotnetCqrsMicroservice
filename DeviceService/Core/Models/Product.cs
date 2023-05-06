using System.Diagnostics.CodeAnalysis;

namespace DeviceService.Core.Models
{
    public record Device
    {
        public required DeviceId Id { get; init; }

        /// <summary>
        /// DeviceProfile assigned to the device.
        /// </summary>
        /// <returns></returns>
        public required DeviceProfile DeviceProfile { get; init; }

        /// <summary>
        /// Device Name
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Optional Device description
        /// </summary>
        public string? Description { get; init; }

        [SetsRequiredMembers]
        public Device(DeviceId id, DeviceProfile deviceProfile, string name, string? description)
        {
            Id = id;
            DeviceProfile = deviceProfile;
            Name = name;
            Description = description;
        }
    }
}
