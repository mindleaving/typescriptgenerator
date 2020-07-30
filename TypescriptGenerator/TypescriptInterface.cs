using System;
using System.Collections.Generic;

namespace TypescriptGenerator
{
    public class TypescriptInterface : ITypescriptObject
    {
        public TypescriptInterface(
            string ns,
            string name,
            List<TypescriptProperty> properties,
            List<Type> directDependencies)
        {
            Namespace = ns;
            Properties = properties;
            DirectDependencies = directDependencies;
            Name = name;
        }

        public string Namespace { get; }
        public string Name { get; }
        public List<string> Modifiers { get; }
        public List<TypescriptProperty> Properties { get; }
        public List<Type> DirectDependencies { get; }
    }
}