namespace CWS.HTTP
{
    using System;
    using System.Text;
    using System.Collections.Generic;

    public class ResponseCookie
    {
        private CookieSameSiteOptions sameSiteOptions;

        public ResponseCookie(string name, string value, int? maxAge = null)
        {
            Path = "/";
            Name = name;
            Value = value;
            IsSecure = true;
            IsHttpOnly = true;
            ExpiresOn = null;
            MaxAge = maxAge;
            SameSiteOptions = CookieSameSiteOptions.Lax;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsHttpOnly { get; set; }

        public bool IsSecure { get; set; }

        public string Path { get; set; }

        public string Domain { get; set; }

        public int? MaxAge { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public CookieSameSiteOptions SameSiteOptions
        {
            get => sameSiteOptions;

            set
            {
                if (value == CookieSameSiteOptions.None)
                {
                    IsSecure = true;
                }

                sameSiteOptions = value;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var nullableAttributes = new Dictionary<string, object>
            {
                { Name, Value },
                { "Expires", ExpiresOn?.ToString("R") },
                { "Max-Age", MaxAge },
                { "Path", Path },
                { "Domain", Domain },
                { "SameSite", SameSiteOptions },
            };

            var booleanAttributes = new Dictionary<string, bool>
            {
                { "Secure", IsSecure },
                { "HttpOnly", IsHttpOnly },
            };

            foreach (var attribute in nullableAttributes)
            {
                if (attribute.Value != null)
                {
                    builder.Append($"{attribute.Key}={attribute.Value}; ");
                }
            }

            foreach (var attribute in booleanAttributes)
            {
                if (attribute.Value)
                {
                    builder.Append($"{attribute.Key}; ");
                }
            }

            return builder.ToString();
        }
    }
}
