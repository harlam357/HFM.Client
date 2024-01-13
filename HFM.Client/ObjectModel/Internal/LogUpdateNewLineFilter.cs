using System.Text;

namespace HFM.Client.ObjectModel.Internal;

internal class LogUpdateNewLineFilter : IJsonStringFilter
{
    public StringBuilder? Filter(string? s) => new(s?
        .Replace("\n", Environment.NewLine, StringComparison.Ordinal)
        .Replace("\\n", Environment.NewLine, StringComparison.Ordinal));

    public StringBuilder? Filter(StringBuilder? s) => s?
        .Replace("\n", Environment.NewLine)
        .Replace("\\n", Environment.NewLine);
}
