using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptNamespace
    {
        public TypescriptNamespace(
            string translatedName,
            string translatedFullName,
            string originalName,
            string originalFullName,
            List<string> modifiers,
            List<ITypescriptObject> types,
            List<TypescriptNamespace> subNamespaces,
            string outputFilename)
        {
            TranslatedName = translatedName;
            TranslatedFullName = translatedFullName;
            OriginalName = originalName;
            OriginalFullName = originalFullName;
            Modifiers = modifiers;
            Types = types;
            SubNamespaces = subNamespaces;
            OutputFilename = outputFilename;
        }

        public string TranslatedName { get; }
        public string TranslatedFullName { get; }
        public string OriginalName { get; }
        public string OriginalFullName { get; }
        public string OutputFilename { get; }
        public List<string> Modifiers { get; }
        public List<ITypescriptObject> Types { get; }
        public List<TypescriptNamespace> SubNamespaces { get; }
    }
}