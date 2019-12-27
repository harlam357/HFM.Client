
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class SlotCollectionTests
    {
        [Test]
        public void SlotCollection_Load_FromClientVersion_7_1_24()
        {
            var slotCollection = SlotCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_slots.txt"));
            Assert.AreEqual(1, slotCollection.Count);
            Assert.AreEqual(0, slotCollection[0].ID);
            Assert.AreEqual("RUNNING", slotCollection[0].Status);
            Assert.AreEqual(SlotStatus.Running, slotCollection[0].SlotStatus);
            Assert.AreEqual("smp:4", slotCollection[0].Description);
            Assert.AreEqual(true, slotCollection[0].SlotOptions.PauseOnStart);
        }
    }
}
