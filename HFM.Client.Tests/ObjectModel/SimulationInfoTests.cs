using System.Text;

using NUnit.Framework;

namespace HFM.Client.ObjectModel;

[TestFixture]
public class SimulationInfoTests
{
    [Test]
    public void SimulationInfo_Load_FromClientVersion_7_1_24()
    {
        var simulationInfo = SimulationInfo.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_simulation-info.txt"), ObjectLoadOptions.None);
        Assert.AreEqual("harlam357", simulationInfo.User);
        Assert.AreEqual(32, simulationInfo.Team);
        Assert.AreEqual(11020, simulationInfo.Project);
        Assert.AreEqual(0, simulationInfo.Run);
        Assert.AreEqual(1921, simulationInfo.Clone);
        Assert.AreEqual(24, simulationInfo.Gen);
        Assert.AreEqual(163, simulationInfo.CoreType);
        Assert.AreEqual("GROGBSMP", simulationInfo.Core);
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
        Assert.AreEqual(new TimeSpan(2, 27, 24), simulationInfo.ETATimeSpan);
        Assert.AreEqual(null, simulationInfo.Progress);
        Assert.AreEqual(null, simulationInfo.Slot);
    }

    [Test]
    public void SimulationInfo_Load_FromClientVersion_7_1_43()
    {
        var simulationInfo = SimulationInfo.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_simulation-info.txt"));
        Assert.AreEqual("harlam357", simulationInfo.User);
        Assert.AreEqual(32, simulationInfo.Team);
        Assert.AreEqual(7610, simulationInfo.Project);
        Assert.AreEqual(630, simulationInfo.Run);
        Assert.AreEqual(0, simulationInfo.Clone);
        Assert.AreEqual(59, simulationInfo.Gen);
        Assert.AreEqual(164, simulationInfo.CoreType);
        Assert.AreEqual("GROGBA4", simulationInfo.Core);
        Assert.AreEqual(2000, simulationInfo.TotalIterations);
        Assert.AreEqual(660, simulationInfo.IterationsDone);
        Assert.AreEqual(0, simulationInfo.Energy);
        Assert.AreEqual(0, simulationInfo.Temperature);
        Assert.AreEqual("2012-01-10T23:20:27", simulationInfo.StartTime);
        Assert.AreEqual(new DateTime(2012, 1, 10, 23, 20, 27), simulationInfo.StartTimeDateTime);
        Assert.AreEqual(1327249371, simulationInfo.Timeout);
        Assert.AreEqual(new DateTime(2012, 1, 22, 16, 22, 51), simulationInfo.TimeoutDateTime);
        Assert.AreEqual(1327924155, simulationInfo.Deadline);
        Assert.AreEqual(new DateTime(2012, 1, 30, 11, 49, 15), simulationInfo.DeadlineDateTime);
        Assert.AreEqual(8682, simulationInfo.ETA);
        Assert.AreEqual(new TimeSpan(2, 24, 42), simulationInfo.ETATimeSpan);
        Assert.AreEqual(null, simulationInfo.Progress);
        Assert.AreEqual(null, simulationInfo.Slot);
    }

    [Test]
    public void SimulationInfo_Load_ReturnsNullWhenJsonStringIsNull()
    {
        Assert.IsNull(SimulationInfo.Load((string)null));
    }

    [Test]
    public void SimulationInfo_Load_ReturnsNullWhenJsonStringBuilderIsNull()
    {
        Assert.IsNull(SimulationInfo.Load((StringBuilder)null));
    }

    [Test]
    public void SimulationInfo_Load_ReturnsNullWhenJsonTextReaderIsNull()
    {
        Assert.IsNull(SimulationInfo.Load((TextReader)null));
    }

    [Test]
    public void SimulationInfo_Load_DateTimeValueDoesNotExist()
    {
        // Act
        var si = SimulationInfo.Load("{ }");
        // Assert
        Assert.IsNull(si.StartTimeDateTime);
    }

    [Test]
    public void SimulationInfo_Load_MalformedDateTimeValue()
    {
        // Act
        var si = SimulationInfo.Load(@"{ ""start_time"": ""May 29 2019-00:11:22"" }");
        // Assert
        Assert.IsNull(si.StartTimeDateTime);
    }

    [Test]
    public void SimulationInfo_Load_TimeSpanValueDoesNotExist()
    {
        // Act
        var si = SimulationInfo.Load("{ }");
        // Assert
        Assert.IsNull(si.ETATimeSpan);
    }
}
