# LockContentionBenchmark_Issue657

This repo benchmarks single-key-locking versus lock-per-key locking for a potential `ConcurrentDuplicateRequestCollapser` Polly policy under consideration (issue 657).  

The underlying implementation for that policy is a `ConcurrentDictionary<string,Lazy<object>>`.

While `ConcurrentDictionary<,>` uses its own internal striped-lock, the [prototype implementation](https://github.com/reisenberger/Polly/commit/d314db4e7b2eb39baac60f43b51134f4e9772e6a) for `ConcurrentDuplicateRequestCollapser` involves locking over ConcurrentDictionary to support the use case of preventing concurrent duplicate requests via a distributed lock in Redis.
