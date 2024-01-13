namespace HFM.Client.ObjectModel.Internal;

internal class OptionsObjectLoader : ObjectLoader<Options>
{
    public override Options? Load(TextReader? textReader)
    {
        if (textReader is null) return null;

        var obj = LoadJsonObject(textReader);
        if (obj is null)
        {
            return null;
        }

        var result = new Options();
        foreach (var t in obj)
        {
            result[t.Key] = t.Value?.GetValue<string>();
        }
        return result;
    }
}
