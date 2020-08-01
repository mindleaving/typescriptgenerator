using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class NamespaceOrganizerSettings
    {
        public NamespaceOrganizerSettings(
            List<string> modifiers = null, 
            List<NamespaceSettings> namespaceSettings = null)
        {
            Modifiers = modifiers ?? new List<string>();
            NamespaceSettings = namespaceSettings ?? new List<NamespaceSettings>();
        }

        public List<string> Modifiers { get; }
        public List<NamespaceSettings> NamespaceSettings { get; }
    }
}