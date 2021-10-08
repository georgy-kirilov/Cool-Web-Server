namespace CWS.HTTP
{
    using Common;

    using System;

    public class Route
    {
        public Route(Method method, string path)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstEmptyOrWhiteSpace(path, nameof(path));

            Method = method;
            Path = path;
        }

        public Method Method { get; }

        public string Path { get; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Route)
            {
                Route other = obj as Route;
                return Method == other.Method && Path.Equals(other.Path, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Method, Path.ToLower());
        }
    }
}
