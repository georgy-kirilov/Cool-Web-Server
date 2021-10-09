namespace CWS.MVC
{
    using CWS.HTTP;

    public abstract class Controller
    {
        protected HttpRequest Request { get; }

        protected HttpResponse Response { get; }


    }
}
