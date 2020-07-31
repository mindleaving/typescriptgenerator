using System.Collections.Generic;

namespace TypescriptGenerator.Settings
{
    public class NamespaceSettings
    {
        public NamespaceSettings(string ns)
        {
            Namespace = ns;
        }

        public string Namespace { get; }
        public string Translation { get; set; }
        public string Filename { get; set; }
        public List<string> Modifiers { get; } = new List<string>();
    }
}