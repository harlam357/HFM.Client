
using System.IO;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SlotOptionsObjectLoader : ObjectLoader<SlotOptions>
    {
        public override SlotOptions Load(TextReader textReader)
        {
            return Load(LoadJObject(textReader));
        }

        internal SlotOptions Load(JObject obj)
        {
            if (obj is null) return null;

            var slotOptions = new SlotOptions();
            slotOptions.ClientType = GetValue<string>(obj, "client-type");
            slotOptions.ClientSubType = GetValue<string>(obj, "client-subtype");
            slotOptions.CPUUsage = GetValue<int?>(obj, "cpu-usage");
            slotOptions.MachineID = GetValue<int?>(obj, "machine-id");
            slotOptions.MaxPacketSize = GetValue<string>(obj, "max-packet-size");
            slotOptions.CorePriority = GetValue<string>(obj, "core-priority");
            slotOptions.NextUnitPercentage = GetValue<int?>(obj, "next-unit-percentage");
            slotOptions.MaxUnits = GetValue<int?>(obj, "max-units");
            slotOptions.Checkpoint = GetValue<int?>(obj, "checkpoint");
            slotOptions.PauseOnStart = GetValue<bool?>(obj, "pause-on-start");
            slotOptions.GPUIndex = GetValue<int?>(obj, "gpu-index");
            slotOptions.GPUUsage = GetValue<int?>(obj, "gpu-usage");
            return slotOptions;
        }
    }
}
