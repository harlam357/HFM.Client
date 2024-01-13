using System.Globalization;

namespace HFM.Client.ObjectModel.Internal;

internal static class DateTimeConverter
{
    internal static DateTime? ConvertToDateTime(string? input)
    {
        if (input is null || input == "<invalid>") return null;

        // ISO 8601
        if (DateTime.TryParse(input, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime value))
        {
            return value;
        }

        // custom format for older v7 clients
        if (DateTime.TryParseExact(input, "dd/MMM/yyyy-HH:mm:ss", CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out value))
        {
            return value;
        }

        return null;
    }
}
