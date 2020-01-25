| Method |        Work | DifferentKeys | LockPerKey |     Mean |     Error |    StdDev |
|------- |------------ |-------------- |----------- |---------:|----------:|----------:|
| **Contention 100** |      **NoWork** |         **False** |      **False** | **135.9 us** | **0.8185 us** | **1.6533 us** |
| **Contention 100** |      **NoWork** |         **False** |       **True** | **134.2 us** | **0.3304 us** | **0.6599 us** |
| **Contention 100** |      **NoWork** |          **True** |      **False** | **137.1 us** | **0.4739 us** | **0.9131 us** |
| **Contention 100** |      **NoWork** |          **True** |       **True** | **133.3 us** | **0.3957 us** | **0.7811 us** |
| **Contention 100** | **NoExtraLock** |         **False** |      **False** | **141.4 us** | **0.5465 us** | **1.0787 us** |
| **Contention 100** | **NoExtraLock** |         **False** |       **True** | **142.4 us** | **0.5610 us** | **1.1074 us** |
| **Contention 100** | **NoExtraLock** |          **True** |      **False** | **162.9 us** | **0.4368 us** | **0.8824 us** |
| **Contention 100** | **NoExtraLock** |          **True** |       **True** | **170.3 us** | **0.4381 us** | **0.8647 us** |
| **Contention 100** |   **ExtraLock** |         **False** |      **False** | **175.9 us** | **3.0013 us** | **6.0627 us** |
| **Contention 100** |   **ExtraLock** |         **False** |       **True** | **142.8 us** | **0.6640 us** | **1.3413 us** |
| **Contention 100** |   **ExtraLock** |          **True** |      **False** | **195.9 us** | **1.8988 us** | **3.8356 us** |
| **Contention 100** |   **ExtraLock** |          **True** |       **True** | **164.1 us** | **0.4940 us** | **0.9751 us** |
