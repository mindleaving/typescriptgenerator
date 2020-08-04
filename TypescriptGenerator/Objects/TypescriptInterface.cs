using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptInterface : ITypescriptObject
    {
        public TypescriptInterface(
            string originalNamespace,
            string translatedNamespace,
            string name,
            List<TypescriptProperty> properties,
            List<Type> directDependencies,
            List<string> modifiers)
        {
            OriginalNamespace = originalNamespace;
            TranslatedNamespace = translatedNamespace;
            Name = name;
            Properties = properties;
            DirectDependencies = directDependencies;
            Modifiers = modifiers;
        }

        public string OriginalNamespace { get; }
        public string TranslatedNamespace { get; }
        public string Name { get; }
        public List<string> Modifiers { get; }
        public List<TypescriptProperty> Properties { get; }
        public List<Type> DirectDependencies { get; }
    }
}