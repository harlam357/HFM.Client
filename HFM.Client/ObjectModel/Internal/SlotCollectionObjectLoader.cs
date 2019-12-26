
using System.IO;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SlotCollectionObjectLoader : ObjectLoader<SlotCollection>
    {
        public override SlotCollection Load(TextReader textReader)
        {
            var array = LoadJArray(textReader);
            
            var collection = new SlotCollection();
            foreach (var token in array.Where(x => x.HasValues))
            {
                collection.Add(LoadSlot(token));
            }
            return collection;
        }

        private Slot LoadSlot(JToken token)
        {
            var obj = LoadJObject(token);

            var slot = new Slot();
            slot.ID = GetValue<int?>(obj, "id");
            slot.Status = GetValue<string>(obj, "status");
            slot.SlotStatus = ConvertToSlotStatus(slot.Status);
            slot.Description = GetValue<string>(obj, "description");
            slot.SlotOptions = SlotOptions.FromObject((JObject)obj["options"]);
            return slot;
        }

        private static JObject LoadJObject(JToken token)
        {
            using (var reader = token.CreateReader())
            {
                return JObject.Load(reader);
            }
        }

        private static SlotStatus ConvertToSlotStatus(string input)
        {
            switch (input)
            {
                case "PAUSED":
                    return SlotStatus.Paused;
                case "RUNNING":
                    return SlotStatus.Running;
                case "FINISHING":
                    return SlotStatus.Finishing;
                case "READY":
                    return SlotStatus.Ready;
                case "STOPPING":
                    return SlotStatus.Stopping;
                case "FAILED":
                    return SlotStatus.Failed;
                default:
                    return SlotStatus.Unknown;
            }
        }
    }
}
