using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypescriptGenerator
{
    public static class PropertyNameSplitter
    {
        /// <summary>
        /// Splits property name by detecting underscores and camel casing.
        /// Returns parts as lower case.
        /// </summary>
        public static IList<string> Split(string propertyName)
        {
            var normalizedName = TrimSymbols(propertyName);
            if (normalizedName.Contains("_"))
            {
                return propertyName.Split(new []{ '_' }, StringSplitOptions.RemoveEmptyEntries)
                    .SelectMany(Split)
                    .ToList();
            }

            if (propertyName == propertyName.ToUpperInvariant())
                return new[] { propertyName.ToLowerInvariant() };
            var parts = new List<string>();
            var currentPart = char.ToLowerInvariant(propertyName.First()) + "";
            for (int i = 1; i < propertyName.Length; i++)
            {
                var c = propertyName[i];
                if (char.IsUpper(c))
                {
                    parts.Add(currentPart);
                    currentPart = "";
                }
                currentPart += char.ToLowerInvariant(c);
            }
            parts.Add(currentPart);
            return parts;
        }

        private static string TrimSymbols(string propertyName)
        {
            var match = Regex.Match(propertyName, "^[^a-zA-Z0-9]*(?<Trimmed>[a-zA-Z0-9].*[a-zA-Z0-9])[^a-zA-Z0-9]*$");
            if (!match.Success)
                return propertyName;
            return match.Groups["Trimmed"].Value;
        }
    }
}
