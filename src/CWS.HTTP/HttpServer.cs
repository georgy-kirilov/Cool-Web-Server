namespace CWS.HTTP
{
    using Common;

    using System;
    using System.Net;
    using System.Net.Sockets;

    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class HttpServer
    {
        private static readonly IPAddress Localhost = IPAddress.Loopback;
        private const int Port = 9999;

        private readonly RoutingTable routingTable;
        private readonly TcpListener listener;

        public HttpServer(RoutingTable routingTable) : this(routingTable, Localhost, Port)
        {
        }

        public HttpServer(RoutingTable routingTable, IPAddress address, int port)
        {
            Guard.AgainstNull(routingTable, nameof(routingTable));
            Guard.AgainstNull(address, nameof(address));

            this.routingTable = routingTable;
            listener = new TcpListener(address, port);
        }

        public async Task StartAsync()
        {
            listener.Start();
            var endPoint = (IPEndPoint)listener.LocalEndpoint;

            Console.WriteLine($"HTTP Server started at {endPoint.Address} on port {endPoint.Port}");
            Console.WriteLine("Listening for request...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                using NetworkStream stream = client.GetStream();

                var response = new HttpResponse(StatusCode.Ok);
                HttpRequest request = await ReadRequestAsync(stream);

                ManageSessionCookie(request, response);
                ManageAction(request, response);

                Console.WriteLine(response);

                await stream.WriteAsync(response.ToByteArray());
            }
        }

        private async Task<HttpRequest> ReadRequestAsync(NetworkStream stream)
        {
            const int BufferSize = 1024;

            var bytesRead = new List<byte>();
            var buffer = new byte[BufferSize];
            int offset = 0;

            while (true)
            {
                int bytesReadCount = await stream.ReadAsync(buffer, offset, buffer.Length);
                offset += bytesReadCount;

                if (bytesReadCount >= buffer.Length)
                {
                    bytesRead.AddRange(buffer);
                }
                else
                {
                    var trimmmedCopy = new byte[bytesReadCount];
                    Array.Copy(buffer, trimmmedCopy, trimmmedCopy.Length);
                    bytesRead.AddRange(trimmmedCopy);
                    break;
                }
            }

            string request = Encoding.UTF8.GetString(bytesRead.ToArray());
            return new HttpRequest(request);
        }

        private void ManageSessionCookie(HttpRequest request, HttpResponse response)
        {
            var sessionCookie = request.SessionCookie ?? Guid.NewGuid().ToString();

            if (request.SessionCookie == null)
            {
                var responseSessionCookie = new ResponseCookie(Constants.SessionCookieName, sessionCookie, 30);
                HttpRequest.Sessions.Add(sessionCookie, new());
                response.Cookies.Add(responseSessionCookie);
            }
        }

        private void ManageAction(HttpRequest request, HttpResponse response)
        {
            var route = new Route(request.Method, request.Path);
            var action = routingTable.GetAction(route);

            if (action != null)
            {
                InvokeAction(action, request, response);
            }
            else
            {
                response.StatusCode = StatusCode.NotFound;
            }
        }

        private void InvokeAction(Action<HttpRequest, HttpResponse> action, HttpRequest request, HttpResponse response)
        {
            try
            {
                action?.Invoke(request, response);
            }
            catch (Exception)
            {
                response.StatusCode = StatusCode.InternalServerError;
            }
        }
    }
}
