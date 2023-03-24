using RedLockNet.SERedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonAppSample
{
    public class Worker
    {
        private readonly RedLockFactory _redLockFactory;

        public Worker(RedLockFactory redLockFactory)
        {
            _redLockFactory = redLockFactory;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var resource = "the-thing-we-are-locking-on";
                var expiry = TimeSpan.FromSeconds(6);
                var wait = TimeSpan.FromSeconds(1);
                var retry = TimeSpan.FromSeconds(1);

                Console.WriteLine($"{DateTime.Now} Worker creating.");
                // blocks until acquired or 'wait' timeout
                await using (var redLock = await _redLockFactory.CreateLockAsync(resource, expiry, wait, retry))
                {
                    bool gotTheLock = false;
                    // make sure we got the lock
                    if (redLock.IsAcquired)
                    {
                        gotTheLock = true;
                        // do stuff
                        Console.WriteLine($"{DateTime.Now} Worker doing stuff...");
                        await Task.Delay(3000, cancellationToken);
                        Console.WriteLine($"{DateTime.Now} Worker finished.");
                    }
                    else
                    {
                        Console.WriteLine($"{DateTime.Now} Worker failed.");
                    }

                    if (gotTheLock)
                        Console.WriteLine($"{DateTime.Now} Worker releasing.");
                }
                // the lock is automatically released at the end of the using block
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
}
