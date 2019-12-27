
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class InfoTests
    {
        [Test]
        public void Info_Load_FromClientVersion_7_1_24()
        {
            var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_info.txt"));
            Assert.AreEqual("http://folding.stanford.edu/", info.Client.Website);
            Assert.AreEqual("(c) 2009,2010 Stanford University", info.Client.Copyright);
            Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
            Assert.AreEqual("--lifeline 1232 --command-port=36330", info.Client.Args);
            Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient/config.xml", info.Client.Config);
            Assert.AreEqual("7.1.24", info.Build.Version);
            Assert.AreEqual("Apr  6 2011", info.Build.Date);
            Assert.AreEqual("21:37:58", info.Build.Time);
            Assert.AreEqual(2908, info.Build.SVNRev);
            Assert.AreEqual("fah/trunk/client", info.Build.Branch);
            Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1110", info.Build.Compiler);
            Assert.AreEqual("/TP /nologo /EHa /wd4297 /wd4103 /wd1786 /Ox -arch:SSE2 /QaxSSE3,SSSE3,SSE4.1,SSE4.2 /Qrestrict /MT", info.Build.Options);
            Assert.AreEqual("win32 Vista", info.Build.Platform);
            Assert.AreEqual(32, info.Build.Bits);
            Assert.AreEqual("Release", info.Build.Mode);
            Assert.AreEqual("Microsoft(R) Windows(R) XP Professional x64 Edition", info.System.OS);
            Assert.AreEqual("Intel(R) Core(TM)2 Quad CPU    Q6600  @ 2.40GHz", info.System.CPU);
            Assert.AreEqual("GenuineIntel Family 6 Model 15 Stepping 11", info.System.CPUID);
            Assert.AreEqual(4, info.System.CPUs);
            Assert.AreEqual("4.00GiB", info.System.Memory);
            Assert.AreEqual(4.0, info.System.MemoryValue);
            Assert.AreEqual("3.10GiB", info.System.FreeMemory);
            Assert.AreEqual(3.1, info.System.FreeMemoryValue);
            Assert.AreEqual("WINDOWS_THREADS", info.System.Threads);
            Assert.AreEqual(1, info.System.GPUs);
            Assert.IsNotNull(info.System.GPUInfos);
            Assert.AreEqual(1, info.System.GPUInfos.Count);
            Assert.AreEqual("ATI:2 Mobility Radeon HD 3600 Series", info.System.GPUInfos[0].GPU);
            Assert.AreEqual("ATI:2 Mobility Radeon HD 3600 Series", info.System.GPUInfos[0].FriendlyName);
            Assert.AreEqual("Not detected", info.System.CUDA);
            Assert.AreEqual(null, info.System.CUDADriver);
            Assert.AreEqual(false, info.System.HasBattery);
            Assert.AreEqual(false, info.System.OnBattery);
            Assert.AreEqual(-5, info.System.UtcOffset);
            Assert.AreEqual(3080, info.System.PID);
            Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient", info.System.CWD);
            Assert.AreEqual(true, info.System.Win32Service);
        }

        [Test]
        public void Info_Load_FromClientVersion_7_1_43()
        {
            var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_info.txt"));
            Assert.AreEqual("http://folding.stanford.edu/", info.Client.Website);
            Assert.AreEqual("(c) 2009-2012 Stanford University", info.Client.Copyright);
            Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
            Assert.AreEqual("--lifeline 2600 --command-port=36330", info.Client.Args);
            Assert.AreEqual("", info.Client.Config);
            Assert.AreEqual("7.1.43", info.Build.Version);
            Assert.AreEqual("Jan  2 2012", info.Build.Date);
            Assert.AreEqual("12:33:05", info.Build.Time);
            Assert.AreEqual(3223, info.Build.SVNRev);
            Assert.AreEqual("fah/trunk/client", info.Build.Branch);
            Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1200", info.Build.Compiler);
            Assert.AreEqual("/TP /nologo /EHa /Qdiag-disable:4297,4103,1786,279 /Ox -arch:SSE /QaxSSE2,SSE3,SSSE3,SSE4.1,SSE4.2 /Qopenmp /Qrestrict /MT", info.Build.Options);
            Assert.AreEqual("win32 XP", info.Build.Platform);
            Assert.AreEqual(32, info.Build.Bits);
            Assert.AreEqual("Release", info.Build.Mode);
            Assert.AreEqual("Microsoft Windows XP Service Pack 2", info.System.OS);
            Assert.AreEqual("Intel(R) Core(TM)2 Quad CPU    Q6600  @ 2.40GHz", info.System.CPU);
            Assert.AreEqual("GenuineIntel Family 6 Model 15 Stepping 11", info.System.CPUID);
            Assert.AreEqual(4, info.System.CPUs);
            Assert.AreEqual("4.00GiB", info.System.Memory);
            Assert.AreEqual(4.0, info.System.MemoryValue);
            Assert.AreEqual("3.22GiB", info.System.FreeMemory);
            Assert.AreEqual(3.22, info.System.FreeMemoryValue);
            Assert.AreEqual("WINDOWS_THREADS", info.System.Threads);
            Assert.AreEqual(1, info.System.GPUs);
            Assert.IsNotNull(info.System.GPUInfos);
            Assert.AreEqual(1, info.System.GPUInfos.Count);
            Assert.AreEqual("NVIDIA:1 GT200b [GeForce GTX 285]", info.System.GPUInfos[0].GPU);
            Assert.AreEqual("GeForce GTX 285", info.System.GPUInfos[0].FriendlyName);
            Assert.AreEqual("1.3", info.System.CUDA);
            Assert.AreEqual("3010", info.System.CUDADriver);
            Assert.AreEqual(false, info.System.HasBattery);
            Assert.AreEqual(false, info.System.OnBattery);
            Assert.AreEqual(-6, info.System.UtcOffset);
            Assert.AreEqual(1520, info.System.PID);
            Assert.AreEqual("", info.System.CWD);
            Assert.AreEqual(false, info.System.Win32Service);
        }

        [Test]
        public void Info_Load_MemoryValueInGigabytes()
        {
            // Arrange
            const string json = @"[ [ ""System"", [ ""Memory"", ""4.00GiB"" ] ] ]";
            // Act
            var info = Info.Load(json);
            // Assert
            Assert.AreEqual(4.0, info.System.MemoryValue);
        }

        [Test]
        public void Info_Load_MemoryValueInMegabytes()
        {
            // Arrange
            const string json = @"[ [ ""System"", [ ""Memory"", ""512.00MiB"" ] ] ]";
            // Act
            var info = Info.Load(json);
            // Assert
            Assert.AreEqual(0.5, info.System.MemoryValue);
        }

        [Test]
        public void Info_Load_MemoryValueInKilobytes()
        {
            // Arrange
            const string json = @"[ [ ""System"", [ ""Memory"", ""262144.00KiB"" ] ] ]";
            // Act
            var info = Info.Load(json);
            // Assert
            Assert.AreEqual(0.25, info.System.MemoryValue);
        }

        [Test]
        public void Info_Load_MemoryValueInUnknownUnits()
        {
            // Arrange
            const string json = @"[ [ ""System"", [ ""Memory"", ""2.00FiB"" ] ] ]";
            // Act
            var info = Info.Load(json);
            // Assert
            Assert.IsNull(info.System.MemoryValue);
        }
    }
}
