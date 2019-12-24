
using System;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class OptionsTests
    {
        private const string OptionsText = @"{
  ""assignment-servers"": ""assign3.stanford.edu:8080 assign4.stanford.edu:80"",
  ""capture-directory"": ""capture"",
  ""capture-sockets"": ""false"",
  ""checkpoint"": ""15"",
  ""child"": ""false"",
  ""client-subtype"": ""STDCLI"",
  ""client-type"": ""normal"",
  ""command-address"": ""0.0.0.0"",
  ""command-allow"": ""127.0.0.1"",
  ""command-allow-no-pass"": ""127.0.0.1"",
  ""command-deny"": ""0.0.0.0/0"",
  ""command-deny-no-pass"": ""0.0.0.0/0"",
  ""command-port"": ""36330"",
  ""config-rotate"": ""true"",
  ""config-rotate-dir"": ""configs"",
  ""config-rotate-max"": ""16"",
  ""core-dir"": ""cores"",
  ""core-key"": null,
  ""core-prep"": null,
  ""core-priority"": ""idle"",
  ""core-server"": null,
  ""cpu-affinity"": ""false"",
  ""cpu-species"": ""X86_PENTIUM_II"",
  ""cpu-type"": ""X86"",
  ""cpu-usage"": ""100"",
  ""cpus"": ""4"",
  ""cycle-rate"": ""4"",
  ""cycles"": ""-1"",
  ""daemon"": ""false"",
  ""data-directory"": ""."",
  ""debug-sockets"": ""false"",
  ""dump-after-deadline"": ""true"",
  ""eval"": null,
  ""exception-locations"": ""true"",
  ""exec-directory"": ""C:\\Program Files (x86)\\FAHClient"",
  ""exit-when-done"": ""false"",
  ""extra-core-args"": null,
  ""force-ws"": null,
  ""gpu"": ""false"",
  ""gpu-assignment-servers"": ""assign-GPU.stanford.edu:80 assign-GPU.stanford.edu:8080"",
  ""gpu-device-id"": null,
  ""gpu-id"": ""0"",
  ""gpu-index"": null,
  ""gpu-vendor-id"": null,
  ""log"": ""log.txt"",
  ""log-color"": ""false"",
  ""log-crlf"": ""true"",
  ""log-date"": ""false"",
  ""log-debug"": ""true"",
  ""log-domain"": ""false"",
  ""log-domain-levels"": null,
  ""log-header"": ""true"",
  ""log-level"": ""true"",
  ""log-no-info-header"": ""true"",
  ""log-redirect"": ""false"",
  ""log-rotate"": ""true"",
  ""log-rotate-dir"": ""logs"",
  ""log-rotate-max"": ""16"",
  ""log-short-level"": ""false"",
  ""log-simple-domains"": ""true"",
  ""log-thread-id"": ""false"",
  ""log-time"": ""true"",
  ""log-to-screen"": ""true"",
  ""log-truncate"": ""false"",
  ""machine-id"": ""0"",
  ""max-delay"": ""21600"",
  ""max-packet-size"": ""normal"",
  ""max-queue"": ""16"",
  ""max-shutdown-wait"": ""60"",
  ""max-slot-errors"": ""5"",
  ""max-unit-errors"": ""5"",
  ""max-units"": ""0"",
  ""memory"": null,
  ""min-delay"": ""60"",
  ""next-unit-percentage"": ""99"",
  ""priority"": null,
  ""no-assembly"": ""false"",
  ""os-species"": ""WIN_2003_SERVER"",
  ""os-type"": ""WIN32"",
  ""passkey"": ""xxxxx"",
  ""password"": ""yyyyy"",
  ""pause-on-battery"": ""false"",
  ""pause-on-start"": ""false"",
  ""pid"": ""false"",
  ""pid-file"": ""Folding@home Client.pid"",
  ""project-key"": ""0"",
  ""proxy"": "":8080"",
  ""proxy-enable"": ""false"",
  ""proxy-pass"": """",
  ""proxy-user"": """",
  ""respawn"": ""false"",
  ""script"": null,
  ""service"": ""false"",
  ""service-description"": ""Folding@home Client"",
  ""service-restart"": ""true"",
  ""service-restart-delay"": ""5000"",
  ""smp"": ""true"",
  ""stack-traces"": ""false"",
  ""team"": ""32"",
  ""threads"": ""4"",
  ""user"": ""harlam357"",
  ""verbosity"": ""5""
}";

        [Test]
        public void Options_FromMessage_Test()
        {
            var options = Options.FromMessage(OptionsText);
            Assert.AreEqual("127.0.0.1", options.Allow);
            Assert.AreEqual("assign3.stanford.edu:8080 assign4.stanford.edu:80", options.AssignmentServers);
            Assert.AreEqual(15, options.Checkpoint);
            Assert.AreEqual("STDCLI", options.ClientSubType);
            Assert.AreEqual(null, options.ClientThreads);
            Assert.AreEqual("normal", options.ClientType);
            Assert.AreEqual("0.0.0.0", options.CommandAddress);
            Assert.AreEqual("127.0.0.1", options.CommandAllowNoPass);
            Assert.AreEqual("0.0.0.0/0", options.Deny);
            Assert.AreEqual("0.0.0.0/0", options.CommandDenyNoPass);
            Assert.AreEqual(null, options.CommandEnable);
            Assert.AreEqual(36330, options.CommandPort);
            Assert.AreEqual(null, options.ConnectionTimeout);
            Assert.AreEqual("idle", options.CorePriority);
            Assert.AreEqual(false, options.CPUAffinity);
            Assert.AreEqual("X86_PENTIUM_II", options.CPUSpecies);
            Assert.AreEqual("X86", options.CPUType);
            Assert.AreEqual(100, options.CPUUsage);
            Assert.AreEqual(4, options.CPUs);
            Assert.AreEqual(true, options.DumpAfterDeadline);
            Assert.AreEqual("C:\\Program Files (x86)\\FAHClient", options.ExecDirectory);
            Assert.AreEqual(false, options.ExitWhenDone);
            Assert.AreEqual(null, options.FoldAnon);
            Assert.AreEqual(false, options.GPU);
            Assert.AreEqual(null, options.GPUUsage);
            Assert.AreEqual(null, options.Idle);
            Assert.AreEqual("log.txt", options.Log);
            Assert.AreEqual(true, options.LogCrlf);
            Assert.AreEqual(null, options.LogDatePeriodically);
            Assert.AreEqual(true, options.LogToScreen);
            Assert.AreEqual(0, options.MachineId);
            Assert.AreEqual("normal", options.MaxPacketSize);
            Assert.AreEqual(16, options.MaxQueue);
            Assert.AreEqual(5, options.MaxSlotErrors);
            Assert.AreEqual(5, options.MaxUnitErrors);
            Assert.AreEqual(99, options.NextUnitPercentage);
            Assert.AreEqual("WIN_2003_SERVER", options.OSSpecies);
            Assert.AreEqual("WIN32", options.OSType);
            Assert.AreEqual("xxxxx", options.Passkey);
            Assert.AreEqual("yyyyy", options.Password);
            Assert.AreEqual(false, options.PauseOnBattery);
            Assert.AreEqual(false, options.PauseOnStart);
            Assert.AreEqual(null, options.Paused);
            Assert.AreEqual(null, options.Power);
            Assert.AreEqual(":8080", options.Proxy);
            Assert.AreEqual(false, options.ProxyEnable);
            Assert.AreEqual(String.Empty, options.ProxyPass);
            Assert.AreEqual(String.Empty, options.ProxyUser);
            Assert.AreEqual(false, options.Service);
            Assert.AreEqual(true, options.ServiceRestart);
            Assert.AreEqual(5000, options.ServiceRestartDelay);
            Assert.AreEqual(true, options.SMP);
            Assert.AreEqual(32, options.Team);
            Assert.AreEqual("harlam357", options.User);
            Assert.AreEqual(5, options.Verbosity);
        }
    }
}
