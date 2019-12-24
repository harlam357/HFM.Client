
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class SlotCollectionTests
    {
        private const string SlotCollectionText = @"[
   {
    ""id"": ""00"",
      ""status"": ""RUNNING"",
      ""description"": ""smp:4"",
      ""options"": {
         ""pause-on-start"": ""true""
      }
   }
]";

        [Test]
        public void SlotCollection_FromMessage_Test()
        {
            var slotCollection = SlotCollection.FromMessage(SlotCollectionText);
            Assert.AreEqual(1, slotCollection.Count);
            Assert.AreEqual(0, slotCollection[0].ID);
            Assert.AreEqual("RUNNING", slotCollection[0].Status);
            Assert.AreEqual(SlotStatus.Running, slotCollection[0].SlotStatus);
            Assert.AreEqual("smp:4", slotCollection[0].Description);
            Assert.AreEqual(true, slotCollection[0].SlotOptions.PauseOnStart);
        }
    }
}
