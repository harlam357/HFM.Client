
using System;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class UnitCollectionTests
    {
        [Test]
        public void UnitCollection_Load_FromClientVersion_7_1_24()
        {
            var unitCollection = UnitCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_units.txt"));
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

        [Test]
        public void UnitCollection_Load_FromClientVersion_7_1_43()
        {
            var unitCollection = UnitCollection.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_units.txt"));
            Assert.AreEqual(1, unitCollection[0].ID);
            Assert.AreEqual("RUNNING", unitCollection[0].State);
            Assert.AreEqual("OK", unitCollection[0].Error);
            Assert.AreEqual(7610, unitCollection[0].Project);
            Assert.AreEqual(630, unitCollection[0].Run);
            Assert.AreEqual(0, unitCollection[0].Clone);
            Assert.AreEqual(59, unitCollection[0].Gen);
            Assert.AreEqual("0xa4", unitCollection[0].Core);
            Assert.AreEqual("0x00000050664f2dd04de6d4f93deb418d", unitCollection[0].UnitHex);
            Assert.AreEqual("33.00%", unitCollection[0].PercentDone);
            Assert.AreEqual(2000, unitCollection[0].TotalFrames);
            Assert.AreEqual(660, unitCollection[0].FramesDone);
            Assert.AreEqual("2012-01-10T23:20:27", unitCollection[0].Assigned);
            Assert.AreEqual(new DateTime(2012, 1, 10, 23, 20, 27), unitCollection[0].AssignedDateTime);
            Assert.AreEqual("2012-01-22T16:22:51", unitCollection[0].Timeout);
            Assert.AreEqual(new DateTime(2012, 1, 22, 16, 22, 51), unitCollection[0].TimeoutDateTime);
            Assert.AreEqual("2012-01-30T11:49:15", unitCollection[0].Deadline);
            Assert.AreEqual(new DateTime(2012, 1, 30, 11, 49, 15), unitCollection[0].DeadlineDateTime);
            Assert.AreEqual("171.64.65.104", unitCollection[0].WorkServer);
            Assert.AreEqual("171.67.108.49", unitCollection[0].CollectionServer);
            Assert.AreEqual(String.Empty, unitCollection[0].WaitingOn);
            Assert.AreEqual(0, unitCollection[0].Attempts);
            Assert.AreEqual("0.00 secs", unitCollection[0].NextAttempt);
            Assert.AreEqual(TimeSpan.Zero, unitCollection[0].NextAttemptTimeSpan);
            Assert.AreEqual(0, unitCollection[0].Slot);
            Assert.AreEqual("2 hours 38 mins", unitCollection[0].ETA);
            Assert.AreEqual(new TimeSpan(2, 38, 0), unitCollection[0].ETATimeSpan);
            Assert.AreEqual(30495.84, unitCollection[0].PPD);
            Assert.AreEqual("2 mins 26 secs", unitCollection[0].TPF);
            Assert.AreEqual(new TimeSpan(0, 2, 26), unitCollection[0].TPFTimeSpan);
            Assert.AreEqual(788, unitCollection[0].BaseCredit);
            Assert.AreEqual(5172.29, unitCollection[0].CreditEstimate);
            
            Assert.AreEqual(2, unitCollection[1].ID);
            Assert.AreEqual("RUNNING", unitCollection[1].State);
            Assert.AreEqual("OK", unitCollection[0].Error);
            Assert.AreEqual(5772, unitCollection[1].Project);
            Assert.AreEqual(7, unitCollection[1].Run);
            Assert.AreEqual(364, unitCollection[1].Clone);
            Assert.AreEqual(252, unitCollection[1].Gen);
            Assert.AreEqual("0x11", unitCollection[1].Core);
            Assert.AreEqual("0x241a68704f0d0e3a00fc016c0007168c", unitCollection[1].UnitHex);
            Assert.AreEqual("51.00%", unitCollection[1].PercentDone);
            Assert.AreEqual(15000, unitCollection[1].TotalFrames);
            Assert.AreEqual(7650, unitCollection[1].FramesDone);
            Assert.AreEqual("2012-01-11T04:21:14", unitCollection[1].Assigned);
            Assert.AreEqual(new DateTime(2012, 1, 11, 4, 21, 14), unitCollection[1].AssignedDateTime);
            Assert.AreEqual("<invalid>", unitCollection[1].Timeout);
            Assert.AreEqual(null, unitCollection[1].TimeoutDateTime);
            Assert.AreEqual("2012-01-14T04:21:14", unitCollection[1].Deadline);
            Assert.AreEqual(new DateTime(2012, 1, 14, 4, 21, 14), unitCollection[1].DeadlineDateTime);
            Assert.AreEqual("171.67.108.11", unitCollection[1].WorkServer);
            Assert.AreEqual("171.67.108.25", unitCollection[1].CollectionServer);
            Assert.AreEqual(String.Empty, unitCollection[1].WaitingOn);
            Assert.AreEqual(0, unitCollection[1].Attempts);
            Assert.AreEqual("0.00 secs", unitCollection[1].NextAttempt);
            Assert.AreEqual(TimeSpan.Zero, unitCollection[1].NextAttemptTimeSpan);
            Assert.AreEqual(1, unitCollection[1].Slot);
            Assert.AreEqual("27 mins 14 secs", unitCollection[1].ETA);
            Assert.AreEqual(new TimeSpan(0, 27, 14), unitCollection[1].ETATimeSpan);
            Assert.AreEqual(0, unitCollection[1].PPD);
            Assert.AreEqual("33.82 secs", unitCollection[1].TPF);
            Assert.AreEqual(new TimeSpan(0, 0, 0, 33, 820), unitCollection[1].TPFTimeSpan);
            Assert.AreEqual(0, unitCollection[1].BaseCredit);
            Assert.AreEqual(0, unitCollection[1].CreditEstimate);
        }
    }
}
