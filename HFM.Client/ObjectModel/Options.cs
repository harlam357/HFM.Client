
using System;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel
{
   /// <summary>
   /// Folding@Home client options message.
   /// </summary>
   public class Options
   {
      public static Options FromMessage(string message)
      {
         var result = new Options();

         var obj = JObject.Parse(message);
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

      private static T GetValue<T>(JObject obj, params string[] names)
      {
         foreach (var name in names)
         {
            var token = obj[name];
            if (token != null)
            {
               try
               {
                  return token.Value<T>();
               }
               catch (FormatException)
               {
                  // if the data changed in a way where the value can
                  // no longer be converted to the target CLR type
               }
            }
         }
         return default(T);
      }

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
