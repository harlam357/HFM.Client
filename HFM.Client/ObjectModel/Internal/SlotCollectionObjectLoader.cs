using System.Text.Json.Nodes;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SlotCollectionObjectLoader : ObjectLoader<SlotCollection>
    {
        public override SlotCollection Load(TextReader textReader)
        {
            if (textReader is null) return null;

            var array = LoadJsonArray(textReader);

            var collection = new SlotCollection();
            foreach (var obj in array.Select(x => x.AsObject()).Where(x => x.Count != 0))
            {
                collection.Add(LoadSlot(obj));
            }
            return collection;
        }

        private Slot LoadSlot(JsonObject obj)
        {
            var slot = new Slot();
            slot.ID = GetValue<string>(obj, "id").ToNullableInt32();
            slot.Status = GetValue<string>(obj, "status");
            slot.Description = GetValue<string>(obj, "description");
            slot.SlotOptions = SlotOptionsObjectLoader.Load(obj["options"]?.AsObject());
            return slot;
        }
    }
}
