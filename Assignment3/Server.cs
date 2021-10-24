#nullable enable
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Assignment3
{
    public class Server
    {
        private readonly TcpListener _server;
        public Server(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();
            ListenClients();
        }

        private string AllMissingRequestElementWithMethod(Request request)
        {
            List<string> result = new(){ReturnStatus.MissingBody};

            if (request.Date == null)
                result.Add(ReturnStatus.MissingDate);
            if (request.Path == null)
                result.Add(ReturnStatus.MissingPath);
            if (request.Body == null)
                result.Add(ReturnStatus.MissingBody);
            
            return result.Count > 0 ? $"4 {string.Join(", ", result)}" : "";
        }

        private void ManageOneClient(object? obj)
        {
            var clientStream = (NetworkStream)obj!;
            var request = Utils.ReadRequest(clientStream);
            AMethod? method = null;
            var response = new Response()
            {
                Status = "",
                Body = null,
            };

            if (request == null)
                return;
            if (request.Method == null)
            {
                response.Status = AllMissingRequestElementWithMethod(request);
            }
            else
            {
                switch (request.Method)
                {
                    case "create":
                        method = new Create(request);
                        break;
                    case "read":
                        method = new Read(request);
                        break;
                    case "update":
                        method = new Update(request);
                        break;
                    case "delete":
                        method = new Delete(request);
                        break;
                    case "echo":
                        method = new Echo(request);
                        break;
                    default:
                        response.Status = $"4 {ReturnStatus.IllegalMethod}";
                        break;
                }
            }
            
            method?.Launch();
            var newResponse = method?.Response();
            if (newResponse != null) response = newResponse;

            var msg = Encoding.UTF8.GetBytes(Utils.ToJson(response)); 
            clientStream.Write(msg, 0, msg.Length);
        }

        private void ListenClients()
        {
            while (true)
            {
                var client = _server.AcceptTcpClient();
                var clientStream = client.GetStream();
                
                Thread t = new(ManageOneClient);
                t.Start(clientStream);
            }
        }
    }
}