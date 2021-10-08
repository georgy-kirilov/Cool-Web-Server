namespace CWS.HTTP
{
    public static class Constants
    {
        // Common
        public const string NewLine = "\r\n";
        public const string Server = "Cool Web Server";
        public const string Version = "HTTP/1.1";

        // Headers
        public const string HeaderKeyValueSeparator = ": ";
        public const string HeaderValueAttributesSeparator = "; ";

        // Cookies
        public const string SessionCookieName = "SID";
        public const string RequestCookieHeader = "Cookie";
        public const string ResponseCookieHeader = "Set-Cookie";
    }
}
