
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client slot collection message.
    /// </summary>
    public class SlotCollection : Collection<Slot>
    {
        /// <summary>
        /// Creates a new <see cref="SlotCollection"/> object from a <see cref="String"/> that contains JSON.
        /// </summary>
        public static SlotCollection Load(string json) => new Internal.SlotCollectionObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="SlotCollection"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public static SlotCollection Load(StringBuilder json) => new Internal.SlotCollectionObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="SlotCollection"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public static SlotCollection Load(TextReader textReader) => new Internal.SlotCollectionObjectLoader().Load(textReader);
    }

    /// <summary>
    /// Folding@Home client slot message.
    /// </summary>
    public class Slot
    {
        public int? ID { get; set; }
        public string Status { get; set; }
        public SlotStatus SlotStatus { get; set; }
        public string Description { get; set; }
        public SlotOptions SlotOptions { get; set; }
    }
}
