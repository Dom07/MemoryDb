using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MemoryDb.Console
{
    public class Server
    {
        private const int Port = 6397;

        private readonly TcpListener _listener;
        private readonly KeyValueStore _memoryDb;

        public Server(KeyValueStore memoryDb)
        {
            _memoryDb = memoryDb;
            _listener = new TcpListener(IPAddress.Any, Port);
        }

        public void Start()
        {
            _listener.Start();
            System.Console.WriteLine($"Server started. Listening on port {Port}...");

            while(true)
            {
                var client = _listener.AcceptTcpClient();
                ClientHandler(client);
            }
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
                        System.Console.WriteLine("Received Message from client: "+receivedString);

                        var encodedResponse = Encoding.UTF8.GetBytes(receivedString);
                        stream.Write(encodedResponse, 0, encodedResponse.Length);
                    }
                    catch(Exception ex)
                    {
                        System.Console.WriteLine("Error Occurred in Client Handler Loop: "+ex);
                    }
                }

                client.Close();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Error occurred in Client Handler: " + ex);
            }
        }
    }
}
