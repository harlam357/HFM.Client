
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client slot options message.
    /// </summary>
    public class SlotOptions
    {
        /// <summary>
        /// Creates a new <see cref="SlotOptions"/> object from a <see cref="string"/> that contains JSON.
        /// </summary>
        public static SlotOptions Load(string json) => new Internal.SlotOptionsObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="SlotOptions"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public static SlotOptions Load(StringBuilder json) => new Internal.SlotOptionsObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="SlotOptions"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public static SlotOptions Load(TextReader textReader) => new Internal.SlotOptionsObjectLoader().Load(textReader);

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
