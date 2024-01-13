using System.Text;

namespace HFM.Client.ObjectModel.Internal;

internal class JsonNewLineFilter : IJsonStringFilter
{
    public StringBuilder? Filter(string? s) => Filter(new StringBuilder(s));

    public StringBuilder? Filter(StringBuilder? s) => s?.Replace("\r", "").Replace("\n", "");
}
