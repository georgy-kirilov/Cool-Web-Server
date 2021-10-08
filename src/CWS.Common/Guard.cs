namespace CWS.Common
{
    using System;

    public static class Guard
    {
        public static void AgainstNull(object param, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void AgainstEmptyOrWhiteSpace(string param, string paramName)
        {
            param = param.Trim();

            if (param == string.Empty)
            {
                throw new ArgumentException($"{paramName} cannot be empty or white space");
            }
        }
    }
}
