using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal;

internal class SimulationInfoObjectLoader : ObjectLoader<SimulationInfo>
{
    public override SimulationInfo? Load(TextReader? textReader)
    {
        if (textReader is null) return null;

        var obj = LoadJsonObject(textReader);
            
        var si = new SimulationInfo();
        si.User = GetValue<string>(obj, "user");
        si.Team = GetValue<string>(obj, "team").ToNullableInt32();
        si.Project = GetValue<int?>(obj, "project");
        si.Run = GetValue<int?>(obj, "run");
        si.Clone = GetValue<int?>(obj, "clone");
        si.Gen = GetValue<int?>(obj, "gen");
        si.CoreType = GetValue<int?>(obj, "core_type");
        si.Core = GetValue<string>(obj, "core");
        si.TotalIterations = GetValue<int?>(obj, "total_iterations");
        si.IterationsDone = GetValue<int?>(obj, "iterations_done");
        si.Energy = GetValue<int?>(obj, "energy");
        si.Temperature = GetValue<int?>(obj, "temperature");
        si.StartTime = GetValue<string>(obj, "start_time");
        si.StartTimeDateTime = DateTimeConverter.ConvertToDateTime(si.StartTime);
        si.Timeout = GetValue<int?>(obj, "timeout");
        si.TimeoutDateTime = ConvertToDateTime(si.Timeout);
        si.Deadline = GetValue<int?>(obj, "deadline");
        si.DeadlineDateTime = ConvertToDateTime(si.Deadline);
        si.ETA = GetValue<int?>(obj, "eta");
        si.ETATimeSpan = ConvertToTimeSpan(si.ETA);
        si.Progress = GetValue<double?>(obj, "progress");
        si.Slot = GetValue<int?>(obj, "slot");
        return si;
    }

    private static DateTime? ConvertToDateTime(int? input)
    {
        if (input is null || input == 0) return null;

        return new DateTime(1970, 1, 1).AddSeconds((int)input);
    }

    private static TimeSpan? ConvertToTimeSpan(int? input)
    {
        if (input is null) return null;

        return TimeSpan.FromSeconds((int)input);
    }
}
