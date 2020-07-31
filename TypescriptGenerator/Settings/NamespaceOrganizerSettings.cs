using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class NamespaceOrganizerSettings
    {
        public NamespaceOrganizerSettings(List<NamespaceSettings> namespaceSettings = null)
        {
            NamespaceSettings = namespaceSettings ?? new List<NamespaceSettings>();
        }

        public List<NamespaceSettings> NamespaceSettings { get; }
    }
}