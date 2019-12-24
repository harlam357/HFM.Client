
using System;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client slot options message.
    /// </summary>
    public class SlotOptions
    {
        public static SlotOptions FromMessage(string message)
        {
            return FromObject(JObject.Parse(message));
        }

        internal static SlotOptions FromObject(JObject obj)
        {
            if (obj == null) return null;

            var result = new SlotOptions();
            result.ClientType = GetValue<string>(obj, "client-type");
            result.ClientSubType = GetValue<string>(obj, "client-subtype");
            result.CPUUsage = GetValue<int?>(obj, "cpu-usage");
            result.MachineID = GetValue<int?>(obj, "machine-id");
            result.MaxPacketSize = GetValue<string>(obj, "max-packet-size");
            result.CorePriority = GetValue<string>(obj, "core-priority");
            result.NextUnitPercentage = GetValue<int?>(obj, "next-unit-percentage");
            result.MaxUnits = GetValue<int?>(obj, "max-units");
            result.Checkpoint = GetValue<int?>(obj, "checkpoint");
            result.PauseOnStart = GetValue<bool?>(obj, "pause-on-start");
            result.GPUIndex = GetValue<int?>(obj, "gpu-index");
            result.GPUUsage = GetValue<int?>(obj, "gpu-usage");
            return result;
        }

        private static T GetValue<T>(JObject obj, params string[] names)
        {
            foreach (var name in names)
            {
                var token = obj[name];
                if (token != null)
                {
                    try
                    {
                        return token.Value<T>();
                    }
                    catch (FormatException)
                    {
                        // if the data changed in a way where the value can
                        // no longer be converted to the target CLR type
                    }
                }
            }
            return default(T);
        }

        public string ClientType { get; set; }
        public string ClientSubType { get; set; }
        public int? CPUUsage { get; set; }
        public int? MachineID { get; set; }
        public string MaxPacketSize { get; set; }
        public string CorePriority { get; set; }
        public int? NextUnitPercentage { get; set; }
        public int? MaxUnits { get; set; }
        public int? Checkpoint { get; set; }
        public bool? PauseOnStart { get; set; }
        public int? GPUIndex { get; set; }
        public int? GPUUsage { get; set; }
    }
}
