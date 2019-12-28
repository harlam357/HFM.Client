
using System.IO;
using System.Text;

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
            Assert.AreEqual("smp:4", slotCollection[0].Description);
            Assert.AreEqual(true, slotCollection[0].SlotOptions.PauseOnStart);
        }

        [Test]
        public void SlotCollection_Load_FromClientVersion_7_1_43()
        {
            var slotCollection = SlotCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_slots.txt"));
            Assert.AreEqual(2, slotCollection.Count);
            Assert.AreEqual(0, slotCollection[0].ID);
            Assert.AreEqual("RUNNING", slotCollection[0].Status);
            Assert.AreEqual("smp:4", slotCollection[0].Description);
            Assert.AreEqual(true, slotCollection[0].SlotOptions.PauseOnStart);
            Assert.AreEqual(1, slotCollection[1].ID);
            Assert.AreEqual("RUNNING", slotCollection[1].Status);
            Assert.AreEqual("gpu:0:\"GT200b [GeForce GTX 285]\"", slotCollection[1].Description);
            Assert.AreEqual(true, slotCollection[1].SlotOptions.PauseOnStart);
        }

        [Test]
        public void SlotCollection_Load_ReturnsNullWhenJsonStringIsNull()
        {
            Assert.IsNull(SlotCollection.Load((string)null));
        }

        [Test]
        public void SlotCollection_Load_ReturnsNullWhenJsonStringBuilderIsNull()
        {
            Assert.IsNull(SlotCollection.Load((StringBuilder)null));
        }

        [Test]
        public void SlotCollection_Load_ReturnsNullWhenJsonTextReaderIsNull()
        {
            Assert.IsNull(SlotCollection.Load((TextReader)null));
        }

        [Test]
        public void SlotCollection_Load_SlotOptionsDoesNotExist()
        {
            // Arrange
            const string json = @"[ { ""id"": ""00"" } ]";
            // Act
            var slotCollection = SlotCollection.Load(json);
            // Assert
            Assert.IsNull(slotCollection[0].SlotOptions);
        }

        [Test]
        public void SlotCollection_Load_MalformedInt32Value()
        {
            // Arrange
            const string json = @"[ { ""id"": ""A"" } ]";
            // Act
            var slotCollection = SlotCollection.Load(json);
            // Assert
            Assert.IsNull(slotCollection[0].ID);
        }
    }
}
