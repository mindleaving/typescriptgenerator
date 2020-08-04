namespace TypescriptGenerator.Extensions
{
    public static class StringExtensions
    {
        public static string RemovePrefix(this string str, string prefix)
        {
            if (prefix == null)
                return str;
            if (!str.StartsWith(prefix))
                return str;
            return str.Substring(prefix.Length);
        }
    }
}