
using System;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
   [TestFixture]
   public class SimulationInfoTests
   {
      private const string SimulationInfoText = @"{
   ""user"": ""harlam357"",
   ""team"": ""32"",
   ""project"": 11020,
   ""run"": 0,
   ""clone"": 1921,
   ""gen"": 24,
   ""core_type"": 163,
   ""core"": ""GROGBSMP"",
   ""description"": """",
   ""total_iterations"": 1000,
   ""iterations_done"": 590,
   ""energy"": 0,
   ""temperature"": 0,
   ""start_time"": ""27/May/2011-19:34:24"",
   ""timeout"": 1307216064,
   ""deadline"": 1307561664,
   ""run_time"": 13028,
   ""simulation_time"": 0,
   ""eta"": 8844,
   ""news"": """"
}";

      [Test]
      public void SimulationInfo_FromMessage_Test()
      {
         var simulationInfo = SimulationInfo.FromMessage(SimulationInfoText);
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
