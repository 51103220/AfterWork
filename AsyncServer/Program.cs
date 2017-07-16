using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncServer
{
    class Server
    {
        // maximum threads in thread pool 
        private const int NUMBER_OF_THREADS = 200;
        private static SemaphoreSlim S = new SemaphoreSlim(NUMBER_OF_THREADS, NUMBER_OF_THREADS);

        private const int PORT = 11111;
        private const int BUFFER_SIZE = 5000;
        static async void RunServerAsync()
        {
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[1];
            var listener = new TcpListener(ipAddress, PORT);
           

            listener.Start();
            Console.WriteLine($"listening from {ipAddress.ToString()}:{PORT} ...");
            try
            {
                while (true)
                {
                    Accept(await listener.AcceptTcpClientAsync());
                }
            }
            finally { listener.Stop(); }
        }

        static async void Accept(TcpClient client)
        {
            await Task.Yield();
            Console.WriteLine($"Connect established from client {((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()}");
            try
            {
                using (client)
                using (NetworkStream n = client.GetStream())
                {
                    await S.WaitAsync();
                    byte[] data = new byte[5000];
                    await n.ReadAsync(data, 0, data.Length);
                      
                    Console.WriteLine($"\t Start to respond to client {Encoding.ASCII.GetString(data)}...waiting for 15s");
                    await Task.Delay(TimeSpan.FromSeconds(15));

                    Array.Reverse(data); // Reverse the byte sequence
                    await n.WriteAsync(data, 0, data.Length);
                    Console.WriteLine("\t done responding to client");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                S.Release();
            }
        }
        
        static void Main(string[] args)
        {
            RunServerAsync();
            Console.ReadKey();
        }
    }
}
