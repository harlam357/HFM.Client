
using System.IO;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class OptionsObjectLoader : ObjectLoader<Options>
    {
        public override Options Load(TextReader textReader)
        {
            if (textReader is null) return null;

            var obj = LoadJObject(textReader);

            var result = new Options();
            foreach (var t in obj)
            {
                result[t.Key] = t.Value.Value<string>();
            }
            return result;
        }
    }
}
