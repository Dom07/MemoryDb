using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Sockets;
using System.Text;

namespace MemoryDb.Client.Core
{
    public class Client
    {
        private const int Port = 6397;
        private const string Host = "localhost";
        private TcpClient client;
        private NetworkStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        public Client()
        {
            Console.Write($"KVLite Client initiated for {Host}:{Port}");
            client = new TcpClient(Host, Port);
            stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            reader = new StreamReader(stream, Encoding.ASCII);
        }

        public async Task<string> Set(string key, string value, string timeToLive)
        {
            string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\",\"ttl\": \"{timeToLive}\"}}";
            writer.WriteLine(command);

            string response = reader.ReadLine()!;
            Console.WriteLine(response);

            command = $"{{\"Operation\": \"GET\", \"key\": \"{key}\"}}";
            writer.WriteLine(command);

            response = reader.ReadLine()!;
            Console.WriteLine(response);

            return response;
        }
    }
}
