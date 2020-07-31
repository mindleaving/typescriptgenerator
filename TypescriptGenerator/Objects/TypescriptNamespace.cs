using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptNamespace
    {
        public TypescriptNamespace(
            string name,
            List<string> modifiers,
            List<ITypescriptObject> types,
            List<TypescriptNamespace> subNamespaces,
            string outputFilename)
        {
            Name = name;
            Modifiers = modifiers;
            Types = types;
            SubNamespaces = subNamespaces;
            OutputFilename = outputFilename;
        }

        public string Name { get; }
        public string OutputFilename { get; }
        public List<string> Modifiers { get; }
        public List<ITypescriptObject> Types { get; }
        public List<TypescriptNamespace> SubNamespaces { get; }
    }
}