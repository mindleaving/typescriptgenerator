using System.Collections.Generic;
using System.Linq;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator.Extensions
{
    public static class NamespaceSettingsExtensions
    {
        public static NamespaceSettings GetMostSpecificMatch(
            this List<NamespaceSettings> namespaceSettingses,
            string ns)
        {
            return namespaceSettingses
                .Where(x => ns.StartsWith(x.Namespace))
                .OrderByDescending(x => x.Namespace.Length)
                .FirstOrDefault();
        }
    }
}
