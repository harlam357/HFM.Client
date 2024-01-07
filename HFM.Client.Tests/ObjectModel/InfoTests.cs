using System.Text;

using NUnit.Framework;

namespace HFM.Client.ObjectModel;

[TestFixture]
public class InfoTests
{
    [Test]
    public void Info_Load_FromClientVersion_7_1_24()
    {
        var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_info.txt"), ObjectLoadOptions.None);
        Assert.AreEqual("http://folding.stanford.edu/", info.Client.Homepage);
        Assert.AreEqual("(c) 2009,2010 Stanford University", info.Client.Copyright);
        Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
        Assert.AreEqual("--lifeline 1232 --command-port=36330", info.Client.Args);
        Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient/config.xml", info.Client.Config);
        Assert.AreEqual("7.1.24", info.Client.Version);
        Assert.AreEqual("Apr  6 2011", info.Client.Date);
        Assert.AreEqual("21:37:58", info.Client.Time);
        Assert.AreEqual(null, info.Client.Revision);
        Assert.AreEqual("fah/trunk/client", info.Client.Branch);
        Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1110", info.Client.Compiler);
        Assert.AreEqual("/TP /nologo /EHa /wd4297 /wd4103 /wd1786 /Ox -arch:SSE2 /QaxSSE3,SSSE3,SSE4.1,SSE4.2 /Qrestrict /MT", info.Client.Options);
        Assert.AreEqual("win32 Vista", info.Client.Platform);
        Assert.AreEqual(32, info.Client.Bits);
        Assert.AreEqual("Release", info.Client.Mode);
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
        Assert.AreEqual(null, info.System.GPUInfos[0].CUDADevice);
        Assert.AreEqual(null, info.System.GPUInfos[0].OpenCLDevice);
        Assert.AreEqual(null, info.System.HasBattery);
        Assert.AreEqual(false, info.System.OnBattery);
        Assert.AreEqual(-5, info.System.UtcOffset);
        Assert.AreEqual(3080, info.System.PID);
        Assert.AreEqual("C:/Documents and Settings/user/Application Data/FAHClient", info.System.CWD);
        Assert.AreEqual(true, info.System.Win32Service);
    }

    [Test]
    public void Info_Load_FromClientVersion_7_1_43()
    {
        var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_1_43_info.txt"), ObjectLoadOptions.None);
        Assert.AreEqual("http://folding.stanford.edu/", info.Client.Homepage);
        Assert.AreEqual("(c) 2009-2012 Stanford University", info.Client.Copyright);
        Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
        Assert.AreEqual("--lifeline 2600 --command-port=36330", info.Client.Args);
        Assert.AreEqual("", info.Client.Config);
        Assert.AreEqual("7.1.43", info.Client.Version);
        Assert.AreEqual("Jan  2 2012", info.Client.Date);
        Assert.AreEqual("12:33:05", info.Client.Time);
        Assert.AreEqual(null, info.Client.Revision);
        Assert.AreEqual("fah/trunk/client", info.Client.Branch);
        Assert.AreEqual("Intel(R) C++ MSVC 1500 mode 1200", info.Client.Compiler);
        Assert.AreEqual("/TP /nologo /EHa /Qdiag-disable:4297,4103,1786,279 /Ox -arch:SSE /QaxSSE2,SSE3,SSSE3,SSE4.1,SSE4.2 /Qopenmp /Qrestrict /MT", info.Client.Options);
        Assert.AreEqual("win32 XP", info.Client.Platform);
        Assert.AreEqual(32, info.Client.Bits);
        Assert.AreEqual("Release", info.Client.Mode);
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
        Assert.AreEqual(null, info.System.GPUInfos[0].CUDADevice);
        Assert.AreEqual(null, info.System.GPUInfos[0].OpenCLDevice);
        Assert.AreEqual(null, info.System.HasBattery);
        Assert.AreEqual(false, info.System.OnBattery);
        Assert.AreEqual(-6, info.System.UtcOffset);
        Assert.AreEqual(1520, info.System.PID);
        Assert.AreEqual("", info.System.CWD);
        Assert.AreEqual(false, info.System.Win32Service);
    }

