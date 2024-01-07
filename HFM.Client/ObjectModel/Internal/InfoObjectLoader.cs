using System.Text.Json;
using System.Text.Json.Nodes;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal;

internal class InfoObjectLoader : ObjectLoader<Info>
{
    public override Info Load(TextReader textReader)
    {
        if (textReader is null) return null;

        var array = LoadJsonArray(textReader);

        var info = new Info();
        var client = GetArrayNode(array, "FAHClient");
        if (client != null)
        {
            FahClientLoader(client, info.Client);
        }
        else
        {
            client = GetArrayNode(array, "Folding@home Client");
            var build = GetArrayNode(array, "Build");
            FoldingAtHomeClientLoader(client, build, info.Client);
        }

        var system = GetArrayNode(array, "System");
        info.System.CPU = GetValue<string>(system, "CPU")?.Trim();
        info.System.CPUID = GetValue<string>(system, "CPU ID");
        info.System.CPUs = GetValue<string>(system, "CPUs").ToNullableInt32();
        info.System.Memory = GetValue<string>(system, "Memory");
        info.System.MemoryValue = ConvertToMemoryValue(info.System.Memory);
        info.System.FreeMemory = GetValue<string>(system, "Free Memory");
        info.System.FreeMemoryValue = ConvertToMemoryValue(info.System.FreeMemory);
        info.System.Threads = GetValue<string>(system, "Threads");
        info.System.OSVersion = GetValue<string>(system, "OS Version");
        info.System.HasBattery = GetValue<string>(system, "Has Battery").ToNullableBoolean();
        info.System.OnBattery = GetValue<string>(system, "On Battery").ToNullableBoolean();
        info.System.UtcOffset = GetValue<string>(system, "UTC Offset", StringComparison.OrdinalIgnoreCase).ToNullableInt32();
        info.System.PID = GetValue<string>(system, "PID").ToNullableInt32();
        info.System.CWD = GetValue<string>(system, "CWD");
        info.System.OS = GetValue<string>(system, "OS");
        info.System.OSArch = GetValue<string>(system, "OS Arch");
        info.System.GPUs = GetValue<string>(system, "GPUs").ToNullableInt32();
        info.System.GPUInfos = BuildGPUInfos(system);
        info.System.Win32Service = GetValue<string>(system, "Win32 Service").ToNullableBoolean();

        return info;
    }

    private static void FahClientLoader(JsonNode client, ClientInfo clientInfo)
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
        clientInfo.Bits = GetValue<string>(client, "Bits").ToNullableInt32();
        clientInfo.Mode = GetValue<string>(client, "Mode");
        clientInfo.Args = GetValue<string>(client, "Args")?.Trim();
        clientInfo.Config = GetValue<string>(client, "Config");
    }

    private static void FoldingAtHomeClientLoader(JsonNode client, JsonNode build, ClientInfo clientInfo)
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
        clientInfo.Bits = GetValue<string>(build, "Bits").ToNullableInt32();
        clientInfo.Mode = GetValue<string>(build, "Mode");
    }

    private static JsonNode GetArrayNode(JsonArray array, string name) => array
        .Select(x => x.AsArray().FirstOrDefault())
        .Where(x => x is not null && x.GetValue<JsonElement>().ValueKind == JsonValueKind.String)
        .FirstOrDefault(x => x.GetValue<string>() == name)?.Parent;

    private static T GetValue<T>(JsonNode node, string name, StringComparison nameComparison = StringComparison.Ordinal)
    {
        if (node is null)
        {
            return default;
        }

        var innerArray = node.AsArray().FirstOrDefault(x => x is JsonArray a && String.Equals(a.FirstOrDefault()?.GetValue<string>(), name, nameComparison));
        var valueNode = innerArray?.AsArray().ElementAtOrDefault(1);
        if (valueNode == null)
        {
            return default;
        }

        try
        {
            return valueNode.GetValue<T>();
        }
        catch (FormatException)
        {
            // The current JsonNode cannot be represented as a {T}.
        }
        catch (InvalidOperationException)
        {
            // The current JsonNode is not a JsonValue or is not compatible with {T}.
        }

        return default;
    }

    private static IDictionary<int, GPUInfo> BuildGPUInfos(JsonNode node)
    {
        var result = Enumerable.Range(0, 8)
            .Select(id =>
            {
                var gpu = GetValue<string>(node, String.Concat("GPU ", id));
                return gpu != null
                    ? new GPUInfo
                    {
                        ID = id,
                        GPU = gpu,
                        CUDADevice = GetValue<string>(node, String.Concat("CUDA Device ", id)),
                        OpenCLDevice = GetValue<string>(node, String.Concat("OpenCL Device ", id))
                    }
                    : null;
            })
            .Where(x => x != null)
            .ToDictionary(x => x.ID);

        return result.Count > 0 ? result : null;
    }

    private static double? ConvertToMemoryValue(string input)
    {
        if (input is null)
        {
            return null;
        }

        // always returns value in gigabytes
        int gigabyteIndex = input.IndexOf("GiB", StringComparison.Ordinal);
        if (gigabyteIndex > 0 && Double.TryParse(input.AsSpan(0, gigabyteIndex), out double value))
        {
            return value;
        }

        int megabyteIndex = input.IndexOf("MiB", StringComparison.Ordinal);
        if (megabyteIndex > 0 && Double.TryParse(input.AsSpan(0, megabyteIndex), out value))
        {
            return value / 1024;
        }

        int kilobyteIndex = input.IndexOf("KiB", StringComparison.Ordinal);
        if (kilobyteIndex > 0 && Double.TryParse(input.AsSpan(0, kilobyteIndex), out value))
        {
            return value / 1048576;
        }

        return null;
    }
}
