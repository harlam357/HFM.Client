
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class SlotOptionsTests
    {
        [Test]
        public void SlotOptions_Load_FromClientVersion_7_1_24()
        {
            var slotOptions = SlotOptions.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_slot-options.txt"));
            Assert.AreEqual("normal", slotOptions.ClientType);
            Assert.AreEqual("SMP", slotOptions.ClientSubType);
            Assert.AreEqual(0, slotOptions.MachineID);
            Assert.AreEqual("normal", slotOptions.MaxPacketSize);
            Assert.AreEqual("idle", slotOptions.CorePriority);
            Assert.AreEqual(99, slotOptions.NextUnitPercentage);
            Assert.AreEqual(0, slotOptions.MaxUnits);
            Assert.AreEqual(15, slotOptions.Checkpoint);
            Assert.AreEqual(true, slotOptions.PauseOnStart);
            Assert.AreEqual(null, slotOptions.GPUIndex);
            Assert.AreEqual(null, slotOptions.GPUUsage);
        }
    }
}
