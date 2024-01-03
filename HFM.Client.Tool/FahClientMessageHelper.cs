namespace HFM.Client.Tool;

internal static class FahClientMessageHelper
{
    internal static string FormatForDisplay(FahClientMessage message) =>
        SetEnvironmentNewLineCharacters(message.ToString());

    private static string SetEnvironmentNewLineCharacters(string text) => text
        .Replace("\n", Environment.NewLine, StringComparison.Ordinal)
        .Replace("\\n", Environment.NewLine, StringComparison.Ordinal);
}
