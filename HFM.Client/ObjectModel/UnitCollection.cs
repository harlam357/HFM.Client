
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client unit collection message.
    /// </summary>
    public class UnitCollection : Collection<Unit>
    {
        /// <summary>
        /// Creates a new <see cref="UnitCollection"/> object from a <see cref="String"/> that contains JSON.
        /// </summary>
        public static UnitCollection Load(string json) => new Internal.UnitCollectionObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="UnitCollection"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public static UnitCollection Load(StringBuilder json) => new Internal.UnitCollectionObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="UnitCollection"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public static UnitCollection Load(TextReader textReader) => new Internal.UnitCollectionObjectLoader().Load(textReader);
    }

    /// <summary>
    /// Folding@Home client unit message.
    /// </summary>
    public class Unit
    {
        public int? ID { get; set; }
        public string State { get; set; }
        public string Error { get; set; }
        public int? Project { get; set; }
        public int? Run { get; set; }
        public int? Clone { get; set; }
        public int? Gen { get; set; }
        public string Core { get; set; }
        public string UnitHex { get; set; }
        public string PercentDone { get; set; }
        public int? TotalFrames { get; set; }
        public int? FramesDone { get; set; }
        public string Assigned { get; set; }
        public DateTime? AssignedDateTime { get; set; }
        public string Timeout { get; set; }
        public DateTime? TimeoutDateTime { get; set; }
        public string Deadline { get; set; }
        public DateTime? DeadlineDateTime { get; set; }
        public string WorkServer { get; set; }
        public string CollectionServer { get; set; }
        public string WaitingOn { get; set; }
        public int? Attempts { get; set; }
        public string NextAttempt { get; set; }
        public TimeSpan? NextAttemptTimeSpan { get; set; }
        public int? Slot { get; set; }
        public string ETA { get; set; }
        public TimeSpan? ETATimeSpan { get; set; }
        public double? PPD { get; set; }
        public string TPF { get; set; }
        public TimeSpan? TPFTimeSpan { get; set; }
        public double? BaseCredit { get; set; }
        public double? CreditEstimate { get; set; }
    }
}
