
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class InfoObjectLoader : ObjectLoader<Info>
    {
        public override Info Load(TextReader textReader)
        {
            var array = LoadJArray(textReader);

            var info = new Info();
            var client = GetArrayToken(array, "Folding@home Client");
            info.Client.Website = GetValue<string>(client, "Website");
            info.Client.Copyright = GetValue<string>(client, "Copyright");
            info.Client.Author = GetValue<string>(client, "Author");
            info.Client.Args = GetValue<string>(client, "Args")?.Trim();
            info.Client.Config = GetValue<string>(client, "Config");

            var build = GetArrayToken(array, "Build");
            info.Build.Version = GetValue<string>(build, "Version");
            info.Build.Date = GetValue<string>(build, "Date");
            info.Build.Time = GetValue<string>(build, "Time");
            info.Build.SVNRev = GetValue<int>(build, "SVN Rev");
            info.Build.Branch = GetValue<string>(build, "Branch");
            info.Build.Compiler = GetValue<string>(build, "Compiler");
            info.Build.Options = GetValue<string>(build, "Options");
            info.Build.Platform = GetValue<string>(build, "Platform");
            info.Build.Bits = GetValue<int>(build, "Bits");
            info.Build.Mode = GetValue<string>(build, "Mode");

            var system = GetArrayToken(array, "System");
            info.System.OS = GetValue<string>(system, "OS");
            info.System.CPU = GetValue<string>(system, "CPU");
            info.System.CPUID = GetValue<string>(system, "CPU ID");
            info.System.CPUs = GetValue<int>(system, "CPUs");
            info.System.Memory = GetValue<string>(system, "Memory");
            info.System.MemoryValue = ConvertToMemoryValue(info.System.Memory);
            info.System.FreeMemory = GetValue<string>(system, "Free Memory");
            info.System.FreeMemoryValue = ConvertToMemoryValue(info.System.FreeMemory);
            info.System.Threads = GetValue<string>(system, "Threads");
            info.System.GPUs = GetValue<int>(system, "GPUs");
            info.System.GPUInfos = BuildGPUInfos(system);
            info.System.CUDA = GetValue<string>(system, "CUDA");
            info.System.CUDADriver = GetValue<string>(system, "CUDA Driver");
            info.System.HasBattery = GetValue<bool>(system, "Has Battery");
            info.System.OnBattery = GetValue<bool>(system, "On Battery");
            info.System.UtcOffset = GetValue<int>(system, "UTC offset");
            info.System.PID = GetValue<int>(system, "PID");
            info.System.CWD = GetValue<string>(system, "CWD");
            info.System.Win32Service = GetValue<bool>(system, "Win32 Service");

            return info;
        }

        private static JToken GetArrayToken(JArray array, string name)
        {
            return array.Descendants().FirstOrDefault(x => x.Type == JTokenType.String && x.Value<string>() == name)?.Parent;
        }

        private static T GetValue<T>(JToken token, string name)
        {
            var innerArray = token?.FirstOrDefault(x => x.FirstOrDefault()?.Value<string>() == name);
            var valueElement = innerArray?.ElementAtOrDefault(1);
            if (valueElement != null)
            {
                try
                {
                    return valueElement.Value<T>();
                }
                catch (FormatException)
                {
                    // if the data changed in a way where the value can
                    // no longer be converted to the target CLR type
                }
            }
            return default(T);
        }

        private static IDictionary<int, GPUInfo> BuildGPUInfos(JToken token)
        {
            var result = Enumerable.Range(0, 8)
                .Select(id =>
                {
                    var gpu = GetValue<string>(token, String.Concat("GPU ", id));
                    return gpu != null ? new GPUInfo { ID = id, GPU = gpu, FriendlyName = ConvertToGPUFriendlyName(gpu) } : null;
                })
                .Where(x => x != null)
                .ToDictionary(x => x.ID);

            return result.Count > 0 ? result : null;
        }

        private static double? ConvertToMemoryValue(string input)
        {
            if (input is null) return null;

            double value;
            // always returns value in gigabytes
            int gigabyteIndex = input.IndexOf("GiB", StringComparison.Ordinal);
            if (gigabyteIndex > 0)
            {
                if (Double.TryParse(input.Substring(0, gigabyteIndex), out value))
                {
                    return value;
                }
            }
            int megabyteIndex = input.IndexOf("MiB", StringComparison.Ordinal);
            if (megabyteIndex > 0)
            {
                if (Double.TryParse(input.Substring(0, megabyteIndex), out value))
                {
                    return value / 1024;
                }
            }
            int kilobyteIndex = input.IndexOf("KiB", StringComparison.Ordinal);
            if (kilobyteIndex > 0)
            {
                if (Double.TryParse(input.Substring(0, kilobyteIndex), out value))
                {
                    return value / 1048576;
                }
            }
            return null;
        }

        private static string ConvertToGPUFriendlyName(string input)
        {
            var regex = new Regex("\\[(?<GpuType>.+)\\]", RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            Match match;
            if ((match = regex.Match(input)).Success)
            {
                return match.Groups["GpuType"].Value;
            }

            return input;
        }
    }
}