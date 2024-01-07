namespace HFM.Client.Internal;

internal static class StringExtensions
{
    internal static int? ToNullableInt32(this string value) =>
        Int32.TryParse(value, out var result) ? result : null;

    internal static double? ToNullableDouble(this string value) =>
        Double.TryParse(value, out var result) ? result : null;

    internal static bool? ToNullableBoolean(this string value) =>
        Boolean.TryParse(value, out var result) ? result : null;
}
