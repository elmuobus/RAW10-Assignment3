// RAW10 - DELEBECQUE Alexis, DUMONT-ROTY Loïc, SHOWIKI Ali Isac
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Assignment3
{
    public class Server
    {
        private IPAddress _ipAddress = Dns.Resolve("localhost").AddressList[0];
        private int _port = 5000;

        public void startServer()
        {
            var server = new TcpListener(_ipAddress, _port);
            var buffer = new byte[1024];
            
            server.Start();
            Console.WriteLine("[INFO] - Server started");
            try
            {
                while (true)
                {
                    Console.WriteLine("[INFO] - Waiting for a connection...");
                    var client = server.AcceptTcpClient();
                    Console.WriteLine($"[INFO] - Client {client} accepted");
                    var stream = client.GetStream();
                    var rdCnt = stream.Read(buffer);
                    var msg = Encoding.UTF8.GetString(buffer, 0, rdCnt);
                    Console.WriteLine($"[INFO] - Client {client} message: '{msg}'");
                    var response = Encoding.UTF8.GetBytes(msg.ToUpper());
                    stream.Write(response);
                    stream.Close();
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"Socket Exception error: {e}");
            }
            finally
            {
                server.Stop();
                Console.WriteLine("[INFO] - Server stopped");
            }
        }
    }
}