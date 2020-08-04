namespace TypescriptGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string RemovePrefix(this string str, string prefix)
        {
            if (str == null)
                return null;
            if (prefix == null)
                return str;
            if (!str.StartsWith(prefix))
                return str;
            return str.Substring(prefix.Length);
        }

        public static string RemoveSuffix(
            this string str,
            string suffix)
        {
            if (str == null)
                return null;
            if (suffix == null)
                return str;
            if (!str.EndsWith(suffix))
                return str;
            return str.Substring(0, str.Length - suffix.Length);
        }
    }
}