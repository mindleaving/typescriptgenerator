namespace TypescriptGenerator
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
    }
}