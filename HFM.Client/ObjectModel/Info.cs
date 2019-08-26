
using System;
using System.Linq;

using Newtonsoft.Json.Linq;

//using HFM.Client.Converters;

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

      /// <summary>
      /// Folding@Home client information.
      /// </summary>
      public ClientInfo Client { get; }

      /// <summary>
      /// Folding@Home client build information.
      /// </summary>
      public BuildInfo Build { get; }

      /// <summary>
      /// Folding@Home client system information.
      /// </summary>
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
         result.Build.SvnRev = GetValue<int>(build, "SVN Rev");
         result.Build.Branch = GetValue<string>(build, "Branch");
         result.Build.Compiler = GetValue<string>(build, "Compiler");
         result.Build.Options = GetValue<string>(build, "Options");
         result.Build.Platform = GetValue<string>(build, "Platform");
         result.Build.Bits = GetValue<int>(build, "Bits");
         result.Build.Mode = GetValue<string>(build, "Mode");

         var system = array[2];
         result.System.OperatingSystem = GetValue<string>(system, "OS");
         result.System.Cpu = GetValue<string>(system, "CPU");
         result.System.CpuId = GetValue<string>(system, "CPU ID");
         result.System.CpuCount = GetValue<int>(system, "CPUs");
         result.System.Memory = GetValue<string>(system, "Memory");
         result.System.MemoryValue = ConvertToMemoryValue(result.System.Memory);
         result.System.FreeMemory = GetValue<string>(system, "Free Memory");
         result.System.FreeMemoryValue = ConvertToMemoryValue(result.System.FreeMemory);

         return result;
      }

      private static T GetValue<T>(JToken token, string name)
      {
         var innerArray = token.FirstOrDefault(x => x.FirstOrDefault()?.Value<string>() == name);
         var valueElement = innerArray?.ElementAtOrDefault(1);
         return valueElement != null ? valueElement.Value<T>() : default(T);
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
      public int SvnRev { get; set; }
      public string Branch { get; set; }
      public string Compiler { get; set; }
      public string Options { get; set; }
      public string Platform { get; set; }
      public int Bits { get; set; }
      public string Mode { get; set; }
   }

   /// <summary>
   /// Folding@Home client system information.
   /// </summary>
   public class SystemInfo
   {
      public string OperatingSystem { get; set; }
      public string Cpu { get; set; }
      public string CpuId { get; set; }
      public int CpuCount { get; set; }
      public string Memory { get; set; }
      public double? MemoryValue { get; set; }
      public string FreeMemory { get; set; }
      public double? FreeMemoryValue { get; set; }
      public string ThreadType { get; set; }
      public int GpuCount { get; set; }
      public string GpuId0 { get; set; }
      //[MessageProperty("GPU 0", typeof(GpuTypeConverter))]
      public string Gpu0 { get; set; }
      public string GpuId1 { get; set; }
      //[MessageProperty("GPU 1", typeof(GpuTypeConverter))]
      public string Gpu1 { get; set; }
      public string GpuId2 { get; set; }
      //[MessageProperty("GPU 2", typeof(GpuTypeConverter))]
      public string Gpu2 { get; set; }
      public string GpuId3 { get; set; }
      //[MessageProperty("GPU 3", typeof(GpuTypeConverter))]
      public string Gpu3 { get; set; }
      public string GpuId4 { get; set; }
      //[MessageProperty("GPU 4", typeof(GpuTypeConverter))]
      public string Gpu4 { get; set; }
      public string GpuId5 { get; set; }
      //[MessageProperty("GPU 5", typeof(GpuTypeConverter))]
      public string Gpu5 { get; set; }
      public string GpuId6 { get; set; }
      //[MessageProperty("GPU 6", typeof(GpuTypeConverter))]
      public string Gpu6 { get; set; }
      public string GpuId7 { get; set; }
      //[MessageProperty("GPU 7", typeof(GpuTypeConverter))]
      public string Gpu7 { get; set; }
      public string Cuda { get; set; }
      public string CudaDriver { get; set; }
      public bool HasBattery { get; set; }
      public bool OnBattery { get; set; }
      public int UtcOffset { get; set; }
      public int ProcessId { get; set; }
      public string WorkingDirectory { get; set; }
      public bool? Win32Service { get; set; }
   }
}
