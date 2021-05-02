
using System.IO;
using System.Text;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class SlotOptionsTests
    {
        [Test]
        public void SlotOptions_Load_FromClientVersion_7_1_24()
        {
            var slotOptions = SlotOptions.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_slot-options.txt"), ObjectLoadOptions.None);
            Assert.AreEqual("normal", slotOptions[Options.ClientType]);
            Assert.AreEqual("SMP", slotOptions["client-subtype"]);
            Assert.AreEqual("0", slotOptions[Options.MachineID]);
            Assert.AreEqual("normal", slotOptions[Options.MaxPacketSize]);
            Assert.AreEqual("idle", slotOptions[Options.CorePriority]);
            Assert.AreEqual("99", slotOptions[Options.NextUnitPercentage]);
            Assert.AreEqual("0", slotOptions["max-units"]);
            Assert.AreEqual("15", slotOptions[Options.Checkpoint]);
            Assert.AreEqual("true", slotOptions[Options.PauseOnStart]);
        }

        [Test]
        public void SlotOptions_Load_FromClientVersion_7_1_43()
        {
            var slotOptions = SlotOptions.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_slot-options.txt"), ObjectLoadOptions.None);
            Assert.AreEqual("normal", slotOptions[Options.ClientType]);
            Assert.AreEqual("GPU", slotOptions["client-subtype"]);
            Assert.AreEqual("1", slotOptions[Options.MachineID]);
            Assert.AreEqual("normal", slotOptions[Options.MaxPacketSize]);
            Assert.AreEqual("idle", slotOptions[Options.CorePriority]);
            Assert.AreEqual("99", slotOptions[Options.NextUnitPercentage]);
            Assert.AreEqual("0", slotOptions["max-units"]);
            Assert.AreEqual("15", slotOptions[Options.Checkpoint]);
            Assert.AreEqual("true", slotOptions[Options.PauseOnStart]);
        }

        [Test]
        public void SlotOptions_Load_FromClientVersion_7_6_6()
        {
            var slotOptions = SlotOptions.Load(TestDataReader.ReadStringBuilder("Client_7_6_6_slot-options.txt"));
            Assert.AreEqual("advanced", slotOptions[Options.ClientType]);
            Assert.AreEqual("0", slotOptions[Options.CUDAIndex]);
            Assert.AreEqual("0", slotOptions[Options.GPUIndex]);
            Assert.AreEqual("1", slotOptions[Options.MachineID]);
            Assert.AreEqual("big", slotOptions[Options.MaxPacketSize]);
            Assert.AreEqual("0", slotOptions[Options.OpenCLIndex]);
            Assert.AreEqual("True", slotOptions[Options.PauseOnStart]);
            Assert.AreEqual("false", slotOptions[Options.Paused]);
        }

        [Test]
        public void SlotOptions_Load_ReturnsNullWhenJsonStringIsNull()
        {
            Assert.IsNull(SlotOptions.Load((string)null));
        }

        [Test]
        public void SlotOptions_Load_ReturnsNullWhenJsonStringBuilderIsNull()
        {
            Assert.IsNull(SlotOptions.Load((StringBuilder)null));
        }

        [Test]
        public void SlotOptions_Load_ReturnsNullWhenJsonTextReaderIsNull()
        {
            Assert.IsNull(SlotOptions.Load((TextReader)null));
        }
    }
}
