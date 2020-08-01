using System.Collections.Generic;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public static class NamespaceTranslator
    {
        public static string Translate(
            string ns,
            List<NamespaceSettings> namespaceSettings)
        {
            if (ns == null)
                return null;
            var matchingNamespaceSetting = namespaceSettings.GetMostSpecificMatch(ns);
            if (matchingNamespaceSetting == null)
                return ns;
            return matchingNamespaceSetting.Translation + ns.Substring(matchingNamespaceSetting.Namespace.Length);
        }
    }
}
