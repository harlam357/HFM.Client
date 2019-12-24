
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home client info message.
    /// </summary>
    public class Info
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        public Info()
        {
            Client = new ClientInfo();
            Build = new BuildInfo();
            System = new SystemInfo();
        }

        public ClientInfo Client { get; }

        public BuildInfo Build { get; }

        public SystemInfo System { get; }

        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="String"/> that contains JSON.
        /// </summary>
        public static Info Load(string json) => new Internal.InfoObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="StringBuilder"/> that contains JSON.
        /// </summary>
        public static Info Load(StringBuilder json) => new Internal.InfoObjectLoader().Load(json);

        /// <summary>
        /// Creates a new <see cref="Info"/> object from a <see cref="TextReader"/> that contains JSON.
        /// </summary>
        public static Info Load(TextReader textReader) => new Internal.InfoObjectLoader().Load(textReader);
    }

    /// <summary>
    /// Folding@Home client information.
    /// </summary>
    public class ClientInfo
    {
        public string Website { get; set; }
        public string Copyright { get; set; }
        public string Author { get; set; }
        public string Args { get; set; }
        public string Config { get; set; }
    }

    /// <summary>
    /// Folding@Home client build information.
    /// </summary>
    public class BuildInfo
    {
        public string Version { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int? SVNRev { get; set; }
        public string Branch { get; set; }
        public string Compiler { get; set; }
        public string Options { get; set; }
        public string Platform { get; set; }
        public int? Bits { get; set; }
        public string Mode { get; set; }
    }

    /// <summary>
    /// Folding@Home client system information.
    /// </summary>
    public class SystemInfo
    {
        public string OS { get; set; }
        public string CPU { get; set; }
        public string CPUID { get; set; }
        public int? CPUs { get; set; }
        public string Memory { get; set; }
        public double? MemoryValue { get; set; }
        public string FreeMemory { get; set; }
        public double? FreeMemoryValue { get; set; }
        public string Threads { get; set; }
        public int? GPUs { get; set; }
        public IDictionary<int, GPUInfo> GPUInfos { get; set; }
        public string CUDA { get; set; }
        public string CUDADriver { get; set; }
        public bool? HasBattery { get; set; }
        public bool? OnBattery { get; set; }
        public int? UtcOffset { get; set; }
        public int? PID { get; set; }
        public string CWD { get; set; }
        public bool? Win32Service { get; set; }
    }

    /// <summary>
    /// Folding@Home client GPU information.
    /// </summary>
    public class GPUInfo
    {
        public int ID { get; set; }
        public string GPU { get; set; }
        public string FriendlyName { get; set; }
    }
}
