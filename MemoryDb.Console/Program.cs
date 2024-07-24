using MemoryDb.Console;

Console.WriteLine("Starting MemoryDb...");

var memoryDb = new KeyValueStore();

memoryDb.Set("Test Key", "Test Value", 5);

Console.WriteLine(memoryDb.Get("Test Key"));

Thread.Sleep(6000);

Console.WriteLine(memoryDb.Get("Test Key"));