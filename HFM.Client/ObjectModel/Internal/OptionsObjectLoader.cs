
using System.IO;

namespace HFM.Client.ObjectModel.Internal
{
    internal class OptionsObjectLoader : ObjectLoader<Options>
    {
        public override Options Load(TextReader textReader)
        {
            var obj = LoadJObject(textReader);

            var result = new Options();
            result.Allow = GetValue<string>(obj, "allow", "command-allow");
            result.AssignmentServers = GetValue<string>(obj, "assignment-servers");
            result.Checkpoint = GetValue<int?>(obj, "checkpoint");
            result.ClientSubType = GetValue<string>(obj, "client-subtype");
            result.ClientThreads = GetValue<int?>(obj, "client-threads");
            result.ClientType = GetValue<string>(obj, "client-type");
            result.CommandAddress = GetValue<string>(obj, "command-address");
            result.CommandAllowNoPass = GetValue<string>(obj, "command-allow-no-pass");
            result.Deny = GetValue<string>(obj, "deny", "command-deny");
            result.CommandDenyNoPass = GetValue<string>(obj, "command-deny-no-pass");
            result.CommandEnable = GetValue<bool?>(obj, "command-enable");
            result.CommandPort = GetValue<int?>(obj, "command-port");
            result.ConnectionTimeout = GetValue<int?>(obj, "connection-timeout");
            result.CorePriority = GetValue<string>(obj, "core-priority");
            result.CPUAffinity = GetValue<bool?>(obj, "cpu-affinity");
            result.CPUSpecies = GetValue<string>(obj, "cpu-species");
            result.CPUType = GetValue<string>(obj, "cpu-type");
            result.CPUUsage = GetValue<int?>(obj, "cpu-usage");
            result.CPUs = GetValue<int?>(obj, "cpus");
            result.DumpAfterDeadline = GetValue<bool?>(obj, "dump-after-deadline");
            result.ExecDirectory = GetValue<string>(obj, "exec-directory");
            result.ExitWhenDone = GetValue<bool?>(obj, "exit-when-done");
            result.FoldAnon = GetValue<bool?>(obj, "fold-anon");
            result.GPU = GetValue<bool?>(obj, "gpu");
            result.GPUUsage = GetValue<int?>(obj, "gpu-usage");
            result.Idle = GetValue<bool?>(obj, "idle");
            result.Log = GetValue<string>(obj, "log");
            result.LogCrlf = GetValue<bool?>(obj, "log-crlf");
            result.LogDatePeriodically = GetValue<int?>(obj, "log-date-periodically");
            result.LogToScreen = GetValue<bool?>(obj, "log-to-screen");
            result.MachineId = GetValue<int?>(obj, "machine-id");
            result.MaxPacketSize = GetValue<string>(obj, "max-packet-size");
            result.MaxQueue = GetValue<int?>(obj, "max-queue");
            result.MaxSlotErrors = GetValue<int?>(obj, "max-slot-errors");
            result.MaxUnitErrors = GetValue<int?>(obj, "max-unit-errors");
            result.NextUnitPercentage = GetValue<int?>(obj, "next-unit-percentage");
            result.OSSpecies = GetValue<string>(obj, "os-species");
            result.OSType = GetValue<string>(obj, "os-type");
            result.Passkey = GetValue<string>(obj, "passkey");
            result.Password = GetValue<string>(obj, "password");
            result.PauseOnBattery = GetValue<bool?>(obj, "pause-on-battery");
            result.PauseOnStart = GetValue<bool?>(obj, "pause-on-start");
            result.Paused = GetValue<bool?>(obj, "paused");
            result.Power = GetValue<string>(obj, "power");
            result.Proxy = GetValue<string>(obj, "proxy");
            result.ProxyEnable = GetValue<bool?>(obj, "proxy-enable");
            result.ProxyPass = GetValue<string>(obj, "proxy-pass");
            result.ProxyUser = GetValue<string>(obj, "proxy-user");
            result.Service = GetValue<bool?>(obj, "service");
            result.ServiceRestart = GetValue<bool?>(obj, "service-restart");
            result.ServiceRestartDelay = GetValue<int?>(obj, "service-restart-delay");
            result.SMP = GetValue<bool?>(obj, "smp");
            result.Team = GetValue<int?>(obj, "team");
            result.User = GetValue<string>(obj, "user");
            result.Verbosity = GetValue<int?>(obj, "verbosity");
            return result;
        }
    }
}
