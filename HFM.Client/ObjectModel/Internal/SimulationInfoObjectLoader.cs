
using System;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel.Internal
{
    internal class SimulationInfoObjectLoader : ObjectLoader<SimulationInfo>
    {
        public override SimulationInfo Load(TextReader textReader)
        {
            var obj = LoadJObject(textReader);
            
            var result = new SimulationInfo();
            result.User = GetValue<string>(obj, "user");
            result.Team = GetValue<int?>(obj, "team");
            result.Project = GetValue<int?>(obj, "project");
            result.Run = GetValue<int?>(obj, "run");
            result.Clone = GetValue<int?>(obj, "clone");
            result.Gen = GetValue<int?>(obj, "gen");
            result.CoreType = GetValue<int?>(obj, "core_type");
            result.Core = GetValue<string>(obj, "core");
            result.Description = GetValue<string>(obj, "description");
            result.TotalIterations = GetValue<int?>(obj, "total_iterations");
            result.IterationsDone = GetValue<int?>(obj, "iterations_done");
            result.Energy = GetValue<int?>(obj, "energy");
            result.Temperature = GetValue<int?>(obj, "temperature");
            result.StartTime = GetValue<string>(obj, "start_time");
            result.StartTimeDateTime = Internal.DateTimeConverter.ConvertToDateTime(result.StartTime);
            result.Timeout = GetValue<int?>(obj, "timeout");
            result.TimeoutDateTime = ConvertToDateTime(result.Timeout);
            result.Deadline = GetValue<int?>(obj, "deadline");
            result.DeadlineDateTime = ConvertToDateTime(result.Deadline);
            result.ETA = GetValue<int?>(obj, "eta");
            result.ETATimeSpan = ConvertToTimeSpan(result.ETA);
            result.News = GetValue<string>(obj, "news");
            result.Slot = GetValue<int?>(obj, "slot");
            return result;
        }

        protected override JObject LoadJObject(TextReader textReader)
        {
            using (var reader = new JsonTextReader(textReader))
            {
                reader.DateParseHandling = DateParseHandling.None;
                return JObject.Load(reader);
            }
        }

        private static DateTime? ConvertToDateTime(int? input)
        {
            if (input == null || input == 0) return null;

            return new DateTime(1970, 1, 1).AddSeconds((int)input);
        }

        private static TimeSpan? ConvertToTimeSpan(int? input)
        {
            if (input == null) return null;

            return TimeSpan.FromSeconds((int)input);
        }
    }
}
