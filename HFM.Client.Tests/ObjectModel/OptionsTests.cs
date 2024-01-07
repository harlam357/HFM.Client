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
            var options = Options.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_options.txt"), ObjectLoadOptions.None);
            Assert.AreEqual("127.0.0.1", options["command-allow"]);
            Assert.AreEqual("15", options[Options.Checkpoint]);
            Assert.AreEqual("normal", options[Options.ClientType]);
            Assert.AreEqual("127.0.0.1", options[Options.CommandAllowNoPass]);
            Assert.AreEqual("0.0.0.0/0", options["command-deny"]);
            Assert.AreEqual("0.0.0.0/0", options[Options.CommandDenyNoPass]);
            Assert.AreEqual("36330", options[Options.CommandPort]);
            Assert.AreEqual("idle", options[Options.CorePriority]);
            Assert.AreEqual("4", options[Options.CPUs]);
            Assert.AreEqual(null, options[Options.FoldAnon]);
            Assert.AreEqual("normal", options[Options.MaxPacketSize]);
            Assert.AreEqual("99", options[Options.NextUnitPercentage]);
            Assert.AreEqual("xxxxx", options[Options.Passkey]);
            Assert.AreEqual("yyyyy", options[Options.Password]);
            Assert.AreEqual("false", options[Options.PauseOnBattery]);
            Assert.AreEqual("false", options[Options.PauseOnStart]);
            Assert.AreEqual(null, options[Options.Paused]);
            Assert.AreEqual(null, options[Options.Power]);
            Assert.AreEqual(":8080", options[Options.Proxy]);
            Assert.AreEqual("false", options[Options.ProxyEnable]);
            Assert.AreEqual(String.Empty, options[Options.ProxyPass]);
            Assert.AreEqual(String.Empty, options[Options.ProxyUser]);
            Assert.AreEqual("false", options[Options.Service]);
            Assert.AreEqual("true", options[Options.SMP]);
            Assert.AreEqual("32", options[Options.Team]);
            Assert.AreEqual("harlam357", options[Options.User]);
        }

        [Test]
        public void Options_Load_FromClientVersion_7_1_43()
        {
            var options = Options.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_options.txt"), ObjectLoadOptions.None);
            Assert.AreEqual("127.0.0.1", options["command-allow"]);
            Assert.AreEqual("15", options[Options.Checkpoint]);
            Assert.AreEqual("normal", options[Options.ClientType]);
            Assert.AreEqual("127.0.0.1", options[Options.CommandAllowNoPass]);
            Assert.AreEqual("0.0.0.0/0", options["command-deny"]);
            Assert.AreEqual("0.0.0.0/0", options[Options.CommandDenyNoPass]);
            Assert.AreEqual("36330", options[Options.CommandPort]);
            Assert.AreEqual("idle", options[Options.CorePriority]);
            Assert.AreEqual(Options.DefaultCPUs, options[Options.CPUs]);
            Assert.AreEqual(null, options[Options.FoldAnon]);
            Assert.AreEqual("normal", options[Options.MaxPacketSize]);
            Assert.AreEqual("99", options[Options.NextUnitPercentage]);
            Assert.AreEqual("xxxxx", options[Options.Passkey]);
            Assert.AreEqual("yyyyy", options[Options.Password]);
            Assert.AreEqual("false", options[Options.PauseOnBattery]);
            Assert.AreEqual("false", options[Options.PauseOnStart]);
            Assert.AreEqual(":8080", options[Options.Proxy]);
            Assert.AreEqual("false", options[Options.ProxyEnable]);
            Assert.AreEqual(String.Empty, options[Options.ProxyPass]);
            Assert.AreEqual(String.Empty, options[Options.ProxyUser]);
            Assert.AreEqual("false", options[Options.Service]);
            Assert.AreEqual("true", options[Options.SMP]);
            Assert.AreEqual("32", options[Options.Team]);
            Assert.AreEqual("harlam357", options[Options.User]);
        }

        [Test]
        public void Options_Load_FromClientVersion_7_6_6()
        {
            var options = Options.Load(TestDataReader.ReadStringBuilder("Client_7_6_6_options.txt"));
            Assert.AreEqual("127.0.0.1 192.168.1.0/24", options[Options.Allow]);
            Assert.AreEqual("ANY", options[Options.Cause]);
            Assert.AreEqual("15", options[Options.Checkpoint]);
            Assert.AreEqual("normal", options[Options.ClientType]);
            Assert.AreEqual("127.0.0.1 192.168.1.0/24", options[Options.CommandAllowNoPass]);
            Assert.AreEqual("0/0", options[Options.Deny]);
            Assert.AreEqual("0/0", options[Options.CommandDenyNoPass]);
            Assert.AreEqual("36330", options[Options.CommandPort]);
            Assert.AreEqual(Options.DefaultCPUs, options[Options.CPUs]);
            Assert.AreEqual("idle", options[Options.CorePriority]);
            Assert.AreEqual("false", options[Options.FoldAnon]);
            Assert.AreEqual(null, options[Options.GPUIndex]);
            Assert.AreEqual(null, options[Options.CUDAIndex]);
            Assert.AreEqual(null, options[Options.OpenCLIndex]);
            Assert.AreEqual("0", options[Options.MachineID]);
            Assert.AreEqual("normal", options[Options.MaxPacketSize]);
            Assert.AreEqual("99", options[Options.NextUnitPercentage]);
            Assert.AreEqual("xxxxx", options[Options.Passkey]);
            Assert.AreEqual(null, options[Options.Password]);
            Assert.AreEqual("true", options[Options.PauseOnBattery]);
            Assert.AreEqual("false", options[Options.PauseOnStart]);
            Assert.AreEqual("false", options[Options.Paused]);
            Assert.AreEqual("full", options[Options.Power]);
            Assert.AreEqual(":8080", options[Options.Proxy]);
            Assert.AreEqual("false", options[Options.ProxyEnable]);
            Assert.AreEqual(String.Empty, options[Options.ProxyPass]);
            Assert.AreEqual(String.Empty, options[Options.ProxyUser]);
            Assert.AreEqual("false", options[Options.Service]);
            Assert.AreEqual("true", options[Options.SMP]);
            Assert.AreEqual("32", options[Options.Team]);
            Assert.AreEqual("harlam357", options[Options.User]);
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
