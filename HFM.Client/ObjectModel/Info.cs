
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

namespace HFM.Client.ObjectModel
{
   /// <summary>
   /// Folding@Home client info message.
   /// </summary>
   public class Info
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Info"/> class.
      /// </summary>
      public Info()
      {
         Client = new ClientInfo();
         Build = new BuildInfo();
         System = new SystemInfo();
      }

      public ClientInfo Client { get; }

      public BuildInfo Build { get; }

      public SystemInfo System { get; }

      /// <summary>
      /// Creates a new <see cref="Info"/> object from the given JSON message.
      /// </summary>
      public static Info FromJson(string message)
      {
         var result = new Info();
         var array = JArray.Parse(message);

         var client = array[0];
         result.Client.Website = GetValue<string>(client, "Website");
         result.Client.Copyright = GetValue<string>(client, "Copyright");
         result.Client.Author = GetValue<string>(client, "Author");
         result.Client.Args = GetValue<string>(client, "Args")?.Trim();
         result.Client.Config = GetValue<string>(client, "Config");

         var build = array[1];
         result.Build.Version = GetValue<string>(build, "Version");
         result.Build.Date = GetValue<string>(build, "Date");
         result.Build.Time = GetValue<string>(build, "Time");
         result.Build.SVNRev = GetValue<int>(build, "SVN Rev");
         result.Build.Branch = GetValue<string>(build, "Branch");
         result.Build.Compiler = GetValue<string>(build, "Compiler");
         result.Build.Options = GetValue<string>(build, "Options");
         result.Build.Platform = GetValue<string>(build, "Platform");
         result.Build.Bits = GetValue<int>(build, "Bits");
         result.Build.Mode = GetValue<string>(build, "Mode");

         var system = array[2];
         result.System.OS = GetValue<string>(system, "OS");
         result.System.CPU = GetValue<string>(system, "CPU");
         result.System.CPUID = GetValue<string>(system, "CPU ID");
         result.System.CPUs = GetValue<int>(system, "CPUs");
         result.System.Memory = GetValue<string>(system, "Memory");
         result.System.MemoryValue = ConvertToMemoryValue(result.System.Memory);
         result.System.FreeMemory = GetValue<string>(system, "Free Memory");
         result.System.FreeMemoryValue = ConvertToMemoryValue(result.System.FreeMemory);
         result.System.Threads = GetValue<string>(system, "Threads");
         result.System.GPUs = GetValue<int>(system, "GPUs");
         result.System.GPUInfos = BuildGPUInfos(system);
         result.System.CUDA = GetValue<string>(system, "CUDA");
         result.System.CUDADriver = GetValue<string>(system, "CUDA Driver");
         result.System.HasBattery = GetValue<bool>(system, "Has Battery");
         result.System.OnBattery = GetValue<bool>(system, "On Battery");
         result.System.UtcOffset = GetValue<int>(system, "UTC offset");
         result.System.PID = GetValue<int>(system, "PID");
         result.System.CWD = GetValue<string>(system, "CWD");
         result.System.Win32Service = GetValue<bool>(system, "Win32 Service");

         return result;
      }

      private static T GetValue<T>(JToken token, string name)
      {
         var innerArray = token.FirstOrDefault(x => x.FirstOrDefault()?.Value<string>() == name);
         var valueElement = innerArray?.ElementAtOrDefault(1);
         if (valueElement != null)
         {
            try
            {
               return valueElement.Value<T>();
            }
            catch (FormatException)
            {
               // if the data changed in a way where the value can
               // no longer be converted to the target CLR type
            }
         }
         return default(T);
      }

      private static IDictionary<int, GPUInfo> BuildGPUInfos(JToken token)
      {
         var result = Enumerable.Range(0, 8)
            .Select(id =>
            {
               var gpu = GetValue<string>(token, String.Concat("GPU ", id));
               return gpu != null ? new GPUInfo { ID = id, GPU = gpu, FriendlyName = ConvertToGPUFriendlyName(gpu) } : null;
            })
            .Where(x => x != null)
            .ToDictionary(x => x.ID);

         return result.Count > 0 ? result : null;
      }

      private static double? ConvertToMemoryValue(string input)
      {
         double value;
         // always returns value in gigabytes
         int gigabyteIndex = input.IndexOf("GiB", StringComparison.Ordinal);
         if (gigabyteIndex > 0)
         {
            if (Double.TryParse(input.Substring(0, gigabyteIndex), out value))
            {
               return value;
            }
         }
         int megabyteIndex = input.IndexOf("MiB", StringComparison.Ordinal);
         if (megabyteIndex > 0)
         {
            if (Double.TryParse(input.Substring(0, megabyteIndex), out value))
            {
               return value / 1024;
            }
         }
         int kilobyteIndex = input.IndexOf("KiB", StringComparison.Ordinal);
         if (kilobyteIndex > 0)
         {
            if (Double.TryParse(input.Substring(0, kilobyteIndex), out value))
            {
               return value / 1048576;
            }
         }
         return null;
      }

      private static string ConvertToGPUFriendlyName(string input)
      {
         if (input == null) return null;

         var regex = new Regex("\\[(?<GpuType>.+)\\]", RegexOptions.ExplicitCapture | RegexOptions.Singleline);
         Match match;
         if ((match = regex.Match(input)).Success)
         {
            return match.Groups["GpuType"].Value;
         }

         return input;
      }
   }

   /// <summary>
   /// Folding@Home client information.
   /// </summary>
   public class ClientInfo
   {
      public string Website { get; set; }
      public string Copyright { get; set; }
      public string Author { get; set; }
      public string Args { get; set; }
      public string Config { get; set; }
   }

   /// <summary>
   /// Folding@Home client build information.
   /// </summary>
   public class BuildInfo
   {
      public string Version { get; set; }
      public string Date { get; set; }
      public string Time { get; set; }
      public int? SVNRev { get; set; }
      public string Branch { get; set; }
      public string Compiler { get; set; }
      public string Options { get; set; }
      public string Platform { get; set; }
      public int? Bits { get; set; }
      public string Mode { get; set; }
   }

   /// <summary>
   /// Folding@Home client system information.
   /// </summary>
   public class SystemInfo
   {
      public string OS { get; set; }
      public string CPU { get; set; }
      public string CPUID { get; set; }
      public int? CPUs { get; set; }
      public string Memory { get; set; }
      public double? MemoryValue { get; set; }
      public string FreeMemory { get; set; }
      public double? FreeMemoryValue { get; set; }
      public string Threads { get; set; }
      public int? GPUs { get; set; }
      public IDictionary<int, GPUInfo> GPUInfos { get; set; }
      public string CUDA { get; set; }
      public string CUDADriver { get; set; }
      public bool? HasBattery { get; set; }
      public bool? OnBattery { get; set; }
      public int? UtcOffset { get; set; }
      public int? PID { get; set; }
      public string CWD { get; set; }
      public bool? Win32Service { get; set; }
   }

   /// <summary>
   /// Folding@Home client GPU information.
   /// </summary>
   public class GPUInfo
   {
      public int ID { get; set; }
      public string GPU { get; set; }
      public string FriendlyName { get; set; }
   }
}
