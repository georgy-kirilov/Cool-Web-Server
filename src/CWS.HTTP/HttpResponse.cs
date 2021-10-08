namespace CWS.HTTP
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    using static Constants;

    public class HttpResponse
    {
        public HttpResponse(StatusCode statusCode, string contentType = MimeTypes.Text)
        {
            Headers = new();
            Cookies = new();

            Body = Array.Empty<byte>();
            StatusCode = statusCode;
            ContentType = contentType;
        }

        public StatusCode StatusCode { get; set; }

        public string ContentType { get; set; }

        public byte[] Body { get; set; }

        public HeaderCollection Headers { get; }

        public List<ResponseCookie> Cookies { get; }

        public byte[] ToByteArray()
        {
            var headersBytes = Encoding.UTF8.GetBytes(ToString());
            var responseBytes = new byte[headersBytes.Length + Body.Length];

            Array.Copy(headersBytes, responseBytes, headersBytes.Length);
            Array.Copy(Body, sourceIndex: 0, responseBytes, headersBytes.Length, Body.Length);

            return responseBytes;
        }

        public override string ToString()
        {
            var builder = new HttpStringBuilder();

            var mandatoryHeaders = new KeyValuePair<string, object>[]
            {
                new("Server", Constants.Server),
                new("Content-Type", ContentType),
                new("Content-Length", Body.Length),
                new("Date", DateTime.UtcNow.ToString("R")),
                new("Connection", "Keep-Alive"),
            };

            foreach (var header in mandatoryHeaders)
            {
                Headers.AddHeader(header.Key, header.Value);
            }

            int statusCode = (int)StatusCode;

            builder.AppendLine($"{Constants.Version} {statusCode} {StatusCode.Message()}");

            foreach (var header in Headers)
            {
                builder.AppendLine($"{header.Key}{HeaderKeyValueSeparator}{header.Value}");
            }

            foreach (var cookie in Cookies)
            {
                builder.AppendLine($"{ResponseCookieHeader}{HeaderKeyValueSeparator}{cookie}");
            }

            builder.AppendLine();

            return builder.ToString();
        }
    }
}
