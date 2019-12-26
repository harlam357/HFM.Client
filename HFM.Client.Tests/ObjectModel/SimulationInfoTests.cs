
using System;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class SimulationInfoTests
    {
        [Test]
        public void SimulationInfo_Load_FromWindowsClientVersion_7_1_24()
        {
            var simulationInfo = SimulationInfo.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_simulation-info_Windows.txt"));
            Assert.AreEqual("harlam357", simulationInfo.User);
            Assert.AreEqual(32, simulationInfo.Team);
            Assert.AreEqual(11020, simulationInfo.Project);
            Assert.AreEqual(0, simulationInfo.Run);
            Assert.AreEqual(1921, simulationInfo.Clone);
            Assert.AreEqual(24, simulationInfo.Gen);
            Assert.AreEqual(163, simulationInfo.CoreType);
            Assert.AreEqual("GROGBSMP", simulationInfo.Core);
            Assert.AreEqual(String.Empty, simulationInfo.Description);
            Assert.AreEqual(1000, simulationInfo.TotalIterations);
            Assert.AreEqual(590, simulationInfo.IterationsDone);
            Assert.AreEqual(0, simulationInfo.Energy);
            Assert.AreEqual(0, simulationInfo.Temperature);
            Assert.AreEqual("27/May/2011-19:34:24", simulationInfo.StartTime);
            Assert.AreEqual(new DateTime(2011, 5, 27, 19, 34, 24), simulationInfo.StartTimeDateTime);
            Assert.AreEqual(1307216064, simulationInfo.Timeout);
            Assert.AreEqual(new DateTime(2011, 6, 4, 19, 34, 24), simulationInfo.TimeoutDateTime);
            Assert.AreEqual(1307561664, simulationInfo.Deadline);
            Assert.AreEqual(new DateTime(2011, 6, 8, 19, 34, 24), simulationInfo.DeadlineDateTime);
            Assert.AreEqual(8844, simulationInfo.ETA);
            // not exactly the same value seen in Unit.EtaTimeSpan
            Assert.AreEqual(new TimeSpan(2, 27, 24), simulationInfo.ETATimeSpan);
            Assert.AreEqual(String.Empty, simulationInfo.News);
            Assert.AreEqual(null, simulationInfo.Slot);
        }
    }
}
