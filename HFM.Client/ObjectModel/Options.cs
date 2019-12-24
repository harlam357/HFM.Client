
using System;
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client options message.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Creates a new <see cref="Options"/> object from a <see cref="String"/> that contains JSON.
        /// </summary>
        public static Options Load(string json) => new Internal.OptionsObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="Options"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public static Options Load(StringBuilder json) => new Internal.OptionsObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="Options"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public static Options Load(TextReader textReader) => new Internal.OptionsObjectLoader().Load(textReader);

        public string Allow { get; set; }
        public string AssignmentServers { get; set; }
        public int? Checkpoint { get; set; }
        public string ClientSubType { get; set; }
        public int? ClientThreads { get; set; }
        public string ClientType { get; set; }
        public string CommandAddress { get; set; }
        public string CommandAllowNoPass { get; set; }
        public string Deny { get; set; }
        public string CommandDenyNoPass { get; set; }
        public bool? CommandEnable { get; set; }
        public int? CommandPort { get; set; }
        public int? ConnectionTimeout { get; set; }
        public string CorePriority { get; set; }
        public bool? CPUAffinity { get; set; }
        public string CPUSpecies { get; set; }
        public string CPUType { get; set; }
        public int? CPUUsage { get; set; }
        public int? CPUs { get; set; }
        public bool? DumpAfterDeadline { get; set; }
        public string ExecDirectory { get; set; }
        public bool? ExitWhenDone { get; set; }
        public bool? FoldAnon { get; set; }
        public bool? GPU { get; set; }
        public int? GPUUsage { get; set; }
        public bool? Idle { get; set; }
        public string Log { get; set; }
        public bool? LogCrlf { get; set; }
        public int? LogDatePeriodically { get; set; }
        public bool? LogToScreen { get; set; }
        public int? MachineId { get; set; }
        public string MaxPacketSize { get; set; }
        public int? MaxQueue { get; set; }
        public int? MaxSlotErrors { get; set; }
        public int? MaxUnitErrors { get; set; }
        public int? NextUnitPercentage { get; set; }
        public string OSSpecies { get; set; }
        public string OSType { get; set; }
        public string Passkey { get; set; }
        public string Password { get; set; }
        public bool? PauseOnBattery { get; set; }
        public bool? PauseOnStart { get; set; }
        public bool? Paused { get; set; }
        public string Power { get; set; }
        public string Proxy { get; set; }
        public bool? ProxyEnable { get; set; }
        public string ProxyPass { get; set; }
        public string ProxyUser { get; set; }
        public bool? Service { get; set; }
        public bool? ServiceRestart { get; set; }
        public int? ServiceRestartDelay { get; set; }
        public bool? SMP { get; set; }
        public int? Team { get; set; }
        public string User { get; set; }
        public int? Verbosity { get; set; }
    }
}
