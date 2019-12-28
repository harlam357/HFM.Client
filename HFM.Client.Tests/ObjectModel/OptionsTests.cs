
using System;
using System.IO;
using System.Text;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class OptionsTests
    {
        [Test]
        public void Options_Load_FromClientVersion_7_1_24()
        {
            var options = Options.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_options.txt"));
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
            Assert.AreEqual("assign-GPU.stanford.edu:80 assign-GPU.stanford.edu:8080", options.GPUAssignmentServers);
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
            Assert.AreEqual(4, options.Threads);
            Assert.AreEqual("harlam357", options.User);
            Assert.AreEqual(5, options.Verbosity);
        }

        [Test]
        public void Options_Load_FromClientVersion_7_1_43()
        {
            var options = Options.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_options.txt"));
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
            Assert.AreEqual("AMD64", options.CPUType);
            Assert.AreEqual(100, options.CPUUsage);
            Assert.AreEqual(-1, options.CPUs);
            Assert.AreEqual(true, options.DumpAfterDeadline);
            Assert.AreEqual("C:\\Program Files (x86)\\FAHClient", options.ExecDirectory);
            Assert.AreEqual(false, options.ExitWhenDone);
            Assert.AreEqual(null, options.FoldAnon);
            Assert.AreEqual(false, options.GPU);
            Assert.AreEqual("assign-GPU.stanford.edu:80 assign-GPU.stanford.edu:8080", options.GPUAssignmentServers);
            Assert.AreEqual(100, options.GPUUsage);
            Assert.AreEqual(null, options.Idle);
            Assert.AreEqual("log.txt", options.Log);
            Assert.AreEqual(true, options.LogCrlf);
            Assert.AreEqual(21600, options.LogDatePeriodically);
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
            Assert.AreEqual(":8080", options.Proxy);
            Assert.AreEqual(false, options.ProxyEnable);
            Assert.AreEqual(String.Empty, options.ProxyPass);
            Assert.AreEqual(String.Empty, options.ProxyUser);
            Assert.AreEqual(false, options.Service);
            Assert.AreEqual(true, options.ServiceRestart);
            Assert.AreEqual(5000, options.ServiceRestartDelay);
            Assert.AreEqual(true, options.SMP);
            Assert.AreEqual(32, options.Team);
            Assert.AreEqual(4, options.Threads);
            Assert.AreEqual("harlam357", options.User);
            Assert.AreEqual(3, options.Verbosity);
        }

        [Test]
        public void Options_Load_ReturnsNullWhenJsonStringIsNull()
        {
            Assert.IsNull(Options.Load((string)null));
        }

        [Test]
        public void Options_Load_ReturnsNullWhenJsonStringBuilderIsNull()
        {
            Assert.IsNull(Options.Load((StringBuilder)null));
        }

        [Test]
        public void Options_Load_ReturnsNullWhenJsonTextReaderIsNull()
        {
            Assert.IsNull(Options.Load((TextReader)null));
        }
    }
}
