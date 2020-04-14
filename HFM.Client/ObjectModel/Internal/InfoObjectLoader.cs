
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
            if (textReader is null) return null;

            var array = LoadJArray(textReader);

            var info = new Info();
            var client = GetArrayToken(array, "FAHClient");
            if (client != null)
            {
                FahClientLoader(client, info.Client);
            }
            else
            {
                client = GetArrayToken(array, "Folding@home Client");
                var build = GetArrayToken(array, "Build");
                FoldingAtHomeClientLoader(client, build, info.Client);
            }

            var system = GetArrayToken(array, "System");
            info.System.CPU = GetValue<string>(system, "CPU")?.Trim();
            info.System.CPUID = GetValue<string>(system, "CPU ID");
            info.System.CPUs = GetValue<int?>(system, "CPUs");
            info.System.Memory = GetValue<string>(system, "Memory");
            info.System.MemoryValue = ConvertToMemoryValue(info.System.Memory);
            info.System.FreeMemory = GetValue<string>(system, "Free Memory");
            info.System.FreeMemoryValue = ConvertToMemoryValue(info.System.FreeMemory);
            info.System.Threads = GetValue<string>(system, "Threads");
            info.System.OSVersion = GetValue<string>(system, "OS Version");
            info.System.HasBattery = GetValue<bool?>(system, "Has Battery");
            info.System.OnBattery = GetValue<bool?>(system, "On Battery");
            info.System.UtcOffset = GetValue<int?>(system, "UTC Offset", StringComparison.OrdinalIgnoreCase);
            info.System.PID = GetValue<int?>(system, "PID");
            info.System.CWD = GetValue<string>(system, "CWD");
            info.System.OS = GetValue<string>(system, "OS");
            info.System.OSArch = GetValue<string>(system, "OS Arch");
            info.System.GPUs = GetValue<int?>(system, "GPUs");
            info.System.GPUInfos = BuildGPUInfos(system);
            info.System.Win32Service = GetValue<bool?>(system, "Win32 Service");

            return info;
        }

        private static void FahClientLoader(JToken client, ClientInfo clientInfo)
        {
            clientInfo.Version = GetValue<string>(client, "Version");
            clientInfo.Author = GetValue<string>(client, "Author");
            clientInfo.Copyright = GetValue<string>(client, "Copyright");
            clientInfo.Homepage = GetValue<string>(client, "Homepage");
            clientInfo.Date = GetValue<string>(client, "Date");
            clientInfo.Time = GetValue<string>(client, "Time");
            clientInfo.Revision = GetValue<string>(client, "Revision");
            clientInfo.Branch = GetValue<string>(client, "Branch");
            clientInfo.Compiler = GetValue<string>(client, "Compiler");
            clientInfo.Options = GetValue<string>(client, "Options");
            clientInfo.Platform = GetValue<string>(client, "Platform");
            clientInfo.Bits = GetValue<int?>(client, "Bits");
            clientInfo.Mode = GetValue<string>(client, "Mode");
            clientInfo.Args = GetValue<string>(client, "Args")?.Trim();
            clientInfo.Config = GetValue<string>(client, "Config");
        }

        private static void FoldingAtHomeClientLoader(JToken client, JToken build, ClientInfo clientInfo)
        {
            clientInfo.Homepage = GetValue<string>(client, "Website");
            clientInfo.Copyright = GetValue<string>(client, "Copyright");
            clientInfo.Author = GetValue<string>(client, "Author");
            clientInfo.Args = GetValue<string>(client, "Args")?.Trim();
            clientInfo.Config = GetValue<string>(client, "Config");

            clientInfo.Version = GetValue<string>(build, "Version");
            clientInfo.Date = GetValue<string>(build, "Date");
            clientInfo.Time = GetValue<string>(build, "Time");
            clientInfo.Branch = GetValue<string>(build, "Branch");
            clientInfo.Compiler = GetValue<string>(build, "Compiler");
            clientInfo.Options = GetValue<string>(build, "Options");
            clientInfo.Platform = GetValue<string>(build, "Platform");
            clientInfo.Bits = GetValue<int?>(build, "Bits");
            clientInfo.Mode = GetValue<string>(build, "Mode");
        }

        private static JToken GetArrayToken(JArray array, string name)
        {
            return array.Descendants().FirstOrDefault(x => x.Type == JTokenType.String && x.Value<string>() == name)?.Parent;
        }

        private static T GetValue<T>(JToken token, string name, StringComparison nameComparison = StringComparison.Ordinal)
        {
            var innerArray = token?.FirstOrDefault(x => String.Equals(x.FirstOrDefault()?.Value<string>(), name, nameComparison));
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
            return default;
        }

        private static IDictionary<int, GPUInfo> BuildGPUInfos(JToken token)
        {
            var result = Enumerable.Range(0, 8)
                .Select(id =>
                {
                    var gpu = GetValue<string>(token, String.Concat("GPU ", id));
                    return gpu != null 
                        ? new GPUInfo
                        {
                            ID = id, 
                            GPU = gpu, 
                            FriendlyName = ConvertToGPUFriendlyName(gpu),
                            CUDADevice = GetValue<string>(token, String.Concat("CUDA Device ", id)),
                            OpenCLDevice = GetValue<string>(token, String.Concat("OpenCL Device ", id))
                        } 
                        : null;
                })
                .Where(x => x != null)
                .ToDictionary(x => x.ID);

            return result.Count > 0 ? result : null;
        }

        private static double? ConvertToMemoryValue(string input)
        {
            if (input is null) return null;

            // always returns value in gigabytes
            int gigabyteIndex = input.IndexOf("GiB", StringComparison.Ordinal);
            if (gigabyteIndex > 0 && Double.TryParse(input.Substring(0, gigabyteIndex), out double value))
            {
                return value;
            }
            int megabyteIndex = input.IndexOf("MiB", StringComparison.Ordinal);
            if (megabyteIndex > 0 && Double.TryParse(input.Substring(0, megabyteIndex), out value))
            {
                return value / 1024;
            }
            int kilobyteIndex = input.IndexOf("KiB", StringComparison.Ordinal);
            if (kilobyteIndex > 0 && Double.TryParse(input.Substring(0, kilobyteIndex), out value))
            {
                return value / 1048576;
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