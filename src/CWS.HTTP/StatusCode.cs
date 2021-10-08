namespace CWS.HTTP
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public enum StatusCode
    {
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,
        PartialContent = 206,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        PermanentRedirect = 308,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        RequestTimeout = 408,
        InternalServerError = 500,
    }

    public static class StatusCodeExtensions
    {
        private static readonly Regex regex = new(@"[A-Z]{1}[a-z]+");

        public static string Message(this StatusCode code)
        {
            var words = regex.Matches(code.ToString()).Select(match => match.Value);
            return string.Join(' ', words);
        }
    }
}
