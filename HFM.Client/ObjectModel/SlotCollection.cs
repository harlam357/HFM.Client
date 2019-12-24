
using System;
using System.Collections.ObjectModel;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel
{
    public class SlotCollection : Collection<Slot>
    {
        public static SlotCollection FromMessage(string message)
        {
            var result = new SlotCollection();

            var jArray = JArray.Parse(message);
            foreach (var token in jArray)
            {
                if (!token.HasValues)
                {
                    continue;
                }

                var slot = Slot.FromJsonArrayElement(token.ToString());
                result.Add(slot);
            }
            return result;
        }
    }

    public class Slot
    {
        internal static Slot FromJsonArrayElement(string element)
        {
            var result = new Slot();

            var obj = JObject.Parse(element);
            result.ID = GetValue<int?>(obj, "id");
            result.Status = GetValue<string>(obj, "status");
            result.SlotStatus = ConvertToSlotStatus(result.Status);
            result.Description = GetValue<string>(obj, "description");
            result.SlotOptions = SlotOptions.FromObject((JObject)obj["options"]);
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

        /// <summary>
        /// Initializes a new <see cref="Slot"/>.
        /// </summary>
        public Slot()
        {
            //SlotOptions = new SlotOptions();
        }

        public int? ID { get; set; }
        public string Status { get; set; }
        public SlotStatus SlotStatus { get; set; }
        public string Description { get; set; }
        public SlotOptions SlotOptions { get; set; }
    }
}
