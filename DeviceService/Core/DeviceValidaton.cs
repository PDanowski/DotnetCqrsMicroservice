using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DeviceService.Core
{
    public static class Validation
    {
        public static Guid AssertNotEmpty(
            [NotNull] this Guid? value,
            [CallerArgumentExpression("value")] string? argumentName = null
        ) =>
            (value != null && value.Value != Guid.Empty)
                ? value.Value
                : throw new ArgumentException(argumentName);

        public static string AssertNotEmpty(
            [NotNull] this string? value,
            [CallerArgumentExpression("value")] string? argumentName = null
        ) =>
            !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException(argumentName);

        public static string? AssertNullOrNotEmpty(
            this string? value,
            [CallerArgumentExpression("value")] string? argumentName = null
        ) => value?.AssertNotEmpty(argumentName);

        public static int AssertPositive(
            this int value,
            [CallerArgumentExpression("value")] string? argumentName = null
        ) =>
            value > 0
                ? value
                : throw new ArgumentOutOfRangeException(argumentName);
    }
}
