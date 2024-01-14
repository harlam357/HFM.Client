using System.Text;

using NUnit.Framework;

namespace HFM.Client.ObjectModel;

[TestFixture]
public class SlotCollectionTests
{
    [Test]
    public void SlotCollection_Load_FromClientVersion_7_1_24()
    {
        var slotCollection = SlotCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_slots.txt"), ObjectLoadOptions.None)!;
        Assert.AreEqual(1, slotCollection.Count);
        Assert.AreEqual(0, slotCollection[0].ID);
        Assert.AreEqual("RUNNING", slotCollection[0].Status);
        Assert.AreEqual("smp:4", slotCollection[0].Description);
        Assert.AreEqual("true", slotCollection[0].SlotOptions![Options.PauseOnStart]);
    }

    [Test]
    public void SlotCollection_Load_FromClientVersion_7_1_43()
    {
        var slotCollection = SlotCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_slots.txt"))!;
        Assert.AreEqual(2, slotCollection.Count);
        Assert.AreEqual(0, slotCollection[0].ID);
        Assert.AreEqual("RUNNING", slotCollection[0].Status);
        Assert.AreEqual("smp:4", slotCollection[0].Description);
        Assert.AreEqual("true", slotCollection[0].SlotOptions![Options.PauseOnStart]);
        Assert.AreEqual(1, slotCollection[1].ID);
        Assert.AreEqual("RUNNING", slotCollection[1].Status);
        Assert.AreEqual("gpu:0:\"GT200b [GeForce GTX 285]\"", slotCollection[1].Description);
        Assert.AreEqual("true", slotCollection[1].SlotOptions![Options.PauseOnStart]);
    }

    [Test]
    public void SlotCollection_Load_FromClientVersion_7_6_21()
    {
        var extractor = new FahClientJsonMessageExtractor();
        var message = extractor.Extract(TestDataReader.ReadStringBuilder("Client_7_6_21_slots.txt"))!;
        var slotCollection = SlotCollection.Load(message.MessageText)!;
        Assert.AreEqual(2, slotCollection.Count);
        Assert.AreEqual(0, slotCollection[0].ID);
        Assert.AreEqual("PAUSED", slotCollection[0].Status);
        Assert.AreEqual("cpu:12", slotCollection[0].Description);
        Assert.AreEqual("advanced", slotCollection[0].SlotOptions![Options.ClientType]);
        Assert.AreEqual("12", slotCollection[0].SlotOptions![Options.CPUs]);
        Assert.AreEqual("big", slotCollection[0].SlotOptions![Options.MaxPacketSize]);
        Assert.AreEqual("True", slotCollection[0].SlotOptions![Options.PauseOnStart]);
        Assert.AreEqual("True", slotCollection[0].SlotOptions![Options.Paused]);
        Assert.AreEqual(3, slotCollection[1].ID);
        Assert.AreEqual("RUNNING", slotCollection[1].Status);
        Assert.AreEqual("gpu:14:0 GA102 [GeForce RTX 3080 Lite Hash Rate]", slotCollection[1].Description);
        Assert.AreEqual("big", slotCollection[1].SlotOptions![Options.MaxPacketSize]);
        Assert.AreEqual("True", slotCollection[1].SlotOptions![Options.PauseOnStart]);
        Assert.AreEqual("False", slotCollection[1].SlotOptions![Options.Paused]);
        Assert.AreEqual("14", slotCollection[1].SlotOptions![Options.PciBus]);
        Assert.AreEqual("0", slotCollection[1].SlotOptions![Options.PciSlot]);
    }

    [Test]
    public void SlotCollection_Load_ReturnsNullWhenJsonStringIsNull()
    {
        Assert.IsNull(SlotCollection.Load((string?)null));
    }

    [Test]
    public void SlotCollection_Load_ReturnsNullWhenJsonStringBuilderIsNull()
    {
        Assert.IsNull(SlotCollection.Load((StringBuilder?)null));
    }

    [Test]
    public void SlotCollection_Load_ReturnsNullWhenJsonTextReaderIsNull()
    {
        Assert.IsNull(SlotCollection.Load((TextReader?)null));
    }

    [Test]
    public void SlotCollection_Load_SlotOptionsDoesNotExist()
    {
        // Arrange
        const string json = @"[ { ""id"": ""00"" } ]";
        // Act
        var slotCollection = SlotCollection.Load(json)!;
        // Assert
        Assert.IsNull(slotCollection[0].SlotOptions);
    }

    [Test]
    public void SlotCollection_Load_MalformedInt32Value()
    {
        // Arrange
        const string json = @"[ { ""id"": ""A"" } ]";
        // Act
        var slotCollection = SlotCollection.Load(json)!;
        // Assert
        Assert.IsNull(slotCollection[0].ID);
    }
}
