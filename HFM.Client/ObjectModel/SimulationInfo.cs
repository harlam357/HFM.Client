
using System;
using System.ComponentModel;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel
{
   /// <summary>
   /// Folding@Home client simulation info message.
   /// </summary>
   public class SimulationInfo
   {
      /// <summary>
      /// Fill the SimulationInfo object with data from the given JsonMessage.
      /// </summary>
      /// <param name="message">Message object containing JSON value and meta-data.</param>
      public static SimulationInfo FromMessage(string message)
      {
         var result = new SimulationInfo();

         var obj = GetJObject(message);
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
         result.StartTimeDateTime = DateTimeConverter.ConvertToDateTime(result.StartTime);
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

      private static JObject GetJObject(string message)
      {
         using (var reader = new JsonTextReader(new StringReader(message)))
         {
            reader.DateParseHandling = DateParseHandling.None;
            return JObject.Load(reader);
         }
      }

      private static T GetValue<T>(JObject obj, params string[] names)
      {
         foreach (var name in names)
         {
            var token = obj[name];
            if (token != null)
            {
               try
               {
                  return token.Value<T>();
               }
               catch (FormatException)
               {
                  // if the data changed in a way where the value can
                  // no longer be converted to the target CLR type
               }
            }
         }
         return default(T);
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

      public string User { get; set; }
      public int? Team { get; set; }
      public int? Project { get; set; }
      public int? Run { get; set; }
      public int? Clone { get; set; }
      public int? Gen { get; set; }
      public int? CoreType { get; set; }
      public string Core { get; set; }
      public string Description { get; set; }
      public int? TotalIterations { get; set; }
      public int? IterationsDone { get; set; }
      public int? Energy { get; set; }
      public int? Temperature { get; set; }
      public string StartTime { get; set; }
      public DateTime? StartTimeDateTime { get; set; }
      public int? Timeout { get; set; }
      public DateTime? TimeoutDateTime { get; set; }
      public int? Deadline { get; set; }
      public DateTime? DeadlineDateTime { get; set; }
      public int? ETA { get; set; }
      public TimeSpan? ETATimeSpan { get; set; }
      public string News { get; set; }
      public int? Slot { get; set; }
   }
}
