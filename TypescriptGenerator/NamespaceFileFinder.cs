using System;
using System.Collections.Generic;
using TypescriptGenerator.Extensions;
using TypescriptGenerator.Settings;

namespace TypescriptGenerator
{
    public static class NamespaceFileFinder
    {
        public static string GetFileContainingType(
            Type type, 
            List<NamespaceSettings> namespaceSettings,
            string defaultFilename)
        {
            var matchingNamespaceSetting = namespaceSettings.GetMostSpecificMatch(type.Namespace);
            if (matchingNamespaceSetting == null)
                return defaultFilename;
            return matchingNamespaceSetting.Filename ?? defaultFilename;
        }
    }
}