    [Test]
    public void Info_Load_FromClientVersion_7_6_6()
    {
        var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_6_6_info.txt"));
        Assert.AreEqual("7.6.6", info.Client.Version);
        Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
        Assert.AreEqual("2020 foldingathome.org", info.Client.Copyright);
        Assert.AreEqual("https://foldingathome.org/", info.Client.Homepage);
        Assert.AreEqual("Apr 13 2020", info.Client.Date);
        Assert.AreEqual("13:44:19", info.Client.Time);
        Assert.AreEqual("428982f54ecbe16c54b1e65f2ccf1161852c0f52", info.Client.Revision);
        Assert.AreEqual("master", info.Client.Branch);
        Assert.AreEqual("Visual C++ 2008", info.Client.Compiler);
        Assert.AreEqual(" /TP  /nologo /EHa /wd4297 /wd4103 /Ox /MT", info.Client.Options);
        Assert.AreEqual("win32 10", info.Client.Platform);
        Assert.AreEqual(32, info.Client.Bits);
        Assert.AreEqual("Release", info.Client.Mode);
        Assert.AreEqual("--config C:\\Users\\user\\AppData\\Roaming\\FAHClient\\config.xml", info.Client.Args);
        Assert.AreEqual("C:\\Users\\user\\AppData\\Roaming\\FAHClient\\config.xml", info.Client.Config);
            
        Assert.AreEqual("AMD Ryzen 5 3600 6-Core Processor", info.System.CPU);
        Assert.AreEqual("AuthenticAMD Family 23 Model 113 Stepping 0", info.System.CPUID);
        Assert.AreEqual(12, info.System.CPUs);
        Assert.AreEqual("15.91GiB", info.System.Memory);
        Assert.AreEqual(15.91, info.System.MemoryValue);
        Assert.AreEqual("12.99GiB", info.System.FreeMemory);
        Assert.AreEqual(12.99, info.System.FreeMemoryValue);
        Assert.AreEqual("WINDOWS_THREADS", info.System.Threads);
        Assert.AreEqual("6.2", info.System.OSVersion);
        Assert.AreEqual(false, info.System.HasBattery);
        Assert.AreEqual(false, info.System.OnBattery);
        Assert.AreEqual(-5, info.System.UtcOffset);
        Assert.AreEqual(7600, info.System.PID);
        Assert.AreEqual("C:\\Users\\user\\AppData\\Roaming\\FAHClient", info.System.CWD);
        Assert.AreEqual("Windows 10 Enterprise", info.System.OS);
        Assert.AreEqual(1, info.System.GPUs);
        Assert.IsNotNull(info.System.GPUInfos);
        Assert.AreEqual(1, info.System.GPUInfos.Count);
        Assert.AreEqual("Bus:8 Slot:0 Func:0 NVIDIA:7 TU116 [GeForce GTX 1660 Ti]", info.System.GPUInfos[0].GPU);
        Assert.AreEqual("Platform:0 Device:0 Bus:8 Slot:0 Compute:7.5 Driver:10.1", info.System.GPUInfos[0].CUDADevice);
        Assert.AreEqual("Platform:0 Device:0 Bus:8 Slot:0 Compute:1.2 Driver:432.0", info.System.GPUInfos[0].OpenCLDevice);
        Assert.AreEqual(false, info.System.Win32Service);
    }

