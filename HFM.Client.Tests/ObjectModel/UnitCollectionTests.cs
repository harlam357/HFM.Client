
using System;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class UnitCollectionTests
    {
        [Test]
        public void UnitCollection_Load_FromWindowsClientVersion_7_1_24()
        {
            var unitCollection = UnitCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_units_Windows.txt"));
            Assert.AreEqual(0, unitCollection[0].ID);
            Assert.AreEqual("RUNNING", unitCollection[0].State);
            Assert.AreEqual(null, unitCollection[0].Error);
            Assert.AreEqual(11020, unitCollection[0].Project);
            Assert.AreEqual(0, unitCollection[0].Run);
            Assert.AreEqual(1921, unitCollection[0].Clone);
            Assert.AreEqual(24, unitCollection[0].Gen);
            Assert.AreEqual("0xa3", unitCollection[0].Core);
            Assert.AreEqual("0x000000210a3b1e5b4d824701aee79f1e", unitCollection[0].UnitHex);
            Assert.AreEqual("59.00%", unitCollection[0].PercentDone);
            Assert.AreEqual(1000, unitCollection[0].TotalFrames);
            Assert.AreEqual(590, unitCollection[0].FramesDone);
            Assert.AreEqual("27/May/2011-19:34:24", unitCollection[0].Assigned);
            Assert.AreEqual(new DateTime(2011, 5, 27, 19, 34, 24), unitCollection[0].AssignedDateTime);
            Assert.AreEqual("04/Jun/2011-19:34:24", unitCollection[0].Timeout);
            Assert.AreEqual(new DateTime(2011, 6, 4, 19, 34, 24), unitCollection[0].TimeoutDateTime);
            Assert.AreEqual("08/Jun/2011-19:34:24", unitCollection[0].Deadline);
            Assert.AreEqual(new DateTime(2011, 6, 8, 19, 34, 24), unitCollection[0].DeadlineDateTime);
            Assert.AreEqual("171.64.65.55", unitCollection[0].WorkServer);
            Assert.AreEqual("171.67.108.26", unitCollection[0].CollectionServer);
            Assert.AreEqual(String.Empty, unitCollection[0].WaitingOn);
            Assert.AreEqual(0, unitCollection[0].Attempts);
            Assert.AreEqual("0.00 secs", unitCollection[0].NextAttempt);
            Assert.AreEqual(TimeSpan.Zero, unitCollection[0].NextAttemptTimeSpan);
            Assert.AreEqual(0, unitCollection[0].Slot);
            Assert.AreEqual("2 hours 28 mins", unitCollection[0].ETA);
            Assert.AreEqual(new TimeSpan(2, 28, 0), unitCollection[0].ETATimeSpan);
            Assert.AreEqual(1749.96, unitCollection[0].PPD);
            Assert.AreEqual("3 mins 38 secs", unitCollection[0].TPF);
            Assert.AreEqual(new TimeSpan(0, 3, 38), unitCollection[0].TPFTimeSpan);
            Assert.AreEqual(443, unitCollection[0].BaseCredit);
            Assert.AreEqual(443, unitCollection[0].CreditEstimate);
        }
    }
}
