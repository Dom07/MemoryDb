using MemoryDb.Console.Models;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MemoryDb.Console.Core
{
    public class Server
    {
        private const int Port = 6397;

        private readonly TcpListener _listener;
        private readonly KeyValueStore _memoryDb;
        private readonly InputParser _inputParser;

        public Server(KeyValueStore memoryDb, InputParser inputParser)
        {
            _memoryDb = memoryDb;
            _inputParser = inputParser;
            _listener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            _listener.Start();
            System.Console.WriteLine($"Server started. Listening on port {Port}...");

            while (true)
            {
                var client = _listener.AcceptTcpClient();
                ClientHandler(client);
            }
        }

        public StatusModel ProcessRequest(string request)
        {
            var response = new StatusModel();

            var command = _inputParser.Parse(request);

            switch (command.Operation)
            {
                case Operation.SET:
                        int timeToLive = (command.TimeToLive != null && int.Parse(command.TimeToLive) > 0) ? int.Parse(command.TimeToLive) : -1;
                        response = _memoryDb.Set(command.Key, command.Value, timeToLive);
                        break;
                case Operation.GET:
                        response = _memoryDb.Get(command.Key);
                        break;
                default:
                        response.Status = Enums.StatusEnum.Error;
                        response.Message = "Incorrect command provided.";
                        break;
            }

            return response;
        }

        private void ClientHandler(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream, Encoding.UTF8);
                var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                while (client.Connected)
                {
                    try
                    {
                        if (!stream.DataAvailable)
                            break;

                        var buffer = new byte[1024];
                        var messageBuilder = new StringBuilder();

                        while (true)
                        {
                            var bytesRead = stream.Read(buffer, 0, buffer.Length);
                            messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                            if (stream.DataAvailable)
                                continue;

                            break;
                        }

                        var receivedData = Encoding.UTF8.GetBytes(messageBuilder.ToString());
                        var receivedString = Encoding.UTF8.GetString(receivedData);
                        System.Console.WriteLine("Received Message from client: " + receivedString);

                        var response = ProcessRequest(receivedString);
                        var responeString = $"Status: {response.Status}, Message: {response.Message}, Value: {response.Value}";
                        var encodedResponse = Encoding.UTF8.GetBytes(responeString);
                        stream.Write(encodedResponse, 0, encodedResponse.Length);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error Occurred in Client Handler Loop: " + ex);
                    }
                }

                client.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error occurred in Client Handler: " + ex);
            }
        }
    }
}
