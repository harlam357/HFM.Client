
using System.IO;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SlotOptionsObjectLoader : ObjectLoader<SlotOptions>
    {
        public override SlotOptions Load(TextReader textReader)
        {
            if (textReader is null) return null;

            var obj = LoadJObject(textReader);
            return Load(obj);
        }

        internal static SlotOptions Load(JObject obj)
        {
            if (obj is null) return null;

            var result = new SlotOptions();
            foreach (var t in obj)
            {
                result[t.Key] = t.Value.Value<string>();
            }
            return result;
        }
    }
}
