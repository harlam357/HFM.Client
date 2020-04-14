
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HFM.Client.ObjectModel
{
    /// <summary>
    /// Folding@Home options message.
    /// </summary>
    public abstract class OptionsBase : IDictionary<string, string>
    {
        private readonly IDictionary<string, string> _inner;

        protected OptionsBase() : this(null)
        {

        }

        protected OptionsBase(IDictionary<string, string> options)
        {
            _inner = options ?? new Dictionary<string, string>();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_inner).GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _inner.Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _inner.Remove(item);
        }

        public int Count => _inner.Count;

        public bool IsReadOnly => _inner.IsReadOnly;

        public void Add(string key, string value)
        {
            _inner.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _inner.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _inner.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return _inner.TryGetValue(key, out value);
        }

        public string this[string key]
        {
            // minor behavior change here to make indexer forgiving
            get => _inner.TryGetValue(key, out var value) ? value : default;
            set => _inner[key] = value;
        }

        public ICollection<string> Keys => _inner.Keys;

        public ICollection<string> Values => _inner.Values;
    }

    /// <summary>
    /// Folding@Home client options message.
    /// </summary>
    public class Options : OptionsBase
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

        public const string Allow = "allow";
        public const string Cause = "cause";
        public const string Checkpoint = "checkpoint";
        public const string ClientType = "client-type";
        public const string CommandAllowNoPass = "command-allow-no-pass";
        public const string Deny = "deny";
        public const string CommandDenyNoPass = "command-deny-no-pass";
        public const string CommandPort = "command-port";
        public const string CPUs = "cpus";
        public const string CorePriority = "core-priority";
        public const string FoldAnon = "fold-anon";
        public const string GPUIndex = "gpu-index";
        public const string CUDAIndex = "cuda-index";
        public const string OpenCLIndex = "opencl-index";
        public const string MachineID = "machine-id";
        public const string MaxPacketSize = "max-packet-size";
        public const string NextUnitPercentage = "next-unit-percentage";
        public const string Passkey = "passkey";
        public const string Password = "password";
        public const string PauseOnBattery = "pause-on-battery";
        public const string PauseOnStart = "pause-on-start";
        public const string Paused = "paused";
        public const string Power = "power";
        public const string Proxy = "proxy";
        public const string ProxyEnable = "proxy-enable";
        public const string ProxyPass = "proxy-pass";
        public const string ProxyUser = "proxy-user";
        public const string Service = "service";
        public const string SMP = "smp";
        public const string Team = "team";
        public const string User = "user";

        public const string DefaultCheckpoint = "15";
        public const string DefaultCPUs = "-1";
        public const string DefaultGPUIndex = "-1";
        public const string DefaultCUDAIndex = "-1";
        public const string DefaultOpenCLIndex = "-1";
    }
}
