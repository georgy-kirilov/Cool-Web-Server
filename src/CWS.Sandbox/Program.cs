namespace CWS.Sandbox
{
    using CWS.HTTP;

    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            var routingTable = new RoutingTable();

            routingTable.AddRoute(Method.Get, "/", Home);
            routingTable.AddRoute(Method.Get, "/Home", Home);
            routingTable.AddRoute(Method.Get, "/About", About);
            routingTable.AddRoute(Method.Get, "/Privacy", Privacy);

            var server = new HttpServer(routingTable);
            await server.StartAsync();
        }

        static readonly string Navbar = File.ReadAllText("./static/navbar.html");

        public static HttpResponse Home(HttpRequest request)
        {
            string html = Navbar + "<h1>Home</h1>";

            var response = new HttpResponse()
            {
                ContentType = MimeTypes.Html,
                Body = Encoding.UTF8.GetBytes(html),
            };

            return response;
        }

        public static HttpResponse About(HttpRequest request)
        {
            string html = Navbar + "<h1>About</h1>";

            var response = new HttpResponse()
            {
                ContentType = MimeTypes.Html,
                Body = Encoding.UTF8.GetBytes(html),
            };

            return response;
        }

        public static HttpResponse Privacy(HttpRequest request)
        {
            string html = Navbar + "<h1>Privacy</h1>";

            var response = new HttpResponse()
            {
                ContentType = MimeTypes.Html,
                Body = Encoding.UTF8.GetBytes(html),
            };

            return response;
        }
    }
}
