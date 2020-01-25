
using System.Net.NetworkInformation;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using HFM.Client.Bench.Mocks;

namespace HFM.Client.Bench
{
    //[MemoryDiagnoser]
    //[SimpleJob(RuntimeMoniker.Net461)]
    //[SimpleJob(RuntimeMoniker.NetCoreApp31)]
    //public class Md5VsSha256
    //{
    //    private const int N = 10000;
    //    private readonly byte[] data;
    //
    //    private readonly SHA256 sha256 = SHA256.Create();
    //    private readonly MD5 md5 = MD5.Create();
    //
    //    public Md5VsSha256()
    //    {
    //        data = new byte[N];
    //        new Random(42).NextBytes(data);
    //    }
    //
    //    [Benchmark]
    //    public byte[] Sha256() => sha256.ComputeHash(data);
    //
    //    [Benchmark]
    //    public byte[] Md5() => md5.ComputeHash(data);
    //}

    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net461)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    public class FahClientCommand
    {
        [Benchmark]
        public int Execute()
        {
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection("", 0, tcpConnectionFactory))
            {
                connection.Open();
                var command = connection.CreateCommand("updates add 0 60 $heartbeat");
                return command.Execute();
            }
        }

        [Benchmark]
        public async Task<int> ExecuteAsync()
        {
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection("", 0, tcpConnectionFactory))
            {
                connection.Open();
                var command = connection.CreateCommand("updates add 0 60 $heartbeat");
                return await command.ExecuteAsync();
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
