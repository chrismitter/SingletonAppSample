// See https://aka.ms/new-console-template for more information
using RedLockNet.SERedis.Configuration;
using RedLockNet.SERedis;
using StackExchange.Redis;
using System.Net;
using SingletonAppSample;

Console.WriteLine("Hello, World!");

var azureEndPoint = new RedLockEndPoint
{
    EndPoint = new DnsEndPoint("singletonappsdemo.redis.cache.windows.net", 6380),
    Password = "xxx",
    Ssl = true
};
var azureEndPoints = new List<RedLockEndPoint>
{
    azureEndPoint
};

var redLockFactory = RedLockFactory.Create(azureEndPoints);

var worker = new Worker(redLockFactory);
var tokenSource = new CancellationTokenSource();

await worker.RunAsync(tokenSource.Token);

tokenSource.Cancel();
redLockFactory.Dispose();

Console.WriteLine("Done.");
Console.Read();
