
| Method |        Work | DifferentKeys | LockPerKey |     Mean |     Error |    StdDev |
|------- |------------ |-------------- |----------- |---------:|----------:|----------:|
| **Contention 1** |      **NoWork** |         **False** |      **False** | **5.147 us** | **0.0216 us** | **0.0415 us** |
| **Contention 1** |      **NoWork** |         **False** |       **True** | **5.127 us** | **0.0148 us** | **0.0292 us** |
| **Contention 1** |      **NoWork** |          **True** |      **False** | **5.197 us** | **0.0274 us** | **0.0547 us** |
| **Contention 1** |      **NoWork** |          **True** |       **True** | **5.168 us** | **0.0200 us** | **0.0405 us** |
| **Contention 1** | **NoExtraLock** |         **False** |      **False** | **5.257 us** | **0.0229 us** | **0.0457 us** |
| **Contention 1** | **NoExtraLock** |         **False** |       **True** | **5.298 us** | **0.0290 us** | **0.0585 us** |
| **Contention 1** | **NoExtraLock** |          **True** |      **False** | **5.337 us** | **0.0460 us** | **0.0929 us** |
| **Contention 1** | **NoExtraLock** |          **True** |       **True** | **5.332 us** | **0.0294 us** | **0.0581 us** |
| **Contention 1** |   **ExtraLock** |         **False** |      **False** | **5.248 us** | **0.0180 us** | **0.0359 us** |
| **Contention 1** |   **ExtraLock** |         **False** |       **True** | **5.334 us** | **0.0305 us** | **0.0595 us** |
| **Contention 1** |   **ExtraLock** |          **True** |      **False** | **5.342 us** | **0.0237 us** | **0.0473 us** |
| **Contention 1** |   **ExtraLock** |          **True** |       **True** | **5.284 us** | **0.0199 us** | **0.0402 us** |
