namespace CWS.HTTP
{
    using System.Text;

    internal class HttpStringBuilder
    {
        private readonly StringBuilder builder = new();

        public void AppendLine(object value = null)
        {
            builder.Append($"{value}{Constants.NewLine}");
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}
