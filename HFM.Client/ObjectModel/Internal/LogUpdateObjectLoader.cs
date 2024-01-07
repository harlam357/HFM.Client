using System.Text;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal;

internal class LogUpdateObjectLoader : ObjectLoader<LogUpdate>
{
    public override LogUpdate Load(string json, ObjectLoadOptions options = ObjectLoadOptions.Default)
    {
        if (json is null) return null;

        return new LogUpdate { Value = FilterJson(json, options) };
    }

    public override LogUpdate Load(StringBuilder json, ObjectLoadOptions options = ObjectLoadOptions.Default)
    {
        if (json is null) return null;

        var copyTo = new StringBuilder();
        json.CopyTo(0, copyTo, 0, json.Length);
        return new LogUpdate { Value = FilterJson(copyTo, options) };
    }

    public override LogUpdate Load(TextReader textReader)
    {
        if (textReader is null) return null;

        return new LogUpdate { Value = FilterJson(new StringBuilder(textReader.ReadToEnd()), EnumerateLogUpdateFilters()) };
    }

    protected override IEnumerable<IJsonStringFilter> EnumerateFilters(ObjectLoadOptions options)
    {
        foreach (var f in EnumerateLogUpdateFilters())
        {
            yield return f;
        }
        if (options.HasFlag(ObjectLoadOptions.DecodeHex)) yield return new JsonHexDecoder();
    }

    protected static IEnumerable<IJsonStringFilter> EnumerateLogUpdateFilters()
    {
        yield return new LogUpdateQuoteFilter();
        yield return new LogUpdateNewLineFilter();
    }
}