    [Test]
    public void Info_Load_FromClientVersion_7_6_21_fr_FR()
    {
        var info = Info.Load(TestDataReader.ReadStringBuilder("Client_7_6_21_fr-FR_info.txt"));
        Assert.AreEqual("7.6.21", info.Client.Version);
        Assert.AreEqual("Joseph Coffland <joseph@cauldrondevelopment.com>", info.Client.Author);
        Assert.AreEqual("2020 foldingathome.org", info.Client.Copyright);
        Assert.AreEqual("https://foldingathome.org/", info.Client.Homepage);
        Assert.AreEqual("Oct 20 2020", info.Client.Date);
        Assert.AreEqual("13:41:04", info.Client.Time);
        Assert.AreEqual("6efbf0e138e22d3963e6a291f78dcb9c6422a278", info.Client.Revision);
        Assert.AreEqual("master", info.Client.Branch);
        Assert.AreEqual("Visual C++ 2015", info.Client.Compiler);
        Assert.AreEqual(" /TP  /nologo /EHa /wd4297 /wd4103 /O2 /Zc:throwingNew /MT", info.Client.Options);
        Assert.AreEqual("win32 10", info.Client.Platform);
        Assert.AreEqual(32, info.Client.Bits);
        Assert.AreEqual("Release", info.Client.Mode);
        Assert.IsNull(info.Client.Args);
        Assert.AreEqual(@"C:\Users\SDP\AppData\Roaming\FAHClient\config.xml", info.Client.Config);

        Assert.AreEqual("Intel(R) Core(TM) i9-9900K CPU @ 3.60GHz", info.System.CPU);
        Assert.AreEqual("GenuineIntel Family 6 Model 158 Stepping 12", info.System.CPUID);
        Assert.AreEqual(16, info.System.CPUs);
        Assert.AreEqual("15.87GiB", info.System.Memory);
        Assert.AreEqual(15.87, info.System.MemoryValue);
        Assert.AreEqual("13.64GiB", info.System.FreeMemory);
        Assert.AreEqual(13.64, info.System.FreeMemoryValue);
        Assert.AreEqual("WINDOWS_THREADS", info.System.Threads);
        Assert.AreEqual("6.2", info.System.OSVersion);
        Assert.AreEqual(false, info.System.HasBattery);
        Assert.AreEqual(false, info.System.OnBattery);
        Assert.AreEqual(2, info.System.UtcOffset);
        Assert.AreEqual(5344, info.System.PID);
        Assert.AreEqual(@"C:\Users\SDP\AppData\Roaming\FAHClient", info.System.CWD);
        Assert.AreEqual("Windows 10 Enterprise", info.System.OS);
        Assert.AreEqual(1, info.System.GPUs);
        Assert.IsNotNull(info.System.GPUInfos);
        Assert.AreEqual(1, info.System.GPUInfos.Count);
        Assert.AreEqual("Bus:0 Slot:2 Func:0 INTEL:1 CFL GT2 [UHD Graphics 630]", info.System.GPUInfos[0].GPU);
        Assert.IsNull(info.System.GPUInfos[0].CUDADevice);
        Assert.AreEqual("Platform:0 Device:0 Bus:NA Slot:NA Compute:3.0 Driver:27.20", info.System.GPUInfos[0].OpenCLDevice);
        Assert.AreEqual(false, info.System.Win32Service);
    }

    [Test]
    public void Info_Load_ReturnsNullWhenJsonStringIsNull()
    {
        Assert.IsNull(Info.Load((string)null));
    }

    [Test]
    public void Info_Load_ReturnsNullWhenJsonStringBuilderIsNull()
    {
        Assert.IsNull(Info.Load((StringBuilder)null));
    }

    [Test]
    public void Info_Load_ReturnsNullWhenJsonTextReaderIsNull()
    {
        Assert.IsNull(Info.Load((TextReader)null));
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

    [Test]
    public void Info_Load_MalformedInt32Value()
    {
        // Arrange
        const string json = @"[ [ ""System"", [ ""GPUs"", ""A"" ] ] ]";
        // Act
        var info = Info.Load(json);
        // Assert
        Assert.IsNull(info.System.GPUs);
    }
}
