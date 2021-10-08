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

        public static void Home(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>Home</h1>";

            response.ContentType = MimeTypes.Html;
            response.Body = Encoding.UTF8.GetBytes(html);
        }

        public static void About(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>About</h1>";

            response.ContentType = MimeTypes.Html;
            response.Body = Encoding.UTF8.GetBytes(html);
        }

        public static void Privacy(HttpRequest request, HttpResponse response)
        {
            string html = Navbar + "<h1>Privacy</h1>";

            response.ContentType = MimeTypes.Html;
            response.Body = Encoding.UTF8.GetBytes(html);
        }
    }
}
