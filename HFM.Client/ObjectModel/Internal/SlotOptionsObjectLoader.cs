using System.Text.Json.Nodes;

namespace HFM.Client.ObjectModel.Internal;

internal class SlotOptionsObjectLoader : ObjectLoader<SlotOptions>
{
    public override SlotOptions Load(TextReader textReader)
    {
        if (textReader is null) return null;

        var obj = LoadJsonObject(textReader);
        return Load(obj);
    }

    internal static SlotOptions Load(JsonObject obj)
    {
        if (obj is null) return null;

        var result = new SlotOptions();
        foreach (var t in obj)
        {
            result[t.Key] = t.Value?.GetValue<string>();
        }
        return result;
    }
}
