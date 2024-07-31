using MemoryDb.Console.Core;

Console.WriteLine("Starting MemoryDb");

var memoryDb = new KeyValueStore();
var inputParser = new InputParser();

var server = new Server(memoryDb, inputParser);

server.Start();