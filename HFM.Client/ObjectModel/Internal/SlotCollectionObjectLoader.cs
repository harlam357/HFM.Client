
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SlotCollectionObjectLoader : ObjectLoader<SlotCollection>
    {
        public override SlotCollection Load(TextReader textReader)
        {
            if (textReader is null) return null;

            var array = LoadJArray(textReader);

            var collection = new SlotCollection();
            foreach (var token in array.Where(x => x.HasValues))
            {
                collection.Add(LoadSlot((JObject)token));
            }
            return collection;
        }

        private Slot LoadSlot(JObject obj)
        {
            var slot = new Slot();
            slot.ID = GetValue<int?>(obj, "id");
            slot.Status = GetValue<string>(obj, "status");
            slot.Description = GetValue<string>(obj, "description");
            slot.SlotOptions = new SlotOptionsObjectLoader().Load((JObject)obj["options"]);
            return slot;
        }
    }
}
