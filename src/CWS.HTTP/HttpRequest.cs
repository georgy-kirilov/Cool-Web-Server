namespace CWS.HTTP
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using static Constants;

    public class HttpRequest
    {
        public static readonly Dictionary<string, Dictionary<string, string>> Sessions = new();

        public HttpRequest(string request)
        {
            Headers = new();
            Cookies = new();
            Query = new();

            var lines = request.Split(NewLine, StringSplitOptions.None);
            var headerLineArguments = lines[0].Split(' ');

            Method = (Method)Enum.Parse(typeof(Method), headerLineArguments[0], ignoreCase: true);
            Path = headerLineArguments[1];

            ParseHeadersAndBody(lines);
            ParseCookies();
            ParseQuery();
        }

        private void ParseHeadersAndBody(string[] lines)
        {
            var bodyBuilder = new StringBuilder();
            bool isInsideHeaders = true;

            for (int index = 1; index < lines.Length; index++)
            {
                string line = lines[index];

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInsideHeaders = false;
                    continue;
                }

                if (isInsideHeaders)
                {
                    ParseHeader(line);
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            Body = bodyBuilder.ToString().TrimEnd();
        }

        private void ParseHeader(string header)
        {
            var headerArguments = header.Split(HeaderKeyValueSeparator);

            string headerName = headerArguments[0], headerValue = headerArguments[1];

            if (Headers.ContainsKey(headerName))
            {
                Headers[headerName] = headerValue;
            }
            else
            {
                Headers.Add(headerName, headerValue);
            }
        }

        private void ParseQuery()
        {
            var queryKeyValuePairs = Path.Split('?');

            foreach (string pair in queryKeyValuePairs)
            {
                var pairArguments = pair.Split('=');

                if (pairArguments.Length == 2)
                {
                    string key = pairArguments[0].ToLower();
                    string value = pairArguments[1];

                    if (!Query.ContainsKey(key))
                    {
                        Query.Add(key, value);
                    }
                    else
                    {
                        Query[key] = value;
                    }
                }
            }
        }

        private void ParseCookies()
        {
            if (Headers.ContainsKey(RequestCookieHeader))
            {
                var cookies = Headers[RequestCookieHeader].Split(HeaderValueAttributesSeparator);

                foreach (string cookie in cookies)
                {
                    var cookieArguments = cookie.Split("=");

                    string cookieName = cookieArguments[0];
                    string cookieValue = cookieArguments[1];

                    if (Cookies.ContainsKey(cookieName))
                    {
                        Cookies[cookieName] = cookieValue;
                    }
                    else
                    {
                        Cookies.Add(cookieName, cookieValue);
                    }

                    if (cookieName == SessionCookieName)
                    {
                        SessionCookie = cookieValue;
                    }
                }
            }
        }

        public string Path { get; }

        public Method Method { get; }

        public string Body { get; private set; }

        public string SessionCookie { get; private set; }

        public Dictionary<string, string> Headers { get; }

        public Dictionary<string, string> Cookies { get; }

        public Dictionary<string, string> Query { get; }
    }
}
