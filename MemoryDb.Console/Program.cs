using MemoryDb.Console;

Console.WriteLine("Starting MemoryDb");

var memoryDb = new KeyValueStore();

var server = new Server(memoryDb);

server.Start();