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

                HttpRequest request = await ReadRequestAsync(stream);

                ResponseCookie session = ManageSessionCookie(request);
                HttpResponse response = ManageAction(request);

                response.Cookies.Add(session);

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

        private ResponseCookie ManageSessionCookie(HttpRequest request)
        {
            var sessionCookie = request.SessionCookie ?? Guid.NewGuid().ToString();

            if (request.SessionCookie == null)
            {
                var responseSessionCookie = new ResponseCookie(Constants.SessionCookieName, sessionCookie);
                HttpRequest.Sessions.Add(sessionCookie, new());
                return responseSessionCookie;
            }

            return null;
        }

        private HttpResponse ManageAction(HttpRequest request)
        {
            var route = new Route(request.Method, request.Path);
            var action = routingTable.GetAction(route);

            if (action != null)
            {
                return InvokeAction(action, request);
            }
            else
            {
                return new HttpResponse(StatusCode.NotFound);
            }
        }

        private HttpResponse InvokeAction(Func<HttpRequest, HttpResponse> action, HttpRequest request)
        {
            try
            {
                return action.Invoke(request);
            }
            catch (Exception)
            {
                return new HttpResponse(StatusCode.InternalServerError);
            }
        }
    }
}
