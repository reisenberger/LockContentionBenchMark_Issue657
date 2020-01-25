using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ConcurrentDictionaryLockContention
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}