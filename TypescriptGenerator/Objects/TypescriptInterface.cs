using System;
using System.Collections.Generic;

namespace TypescriptGenerator.Objects
{
    public class TypescriptInterface : ITypescriptObject
    {
        public TypescriptInterface(
            string ns,
            string name,
            List<TypescriptProperty> properties,
            List<Type> directDependencies,
            List<string> modifiers)
        {
            Namespace = ns;
            Properties = properties;
            DirectDependencies = directDependencies;
            Modifiers = modifiers;
            Name = name;
        }

        public string Namespace { get; }
        public string Name { get; }
        public List<string> Modifiers { get; }
        public List<TypescriptProperty> Properties { get; }
        public List<Type> DirectDependencies { get; }
    }
}