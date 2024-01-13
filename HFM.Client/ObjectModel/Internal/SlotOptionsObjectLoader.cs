using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HFM.Client.ObjectModel.Internal;

internal class SlotOptionsObjectLoader : ObjectLoader<SlotOptions>
{
    public override SlotOptions? Load(TextReader? textReader)
    {
        if (textReader is null) return null;

        var obj = LoadJsonObject(textReader);
        return Load(obj);
    }

    internal static SlotOptions? Load(JsonObject? obj)
    {
        if (obj is null) return null;

        var result = new SlotOptions();
        foreach (var t in obj)
        {
            var element = t.Value?.GetValue<JsonElement>();
            result[t.Key] = element switch
            {
                { ValueKind: JsonValueKind.Number } => t.Value?.GetValue<int>().ToString(CultureInfo.InvariantCulture),
                { ValueKind: JsonValueKind.True or JsonValueKind.False } => t.Value?.GetValue<bool>().ToString(),
                _ => t.Value?.GetValue<string>()
            };
        }

        return result;
    }
}
