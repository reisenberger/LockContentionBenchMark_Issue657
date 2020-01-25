using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace ConcurrentDictionaryLockContention
{
    [Config(typeof(BenchmarkConfig))]
    public class Benchmarks
    {
        public enum TheWork
        {
            NoWork,
            NoExtraLock,
            ExtraLock,
        }

        [ParamsAllValues]
        public TheWork Work { get; set; }

        [ParamsAllValues]
        public bool DifferentKeys { get; set; }

        [ParamsAllValues]
        public bool LockPerKey { get; set; }

        private string GetKey(int seed) => DifferentKeys ? seed.ToString() : commonKey;

        private object GetLockObject(int seed) => LockPerKey ? lockObjects[seed] : commonLockObject;

        public static int ParallelTasks = 100;

        public readonly Task<Lazy<object>>[] tasks;

        public readonly object[] lockObjects;

        public Task[] tasksCast;

        public ConcurrentDictionary<string, Lazy<object>> collapser = new ConcurrentDictionary<string, Lazy<object>>(
            /*ParallelTasks, Enumerable.Empty<KeyValuePair<string, Lazy<object>>>(), EqualityComparer<string>.Default*/ // Explicitly setting the concurrencyLevel at creation did not significantly alter this benchmark.
            );

        private readonly Func<object, CancellationToken, object> commonAction = (o, token) => new object();
        private readonly object commonInputObject = new object();
        private readonly CancellationToken commonToken = default;

        private readonly object commonLockObject = new object();
        private const string commonKey = "SameKey";

        private ManualResetEventSlim startSignal;

        private readonly SpinWait spinner = new SpinWait();

        private int WaitingForStartSignal = 0;

        /*private int ActualContentionEncountered = 0;*/

        private readonly Lazy<object> flyweightWork = new Lazy<object>();

        public Benchmarks()
        {
            tasks = new Task<Lazy<object>>[ParallelTasks];

            lockObjects = new object[ParallelTasks];
            for (int i = 0; i < ParallelTasks; i++)
            {
                lockObjects[i] = new object();
            }

            // Avoid thread-pool starvation influencing test results.
            ThreadPool.GetMinThreads(out _, out int completionPortThreads);
            ThreadPool.SetMinThreads(ParallelTasks + 25, completionPortThreads);
        }

        public void Setup()
        {
            collapser = collapser = new ConcurrentDictionary<string, Lazy<object>>(
                /*ParallelTasks, Enumerable.Empty<KeyValuePair<string, Lazy<object>>>(), EqualityComparer<string>.Default*/ // Explicitly setting the concurrencyLevel at creation did not significantly alter this benchmark.
            );

            startSignal = new ManualResetEventSlim(false);

            Interlocked.Exchange(ref WaitingForStartSignal, 0);
            /*Interlocked.Exchange(ref ActualContentionEncountered, 0);*/

            Lazy<object> BenchmarkedActivity(string key, object lockObject)
            {
                Interlocked.Increment(ref WaitingForStartSignal);
                startSignal.WaitHandle.WaitOne();

                if (Work == TheWork.ExtraLock)
                {
                    // The commented-out line below can be commented back in (in conjunction with its Console.WriteLine and other related lines) to get a visual indication of how many tasks actually experience lock contention.
                    /*if (!Monitor.TryEnter(lockObject, 0)) { Interlocked.Increment(ref ActualContentionEncountered); } else { Monitor.Exit(lockObject); }*/

                    lock (lockObject)
                    {
                        return collapser.GetOrAdd(key, new Lazy<object>(
                            () => commonAction(commonInputObject, commonToken), /* we don't care what this factory does; we are not running it. We are benchmarking locking over the ConcurrentDictionary<,>.GetOrAdd(). But for similarity to proposed Polly code, it is a delegate of the same form Polly uses. */
                            LazyThreadSafetyMode.ExecutionAndPublication));
                    }
                }

                else if (Work == TheWork.NoExtraLock)
                {
                    return collapser.GetOrAdd(key, new Lazy<object>(
                        () => commonAction(commonInputObject, commonToken), /* we don't care what this factory does; we are not running it. We are benchmarking locking over the ConcurrentDictionary<,>.GetOrAdd(). But for similarity to proposed Polly code, it is a delegate of the same form Polly uses. */
                        LazyThreadSafetyMode.ExecutionAndPublication));
                }

                else if (Work == TheWork.NoWork) { return flyweightWork; }

                else throw new InvalidOperationException($"Unknown value for {nameof(TheWork)}");
            }

            // Set up actions which will contend in parallel.
            for (int i = 0; i < ParallelTasks; i++)
            {
                var key = GetKey(i);
                var lockObject = GetLockObject(i);
                tasks[i] = Task.Run(() => BenchmarkedActivity(key, lockObject));
            }

            tasksCast = tasks.Select(t => t as Task).ToArray();

            /*Console.WriteLine($"Potential contention after queueing tasks: {WaitingForStartSignal}");*/

            // To maximise contention, ensure all Tasks have actually started and are gated at the ManualResetEvent, before proceeding.  
            while (WaitingForStartSignal < ParallelTasks)
            {
                spinner.SpinOnce();
                Thread.Yield();
            }

            /*Console.WriteLine($"Potential contention at the starting gate: {WaitingForStartSignal}");*/
        }

        public void TearDown()
        {
            for (int i = 0; i < ParallelTasks; i++)
            {
                tasks[i] = null;
            }

            collapser = null;

            /*Console.WriteLine($"Actual lock contention encountered: {ActualContentionEncountered} ");*/

            startSignal.Dispose();
            spinner.Reset();
        }

        [Benchmark]
        public void PCLOCD() // ParallelContendLockOverConcurrentDictionary
        {
            Setup();

            startSignal.Set();
            Task.WaitAll(tasksCast);

            TearDown();
        }

        /*
        [Benchmark]
        public object PureConcurrentDictionaryGetOrAdd()
        {
            return collapser.GetOrAdd(commonKey, new Lazy<object>(
                () => commonAction(commonInputObject, commonToken), /* for the purposes of the benchmark we don't care what this factory does; we are not running it. We are benchmarking locking over the ConcurrentDictionary<,>.GetOrAdd() #1#
                LazyThreadSafetyMode.ExecutionAndPublication));
        }
        */

    }
}
