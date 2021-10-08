namespace CWS.HTTP
{
    using System.Collections;
    using System.Collections.Generic;

    public class HeaderCollection : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly Dictionary<string, string> headers = new();

        public void AddHeader(string name, object value)
        {
            if (headers.ContainsKey(name))
            {
                headers[name] = value.ToString();
            }
            else
            {
                headers.Add(name, value.ToString());
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var header in headers)
            {
                yield return header;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
