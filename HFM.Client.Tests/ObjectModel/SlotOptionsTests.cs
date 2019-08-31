
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
   [TestFixture]
   public class SlotOptionsTests
   {
      private const string SlotOptionsText = @"{
   ""client-type"": ""normal"",
   ""client-subtype"": ""SMP"",
   ""machine-id"": ""0"",
   ""max-packet-size"": ""normal"",
   ""core-priority"": ""idle"",
   ""next-unit-percentage"": ""99"",
   ""max-units"": ""0"",
   ""checkpoint"": ""15"",
   ""pause-on-start"": ""true"",
   ""gpu-vendor-id"": null,
   ""gpu-device-id"": null
}";

      [Test]
      public void SlotOptions_FromMessage_Test()
      {
         var slotOptions = SlotOptions.FromMessage(SlotOptionsText);
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
