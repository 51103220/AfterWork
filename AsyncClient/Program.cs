using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncClient
{
    class Server
    {
        private const int PORT = 11111;
        private const int BUFFER_SIZE = 5000;
        private const int NUM_OF_REQUESTS = 1000;

        static async Task<string> Call(int index)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // 1. connect
                    IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[1];
                    await client.ConnectAsync(ipAddress, PORT);
                    Stream stream = client.GetStream();
                    Console.WriteLine($"Request {index} connected to server.");

                    // 2. send
                    string str = $"Request {index}";

                    byte[] data = Encoding.ASCII.GetBytes(str);

                    await stream.WriteAsync(data, 0, data.Length);

                    // 3. receive
                    byte[] received = new byte[5000];
                    await stream.ReadAsync(received, 0, received.Length);
                    var response = Encoding.ASCII.GetString(data);
                    Console.WriteLine($"\t Request {index} has reponse: {response}");
                    return response;
                }
                
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            return string.Empty;

        }
        static Task<string[]> MakeRequestsAsync()
        {
            List<string> responses = new List<string>();
            int[] indexes = new int[NUM_OF_REQUESTS];
            for (var i = 0; i < NUM_OF_REQUESTS; i++)
            {
                indexes[i] = i;
            }
            return Task.WhenAll(indexes.Select(async i => await Call(i)).ToList());
            
        }

        static void Main(string[] args)
        {
            string[] responses = MakeRequestsAsync().Result;

            var nbOfSuccessfulResponses = responses.Count(x => x != string.Empty);
            Console.WriteLine($"{nbOfSuccessfulResponses}/{NUM_OF_REQUESTS} requests is made successfully");

            Console.WriteLine("Done Testing");
            Console.ReadKey();
        }
    }
}
